using UnityEngine;

namespace VRC2.Authority
{
    public class KinematicHook : AuthorityHook
    {
        private Rigidbody _rigidbody;

        [HideInInspector]
        public Rigidbody rigidbody
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

        public override void DisableP1()
        {
            // disable on P1 side, so the object can drop because of gravity
            // and the transform will synchronize to P2 side.
            if (rigidbody == null) return;
            
            rigidbody.isKinematic = false;
        }

        public override void DisableP2()
        {
            if (rigidbody == null) return;
            
            rigidbody.isKinematic = false;
        }

        public override void EnableP2()
        {
            // because default is false, it is needed to enable again
            if (rigidbody == null) return;
            
            rigidbody.isKinematic = true;
        }

        public override void Default()
        {
            // disable it under no-network circumstance
            if (rigidbody == null) return;
            
            rigidbody.isKinematic = false;
        }
    }
}