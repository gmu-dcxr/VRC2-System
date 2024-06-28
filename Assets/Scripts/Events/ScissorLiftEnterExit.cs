using System;
using UnityEngine;
using VRC2.Character;

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

        public GameObject centerEyeAnchor;

        [Space(30)] [Header("Settings")] public Transform anchor;
        public float distanceThreshold = 0.5f;

        [Space(30)] [Header("Text Hint")] public TextMesh textMesh;

        private string enterText = "Press X to Enter";
        private string exitText = "Press X to Exit";

        private bool entered = false;

        public bool Entered => entered;


        // refactor
        private Vector3 enterAnchor;
        private Vector3 enterPosition;

        private Vector3 _enterFoot
        {
            get
            {
                if (_player != null)
                {
                    return _player.GetComponent<AvatarLocator>().leftFootBall.position;
                }

                return Vector3.zero;
            }
        }

        private Vector3 enterFoot;

        private GameObject _cam => playerHelper.CameraRig;
        private GameObject _player => playerHelper.localPlayer;

        private void Start()
        {
            textMesh.text = "";
        }

        void EnterLift()
        {
            enterAnchor = anchor.position;
            enterPosition = _cam.transform.position;
            enterFoot = _enterFoot;
            entered = true;
        }

        void ExitLift()
        {
            entered = false;
            // update y only
            var position = _cam.transform.position;
            position.y = enterPosition.y;
            _cam.transform.position = position;
            if (_player != null)
            {
                _player.transform.position = position;
            }
        }

        private void LateUpdate()
        {
            var p1 = centerEyeAnchor.transform.position;
            var p2 = anchor.position;

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

        // force player can not move in lift
        private void Update()
        {
            if (!entered) return;

            if (_player != null)
            {
                var yOffset = anchor.position.y - enterFoot.y;

                var position = _cam.transform.position;
                position.x = anchor.position.x;
                position.z = anchor.position.z;

                position.y = enterPosition.y + yOffset;

                _cam.transform.position = position;
                _player.transform.position = position;
            }
            else
            {
                // calculate offset
                var offset = anchor.position - enterAnchor;
                var position = enterPosition + offset;

                _cam.transform.position = position;
            }
        }
    }
}