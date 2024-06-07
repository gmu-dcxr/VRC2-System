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

        [Space(30)] [Header("Text Hint")] public TextMesh textMesh;

        private string enterText = "Press X to Enter";
        private string exitText = "Press X to Exit";

        private bool entered = false;

        public bool Entered => entered;

        // set tempParent to be the parent of camera rig and the player,
        // and set it to be the child of targetTransform
        private GameObject tempParent = null;

        private void Start()
        {
            textMesh.text = "";
        }

        void EnterLift()
        {

            if (tempParent == null)
            {
                tempParent = new GameObject();
            }

            // reset position and rotation
            tempParent.transform.position = Vector3.zero;
            tempParent.transform.rotation = Quaternion.identity;

            // set parent
            if (player != null)
            {
                player.transform.parent = tempParent.transform;
            }

            cameraRig.parent = tempParent.transform;

            // set parent
            tempParent.transform.parent = targetTransform;

            entered = true;
        }

        void ExitLift()
        {
            // get y offset
            var y = tempParent.transform.position.y;
            tempParent.transform.parent = null;

            // unparent
            cameraRig.transform.parent = null;

            // update y
            var pos = cameraRig.transform.position;
            pos.y -= y;
            cameraRig.transform.position = pos;

            if (player != null)
            {
                player.transform.parent = null;

                pos = player.transform.position;
                pos.y -= y;

                player.transform.position = pos;
            }

            entered = false;
        }

        // force player can not move in lift
        private void Update()
        {
            var p1 = centerEyeAnchor.transform.position;
            var p2 = targetTransform.position;

            p1.y = 0;
            p2.y = 0;

            // check button event
            var keyX = OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch);

            // text always shows up when entered
            if (entered)
            {
                textMesh.text = exitText;
                if (keyX)
                {
                    // exit
                    ExitLift();
                }
            }
            else
            {
                // show text only if distance threshold is satisfied
                if (Vector3.Distance(p1, p2) < distanceThreshold)
                {
                    textMesh.text = enterText;
                    if (keyX)
                    {
                        // enter
                        EnterLift();
                    }
                }
                else
                {
                    textMesh.text = "";
                }
            }
        }
    }
}