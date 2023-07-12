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
        [Header("Controller RayInteractors")] [SerializeField]
        private RayInteractor left;

        [SerializeField] private RayInteractor right;

        [Header("Quad")] [SerializeField] private GameObject quad;

        [Header("Labels")] [SerializeField] private TextMeshPro _horizontal;
        [SerializeField] private TextMeshPro _vertical;
        [SerializeField] private TextMeshPro _diagonal;

        private string format = "f2";

        // measure distance only when two controllers' select button (trigger) are pressed.

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var l = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
            var r = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);

            if (l && r)
            {
                var (h, v, d) = GetTouchPointsDistances();
                if (h >= 0)
                {
                    // valid
                    // TODO: Convert to the real world unit
                    SetTexts(h, v, d);
                    return;
                }
            }

            ClearTexts();
        }

        void ClearTexts()
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
                point = interactor.CollisionInfo.Value.Point;
                return true;
            }

            point = Vector3.zero;
            return false;
        }

        (float, float, float) GetTouchPointsDistances()
        {
            Vector3 leftValue;
            var left = GetTouchPoint(this.left, out leftValue);

            Vector3 rightValue;
            var right = GetTouchPoint(this.right, out rightValue);

            float h = -1, v = -1, d = -1;

            if (left && right)
            {
                h = Math.Abs(leftValue.x - rightValue.x);
                v = Math.Abs(leftValue.y = rightValue.y);
                d = Vector3.Distance(leftValue, rightValue);
            }

            return (h, v, d);
        }
    }
}