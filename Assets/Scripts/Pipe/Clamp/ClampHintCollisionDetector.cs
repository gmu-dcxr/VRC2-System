using System;
using UnityEngine;
using VRC2.Events;

namespace VRC2.Pipe
{
    public class ClampHintCollisionDetector : MonoBehaviour
    {
        private ClampHintManager _hintManager;

        private void Start()
        {
            _hintManager = gameObject.GetComponentInParent<ClampHintManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.wallTag))
            {
                // pipe leaves the wall
                var root = PipeHelper.GetRoot(gameObject);

                // add rigid body etc.
                PipeHelper.AfterMove(ref root);

                var children = Utils.GetChildren<ClampHintManager>(root);

                foreach (var child in children)
                {
                    var chm = child.GetComponent<ClampHintManager>();
                    chm.OnTheWall = false;
                    chm.Clamped = false;
                }
            }
            else if (go.CompareTag(GlobalConstants.clampObjectTag) && CheckClampSizeMatch(go))
            {
                _hintManager.Clamped = false;
            }
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var go = other.gameObject;
            print($"OnTriggerEnterAndStay: {go.name} - {go.tag} - {_hintManager.OnTheWall}");
            if (go.CompareTag(GlobalConstants.clampObjectTag) && CheckClampSizeMatch(go) && _hintManager.OnTheWall)
            {
                print($"{CheckClampSizeMatch(go)}");
                _hintManager.Clamped = true;
            }
        }

        GameObject GetPipeRoot()
        {
            // clamp hint - 1 45 1 - 1 inch 45 deg pipe
            var t = gameObject.transform;
            var go = t.parent.parent.gameObject;
            return go;
        }

        bool CheckClampSizeMatch(GameObject clamp)
        {
            var csi = clamp.GetComponent<ClampScaleInitializer>();
            var clampSize = $"{csi.clampSize}";

            // get pipe size
            var root = GetPipeRoot();

            var pm = root.GetComponent<PipeManipulation>();
            var size = pm.diameter;

            var name = Utils.GetDisplayName<PipeConstants.PipeDiameter>(size);

            // Diameter_1 matches clamp size 1
            return name.EndsWith(clampSize);
        }
    }
}