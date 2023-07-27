using System;
using UnityEngine;

using VRC2.Pipe;

namespace VRC2.Events
{
    public class ClampHintManager: MonoBehaviour
    {
        public GameObject hint;

        private bool positioned = false;

        // whether to show it, the right part of the connected pipe will be set to false
        [HideInInspector] public bool CanShow = true;
        
        // show it only when it's on the wall
        [HideInInspector] public bool OnTheWall = false;
        
        private void Start()
        {
            Show();
        }

        public void Show()
        {
            // show only when CanShow is true
            if (!CanShow) return;
            
            // show this only when pipe is on the wall
            if (!OnTheWall) return;
            
            if (!positioned)
            {
                MoveHint();
            }
            hint.SetActive(true);
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
        }
    }
}