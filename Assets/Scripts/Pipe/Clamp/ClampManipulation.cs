using System;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Events;

namespace VRC2.Pipe.Clamp
{
    public class ClampManipulation : MonoBehaviour
    {
        [HideInInspector] public bool collidingWall { get; set; }

        // whether the transform is compensated
        [HideInInspector] public bool compensated { get; set; }

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
    }
}