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

        [SerializeField, HideInInspector] private bool _updateRootPose = false;

        [SerializeField, HideInInspector] private bool _updateRootScale = false;

        [SerializeField, HideInInspector] private bool _enablePhysicsCapsules = false;

        [SerializeField, HideInInspector] private bool _applyBoneTranslations = true;

        private GameObject _bonesGO;
        private GameObject _bindPosesGO;
        private GameObject _capsulesGO;

        protected List<OVRBone> _bones;
        private List<OVRBone> _bindPoses;
        private List<OVRBoneCapsule> _capsules;
        
        public bool IsInitialized { get; private set; }

        public IList<OVRBone> Bones { get; protected set; }
        public IList<OVRBone> BindPoses { get; private set; }
        public IList<OVRBoneCapsule> Capsules { get; private set; }


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
        }

        protected override void Start()
        {
            IsInitialized = false;
            if (_networkObject.IsValid && _networkObject.HasInputAuthority)
            {
                // local end
                base.Start();
            }
        }

        protected override void Update()
        {
            if (_networkObject.IsValid && !_networkObject.HasInputAuthority)
            {
                if (!IsInitialized)
                {
                    // initalize first
                    InitializeBones();
                    InitializeBindPose();
                    InitializeCapsules();
                    IsInitialized = true;
                }

                UpdateNetworkSkeleton();
            }
            else
            {
                // local end
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

        protected override void InitializeBones()
        {
            base.InitializeBones();
        }

        private void InitializeBindPose()
        {
            if (!_bindPosesGO)
            {
                _bindPosesGO = new GameObject("BindPoses");
                _bindPosesGO.transform.SetParent(transform, false);
                _bindPosesGO.transform.localPosition = Vector3.zero;
                _bindPosesGO.transform.localRotation = Quaternion.identity;
            }

            if (_bindPoses == null || _bindPoses.Count != _bones.Count)
            {
                _bindPoses = new List<OVRBone>(new OVRBone[_bones.Count]);
                BindPoses = _bindPoses.AsReadOnly();
            }

            // pre-populate bones list before attempting to apply bone hierarchy
            for (int i = 0; i < _bindPoses.Count; ++i)
            {
                OVRBone bone = _bones[i];
                OVRBone bindPoseBone = _bindPoses[i] ?? (_bindPoses[i] = new OVRBone());
                bindPoseBone.Id = bone.Id;
                bindPoseBone.ParentBoneIndex = bone.ParentBoneIndex;

                Transform trans = bindPoseBone.Transform
                    ? bindPoseBone.Transform
                    : (bindPoseBone.Transform =
                        new GameObject(BoneLabelFromBoneId(_skeletonType, bindPoseBone.Id)).transform);
                trans.localPosition = bone.Transform.localPosition;
                trans.localRotation = bone.Transform.localRotation;
            }

            for (int i = 0; i < _bindPoses.Count; ++i)
            {
                if (!IsValidBone((BoneId)_bindPoses[i].ParentBoneIndex) ||
                    IsBodySkeleton(_skeletonType)) // Body bones are always in tracking space
                {
                    _bindPoses[i].Transform.SetParent(_bindPosesGO.transform, false);
                }
                else
                {
                    _bindPoses[i].Transform.SetParent(_bindPoses[_bindPoses[i].ParentBoneIndex].Transform, false);
                }
            }
        }

        private void InitializeCapsules()
        {
            bool flipX = IsHandSkeleton(_skeletonType);

            if (_enablePhysicsCapsules)
            {
                if (!_capsulesGO)
                {
                    _capsulesGO = new GameObject("Capsules");
                    _capsulesGO.transform.SetParent(transform, false);
                    _capsulesGO.transform.localPosition = Vector3.zero;
                    _capsulesGO.transform.localRotation = Quaternion.identity;
                }

                if (_capsules == null || _capsules.Count != _skeleton.NumBoneCapsules)
                {
                    _capsules = new List<OVRBoneCapsule>(new OVRBoneCapsule[_skeleton.NumBoneCapsules]);
                    Capsules = _capsules.AsReadOnly();
                }

                for (int i = 0; i < _capsules.Count; ++i)
                {
                    OVRBone bone = _bones[_skeleton.BoneCapsules[i].BoneIndex];
                    OVRBoneCapsule capsule = _capsules[i] ?? (_capsules[i] = new OVRBoneCapsule());
                    capsule.BoneIndex = _skeleton.BoneCapsules[i].BoneIndex;

                    if (capsule.CapsuleRigidbody == null)
                    {
                        capsule.CapsuleRigidbody =
                            new GameObject(BoneLabelFromBoneId(_skeletonType, bone.Id) + "_CapsuleRigidbody")
                                .AddComponent<Rigidbody>();
                        capsule.CapsuleRigidbody.mass = 1.0f;
                        capsule.CapsuleRigidbody.isKinematic = true;
                        capsule.CapsuleRigidbody.useGravity = false;
                        capsule.CapsuleRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    }

                    GameObject rbGO = capsule.CapsuleRigidbody.gameObject;
                    rbGO.transform.SetParent(_capsulesGO.transform, false);
                    rbGO.transform.position = bone.Transform.position;
                    rbGO.transform.rotation = bone.Transform.rotation;

                    if (capsule.CapsuleCollider == null)
                    {
                        capsule.CapsuleCollider =
                            new GameObject(BoneLabelFromBoneId(_skeletonType, bone.Id) + "_CapsuleCollider")
                                .AddComponent<CapsuleCollider>();
                        capsule.CapsuleCollider.isTrigger = false;
                    }

                    var p0 = flipX
                        ? _skeleton.BoneCapsules[i].StartPoint.FromFlippedXVector3f()
                        : _skeleton.BoneCapsules[i].StartPoint.FromFlippedZVector3f();
                    var p1 = flipX
                        ? _skeleton.BoneCapsules[i].EndPoint.FromFlippedXVector3f()
                        : _skeleton.BoneCapsules[i].EndPoint.FromFlippedZVector3f();
                    var delta = p1 - p0;
                    var mag = delta.magnitude;
                    var rot = Quaternion.FromToRotation(Vector3.right, delta);
                    capsule.CapsuleCollider.radius = _skeleton.BoneCapsules[i].Radius;
                    capsule.CapsuleCollider.height = mag + _skeleton.BoneCapsules[i].Radius * 2.0f;
                    capsule.CapsuleCollider.direction = 0;
                    capsule.CapsuleCollider.center = Vector3.right * mag * 0.5f;

                    GameObject ccGO = capsule.CapsuleCollider.gameObject;
                    ccGO.transform.SetParent(rbGO.transform, false);
                    ccGO.transform.localPosition = p0;
                    ccGO.transform.localRotation = rot;
                }
            }
        }
    }
}