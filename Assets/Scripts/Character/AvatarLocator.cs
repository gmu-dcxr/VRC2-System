using System;
using UnityEngine;
using VRC2.ScenariosV2.Tool;

namespace VRC2.Character
{
    public class AvatarLocator : MonoBehaviour
    {
        public Transform viewPoint;
        public Transform leftFootBall;
        [ReadOnly] public float initHeight;

        private void Start()
        {
            initHeight = GetHeight();
        }

        public float GetHeight()
        {
            return viewPoint.position.y - leftFootBall.position.y;
        }
    }
}