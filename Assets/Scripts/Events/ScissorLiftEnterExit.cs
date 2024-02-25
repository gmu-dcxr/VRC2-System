using System;
using UnityEngine;

namespace VRC2.Events
{
    public class ScissorLiftEnterExit : MonoBehaviour
    {
        private PlayerHelper _playerHelper;

        private PlayerHelper playerHelper
        {
            get
            {
                if (_playerHelper == null)
                {
                    _playerHelper = FindFirstObjectByType<PlayerHelper>();
                }

                return _playerHelper;
            }
        }

        private GameObject player
        {
            get => playerHelper.localPlayer;
        }
        public GameObject centerEyeAnchor;

        [Space(30)] [Header("Camera")] public Transform cameraRig;

        [Space(30)] [Header("Settings")] public Transform targetTransform;
        public float distanceThreshold = 0.5f;
        

        void EnterLift()
        {
            // re-find local player
            playerHelper.ResetLocalPlayer();
            
            player.transform.parent = targetTransform;
            // update position
            var p = Vector3.zero;
            p.y = targetTransform.position.y;
            player.transform.position = p;

            cameraRig.parent = targetTransform;
            cameraRig.transform.position = p;
        }

        void ExitLift()
        {
            player.transform.parent = null;
            // update position
            player.transform.position = Vector3.zero;

            cameraRig.transform.parent = null;
            cameraRig.transform.position = Vector3.zero;
        }

        // force player can not move in lift
        private void Update()
        {
            // check button event
            var keyX = OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch);

            var p = player.transform;

            var p1 = centerEyeAnchor.transform.position;
            var p2 = targetTransform.position;

            p1.y = 0;
            p2.y = 0;

            if ((Input.GetKeyUp(KeyCode.E) || keyX) && Vector3.Distance(p1, p2) < distanceThreshold)
            {
                if (p.parent == null)
                {
                    // enter
                    EnterLift();
                }
                else
                {
                    // exit
                    ExitLift();
                }
            }
        }
    }
}