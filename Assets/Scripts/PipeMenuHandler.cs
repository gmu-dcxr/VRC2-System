using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Events;

namespace VRC2
{
    [RequireComponent(typeof(ModalDialogGetter))]
    public class PipeMenuHandler : MonoBehaviour
    {
        private ModalDialog modalDialog;

        bool IsDialogShowing
        {
            get { return modalDialog.gameObject.activeSelf; }
        }

        private void Start()
        {
            // initialize modal dialog
            modalDialog = gameObject.GetComponent<ModalDialogGetter>().ModalDialog;

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
            // set dialog window
            modalDialog.UpdateDialog("Tip", "Pick up a pipe", "OK", null,
                PipeInstallEvent.P1PickUpPipe);
            modalDialog.show(true);
        }

        public void OnCheckPipeSizeColor()
        {
            if (IsDialogShowing) return;

            Debug.Log("You clicked Check Pipe");

            if (GlobalConstants.Checker)
            {
                modalDialog.UpdateDialog("Tip", "Are the color and size of the pipe correct?", "Yes", "No",
                    PipeInstallEvent.P2CheckSizeAndColor);
                modalDialog.show(true);
            }
        }

        public void OnMeasureDistance()
        {
            if (IsDialogShowing) return;
            Debug.Log("You clicked measure distance");
            if (GlobalConstants.Checker)
            {
                modalDialog.UpdateDialog("Measure Distance", "TODO: instruct how to measure distance.", "Yes", null,
                    PipeInstallEvent.P2MeasureDistance);
                modalDialog.show(true);
            }
        }

        public void OnCommandRobot()
        {
            if (IsDialogShowing) return;
            Debug.Log("You clicked command robot");
            if (GlobalConstants.Checker)
            {
                modalDialog.UpdateDialog("Command Robot", "Command the robot to bend ro cut the pipe", "Yes", null,
                    PipeInstallEvent.P2CommandRobotBendOrCut);
                modalDialog.show(true);
            }
        }

        public void OnCheckLengthAngle()
        {
            if (IsDialogShowing) return;
            Debug.Log("You clicked check length and angle");
            if (GlobalConstants.Checker)
            {
                modalDialog.UpdateDialog("Check Length and angle", "Are the length and the angle correct?", "Yes", "No",
                    PipeInstallEvent.P2CheckLengthAndAngle);
                modalDialog.show(true);
            }
        }

        public void OnCheckLevel()
        {
            if (IsDialogShowing) return;
            Debug.Log("You clicked check level");
            if (GlobalConstants.Checker)
            {
                modalDialog.UpdateDialog("Check Level", "Are the horizontal and vertical levels correct?", "Yes", "No",
                    PipeInstallEvent.P2CheckLevel);
                modalDialog.show(true);
            }
        }

        #endregion

        #region Participants' Actions

        void P1MayStartGlue()
        {
            if (IsDialogShowing) return;
            Debug.Log("P1 can glue the pipe");
            if (GlobalConstants.Checkee)
            {
                modalDialog.UpdateDialog("Tip", "You may start gluing pipe.", "Yes", null,
                    PipeInstallEvent.P1Glue);
                modalDialog.show(true);
            }
        }

        void P1MayStartPlace()
        {
            if (IsDialogShowing) return;
            Debug.Log("P1 can place the pipe");
            if (GlobalConstants.Checkee)
            {
                modalDialog.UpdateDialog("Tip", "You may start placing pipe.", "Yes", null,
                    PipeInstallEvent.P1Place);
                modalDialog.show(true);
            }
        }

        void P1MayStartAdjust()
        {
            if (IsDialogShowing) return;
            Debug.Log("P1 can adjust the pipe");
            if (GlobalConstants.Checkee)
            {
                modalDialog.UpdateDialog("Tip", "You may start adjusting pipe.", "Yes", null,
                    PipeInstallEvent.P1Adjust);
                modalDialog.show(true);
            }
        }

        void P1MayStartClamp()
        {
            if (IsDialogShowing) return;
            Debug.Log("P1 can clamp the pipe");
            if (GlobalConstants.Checkee)
            {
                modalDialog.UpdateDialog("Tip", "You may start clamping pipe.", "Yes", null,
                    PipeInstallEvent.P1Clamp);
                modalDialog.show(true);
            }
        }

        #endregion

        #region Dialog Buttons Event

        public void DialogButton1Clicked()
        {
            Debug.Log("DialogButton1Clicked");
            DialogButton1EventHandler();
            modalDialog.show(false);
        }

        public void DialogButton2Clicked()
        {
            Debug.Log("DialogButton2Clicked");
            DialogButton2EventHandler();
            modalDialog.show(false);
        }

        void DialogButton1EventHandler()
        {
            // when the 1st button on the dialog was pressed
            var ev = modalDialog.currentEvent;
            switch (ev)
            {
                case PipeInstallEvent.P1PickUpPipe:
                    // initialize pipe
                    var ev1 = gameObject.GetComponent<P1PickUpPipeEvent>();
                    ev1.Execute();
                    break;
                case PipeInstallEvent.P2CheckSizeAndColor:
                    // use RPC to send check result
                    var ev2 = gameObject.GetComponent<P2CheckSizeAndColorEvent>();
                    ev2.Initialize(GlobalConstants.DialogFirstButton);
                    ev2.Execute();
                    break;
                case PipeInstallEvent.P1GetSizeAndColorResult:
                    // size and color check pass, nothing to do
                    break;

                case PipeInstallEvent.P2MeasureDistance:
                    // after p2 measured the distance, instruct the robot
                    // stop first
                    gameObject.GetComponent<P2MeasureDistanceEvent>().Stop();
                    // update event
                    modalDialog.currentEvent = PipeInstallEvent.P2CommandRobotBendOrCut;

                    var ev3 = gameObject.GetComponent<P2CommandRobotEvent>();
                    ev3.Execute();
                    break;
                case PipeInstallEvent.P2CheckLengthAndAngle:
                    // nothing to do on P1 side
                    break;

                case PipeInstallEvent.P1GetLengthAndAngleResult:
                    // this is from RPC
                    if (modalDialog.checkResult)
                    {
                        // pass
                        // length and angle are right, let P1 glue it
                        P1MayStartGlue();
                    }

                    break;

                case PipeInstallEvent.P1Glue:
                    P1MayStartPlace();
                    break;

                case PipeInstallEvent.P2CheckLevel:
                    // use RPC to send check result
                    var ev4 = gameObject.GetComponent<P2CheckSizeAndColorEvent>();
                    ev4.Initialize(GlobalConstants.DialogFirstButton);
                    ev4.Execute();
                    break;

                case PipeInstallEvent.P1GetLevelResult:
                    // p1 get check level
                    if (modalDialog.checkResult)
                    {
                        // pass
                        P1MayStartClamp();
                    }
                    else
                    {
                        // failed
                        P1MayStartAdjust();
                    }

                    break;

                default:
                    break;
            }
        }

        void DialogButton2EventHandler()
        {
            // when the 2nd button on the dialog was pressed
            var ev = modalDialog.currentEvent;
            switch (ev)
            {
                case PipeInstallEvent.P1PickUpPipe:
                    break;
                case PipeInstallEvent.P2CheckSizeAndColor:
                    // use RPC to send check result
                    var ev2 = gameObject.GetComponent<P2CheckSizeAndColorEvent>();
                    ev2.Initialize(GlobalConstants.DialogSecondButton);
                    ev2.Execute();
                    break;
                case PipeInstallEvent.P1GetSizeAndColorResult:
                    // size and color check failed, p1 need to redo pick up pipe event
                    // TODO: remove current picked pipe first
                    break;

                case PipeInstallEvent.P2MeasureDistance:
                    // don't command robot
                    gameObject.GetComponent<P2MeasureDistanceEvent>().Stop();
                    break;
                case PipeInstallEvent.P2CommandRobotBendOrCut:
                    // don't command robot
                    break;

                case PipeInstallEvent.P2CheckLengthAndAngle:
                    // length or angle is wrong, re-command robot
                    // update event
                    modalDialog.currentEvent = PipeInstallEvent.P2CommandRobotBendOrCut;

                    var ev3 = gameObject.GetComponent<P2CommandRobotEvent>();
                    ev3.Execute();
                    break;

                case PipeInstallEvent.P2CheckLevel:
                    // use RPC to send check result
                    var ev4 = gameObject.GetComponent<P2CheckLevelEvent>();
                    ev4.Initialize(GlobalConstants.DialogFirstButton);
                    ev4.Execute();
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}