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

        [Header("Label")] [SerializeField] private TextMeshPro _label;

        // measure distance only when two controllers' select button (trigger) are pressed.

        // Start is called before the first frame update
        void Start()
        {
            // hide it 
            _label.text = "";
        }

        // Update is called once per frame
        void Update()
        {
            var l = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
            var r = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);

            if (l && r)
            {
                var d = GetTouchPointsDistance();
                if (d >= 0)
                {
                    // invalid
                    _label.text = $"Distance: {d.ToString("f2")}";
                    return;
                }
            }

            _label.text = "";
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

        float GetTouchPointsDistance()
        {
            Vector3 leftValue;
            var left = GetTouchPoint(this.left, out leftValue);

            Vector3 rightValue;
            var right = GetTouchPoint(this.right, out rightValue);

            if (left && right)
            {
                return Vector3.Distance(leftValue, rightValue);
            }

            return -1;
        }
    }
}
