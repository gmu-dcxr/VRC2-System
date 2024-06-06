using System;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Events;
using VRC2.Hack;

namespace VRC2.Pipe
{
    public class ClampHintCollisionDetector : MonoBehaviour
    {
        private ClampHintManager _hintManager;

        private ClampHintManager hintManager
        {
            get
            {
                if (_hintManager == null)
                {
                    _hintManager = gameObject.GetComponentInParent<ClampHintManager>();
                }

                return _hintManager;
            }
        }

        private PipeGrabFreeTransformer _transformer;

        private GameObject _root;

        private GameObject root
        {
            get
            {
                if (_root == null)
                {
                    _root = PipeHelper.GetRoot(gameObject);
                }

                return _root;
            }
        }

        private PipeGrabFreeTransformer transformer
        {
            get
            {
                if (_transformer == null)
                {
                    _transformer = root.GetComponent<PipeGrabFreeTransformer>();
                }

                return _transformer;
            }
        }

        private PipeManipulation pm => root.GetComponent<PipeManipulation>();

        private bool requested = false;

        private void Start()
        {
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
            // update Clamped flag 
            if (go.CompareTag(GlobalConstants.clampObjectTag))
            {
                hintManager.Clamped = false;
                requested = false;
                // check
                CheckPipeInteractable();
            }
            else if (go.CompareTag(GlobalConstants.wallTag))
            {
                hintManager.OnTheWall = false;
            }
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.clampObjectTag) && CheckClampSizeMatch(go))
            {
                hintManager.Clamped = true;
                // disable pipe interaction, set to not held
                UpdatePipeInteraction(false, false);
                // request update
                UpdatePipeContainer();
            }
            else if (go.CompareTag(GlobalConstants.wallTag))
            {
                hintManager.OnTheWall = true;
            }
        }

        void UpdatePipeContainer()
        {
            var pcm = root.GetComponent<PipesContainerManager>();
            if (!requested && pcm != null && pcm.collidingWall && !pcm.heldByController)
            {
                requested = true;
                pcm.selfCompensated = false;
                pcm.SelfCompensate();
            }
        }

        void UpdatePipeInteraction(bool enable, bool held)
        {
            if (pm != null)
            {
                pm.SetInteraction(enable);
                pm.SetHeldByController(held);
            }
        }

        void CheckPipeInteractable()
        {
            // check whether to fall
            if (pm != null)
            {
                pm.CheckAfterUnclamp();
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