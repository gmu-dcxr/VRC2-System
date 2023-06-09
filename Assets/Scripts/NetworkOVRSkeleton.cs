using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace VRC2
{
    public class NetworkOVRSkeleton : OVRSkeleton, ISerializationCallbackReceiver
    {
        [HideInInspector] [SerializeField] private List<Transform> _customBones_V2;
        public List<Transform> CustomBones => _customBones_V2;

        /// <summary>
        /// List of skeleton structures to be retargeted to the supported format for body tracking.
        /// </summary>
        public enum RetargetingType
        {
            /// <summary>The default skeleton structure of the Oculus tracking system</summary>
            OculusSkeleton
        }

        private readonly Quaternion wristFixupRotation = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

        [SerializeField, HideInInspector] public RetargetingType retargetingType = RetargetingType.OculusSkeleton;

        private IOVRSkeletonDataProvider _dataProvider;

        private bool _updateRootPose = false;

        private bool _updateRootScale = false;

        private NetworkObject _networkObject;

        protected override Transform GetBoneTransform(BoneId boneId) => _customBones_V2[(int)boneId];

#if UNITY_EDITOR
        private bool _shouldSetDirty;

        private void OnValidate()
        {
            if (!_shouldSetDirty) return;

            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            UnityEditor.EditorUtility.SetDirty(this);
            _shouldSetDirty = false;
        }
#endif

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            AllocateBones();
        }

        protected override void Awake()
        {
            base.Awake();

            // rewrite dataProvider
            _dataProvider = gameObject.GetComponent<IOVRSkeletonDataProvider>();

            if (_dataProvider == null)
            {
                Debug.LogError("Failed to locate skeleton data provider");
            }

            if (_networkObject == null)
            {
                _networkObject = gameObject.GetComponent<NetworkObject>();
                if (_networkObject == null)
                {
                    Debug.LogError("Failed to locate network object");
                }
            }

            if (_networkObject.IsValid && !_networkObject.HasInputAuthority)
            {
                // initialize first
                base.Start();
            }
        }

        protected override void Update()
        {
            if (_networkObject.IsValid && !_networkObject.HasInputAuthority)
            {
                UpdateNetworkSkeleton();
            }
            else
            {
                base.UpdateSkeleton();
            }
        }

        private void AllocateBones()
        {
            if (_customBones_V2.Count == (int)BoneId.Max) return;

            // Make sure we have the right number of bones
            while (_customBones_V2.Count < (int)BoneId.Max)
            {
                _customBones_V2.Add(null);
            }

#if UNITY_EDITOR
            _shouldSetDirty = true;
#endif
        }

        internal void SetSkeletonType(SkeletonType skeletonType)
        {
            _skeletonType = skeletonType;
            _customBones_V2 ??= new List<Transform>();

            AllocateBones();
        }

        protected void UpdateNetworkSkeleton()
        {
            var data = _dataProvider.GetSkeletonPoseData();

            if (!data.IsDataValid)
            {
                return;
            }

            if (_updateRootPose)
            {
                transform.localPosition = data.RootPose.Position.FromFlippedZVector3f();
                transform.localRotation = data.RootPose.Orientation.FromFlippedZQuatf();
            }

            if (_updateRootScale)
            {
                transform.localScale = new Vector3(data.RootScale, data.RootScale, data.RootScale);
            }

            for (var i = 0; i < _bones.Count; ++i)
            {
                var boneTransform = _bones[i].Transform;
                if (boneTransform == null) continue;

                if (IsBodySkeleton(_skeletonType))
                {
                    boneTransform.localPosition = data.BoneTranslations[i].FromFlippedZVector3f();
                    boneTransform.localRotation = data.BoneRotations[i].FromFlippedZQuatf();
                }
                else if (IsHandSkeleton(_skeletonType))
                {
                    boneTransform.localRotation = data.BoneRotations[i].FromFlippedXQuatf();

                    if (_bones[i].Id == BoneId.Hand_WristRoot)
                    {
                        boneTransform.localRotation *= wristFixupRotation;
                    }
                }
                else
                {
                    boneTransform.localRotation = data.BoneRotations[i].FromFlippedZQuatf();
                }
            }
        }

        private static bool IsBodySkeleton(SkeletonType type) => type == SkeletonType.Body;

        private static bool IsHandSkeleton(SkeletonType type) =>
            type == SkeletonType.HandLeft || type == SkeletonType.HandRight;
    }
}