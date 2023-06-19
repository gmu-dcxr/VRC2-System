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
        private PipeInstallEvent _currentEvent = PipeInstallEvent.EmptyEvent;

        bool IsDialogShowing
        {
            get { return modalDialog.gameObject.activeSelf; }
        }

        private void Start()
        {
            // disable modal dialog first
            modalDialog.show(false);

            // add event listener for dialog window
            modalDialog.button1Events.WhenRelease.AddListener(() => { DialogButton1Clicked(); });
            modalDialog.button2Events.WhenRelease.AddListener(() => { DialogButton2Clicked(); });
        }

        #region Menu Button Events

        public void OnPickAPipe()
        {
            // simulate modal window, ignore the event when the dialog is showing.
            if (IsDialogShowing) return;

            Debug.Log("You clicked Pick A pipe");
            // current event
            _currentEvent = PipeInstallEvent.P1PickUpPipe;

            // set dialog window
            modalDialog.UpdateDialog("Tip", "Pick up a pipe", "OK", "Cancel");
            modalDialog.show(true);
        }

        public void OnCheckPipe()
        {
            if (IsDialogShowing) return;

            Debug.Log("You clicked Check Pipe");

            if (GlobalConstants.Checker)
            {
                _currentEvent = PipeInstallEvent.P2CheckPipe;
                modalDialog.UpdateDialog("Tip", "Are the color and size of the pipe correct?", "Yes", "No");
                modalDialog.show(true);
            }
        }

        #endregion

        #region Dialog Buttons Event

        public void DialogButton1Clicked()
        {
            Debug.Log("DialogButton1Clicked");
            DialogButton1EventHandler(_currentEvent);
            modalDialog.show(false);
        }

        public void DialogButton2Clicked()
        {
            Debug.Log("DialogButton2Clicked");
            DialogButton2EventHandler(_currentEvent);
            modalDialog.show(false);
        }

        void DialogButton1EventHandler(PipeInstallEvent eventType)
        {
            // when the 1st button on the dialog was pressed
            switch (eventType)
            {
                case PipeInstallEvent.P1PickUpPipe:
                    // initialize pipe
                    var ev1 = gameObject.GetComponent<P1PickUpPipeEvent>();
                    ev1.Execute();
                    break;
                case PipeInstallEvent.P2CheckPipe:
                    // use RPC to send check result
                    var ev2 = gameObject.GetComponent<P2CheckPipeEvent>();
                    ev2.Initialize(modalDialog, GlobalConstants.DialogFirstButton);
                    ev2.Execute();
                    break;
                default:
                    modalDialog.show(false);
                    break;
            }
        }

        void DialogButton2EventHandler(PipeInstallEvent eventType)
        {
            // when the 2nd button on the dialog was pressed
            switch (eventType)
            {
                case PipeInstallEvent.P1PickUpPipe:
                    break;
                case PipeInstallEvent.P2CheckPipe:
                    // use RPC to send check result
                    var ev2 = gameObject.GetComponent<P2CheckPipeEvent>();
                    ev2.Initialize(modalDialog, GlobalConstants.DialogSecondButton);
                    ev2.Execute();
                    break;
                default:
                    modalDialog.show(false);
                    break;
            }
        }

        #endregion
    }
}