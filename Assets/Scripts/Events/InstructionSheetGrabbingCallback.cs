using System;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUITable;
using VRC2.Task;

namespace VRC2.Events
{
    public class InstructionSheetGrabbingCallback : MonoBehaviour
    {
        public GameObject dialog;

        // public TextMeshProUGUI titleUI;
        // public TextMeshProUGUI contentUI;
        public float distance;
        public float yOffset;

        [HideInInspector] public string title = "Instruction";
        [HideInInspector] public string content = "This is instruction";


        [Header("Table")] public Table srcTable;
        public Table localTable;

        [Space(30)] [Header("Rule")] public Text srcRule;
        public Text localRule;

        [Space(30)] [Header("Layout")] public ImageAsTexture srcIAT;
        public ImageAsTexture localIAT;



        private PointableUnityEventWrapper _wrapper;

        // private Transform _cameraTransform;

        private OVRCameraRig _cameraRig;

        private void Start()
        {

            _cameraRig = FindObjectOfType<OVRCameraRig>();
            // _cameraTransform = Camera.main.transform;

            _wrapper = gameObject.GetComponent<PointableUnityEventWrapper>();

            _wrapper.WhenSelect.AddListener(OnSelect);
            _wrapper.WhenRelease.AddListener(OnRelease);

            // dialog.SetActive(false);
        }

        void OnSelect()
        {
            print("On select");

            dialog.SetActive(true);

            // sync
            SyncAttributes();

            // titleUI.text = title;
            // contentUI.text = content;
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
            var t = _cameraRig.centerEyeAnchor;
            var rotation = Quaternion.LookRotation(t.forward, t.up);

            var rot = rotation.eulerAngles;
            rot.x += 25f;
            
            dialog.transform.rotation = Quaternion.Euler(rot);
            dialog.transform.position = t.position + t.forward * distance + t.up * yOffset;
        }

        void SyncAttributes()
        {
            // table
            localTable.targetCollection.target = srcTable.targetCollection.target;
            localTable.targetCollection.componentName = srcTable.targetCollection.componentName;
            localTable.targetCollection.memberName = srcTable.targetCollection.memberName;

            localTable.Initialize();

            // rule
            localRule.text = srcRule.text;

            // layout
            var texture = srcIAT.GetTexture();
            localIAT.SetTexture(texture);
        }

        public void UpdateInstruction(string folder, string filename)
        {
            srcIAT.UpdateFolderFilename(folder, filename);
            localIAT.UpdateFolderFilename(folder, filename);
        }

        public (string, string) BackupInstruction()
        {
            return (srcIAT.folder, srcIAT.name);
        }
    }
}