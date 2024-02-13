using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

namespace VRC2
{
    public class NetworkOVRBody : NetworkBehaviour,
        OVRSkeleton.IOVRSkeletonDataProvider,
        OVRSkeletonRenderer.IOVRSkeletonRendererDataProvider
    {
        private OVRPlugin.BodyState _bodyState;

        private OVRPlugin.Quatf[] _boneRotations;

        private OVRPlugin.Vector3f[] _boneTranslations;

        private bool _dataChangedSinceLastQuery;

        private bool _hasData;

        private const OVRPermissionsRequester.Permission BodyTrackingPermission =
            OVRPermissionsRequester.Permission.BodyTracking;

        private Action<string> _onPermissionGranted;
        private static int _trackingInstanceCount;

        // Synchronize SkeletonPoseData through Fish-Net
        [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner, Channel = Channel.Reliable)]
        public OVRSkeleton.SkeletonPoseData skeletonPoseData { get; [ServerRpc(RunLocally = true)] set; }

        // private void OnSkeletonPoseData(OVRSkeleton.SkeletonPoseData prev, OVRSkeleton.SkeletonPoseData next, bool asServer)
        // {
        //     print($"on_data : {asServer}");
        // }

        /// <summary>
        /// The raw <see cref="BodyState"/> data used to populate the <see cref="OVRSkeleton"/>.
        /// </summary>
        public OVRPlugin.BodyState? BodyState => _hasData ? _bodyState : default(OVRPlugin.BodyState?);

        private void Awake()
        {
            if (!base.isActiveAndEnabled || (base.isActiveAndEnabled && base.IsClient))
            {
                _onPermissionGranted = OnPermissionGranted;
            }
        }

        private void Start()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            if (base.isActiveAndEnabled && !Owner.IsLocalClient)
            {
                Debug.LogWarning("Override NetworkOVRBody OnEnable");
                if (!enabled) enabled = true;
                if (!_hasData) _hasData = true;
                if (!_dataChangedSinceLastQuery) _dataChangedSinceLastQuery = true;
                return;
            }

            _trackingInstanceCount++;
            _dataChangedSinceLastQuery = false;
            _hasData = false;


            if (!StartBodyTracking())
            {
                enabled = false;
                return;
            }

            if (OVRPlugin.nativeXrApi == OVRPlugin.XrApi.OpenXR)
            {
                GetBodyState(OVRPlugin.Step.Render);
            }
            else
            {
                enabled = false;
                Debug.LogWarning($"[{nameof(OVRBody)}] Body tracking is only supported by OpenXR and is unavailable.");
            }
        }

        private void OnPermissionGranted(string permissionId)
        {
            if (permissionId == OVRPermissionsRequester.GetPermissionId(BodyTrackingPermission))
            {
                OVRPermissionsRequester.PermissionGranted -= _onPermissionGranted;
                enabled = true;
            }
        }

        private bool StartBodyTracking()
        {
            if (!OVRPermissionsRequester.IsPermissionGranted(BodyTrackingPermission))
            {
                OVRPermissionsRequester.PermissionGranted -= _onPermissionGranted;
                OVRPermissionsRequester.PermissionGranted += _onPermissionGranted;
                return false;
            }

            if (!OVRPlugin.StartBodyTracking())
            {
                Debug.LogWarning($"[{nameof(OVRBody)}] Failed to start body tracking.");
                return false;
            }

            return true;
        }

        private void OnDisable()
        {
            if (--_trackingInstanceCount == 0)
            {
                OVRPlugin.StopBodyTracking();
            }
        }

        private void OnDestroy()
        {
            OVRPermissionsRequester.PermissionGranted -= _onPermissionGranted;
        }

        private void Update() => GetBodyState(OVRPlugin.Step.Render);

        private void GetBodyState(OVRPlugin.Step step)
        {
            if (base.isActiveAndEnabled && !Owner.IsLocalClient)
            {
                if (!enabled) enabled = true;
                if (!_hasData) _hasData = true;
                if (!_dataChangedSinceLastQuery) _dataChangedSinceLastQuery = true;
                return;
            }

            if (OVRPlugin.GetBodyState(step, ref _bodyState))
            {
                _hasData = true;
                _dataChangedSinceLastQuery = true;
            }
            else
            {
                _hasData = false;
            }
        }

        OVRSkeleton.SkeletonType OVRSkeleton.IOVRSkeletonDataProvider.GetSkeletonType() =>
            OVRSkeleton.SkeletonType.Body;

        OVRSkeleton.SkeletonPoseData OVRSkeleton.IOVRSkeletonDataProvider.GetSkeletonPoseData()
        {
            if (!base.isActiveAndEnabled || Owner.IsLocalClient)
            {
                if (!_hasData) return default;

                if (_dataChangedSinceLastQuery)
                {
                    // Make sure arrays have been allocated
                    Array.Resize(ref _boneRotations, _bodyState.JointLocations.Length);
                    Array.Resize(ref _boneTranslations, _bodyState.JointLocations.Length);

                    // Copy joint poses into bone arrays
                    for (var i = 0; i < _bodyState.JointLocations.Length; i++)
                    {
                        var jointLocation = _bodyState.JointLocations[i];
                        if (jointLocation.OrientationValid)
                        {
                            var orientation = jointLocation.Pose.Orientation;
                            _boneRotations[i] = orientation;
                        }

                        if (jointLocation.PositionValid)
                        {
                            var position = jointLocation.Pose.Position;
                            _boneTranslations[i] = position;
                        }
                    }

                    _dataChangedSinceLastQuery = false;
                }

                // render locally
                skeletonPoseData = new OVRSkeleton.SkeletonPoseData
                {
                    IsDataValid = true,
                    IsDataHighConfidence = _bodyState.Confidence > .5f,
                    RootPose = _bodyState.JointLocations[(int)OVRPlugin.BoneId.Body_Root].Pose,
                    RootScale = 1.0f,
                    BoneRotations = _boneRotations,
                    BoneTranslations = _boneTranslations,
                    SkeletonChangedCount = (int)_bodyState.SkeletonChangedCount,
                };
                return skeletonPoseData;
            }

            // use synchronized
            return skeletonPoseData;
        }


        OVRSkeletonRenderer.SkeletonRendererData
            OVRSkeletonRenderer.IOVRSkeletonRendererDataProvider.GetSkeletonRendererData() => _hasData
            ? new OVRSkeletonRenderer.SkeletonRendererData
            {
                RootScale = 1.0f,
                IsDataValid = true,
                IsDataHighConfidence = true,
                ShouldUseSystemGestureMaterial = false,
            }
            : default;
    }
}