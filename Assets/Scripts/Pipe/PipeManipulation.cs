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

        private Renderer _rendererA;
        private Renderer _rendererB;

        private bool beingSelected;

        private MeshCollider _meshColliderA;
        private MeshCollider _meshColliderB;


        // Start is called before the first frame update
        void Start()
        {
            _rendererA = segmentA.GetComponent<Renderer>();
            _rendererB = segmentB.GetComponent<Renderer>();

            _meshColliderA = segmentA.GetComponent<MeshCollider>();
            _meshColliderB = segmentB.GetComponent<MeshCollider>();

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
                SetTriggers(true);
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
            Debug.Log("Pipe Cancel");
        }

        public void OnSelect()
        {
            Debug.Log("Pipe OnSelect");

            if (heldByRightHand())
            {
                beingSelected = true;
                SetTriggers(false);
            }

            // update current select pipe
            GlobalConstants.selectedPipe = gameObject;
        }

        public void OnUnselect()
        {
            Debug.Log("Pipe OnUnselect");
        }

        #endregion

        #region Get Material By Parameters

        Material GetMaterial(PipeDiameter d, PipeType t, PipeColor c)
        {
            // TODO
            return null;
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