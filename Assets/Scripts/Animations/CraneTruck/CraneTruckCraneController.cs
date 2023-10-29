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
        [Header("Controller")] public Manipulator manipulator;
        public HookManip hookManip;
        public CraneTruckInputRecording recording;
        public CargoManipulator cargoManipulator;
        
        [Space(30)]
        [Header("Reference")] public Transform manipArrowRotation;
        public Transform manipArrow0;
        public Transform pointDistanceA;
        public Transform pointCargo;

        [Space(30)] [Header("Cargo")] public GameObject cargo;

        [Space(30)] [Header("Threshold")] public float cargoHookPickup = 0.75f; // for pickup
        public float armUpThreshold = 0.34f;

        [HideInInspector] public float hookDistanceInit; // init hook
        public float hookDistanceDropoff; // hook distance for dropoff 


        private CraneStatus status;

        private bool cargoSeized = false;

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

        void Start()
        {
            // initialize with crane mode
            recording.truckMode = false;
        }

        private void Update()
        {
            var lrr = LeftRightRotation;
            var udr = UpDownRotation;
            var al = ArmLength;
            var hd = HookDistance;
            var hcd = HookCargoDistance;

            print($"lr: {lrr} ud: {udr} arm: {al} hook: {hd} hook-cargo: {hcd}");

            switch (status)
            {
                case CraneStatus.Idle:
                    break;
                
                case CraneStatus.PrepareSeize:
                    if (PrepareSeize())
                    {
                        status = CraneStatus.SeizeCargo;
                        cargoSeized = false;
                    }
                    break;
                case CraneStatus.SeizeCargo:
                    if (!cargoSeized)
                    {
                        SeizeCargo();
                    }
                    break;
                
                default:
                    break;
            }
            
        }

        #region Crane control

        bool PrepareSeize()
        {
            if (HookCargoDistance < cargoHookPickup) return true;
            
            hookManip.DownHook();
            return false;
        }

        void SeizeCargo()
        {
            cargoManipulator.SeizeCargo(true);
            cargoSeized = true;
        }

        

        #endregion

        #region Debug

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Crane"))
            {
                status = CraneStatus.PrepareSeize;
            }
        }

        #endregion
    }
}