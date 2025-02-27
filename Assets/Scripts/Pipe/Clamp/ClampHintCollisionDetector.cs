﻿using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VRC2.Events;
using VRC2.Hack;
using VRC2.Pipe.Clamp;

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

        // get root everytime as connecting pipe will change it
        private GameObject root => PipeHelper.GetRoot(gameObject);

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

        private PipesContainerManager pcm => root.GetComponent<PipesContainerManager>();

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
            if (go.CompareTag(GlobalConstants.clampObjectTag) && hintManager.Clamped)
            {
                hintManager.SetClamped(false);
                // check
                CheckPipeInteractable();
            }
            else if (go.CompareTag(GlobalConstants.wallTag))
            {
                hintManager.SetOnTheWall(false);
            }
        }

        void UpdateP2Clamp(GameObject go, bool kinematic)
        {
            var clamp = go.transform.parent;
            var runner = clamp.GetComponent<NetworkObject>().Runner;
            if (runner != null && runner.IsClient)
            {
                // p2 side
                var rb = clamp.GetComponent<Rigidbody>();
                rb.isKinematic = kinematic;
            }
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.clampObjectTag) && CheckClampSizeMatch(go) && !hintManager.Clamped)
            {
                var csm = go.GetComponent<ClampStatusMonitor>();
                var cgc = go.transform.parent.GetComponent<ClampGrabbingCallback>();
                var runner = cgc.gameObject.GetComponent<NetworkObject>().Runner;
                if (runner != null && runner.IsServer && !cgc.selected && csm.InUse)
                {
                    // do nothing if clamp is used
                    return;
                }

                hintManager.SetClamped(true);
                UpdateP2Clamp(go, true);
                // disable pipe interaction, set to not held
                UpdatePipeInteraction(false, false);
                // update status
                csm.InUse = true;
            }
            else if (go.CompareTag(GlobalConstants.wallTag))
            {
                hintManager.SetOnTheWall(true);
            }
        }

        void UpdatePipeInteraction(bool enable, bool held)
        {
            if (pm != null)
            {
                pm.SetInteraction(enable);
                pm.SetHeldByController(held);
            }
            else if (pcm != null)
            {
                pcm.SetInteraction(enable);
                pcm.SetHeldByController(held);
            }
        }

        void CheckPipeInteractable()
        {
            // check whether to fall
            if (pm != null)
            {
                pm.CheckAfterUnclamp();
            }
            else if (pcm != null)
            {
                pcm.CheckAfterUnclamp();
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