using System;
using UnityEngine;

namespace VRC2.Crane
{
    public class VRJoyStickersController : MonoBehaviour
    {
        private TowerControllerCrane _controllerCrane;

        private GameObject leftJoySticker;
        private GameObject rightJoySticker;

        [Header("Controllers")] public string left = "Tower Cabin_key3";
        public string right = "Tower Cabin_key4";

        private Quaternion leftRot;
        private Quaternion rightRot;
        
        private void Start()
        {
            _controllerCrane = GameObject.FindObjectOfType<TowerControllerCrane>();

            leftJoySticker = GameObject.Find(left);
            rightJoySticker = GameObject.Find(right);

            leftRot = leftJoySticker.transform.localRotation;
            rightRot = rightJoySticker.transform.localRotation;
        }

        void ResetStatus()
        {
            leftJoySticker.transform.localRotation = leftRot;
            rightJoySticker.transform.localRotation = rightRot;
        }

        #region Rotate Controller

        void Forward(GameObject controller)
        {
            controller.transform.localRotation = Quaternion.Euler(0,0, 45);
        }

        void Backward(GameObject controller)
        {
            controller.transform.localRotation = Quaternion.Euler(0,0, -45);
        }

        void Left(GameObject controller)
        {
            controller.transform.localRotation = Quaternion.Euler(-45, 0,0);
        }

        void Right(GameObject controller)
        {
            controller.transform.localRotation = Quaternion.Euler(45, 0, 0);
        }


        #endregion

        private void Update()
        {
            var c = _controllerCrane;
            // left 
            if (c.isForwardBoomCart())
            {
                Forward(leftJoySticker);
            }
            else if (c.isForwardBoomCartUp())
            {
                ResetStatus();
            }
            else if (c.isBackBoomCart())
            {
                Backward(leftJoySticker);
            }
            else if (c.isBackBoomCartUp())
            {
                ResetStatus();
            }
            else if (c.isLeftCrane())
            {
                Left(leftJoySticker);
            }
            else if (c.isLeftCraneUp())
            {
                ResetStatus();
            }
            else if (c.isRightCrane())
            {
                Right(leftJoySticker);
            }
            else if (c.isRightCraneUp())
            {
                ResetStatus();
            }
            // right 
            else if (c.isUpMovingHook())
            {
                Forward(rightJoySticker);
            }
            else if (c.isUpMovingHookUp())
            {
                ResetStatus();
            }
            else if (c.isDownMovingHook())
            {
                Backward(rightJoySticker);
            }
            else if (c.isDownMovingHookUp())
            {
                ResetStatus();
            }
            else if (c.isLeftRotationCargo())
            {
                Left(rightJoySticker);
            }
            else if (c.isLeftRotationCargoUp())
            {
                ResetStatus();
            }
            else if (c.isRightRotationCargo())
            {
                Right(rightJoySticker);
            }
            else if (c.isRightRotationCargoUp())
            {
                ResetStatus();
            }
        }
    }
}