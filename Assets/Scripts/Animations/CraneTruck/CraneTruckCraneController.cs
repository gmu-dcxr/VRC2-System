using System;
using UnityEngine;

namespace VRC2.Animations.CraneTruck
{
    enum CraneStatus
    {
        Idle,
        PrepareSeize,
        SeizeCargo,
        UpArm,
        ExtendArm,
        RotateArm,
        DownHook,
        ReleaseCargo,
        Reset
    }

    public class CraneTruckCraneController : MonoBehaviour
    {
        [Header("Reference")] public Transform manipArrowRotation;
        public Transform manipArrow0;
        public Transform pointDistanceA;
        public Transform pointCargo;

        [Space(30)] [Header("Cargo")] public GameObject cargo;

        [Space(30)] [Header("Threshold")] public float cargoHookPickup; // for pickup

        [HideInInspector] public float hookDistanceInit; // init hook
        public float hookDistanceDropoff; // hook distance for dropoff 


        private CraneStatus _status;

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

        public float HookCargoDistance
        {
            get => Vector3.Distance(pointCargo.position, cargo.transform.position);
        }

        #endregion

        private void Update()
        {
            var lrr = LeftRightRotation;
            var udr = UpDownRotation;
            var al = ArmLength;
            var hd = HookDistance;
            var hcd = HookCargoDistance;

            print($"lr: {lrr} ud: {udr} arm: {al} hook: {hd} hook-cargo: {hcd}");
        }
    }
}