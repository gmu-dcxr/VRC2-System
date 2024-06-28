using System;
using UnityEngine;
using VRC2.ScenariosV2.Tool;

namespace VRC2.Character
{
    public class AvatarLocator : MonoBehaviour
    {
        public Transform viewPoint;
        public Transform leftFootBall;
        [ReadOnly] public Vector3 Offset;
        [ReadOnly] public float Height;

        private void Start()
        {
            Offset = viewPoint.position - leftFootBall.position;
            
            Offset.x = 0;
            Offset.z = 0;
            
            Height = Vector3.Distance(viewPoint.position, leftFootBall.position);
        }
    }
}