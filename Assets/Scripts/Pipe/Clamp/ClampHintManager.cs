using System;
using UnityEngine;
using VRC2.Hack;
using VRC2.Pipe;

namespace VRC2.Pipe
{
    public class ClampHintManager: MonoBehaviour
    {
        public GameObject hint;

        private bool positioned = false;

        // whether to show it, the right part of the connected pipe will be set to false
        [HideInInspector]public bool CanShow = true;
        
        // show it only when it's on the wall
        [HideInInspector]public bool OnTheWall = false;

        [HideInInspector]public bool Clamped = false;
        
        private void Start()
        {
            // Show();
            Hide();
        }

        public void Update()
        {
            // show only when CanShow is true and only when pipe is on the wall
            if (!CanShow || !OnTheWall || Clamped)
            {
                if (hint.activeSelf)
                {
                    hint.SetActive(false);
                }
            }
            else
            {
                if (!positioned)
                {
                    MoveHint();
                }

                if (!hint.activeSelf)
                {
                    hint.SetActive(true);   
                }
            }

            if (OnTheWall && hint.activeSelf)
            {
                // check whether to fall
                var root = PipeHelper.GetRoot(gameObject);
                // request check fallable
                if (root.GetComponent<PipeGrabFreeTransformer>().isSimplePipe)
                {
                    print("PipeManipulation RequestCheckingFallable");
                    // pipe manipulation
                    root.GetComponent<PipeManipulation>().RequestCheckingFallable();
                }
                else
                {
                    print("PipesContainerManager RequestCheckingFallable");
                    root.GetComponent<PipesContainerManager>().RequestCheckingFallable();
                }   
            }
        }

        public void Hide()
        {
            hint.SetActive(false);
        }

        void MoveHint()
        {
            var (cc, cr) = PipeHelper.GetRightMostCenter(gameObject);

            var rot = gameObject.transform.rotation;
            var up = gameObject.transform.up;
            var forward = Vector3.Cross(cr, up);

            rot = Quaternion.LookRotation(forward, up);

            hint.transform.position = cc;
            hint.transform.rotation = rot;

            positioned = true;
        }
    }
}