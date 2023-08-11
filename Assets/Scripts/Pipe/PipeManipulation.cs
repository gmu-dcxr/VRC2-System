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

        // [Header("Materials")] [SerializeField] private Material _magentaMaterial;
        // [SerializeField] private Material _blueMaterial;
        // [SerializeField] private Material _yellowMaterial;
        // [SerializeField] private Material _greenMaterial;

        [Header("Pipe Settings")]
        // current color
        public PipeColor pipeColor = PipeColor.Green;

        public PipeType pipeType = PipeType.Sewage;
        public PipeBendAngles angle = PipeBendAngles.Angle_0;
        public PipeDiameter diameter = PipeDiameter.Diameter_1;
        public float segmentALength = 1.0f;
        public float segmentBLength = 1.0f;

        // the default length for each segment
        private float defaultSegmentLengthInFeet = 5;

        private PointableUnityEventWrapper _wrapper;

        private NetworkGrabbable _networkGrabbable;

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

        private bool beingSelected;

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


        // Start is called before the first frame update
        void Start()
        {
            // set length
            SetLength(segmentALength, segmentBLength);

            // set materials
            SetMaterial();

            beingSelected = false;

            // bind event
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();
            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenSelect.AddListener(OnUnselect);
            _wrapper.WhenMove.AddListener(OnMove);
            _wrapper.WhenCancel.AddListener(OnCancel);
            _wrapper.WhenRelease.AddListener(OnRelease);
            // _wrapper.WhenHover.AddListener(OnHover);
            // _wrapper.WhenUnhover.AddListener(OnUnhover);

            _networkGrabbable = gameObject.GetComponent<NetworkGrabbable>();
        }

        // Update is called once per frame
        void Update()
        {
            if (beingSelected && !OVRInput.Get(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.RTouch))
            {
                // pipe was released
                beingSelected = false;
                // SetTriggers(true);
            }
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

        void OnHover()
        {
        }

        void OnUnhover()
        {
        }

        void OnMove()
        {

        }

        void OnCancel()
        {
            // Debug.Log("Pipe Cancel");
        }

        public void OnSelect()
        {
            // Debug.Log("Pipe OnSelect");

            // if (heldByRightHand())
            // {
            //     beingSelected = true;
            //     SetTriggers(false);
            // }

            // update current select pipe
            // GlobalConstants.selectedPipe = gameObject;

            // enable kinematic
            _rigidbody.isKinematic = true;
        }

        public void OnUnselect()
        {
            // Debug.Log("Pipe OnUnselect");
        }

        public void OnRelease()
        {
            // print("Pipe OnRelease");
            // disable kinematic to let it drop
            _rigidbody.isKinematic = false;
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