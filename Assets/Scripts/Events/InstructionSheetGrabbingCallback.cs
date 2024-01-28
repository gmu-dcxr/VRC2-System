using System;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

namespace VRC2.Events
{
    public class InstructionSheetGrabbingCallback : MonoBehaviour
    {
        public GameObject dialog;
        public TextMeshProUGUI titleUI;
        public TextMeshProUGUI contentUI;
        public float distance;

        public string title = "Instruction";
        public string content = "This is instruction";

        private PointableUnityEventWrapper _wrapper;

        private Transform _cameraTransform;

        private void Start()
        {
            _cameraTransform = Camera.main.transform;

            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();

            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenRelease.AddListener(OnRelease);

            // dialog.SetActive(false);
        }

        void OnSelect()
        {
            print("On select");

            dialog.SetActive(true);

            titleUI.text = title;
            contentUI.text = content;
        }

        void OnRelease()
        {
            print("On release");

            dialog.SetActive(false);
        }

        private void Update()
        {
            if (dialog.activeSelf)
            {
                MoveDialogFaceHeadset();
            }
        }

        void MoveDialogFaceHeadset()
        {
            var forward = _cameraTransform.forward;

            dialog.transform.rotation = _cameraTransform.rotation;
            dialog.transform.position = _cameraTransform.position + forward * distance;
        }
    }
}