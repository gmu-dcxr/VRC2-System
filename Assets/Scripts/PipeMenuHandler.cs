using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Events;

namespace VRC2
{
    public class PipeMenuHandler : MonoBehaviour
    {
        [Header("Dialog Window")] [SerializeField]
        private ModalDialog modalDialog;

        // current event 
        private string _currentEvent = PipeInstallEvent.EmptyEvent;

        private void Start()
        {
            // disable modal dialog first
            modalDialog.show(false);

            // add event listener
            modalDialog.button1Events.WhenRelease.AddListener(() => { DialogButton1Clicked(); });
            modalDialog.button2Events.WhenRelease.AddListener(() => { DialogButton2Clicked(); });
        }

        public void OnPickAPipe()
        {
            // hide self
            Debug.Log("You clicked Pick A pipe");
            // current event
            _currentEvent = PipeInstallEvent.P1PickUpPipe;

            // set dialog window
            modalDialog.UpdateDialog("Tip", "Pick up a pipe", "OK", "Cancel");
            modalDialog.show(true);
        }

        public void DialogButton1Clicked()
        {
            Debug.Log("DialogButton1Clicked");
            EventHandler(_currentEvent);
            modalDialog.show(false);
        }

        public void DialogButton2Clicked()
        {
            Debug.Log("DialogButton2Clicked");
            modalDialog.show(false);
        }

        void EventHandler(string eventType)
        {
            if (eventType == PipeInstallEvent.P1PickUpPipe)
            {
                // initialize pipe
                var ev = gameObject.GetComponent<P1PickUpPipeEvent>();
                ev.Execute();
            }
        }
    }
}