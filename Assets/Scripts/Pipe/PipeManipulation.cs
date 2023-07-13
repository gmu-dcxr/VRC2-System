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
        public PipeConstants.PipeColor pipeColor = PipeConstants.PipeColor.Green;

        public PipeType pipeType = PipeType.Sewage;
        public PipeBendAngles angle = PipeBendAngles.Angle_0;
        public PipeDiameter diameter = PipeDiameter.Diameter_1;
        public float segmentALength = 1.0f;
        public float segmentBLength = 1.0f;

        // the default length for each segment
        private float defaultSegmentLengthInFeet = 5;

        private PointableUnityEventWrapper _wrapper;

        private Renderer _rendererA;
        private Renderer _rendererB;


        // Start is called before the first frame update
        void Start()
        {
            _rendererA = segmentA.GetComponent<Renderer>();
            _rendererB = segmentB.GetComponent<Renderer>();

            // set length
            SetLength(segmentALength, segmentBLength);

            // set materials
            SetMaterial();

            // bind event
            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();
            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenSelect.AddListener(OnUnselect);
            // _wrapper.WhenHover.AddListener(OnHover);
            // _wrapper.WhenUnhover.AddListener(OnUnhover);
        }

        // Update is called once per frame
        void Update()
        {
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

        public void SetLength(float a, float b)
        {
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

        public void EnableCollisionDetector(bool flag)
        {
            var cd = gameObject.GetComponentInChildren<PipeCollisionDetector>(false);
            cd.enableDetection = flag;
        }

        void OnHover()
        {
            EnableCollisionDetector(true);
        }

        void OnUnhover()
        {
            EnableCollisionDetector(false);
        }

        public void OnSelect()
        {
            Debug.Log("Pipe OnSelect");
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
    }
}