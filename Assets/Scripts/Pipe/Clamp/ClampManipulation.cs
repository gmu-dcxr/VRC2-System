using System;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Events;

namespace VRC2.Pipe.Clamp
{
    public class ClampManipulation : MonoBehaviour
    {
        [HideInInspector] public bool collidingWall { get; set; }

        private ClampScaleInitializer _clampScaleInitializer;

        private ClampScaleInitializer clampScaleInitializer
        {
            get
            {
                if (_clampScaleInitializer == null)
                {
                    _clampScaleInitializer = GetComponentInChildren<ClampScaleInitializer>();
                }

                return _clampScaleInitializer;
            }
        }

        public int ClampSize => clampScaleInitializer.clampSize;

        private ClampStatusMonitor StatusMonitor
        {
            get
            {
                if (clampScaleInitializer != null)
                {
                    return clampScaleInitializer.gameObject.GetComponent<ClampStatusMonitor>();
                }

                return null;
            }
        }

        private Rigidbody _rigidbody;

        private Rigidbody rigidbody
        {
            get
            {
                if (_rigidbody == null)
                {
                    _rigidbody = gameObject.GetComponent<Rigidbody>();
                }

                return _rigidbody;
            }
        }

        public void SetKinematic(bool value)
        {
            rigidbody.isKinematic = value;
        }

        public void UpdateStatus(bool inuse)
        {
            if (StatusMonitor != null)
            {
                StatusMonitor.InUse = inuse;
            }
        }
    }
}