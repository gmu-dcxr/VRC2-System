using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using VRC2.Events;
using VRC2.Hack;
using VRC2.Pipe;
using PipeType = VRC2.Pipe.PipeConstants.PipeType;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeDiameter = VRC2.Pipe.PipeConstants.PipeDiameter;
using PipeColor = VRC2.Pipe.PipeConstants.PipeColor;
using PipeParameters = VRC2.Pipe.PipeConstants.PipeParameters;

namespace VRC2
{
    [RequireComponent(typeof(PointableUnityEventWrapper))]
    public class PipeManipulation : MonoBehaviour
    {
        [Header("Segments")] [SerializeField] private GameObject segmentA;
        [SerializeField] private GameObject segmentB;
        [SerializeField] private GameObject segmentMid;

        public GameObject SegA => segmentA;
        public GameObject SegB => segmentB;
        public GameObject SegM => segmentMid;

        [Header("Pipe Settings")]
        // current color
        public PipeColor pipeColor = PipeColor.Green;

        public PipeType pipeType = PipeType.Sewage;
        public PipeBendAngles angle = PipeBendAngles.Angle_0;
        public PipeDiameter diameter = PipeDiameter.Diameter_1;
        public float segmentALength = 1.0f;
        public float segmentBLength = 1.0f;

        [Header("Pipe Type Label")] public TextMesh pipeLabel;

        // the default length for each segment
        private float defaultSegmentLengthInFeet = 5;

        private PointableUnityEventWrapper _wrapper;

        private NetworkGrabbable _networkGrabbable;

        // pipe is held by controller
        [HideInInspector] public bool heldByController = false;

        [HideInInspector] public bool collidingWall { get; set; }

        public bool IsStraight
        {
            get => angle == PipeBendAngles.Angle_0;
        }

        public bool NotBeingCut // original length
        {
            get => segmentA.transform.localScale.x == 1.0 && segmentB.transform.localScale.x == 1.0;
        }

        [HideInInspector]
        public PipeParameters pipeParameters
        {
            get
            {
                var para = new PipeParameters();
                para.diameter = diameter;
                para.type = pipeType;
                para.angle = angle;
                para.a = segmentALength;
                para.b = segmentBLength;
                return para;
            }
        }

        private Renderer _rendererA
        {
            get => segmentA.GetComponent<Renderer>();
        }


        private Renderer _rendererB
        {
            get => segmentB.GetComponent<Renderer>();
        }

        private Renderer _renderMid
        {
            get
            {
                if (segmentMid == null) return null;
                return segmentMid.GetComponent<Renderer>();
            }
        }

        private MeshCollider _meshColliderA
        {
            get => segmentA.GetComponent<MeshCollider>();
        }

        private MeshCollider _meshColliderB
        {
            get => segmentB.GetComponent<MeshCollider>();
        }

        private Rigidbody _rigidbody
        {
            get => GetComponent<Rigidbody>();
        }

        #region Hint Managers

        private List<ClampHintManager> _clampHintsManagers;

        [HideInInspector]
        public List<ClampHintManager> clampHintsManagers
        {
            get
            {
                if (_clampHintsManagers == null)
                {
                    _clampHintsManagers = new List<ClampHintManager>();
                    var children = Utils.GetChildren<ClampHintManager>(gameObject);

                    foreach (var child in children)
                    {
                        var chm = child.GetComponent<ClampHintManager>();
                        _clampHintsManagers.Add(chm);
                    }
                }

                return _clampHintsManagers;
            }
        }

        #endregion

        #region Distance Grab Interactable

        // enable/disable to let the pipe interactable/not-interactable

        private DistanceGrabInteractable _distanceGrabInteractable;

        private DistanceGrabInteractable distanceGrabInteractable
        {
            get
            {
                if (_distanceGrabInteractable == null)
                {
                    _distanceGrabInteractable = gameObject.GetComponentInChildren<DistanceGrabInteractable>();
                }

                return _distanceGrabInteractable;
            }
        }

        private void SetInteractable(bool enabled)
        {
            if (distanceGrabInteractable == null || distanceGrabInteractable.enabled == enabled) return;

            distanceGrabInteractable.enabled = enabled;
        }

        #endregion

        #region Simple Object Checking

        private bool simplenessChecked = false;
        private bool _isSimplePipe;

        private bool isSimplePipe
        {
            get
            {
                if (!simplenessChecked)
                {
                    var root = PipeHelper.GetRoot(gameObject);

                    _isSimplePipe = root == gameObject;

                    simplenessChecked = true;
                }

                return _isSimplePipe;
            }
        }

        #endregion

        #region Compensation for single pipe collision with the wall

        private PipeGrabFreeTransformer _freeTransformer;

        private PipeGrabFreeTransformer freeTransformer
        {
            get
            {
                if (_freeTransformer == null)
                {
                    _freeTransformer = gameObject.GetComponent<PipeGrabFreeTransformer>();
                }

                return _freeTransformer;
            }
        }

        #endregion

        private bool IsConnector => gameObject.name.ToLower().Contains("connector");

        // Start is called before the first frame update
        void Start()
        {
            // do nothing for a connector
            if (IsConnector) return;

            // set length
            SetLength(segmentALength, segmentBLength);

            // set materials
            SetMaterial();

            // bind event
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();
            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenRelease.AddListener(OnRelease);

            _networkGrabbable = gameObject.GetComponent<NetworkGrabbable>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CheckAfterUnclamp()
        {
            // this is necessary when unclamping
            if (!heldByController)
            {
                // simulate release
                freeTransformer.SimulateRelease();

                SetInteraction(ShouldFall());
            }
        }

        public void SetHeldByController(bool held)
        {
            heldByController = held;
            // release when the state is updated outside
            if (!held && freeTransformer != null)
            {
                // simulate release
                freeTransformer.SimulateRelease();
            }
        }

        public void SetInteraction(bool enable)
        {
            SetKinematic(!enable);
            SetInteractable(enable);
        }

        public bool ShouldFall()
        {
            foreach (var chm in clampHintsManagers)
            {
                if (chm.Clamped) return false;
            }

            return true;
        }

        public void SetTriggers(bool flag)
        {
            print($"Update triggers for {gameObject.name} - {flag}");
            _meshColliderA.isTrigger = flag;
            _meshColliderB.isTrigger = flag;
        }

        public void SetMaterial()
        {
            var m = GetMaterial(diameter, pipeType, pipeColor);
            if (m == null)
            {
                Debug.LogWarning($"Not found material for ({diameter}, {pipeType}, {pipeColor})");
                return;
            }

            _rendererA.material = m;
            _rendererB.material = m;
            if (_renderMid != null)
            {
                _renderMid.material = m;
            }

            UpdatePipeTypeLabel(pipeType);
        }

        void UpdatePipeTypeLabel(PipeType t)
        {
            if (pipeLabel != null)
            {
                // update type
                // format: type (length)
                var type = Utils.GetDisplayName<PipeType>(t);
                var length = segmentALength + segmentBLength;
                pipeLabel.text = $"{type} ({PipeHelper.FormatLength(length)})";
            }
        }

        public void SetMaterial(PipeDiameter d, PipeType t, PipeColor c)
        {
            diameter = d;
            pipeType = t;
            pipeColor = c;

            SetMaterial();
        }

        public void SetMaterial(PipeParameters param)
        {
            SetMaterial(param.diameter, param.type, param.color);
        }

        public void SetMaterial(PipeType type, PipeColor color)
        {
            pipeType = type;
            pipeColor = color;
            SetMaterial();
        }

        public void SetLength(float a, float b)
        {
            // update 
            segmentALength = a;
            segmentBLength = b;

            // calculate the x-scale factor for the object
            var fa = a / defaultSegmentLengthInFeet;
            var fb = b / defaultSegmentLengthInFeet;

            var sa = segmentA.transform.localScale;
            sa.x = fa;

            segmentA.transform.localScale = sa;

            var sb = segmentB.transform.localScale;
            sb.x = fb;

            segmentB.transform.localScale = sb;

            // update texture
            UpdateTexture(segmentA, fa);
            UpdateTexture(segmentB, fb);
        }

        public bool IsNotCut()
        {
            var sax = segmentA.transform.localScale.x;
            var sbx = segmentB.transform.localScale.x;
            return (sax == 1) && (sbx == 1);
        }

        float GetSegmentLength(GameObject go)
        {
            return PipeHelper.GetExtendsX(go);
        }

        public float GetSegmentALength()
        {
            return GetSegmentLength(segmentA);
        }

        public float GetSegmentBLength()
        {
            return GetSegmentLength(segmentB);
        }

        // the real diameter is the extents y or z
        public float GetRealDiameter()
        {
            return PipeHelper.GetExtendsZ(segmentA);
        }

        void UpdateTexture(GameObject pipe, float xValue, float yValue = 1.0f)
        {
            // By: Will @ willfredranc@gmail.com

            //Pipe with texture
            //Scale texture on given pipe based on x value of pipe scale.
            //check pipe type (water has multiple textures)
            // float xValue = pipe.transform.localScale.x;
            // float yValue = pipe.transform.localScale.y;
            Renderer r = pipe.GetComponent<Renderer>();
            //material of pipe, changes to this only effect this object. ONLY AT RUNTIME
            Material m = r.material;
            m.SetTextureScale("_MainTex", new Vector2(xValue, yValue));
            //Having offset set between values 0.1-0.4, inclusive, seemed to properly align texture. 
            m.SetTextureOffset("_MainTex", new Vector2(0.1f, 0.0f));

            m.SetTextureScale("_BumpMap", new Vector2(xValue, yValue));
            m.SetTextureOffset("_BumpMap", new Vector2(0.1f, 0.0f));
            m.SetTextureScale("_ParallaxMap", new Vector2(xValue, yValue));
            m.SetTextureOffset("_ParallaxMap", new Vector2(0.1f, 0.0f));
        }

        #region Pointable Event

        void SetKinematic(bool enable)
        {
            if (_rigidbody == null) return;

            _rigidbody.isKinematic = enable;
        }

        public void OnSelect()
        {
            print("OnSelect");
            heldByController = true;
            // enable kinematic
            SetKinematic(true);
            // reset
            freeTransformer.ResetSimulatedRelease();
            // force move pipe away
            StartCoroutine(freeTransformer.ForceMoveAway());
        }

        public void OnRelease()
        {
            print("OnRelease");
            heldByController = false;

            if (freeTransformer.Compensated)
            {
                // disable kinematic to let it drop when should fall
                SetKinematic(!ShouldFall());
            }
            else
            {
                SetKinematic(false);
            }
        }

        #endregion

        #region Get Material By Parameters

        Material GetMaterial(PipeDiameter d, PipeType t, PipeColor c)
        {
            var para = new PipeParameters();
            para.diameter = d;
            para.type = t;
            para.color = c;
            return GetMaterial(para);
        }

        Material GetMaterial(PipeParameters para)
        {
            return PipeHelper.LoadPipeMaterial(para);
        }



        #endregion

        #region Pipe-pipe Connecting

        bool heldByRightHand()
        {
            // var lb = OVRInput.Get(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.LTouch); // grip button
            var rb = OVRInput.Get(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.RTouch);

            if (!rb) return false;

            var points = _networkGrabbable.GrabPoints;
            if (points.Count < 1)
            {
                return false;
            }

            var grabpoint = _networkGrabbable.GrabPoints[0];

            var l = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            var r = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);


            var ld = Vector3.Distance(l, grabpoint.position);
            var rd = Vector3.Distance(r, grabpoint.position);

            if (ld > rd)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}