﻿using System;
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


        private IDictionary<int, float> _clampExtendsZ;

        public float GetClampExtendsZ()
        {
            if (_clampExtendsZ == null)
            {
                _clampExtendsZ = new Dictionary<int, float>();

                _clampExtendsZ.Add(1, 0.00836f);
                _clampExtendsZ.Add(2, 0.02786f);
                _clampExtendsZ.Add(3, 0.03901f);
                _clampExtendsZ.Add(4, 0.05015f);
            }

            // BUG: Dynamically getting the bounds doesn't work in VR, use the predefined size instead.

            var size = gameObject.GetComponentInChildren<ClampScaleInitializer>().clampSize;

            return _clampExtendsZ[size];
        }

        public void SetKinematic(bool value)
        {
            rigidbody.isKinematic = value;
        }
    }
}