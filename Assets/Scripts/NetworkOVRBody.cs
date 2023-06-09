using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace VRC2
{

    [ProtoContract]
    public struct Quatf
    {
        [ProtoMember(1)] public float x;
        [ProtoMember(2)] public float y;
        [ProtoMember(3)] public float z;
        [ProtoMember(4)] public float w;
    }

    [ProtoContract]
    public struct Vector3f
    {
        [ProtoMember(1)] public float x;
        [ProtoMember(2)] public float y;
        [ProtoMember(3)] public float z;
    }

    [ProtoContract]
    public struct Posef
    {
        [ProtoMember(1)] public Quatf Orientation;
        [ProtoMember(2)] public Vector3f Position;
    }

    [ProtoContract]
    public struct SkeletonPoseData
    {
        [ProtoMember(1)] public Posef RootPose { get; set; }
        [ProtoMember(2)] public float RootScale { get; set; }
        [ProtoMember(3)] public Quatf[] BoneRotations { get; set; }
        [ProtoMember(4)] public bool IsDataValid { get; set; }
        [ProtoMember(5)] public bool IsDataHighConfidence { get; set; }
        [ProtoMember(6)] public Vector3f[] BoneTranslations { get; set; }
        [ProtoMember(7)] public int SkeletonChangedCount { get; set; }

        public static void SetRootPose(ref Posef pose, OVRPlugin.Posef src)
        {
            var p = src.Position;
            var q = src.Orientation;

            pose.Position.x = p.x;
            pose.Position.y = p.y;
            pose.Position.z = p.z;

            pose.Orientation.x = q.x;
            pose.Orientation.y = q.y;
            pose.Orientation.z = q.z;
            pose.Orientation.w = q.w;
        }

        public static void SetBoneRotation(ref Quatf[] quatf, OVRPlugin.Quatf src, int idx)
        {
            quatf[idx].x = src.x;
            quatf[idx].y = src.y;
            quatf[idx].z = src.z;
            quatf[idx].w = src.w;
        }

        public static void SetBoneTranslation(ref Vector3f[] vector3f, OVRPlugin.Vector3f src, int idx)
        {
            vector3f[idx].x = src.x;
            vector3f[idx].y = src.y;
            vector3f[idx].z = src.z;
        }
    }

    public class NetworkOVRBody : MonoBehaviour,
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

        // protobuf SkeletonPoseData
        private SkeletonPoseData _networkSkeletonPoseData;
        private Quatf[] _networkBoneRotations;
        private Vector3f[] _networkBoneTranslations;
        private Posef _networkRootPose;

        /// <summary>
        /// The raw <see cref="BodyState"/> data used to populate the <see cref="OVRSkeleton"/>.
        /// </summary>
        public OVRPlugin.BodyState? BodyState => _hasData ? _bodyState : default(OVRPlugin.BodyState?);

        private void Awake()
        {
            _onPermissionGranted = OnPermissionGranted;
        }

        private void OnEnable()
        {
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
            if (!_hasData) return default;

            if (_dataChangedSinceLastQuery)
            {
                // Make sure arrays have been allocated
                Array.Resize(ref _boneRotations, _bodyState.JointLocations.Length);
                Array.Resize(ref _boneTranslations, _bodyState.JointLocations.Length);

                Array.Resize(ref _networkBoneRotations, _bodyState.JointLocations.Length);
                Array.Resize(ref _networkBoneTranslations, _bodyState.JointLocations.Length);


                // Copy joint poses into bone arrays
                for (var i = 0; i < _bodyState.JointLocations.Length; i++)
                {
                    var jointLocation = _bodyState.JointLocations[i];
                    if (jointLocation.OrientationValid)
                    {
                        var orientation = jointLocation.Pose.Orientation;
                        _boneRotations[i] = orientation;

                        SkeletonPoseData.SetBoneRotation(ref _networkBoneRotations, orientation, i);
                    }

                    if (jointLocation.PositionValid)
                    {
                        var position = jointLocation.Pose.Position;
                        _boneTranslations[i] = position;

                        SkeletonPoseData.SetBoneTranslation(ref _networkBoneTranslations, position, i);
                    }
                }

                _dataChangedSinceLastQuery = false;
            }

            SkeletonPoseData.SetRootPose(ref _networkRootPose,
                _bodyState.JointLocations[(int)OVRPlugin.BoneId.Body_Root].Pose);

            _networkSkeletonPoseData = new SkeletonPoseData()
            {
                IsDataValid = true,
                IsDataHighConfidence = _bodyState.Confidence > .5f,
                RootPose = _networkRootPose,
                RootScale = 1.0f,
                BoneRotations = _networkBoneRotations,
                BoneTranslations = _networkBoneTranslations,
                SkeletonChangedCount = (int)_bodyState.SkeletonChangedCount,
            };

            return new OVRSkeleton.SkeletonPoseData
            {
                IsDataValid = true,
                IsDataHighConfidence = _bodyState.Confidence > .5f,
                RootPose = _bodyState.JointLocations[(int)OVRPlugin.BoneId.Body_Root].Pose,
                RootScale = 1.0f,
                BoneRotations = _boneRotations,
                BoneTranslations = _boneTranslations,
                SkeletonChangedCount = (int)_bodyState.SkeletonChangedCount,
            };
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