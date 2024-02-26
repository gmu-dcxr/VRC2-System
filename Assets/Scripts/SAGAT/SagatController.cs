using System;
using UnityEngine;
using VRC2.Record;

namespace VRC2.SAGAT
{
    public class SagatController : MonoBehaviour
    {
        public GameObject canvas;
        public GameObject mainEventSystem;
        public GameObject curEventSystem;

        public GameObject centerEyeAnchor;

        public MicrophoneSelector microphoneSelector;

        private bool mainEventSystemStatus;

        private void Start()
        {
            // hide in the begining
            HideSAGAT();

            microphoneSelector.RequestReturnToVR += RequestReturnToVR;
        }

        private void RequestReturnToVR()
        {
            StopSAGAT();
        }

        #region API

        public void StartSAGAT()
        {
            mainEventSystemStatus = mainEventSystem.activeSelf;

            centerEyeAnchor.SetActive(false);
            canvas.SetActive(true);
            mainEventSystem.SetActive(!mainEventSystemStatus);
            curEventSystem.SetActive(true);
        }

        public void StopSAGAT()
        {
            HideSAGAT();
            mainEventSystem.SetActive(mainEventSystemStatus);
        }

        public void HideSAGAT()
        {
            centerEyeAnchor.SetActive(true);
            canvas.SetActive(false);
            curEventSystem.SetActive(false);
        }

        public void UpdateScenarioText(string s)
        {
            microphoneSelector.SetScenarioText(s);
        }

        public void UpdateQuestions(string clsname)
        {
            microphoneSelector.LoadForClass(clsname);
        }

        #endregion
    }
}