using System;
using UnityEngine;

namespace VRC2.Events
{
    public class ScissorLiftController : MonoBehaviour
    {
        [Header("Speed")] public float upDown = 1.0f;
        public float leftRight = 0.1f;
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        void PlayAnimator(float speed)
        {
            animator.speed = speed;
            animator.Play("move", -1, float.NegativeInfinity);
        }

        void StopAnimator()
        {
            animator.speed = 0;
        }


        void Up(float speed)
        {
            animator.SetFloat("Direction", 1);
            PlayAnimator(speed);
        }

        void Down(float speed)
        {
            animator.SetFloat("Direction", -1);
            PlayAnimator(speed);
        }

        void Left(float speed)
        {
            transform.Translate(0, 0, -speed * Time.deltaTime, Space.Self);
        }

        void Right(float speed)
        {
            transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
        }

        bool ReachTop()
        {
            var frame = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            return frame > 1.0f;
        }

        bool ReachBottom()
        {
            var frame = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            return frame < 0.0f;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (!ReachTop())
                {
                    Up(upDown);
                }
                else
                {
                    StopAnimator();
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (!ReachBottom())
                {
                    Down(upDown);
                }
                else
                {
                    StopAnimator();
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Left(leftRight);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Right(leftRight);
            }
            else
            {
                StopAnimator();
            }
        }
    }
}