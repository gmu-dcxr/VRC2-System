using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2
{

    [RequireComponent(typeof(LineRenderer))]
    public class EyeTrackingRay : MonoBehaviour
    {
        public float rayDistance = 1.0f;

        public float rayWidth = 0.01f;

        public LayerMask layersToInclude;

        public Color rayColorDefaultState = Color.yellow;
        public Color rayColorHoverState = Color.red;

        private LineRenderer _lineRenderer;

        private List<EyeInteractable> _eyeInteractables = new List<EyeInteractable>();

        // Start is called before the first frame update
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            SetupRay();
        }

        void SetupRay()
        {
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.positionCount = 2;
            _lineRenderer.startWidth = rayWidth;
            _lineRenderer.endWidth = rayWidth;

            _lineRenderer.startColor = rayColorDefaultState;
            _lineRenderer.endColor = rayColorDefaultState;

            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1,
                new Vector3(transform.position.x, transform.position.y, transform.position.z + rayDistance));
        }

        private void FixedUpdate()
        {
            RaycastHit hit;
            Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistance;

            if (Physics.Raycast(transform.position, rayCastDirection, out hit, Mathf.Infinity, layersToInclude))
            {
                UnSelect();
                _lineRenderer.startColor = rayColorHoverState;
                _lineRenderer.endColor = rayColorHoverState;

                var go = hit.transform.gameObject;
                EyeInteractable ei;
                if (go.TryGetComponent<EyeInteractable>(out ei))
                {
                    _eyeInteractables.Add(ei);
                    ei.IsHovered = true;
                    ei.eyeInteractor = transform;
                }
            }
            else
            {
                _lineRenderer.startColor = rayColorDefaultState;
                _lineRenderer.endColor = rayColorDefaultState;
                UnSelect(true);
            }

        }

        void UnSelect(bool clear = false)
        {
            foreach (var interactable in _eyeInteractables)
            {
                interactable.IsHovered = false;
            }

            if (clear)
            {
                _eyeInteractables.Clear();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}