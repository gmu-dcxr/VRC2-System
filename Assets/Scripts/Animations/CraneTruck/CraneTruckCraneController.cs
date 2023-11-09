using System;
using UnityEngine;

namespace VRC2.Animations.CraneTruck
{
    [HideInInspector] public enum CraneStatus
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

        [Space(30)] [Header("Reference")] public Transform manipArrowRotation;
        public Transform manipArrow0;
        public Transform pointDistanceA;
        public Transform pointCargo;

        [Space(30)] [Header("Cargo")] public GameObject cargo;

        [Space(30)] [Header("Threshold")]
        public float cargoHookPickup = 0.75f; // maximum distance between hook and cargo for pickup

        public float armUpThreshold = 0.34f; // arm up maximum

        [HideInInspector] public float armForwardInit; // arm length initial value
        public float armForwardThreshold = 8.0f; // arm maximum length
        public float armRotationThreshold = 0.33f; // arm rotation maximum (left, right) 

        [HideInInspector] public float hookDistanceInit; // init hook for resetting
        public float hookDistanceDropoff = 6.5f; // maximum distance between hook and cargo for dropoff 


        [HideInInspector] public CraneStatus status;

        private bool cargoSeized = false;

        #region Reset flags

        private bool hookReset = false; // hook up / down
        private bool armLengthReset = false; // extend / shrink
        private bool armRotationUpReset = false; // up / down
        private bool armRotationLeftReset = false; // left / right



        #endregion

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

            armForwardInit = ArmLength;
            hookDistanceInit = HookDistance;
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

                    // clear reset status
                    hookReset = false;
                    armLengthReset = false;
                    armRotationLeftReset = false;
                    armRotationUpReset = false;

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
                    else
                    {
                        status = CraneStatus.UpArm;
                    }

                    break;

                case CraneStatus.UpArm:
                    if (UpArm())
                    {
                        status = CraneStatus.ExtendArm;
                    }

                    break;

                case CraneStatus.ExtendArm:
                    if (ExtendArm())
                    {
                        status = CraneStatus.RotateArm;
                    }

                    break;

                case CraneStatus.RotateArm:
                    // tilt may occur here
                    if (RotateArm())
                    {
                        status = CraneStatus.DownHook;
                    }

                    break;

                case CraneStatus.DownHook:
                    if (DownHook())
                    {
                        status = CraneStatus.ReleaseCargo;
                    }

                    break;

                case CraneStatus.ReleaseCargo:
                    if (cargoSeized)
                    {
                        ReleaseCargo();
                    }
                    else
                    {
                        // to reset
                        status = CraneStatus.Reset;
                    }

                    break;

                case CraneStatus.Reset:
                    if (ResetCrane())
                    {
                        print("Reset crane is done");
                        status = CraneStatus.Idle;
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

        bool UpArm()
        {
            if (UpDownRotation > armUpThreshold) return true;

            manipulator.UpArm();

            return false;
        }

        bool ExtendArm()
        {
            if (ArmLength > armForwardThreshold) return true;

            manipulator.ExtendArm();
            return false;
        }

        bool RotateArm()
        {
            if (LeftRightRotation > armRotationThreshold) return true;

            manipulator.LeftArm();
            return false;
        }

        bool DownHook()
        {
            if (HookDistance > hookDistanceDropoff) return true;

            hookManip.DownHook();
            return false;
        }

        void ReleaseCargo()
        {
            cargoManipulator.SeizeCargo(false);
            cargoSeized = false;
        }

        bool ResetCrane()
        {
            var eps = 1e-2;
            // up hook to the initial position
            if (!hookReset)
            {
                if (Math.Abs(HookDistance - hookDistanceInit) > eps)
                {
                    hookManip.UpHook();
                }
                else
                {
                    hookReset = true;
                }

                return false;
            }

            // rotate back
            if (!armRotationLeftReset)
            {


                if (Math.Abs(LeftRightRotation) > eps)
                {
                    manipulator.RightArm();
                }
                else
                {
                    armRotationLeftReset = true;
                }

                return false;
            }

            // shrink arm
            if (!armLengthReset)
            {
                if (Math.Abs(ArmLength - armForwardInit) > eps)
                {
                    manipulator.ShrinkArm();
                }
                else
                {
                    armLengthReset = true;
                }

                return false;
            }

            // down arm
            if (!armRotationUpReset)
            {
                if (Math.Abs(UpDownRotation) > eps)
                {
                    manipulator.DownArm();
                }
                else
                {
                    armRotationUpReset = true;
                }

                return false;
            }

            return true;
        }

        #endregion

        #region Debug

        private void OnGUI()
        {
            if (GUI.Button(new Rect(1000, 10, 100, 50), "Crane"))
            {
                status = CraneStatus.PrepareSeize;
            }
        }

        #endregion
    }
}