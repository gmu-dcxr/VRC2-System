using UnityEngine;

namespace VRC2.Authority
{
    public class GravityHook : AuthorityHook
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
            rigidbody.useGravity = false;
        }

        public override void DisableP2()
        {
            rigidbody.useGravity = false;
        }
    }
}