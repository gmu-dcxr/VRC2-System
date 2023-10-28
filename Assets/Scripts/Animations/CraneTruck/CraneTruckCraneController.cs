using System;
using UnityEngine;

namespace VRC2.Animations.CraneTruck
{
    public class CraneTruckCraneController: MonoBehaviour
    {
        [Header("Reference")] public Transform manipArrowRotation;
        public Transform manipArrow0;
        public Transform pointDistanceA;
        public Transform pointCargo;


        #region Derived properties

        public float LeftRightRotation
        {
            get => manipArrowRotation.localRotation.y;
        }

        public float UpDownRotation
        {
            get => manipArrow0.localRotation.x;
        }

        public float ArmLength
        {
            get => Vector3.Distance(pointDistanceA.position, manipArrow0.position);
        }

        public float HookDistance
        {
            get => Vector3.Distance(pointCargo.position, pointDistanceA.position);
        }

        #endregion

        private void Update()
        {
            var lrr = LeftRightRotation;
            var udr = UpDownRotation;
            var al = ArmLength;
            var hd = HookDistance;
            
            print($"lr: {lrr} ud: {udr} arm: {al} hook: {hd}");
        }
    }
}