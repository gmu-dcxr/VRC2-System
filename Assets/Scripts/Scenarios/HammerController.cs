using System;
using UnityEngine;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class HammerController : MonoBehaviour
    {
        public GameObject hammer;

        public Transform fall;

        private Vector3 pos;
        private Quaternion rot;

        private Rigidbody _rigidbody;

        private bool moving = false;
        private bool falling = false;

        private float distanceThreshold = 0.5f;

        private void Start()
        {
            pos = hammer.transform.position;
            rot = hammer.transform.rotation;

            _rigidbody = hammer.GetComponent<Rigidbody>();

            // make it not fall
            _rigidbody.isKinematic = true;

            // use the same y position
            var vec = fall.transform.position;
            vec.y = hammer.transform.position.y;
            fall.transform.position = vec;
        }

        private void Update()
        {
            if (!moving) return;

            if (HammerOnGround())
            {
                // reset
                ResetStatus();
            }

            if (!falling)
            {
                if (ReachFall())
                {
                    falling = true;
                    _rigidbody.isKinematic = false;
                }
                else
                {
                    var direction = (fall.transform.position - hammer.transform.position).normalized;
                    hammer.transform.position += direction * Time.deltaTime;
                }
            }
        }

        bool HammerOnGround()
        {
            return falling && !_rigidbody.isKinematic && _rigidbody.velocity.magnitude <= 0;
        }

        bool ReachFall()
        {
            var d = Vector3.Distance(hammer.transform.position, fall.transform.position);

            return d < distanceThreshold;

        }

        void ResetStatus()
        {
            hammer.transform.position = pos;
            hammer.transform.rotation = rot;

            _rigidbody.isKinematic = true;

            moving = true;
            falling = false;
        }

        public void Animate()
        {
            moving = true;
            falling = false;
        }
    }
}