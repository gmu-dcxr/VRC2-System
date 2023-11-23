using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;


namespace VRC2
{
    public class DistanceMeasurer : MonoBehaviour
    {
        [Header("Oculus Cursor")] [SerializeField]
        private GameObject left;

        [SerializeField] private GameObject right;

        [Header("Quad")] [SerializeField] private GameObject quad;

        [Header("Labels")] [SerializeField] private TextMeshPro _horizontal;
        [SerializeField] private TextMeshPro _vertical;
        [SerializeField] private TextMeshPro _diagonal;

        private string format = "f2";

        // ratio = model_length / measured_length
        private float ratio = 3.25f;

        // measure distance only when two controllers' select button (trigger) are pressed.

        #region Anchor-based Distance Measuring

        private RayInteractor _leftRayInteractor;

        private RayInteractor LeftRayInteractor
        {
            get
            {
                if (_leftRayInteractor == null)
                {
                    var go = GetRayInteractor(left);
                    if (go != null)
                    {
                        return go.GetComponent<RayInteractor>();
                    }
                }

                return _leftRayInteractor;
            }
        }

        private RayInteractor _rightRayInteractor;

        private RayInteractor RightRayInteractor
        {
            get
            {
                if (_rightRayInteractor == null)
                {
                    var go = GetRayInteractor(right);
                    if (go != null)
                    {
                        return go.GetComponent<RayInteractor>();
                    }
                }

                return _rightRayInteractor;
            }
        }

        // the name of the anchor cube
        private string anchorCubeName = "Cube";

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            print($"left ray: {LeftRayInteractor != null}");
            print($"right ray: {RightRayInteractor != null}");
        }

        // Update is called once per frame
        void Update()
        {
            var l = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
            var r = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
            
            if (l && r)
            {
                // var (h, v, d) = GetTouchPointsDistances();
                // use the anchor based method
                var (h, v, d) = GetAnchorPointsDistances();
                if (h >= 0)
                {
                    // valid
                    SetTexts(h, v, d);
                    return;
                }
            }

            ClearTexts();
        }

        public void ClearTexts()
        {
            quad.SetActive(false);
            _horizontal.text = "";
            _vertical.text = "";
            _diagonal.text = "";
        }

        string FormatDistance(float value)
        {
            return $"{value.ToString(format)}'";
        }

        void SetTexts(float horizontal, float vertical, float diagonal)
        {
            quad.SetActive(true);
            _horizontal.text = FormatDistance(horizontal);
            _vertical.text = FormatDistance(vertical);
            _diagonal.text = FormatDistance(diagonal);
        }

        bool GetTouchPoint(RayInteractor interactor, out Vector3 point)
        {
            if (interactor.CollisionInfo.HasValue)
            {
                RaycastHit hit;
                if (Physics.Raycast(interactor.Ray, out hit, 50f)) // 50 should be large enough
                {
                    if (hit.transform.gameObject.name.Equals(anchorCubeName))
                    {
                        point = hit.transform.position;
                        return true;
                    }
                }
            }

            point = Vector3.zero;
            return false;
        }

        (float, float, float) GetAnchorPointsDistances()
        {
            var leftValue = Vector3.zero;
            var rightValue = Vector3.zero;
            var l = GetTouchPoint(LeftRayInteractor, out leftValue);
            var r = GetTouchPoint(RightRayInteractor, out rightValue);

            float h = -1, v = -1, d = -1;

            if (l && r)
            {
                // multiply ratio to get the length in model scale 
                h = Math.Abs(leftValue.z - rightValue.z) * ratio;
                v = Math.Abs(leftValue.y - rightValue.y) * ratio;

                d = (float)Math.Sqrt(h * h + v * v);
            }

            return (h, v, d);
        }

        (float, float, float) GetTouchPointsDistances()
        {
            Vector3 leftValue = left.transform.position;
            // var left = GetTouchPoint(this.left, out leftValue);

            Vector3 rightValue = right.transform.position;
            // var right = GetTouchPoint(this.right, out rightValue);

            float h = -1, v = -1, d = -1;

            if (left && right)
            {
                // multiply ratio to get the length in model scale 
                h = Math.Abs(leftValue.z - rightValue.z) * ratio;
                v = Math.Abs(leftValue.y - rightValue.y) * ratio;

                d = (float)Math.Sqrt(h * h + v * v);
            }

            return (h, v, d);
        }

        GameObject GetRayInteractor(GameObject cursor)
        {
            // OculusCursor -- RayCasterCursorVisual -- Visuals -- ControllerRayInteractor
            try
            {
                return cursor.transform.parent.parent.parent.gameObject;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to get RayInteractor");
                return null;
            }
        }
    }
}