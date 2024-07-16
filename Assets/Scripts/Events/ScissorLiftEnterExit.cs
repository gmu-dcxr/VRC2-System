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

        [Tooltip("Fine-tune the height to make feet stick to the base of lift")]
        public float yOffset = 0.6f;

        [Space(30)] [Header("Text Hint")] public TextMesh textMesh;

        private string enterText = "Press X to Enter";
        private string exitText = "Press X to Exit";

        private bool entered = false;

        public bool Entered => entered;

        public System.Action OnExitLift;
        [HideInInspector] public bool onResetting = false;


        // refactor
        private Vector3 enterAnchor;
        private Vector3 enterPosition;

        private AvatarLocator _avatarLocator
        {
            get
            {
                if (_player != null)
                {
                    return _player.GetComponent<AvatarLocator>();
                }

                return null;
            }
        }

        private Transform _foot
        {
            get
            {
                if (_avatarLocator != null)
                {
                    return _avatarLocator.leftFootBall;
                }

                return null;
            }
        }

        private GameObject _cam => playerHelper.CameraRig;
        private GameObject _player => playerHelper.localPlayer;

        public ScissorLiftController controller { get; set; }

        private void Start()
        {
            textMesh.text = "";
        }

        void EnterLift()
        {
            enterAnchor = anchor.position;
            enterPosition = _cam.transform.position;
            entered = true;
            onResetting = false;
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

            if (OnExitLift != null)
            {
                OnExitLift();
                onResetting = true;
            }
        }

        private void LateUpdate()
        {
            // return if controller.IsClient do nothing on resetting
            if (controller.IsClient || onResetting)
            {
                // clear text
                textMesh.text = "";
                return;
            }

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
            if (controller.IsClient || !entered) return;

            var position = _cam.transform.position;

            // up-down
            position.y = enterPosition.y + yOffset + anchor.position.y - enterAnchor.y;
            // left-right
            position.z = enterPosition.z + anchor.position.z - enterAnchor.z;

            _cam.transform.position = position;

            if (_player != null)
            {
                _player.transform.position = position;
            }
        }
    }
}