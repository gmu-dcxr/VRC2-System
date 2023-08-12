using System;
using System.Collections;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;

namespace VRC2.Scenarios
{
    public class ExcavatorController : MonoBehaviour
    {
        public GameObject excavator;

        private BackhoeController _backhoeController;

        private WSMVehicleController _vehicleController;

        private bool extendBoom = false;
        private bool extendArm = false;
        private bool extendRearBucket = false;

        private bool retractBoom = false;
        private bool retractArm = false;
        private bool retractRearBucket = false;

        private bool rotateBoom = false;
        private bool centerBoom = false;

        private bool lowerStabilizers = false;

        private bool moving = false;


        private void Start()
        {
            moving = false;

            _backhoeController = excavator.GetComponent<BackhoeController>();
            _vehicleController = excavator.GetComponent<WSMVehicleController>();
        }

        private void Update()
        {
            if (lowerStabilizers)
            {
                _backhoeController.MoveStabilizerLegs(-1);
            }

            // if (moving)
            // {
            //     StartVehicle();
            //     MoveBackward();
            // }
            // else
            // {
            //     StopVehicle();
            // }

            if (extendBoom)
            {
                ExtendBoom();
            }

            if (extendArm)
            {
                ExtendArm();
            }

            if (extendRearBucket)
            {
                ExtendRearBucket();
            }

            if (retractBoom)
            {
                RetractBoom();
            }

            if (retractArm)
            {
                RetractArm();
            }

            if (retractRearBucket)
            {
                RetractRearBucket();
            }

            if (rotateBoom)
            {
                RotateBoom();
            }

            if (centerBoom)
            {
                CenterBoom();
            }


        }

        void ResetStatus()
        {
            print("Reset status");
            extendBoom = false;
            extendArm = false;
            extendRearBucket = false;
            retractBoom = false;
            retractArm = false;
            retractRearBucket = false;
            rotateBoom = false;
            centerBoom = false;
            // lowerStabilizers = false;
            moving = false;
            
            StopAllCoroutines();
            StartCoroutine(ExcavatorDig(false));
        }

        IEnumerator ExcavatorDig(bool stablize)
        {
            if (stablize)
            {
                lowerStabilizers = true;
                yield return new WaitForSeconds(2f);   
            }
            extendArm = true;
            yield return new WaitForSeconds(3f);
            extendArm = false;
            extendRearBucket = true;
            extendBoom = true;
            yield return new WaitForSeconds(2f);
            extendRearBucket = false;
            extendBoom = false;
            retractArm = true;
            yield return new WaitForSeconds(3f);
            retractArm = false;
            retractBoom = true;
            retractRearBucket = true;
            yield return new WaitForSeconds(2f);
            retractBoom = false;
            retractRearBucket = false;
            yield return new WaitForSeconds(2f);
            rotateBoom = true;
            yield return new WaitForSeconds(2f);
            rotateBoom = false;
            yield return new WaitForSeconds(2f);
            extendArm = true;
            yield return new WaitForSeconds(2f);
            extendArm = false;
            extendBoom = true;
            yield return new WaitForSeconds(1f);
            extendBoom = false;
            extendRearBucket = true;
            yield return new WaitForSeconds(3f);
            extendRearBucket = false;
            retractArm = true;
            retractBoom = true;
            retractRearBucket = true;
            yield return new WaitForSeconds(3f);
            retractArm = false;
            retractBoom = false;
            retractRearBucket = false;
            centerBoom = true;
            yield return new WaitForSeconds(2f);
            centerBoom = false;
            
            ResetStatus();
            yield break;
        }

        #region Excavator Control

        void ExtendBoom()
        {
            _backhoeController.MoveBoom(1);
        }

        void RetractBoom()
        {
            _backhoeController.MoveBoom(-1);
        }

        void ExtendArm()
        {
            _backhoeController.MoveArm(1);
        }

        void RetractArm()
        {
            _backhoeController.MoveArm(-1);
        }

        void ExtendRearBucket()
        {
            _backhoeController.MoveRearBucket(1);
        }

        void RetractRearBucket()
        {
            _backhoeController.MoveRearBucket(-1);
        }

        void RotateBoom()
        {
            _backhoeController.MoveSwingFrame(1);
        }

        void CenterBoom()
        {
            _backhoeController.MoveSwingFrame(-1);
        }


        #endregion

        public void Animate()
        {
            StartCoroutine(ExcavatorDig(true));
        }
    }
}