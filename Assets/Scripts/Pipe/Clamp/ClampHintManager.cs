using System;
using UnityEngine;

using VRC2.Pipe;

namespace VRC2.Events
{
    public class ClampHintManager: MonoBehaviour
    {
        public GameObject hint;

        private bool positioned = false;
        private void Start()
        {
            Show();
        }

        public void Show()
        {
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