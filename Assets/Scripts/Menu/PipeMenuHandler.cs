using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC2.Events;

namespace VRC2
{
    [RequireComponent(typeof(ModalDialogManager))]
    public class PipeMenuHandler : MonoBehaviour
    {
        private ModalDialog modalDialog;

        private ModalDialogManager dialogManager;


        internal void ShowModalDialog(bool flag)
        {
            dialogManager.Show(flag);
        }

        private void Start()
        {
            // initialize modal dialog
            dialogManager = gameObject.GetComponent<ModalDialogManager>();
            modalDialog = dialogManager.modalDialog;

            // disable modal dialog first
            ShowModalDialog(false);

            // add event listener for dialog window
            modalDialog.button1Events.WhenRelease.AddListener(() => { DialogButton1Clicked(); });
            modalDialog.button2Events.WhenRelease.AddListener(() => { DialogButton2Clicked(); });

            var ev0 = gameObject.GetComponent<P1CommandAIDroneEvent>();
            Debug.LogWarning($"ev0 == null: {ev0 == null}");
        }

        #region Menu Button Events

        /*
         * P2 menu event flows:
         *  1. OnGiveInstruction() - P2GiveInstruction - P1GetInstruction
         *  2. OnCheckPipeSizeColor() - P2CheckSizeAndColor - P1GetSizeAndColorResult
         *  3. OnMeasureDistance() - P2MeasureDistance - P2MeasureDistanceResult
         *  4. OnCommandRobot() - P2CommandRobotBendOrCut - 
         *  5. OnCheckLengthAngle() - P2CheckLengthAndAngle - P2CommandRobotBendOrCut(false)
         *  6. OnCheckLevel() - P2CheckLevel - P1GetLevelResult
         */

        public void OnGiveInstruction()
        {
            // p2 gives p1 instruction, size and color
            // TODO: design such panel to allow P2 to select
            Debug.Log("You clicked Give Instruction");

            dialogManager.UpdateDialog("Tip", "Give Instruction to P1", "OK", null,
                PipeInstallEvent.P2GiveInstruction);
            ShowModalDialog(true);
        }

        public void OnCheckStorage()
        {
            // p1 check storage
            Debug.Log("You clicked Check Storage");

            dialogManager.UpdateDialog("Check Storage", "Is the storage enough?", "Enough", "Lack",
                PipeInstallEvent.P1CheckStorage);
            ShowModalDialog(true);
        }

        public void OnPickupPipe()
        {
            // simulate modal window, ignore the event when the dialog is showing.
            Debug.Log("You clicked Pick A pipe");
            // set dialog window
            dialogManager.UpdateDialog("Tip", "Pick up a pipe", "OK", null,
                PipeInstallEvent.P1PickUpPipe);
            ShowModalDialog(true);
        }

        public void OnCheckPipeSizeColor()
        {
            Debug.Log("You clicked Check Pipe");

            dialogManager.UpdateDialog("Tip", "Are the color and size of the pipe correct?", "Yes", "No",
                PipeInstallEvent.P2CheckSizeAndColor);
            ShowModalDialog(true);
        }

        public void OnMeasureDistance()
        {
            Debug.Log("You clicked measure distance");

            dialogManager.UpdateDialog("Measure Distance", "TODO: instruct how to measure distance.", "Yes", null,
                PipeInstallEvent.P2MeasureDistance);
            ShowModalDialog(true);
        }

        public void OnCommandRobot()
        {
            Debug.Log("You clicked command robot");

            dialogManager.UpdateDialog("Command Robot", "Command the robot to bend ro cut the pipe", "Yes", null,
                PipeInstallEvent.P2CommandRobotBendOrCut);
            ShowModalDialog(true);
        }

        public void OnCheckLengthAngle()
        {
            Debug.Log("You clicked check length and angle");

            dialogManager.UpdateDialog("Check Length and angle", "Are the length and the angle correct?", "Yes", "No",
                PipeInstallEvent.P2CheckLengthAndAngle);
            ShowModalDialog(true);
        }

        public void OnCheckLevel()
        {
            Debug.Log("You clicked check level");
            dialogManager.UpdateDialog("Check Level", "Are the horizontal and vertical levels correct?", "Yes", "No",
                PipeInstallEvent.P2CheckLevel);
            ShowModalDialog(true);
        }

        #endregion

        #region Participants' Actions

        void P1MayStartPickup()
        {
            Debug.Log("P1 can pick up a pipe");
            // set dialog window
            dialogManager.UpdateDialog("Tip", "Pick up a pipe", "OK", null,
                PipeInstallEvent.P1PickUpPipe);
            ShowModalDialog(true);
        }

        void P1CommandAIDrone()
        {
            Debug.Log("P1 may command AI drone.");
            // set dialog window
            dialogManager.UpdateDialog("Tip", "Command AI drone to deliver.", "OK", null,
                PipeInstallEvent.P1CommandAIDrone);
            ShowModalDialog(true);
        }

        void P1MayStartGlue()
        {
            Debug.Log("P1 can glue the pipe");
            dialogManager.UpdateDialog("Tip", "You may start gluing pipe.", "Yes", null,
                PipeInstallEvent.P1Glue);
            ShowModalDialog(true);
        }

        void P1MayStartPlace()
        {
            Debug.Log("P1 can place the pipe");
            dialogManager.UpdateDialog("Tip", "You may start placing pipe.", "Yes", null,
                PipeInstallEvent.P1Place);
            ShowModalDialog(true);
        }

        void P1MayStartAdjust()
        {
            Debug.Log("P1 can adjust the pipe");
            dialogManager.UpdateDialog("Tip", "You may start adjusting pipe.", "Yes", null,
                PipeInstallEvent.P1Adjust);
            ShowModalDialog(true);
        }

        void P1MayStartClamp()
        {
            Debug.Log("P1 can clamp the pipe");
            dialogManager.UpdateDialog("Tip", "You may start clamping pipe.", "Yes", null,
                PipeInstallEvent.P1Clamp);
            ShowModalDialog(true);
        }

        #endregion

        #region Dialog Buttons Event

        public void DialogButton1Clicked()
        {
            Debug.Log("DialogButton1Clicked");
            // ShowModalDialog(false);
            DialogButton1EventHandler();
        }

        public void DialogButton2Clicked()
        {
            Debug.Log("DialogButton2Clicked");
            // ShowModalDialog(false);
            DialogButton2EventHandler();
        }

        void DialogButton1EventHandler()
        {
            // when the 1st button on the dialog was pressed
            var ev = modalDialog.currentEvent;
            switch (ev)
            {
                case PipeInstallEvent.P2GiveInstruction:
                    // hide dialog
                    ShowModalDialog(false);

                    var e = gameObject.GetComponent<P2GiveP1InstructionEvent>();
                    e.Execute();
                    break;

                case PipeInstallEvent.P1GetInstruction:
                    // p1 to pick up a pipe
                    // hide dialog
                    ShowModalDialog(false);

                    break;

                case PipeInstallEvent.P1CheckStorage:
                    // enough
                    P1MayStartPickup();
                    break;

                case PipeInstallEvent.P1CommandAIDrone:
                    // hide dialog
                    ShowModalDialog(false);

                    var ev0 = gameObject.GetComponent<P1CommandAIDroneEvent>();
                    ev0.Execute();
                    break;

                case PipeInstallEvent.P1PickUpPipe:
                    // initialize pipe
                    // hide dialog
                    ShowModalDialog(false);

                    var ev1 = gameObject.GetComponent<P1PickupPipeEvent>();
                    ev1.Execute();
                    break;
                case PipeInstallEvent.P2CheckSizeAndColor:
                    ShowModalDialog(false);
                    // use RPC to send check result
                    var ev2 = gameObject.GetComponent<P2CheckSizeAndColorEvent>();
                    ev2.Initialize(GlobalConstants.DialogFirstButton);
                    ev2.Execute();
                    break;
                case PipeInstallEvent.P1GetSizeAndColorResult:
                    // size and color check pass, nothing to do
                    break;

                case PipeInstallEvent.P2MeasureDistance:
                    var ev21 = gameObject.GetComponent<P2MeasureDistanceEvent>();
                    ev21.Execute();
                    break;


                case PipeInstallEvent.P2MeasureDistanceResult:
                    // after p2 measured the distance, instruct the robot
                    ShowModalDialog(false);

                    var ev3 = gameObject.GetComponent<P2CommandRobotEvent>();
                    ev3.Execute();
                    break;

                case PipeInstallEvent.P2CommandRobotBendOrCut:
                    // command robot
                    ShowModalDialog(false);

                    var ev31 = gameObject.GetComponent<P2CommandRobotEvent>();
                    ev31.Execute();
                    break;

                case PipeInstallEvent.P2CheckLengthAndAngle:
                    // pass: nothing to do on P2 side, P1 may start glue
                    ShowModalDialog(false);
                    Debug.Log("P1 may start glue it");
                    break;

                case PipeInstallEvent.P1Glue:
                    P1MayStartPlace();
                    break;

                case PipeInstallEvent.P2CheckLevel:
                    ShowModalDialog(false);
                    // use RPC to send check result
                    var ev4 = gameObject.GetComponent<P2CheckLevelEvent>();
                    ev4.Initialize(GlobalConstants.DialogFirstButton);
                    ev4.Execute();
                    break;

                case PipeInstallEvent.P1GetLevelResult:
                    ShowModalDialog(false);
                    // p1 get check level
                    if (modalDialog.checkResult)
                    {
                        // pass
                        // P1MayStartClamp();
                        Debug.Log("P1 may clamp it");
                    }
                    else
                    {
                        // failed
                        // P1MayStartAdjust();
                        Debug.Log("P1 may adjust it");
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
                case PipeInstallEvent.P1CheckStorage:
                    // lack
                    // command AI drone to deliver pipes
                    P1CommandAIDrone();
                    break;

                case PipeInstallEvent.P1PickUpPipe:
                    break;
                case PipeInstallEvent.P2CheckSizeAndColor:
                    ShowModalDialog(false);
                    // use RPC to send check result
                    var ev2 = gameObject.GetComponent<P2CheckSizeAndColorEvent>();
                    ev2.Initialize(GlobalConstants.DialogSecondButton);
                    ev2.Execute();
                    break;
                case PipeInstallEvent.P1GetSizeAndColorResult:
                    // size and color check failed, p1 need to redo pick up pipe event
                    // TODO: remove current picked pipe first
                    ShowModalDialog(false);
                    break;

                case PipeInstallEvent.P2MeasureDistanceResult:
                    // after p2 measured the distance
                    ShowModalDialog(false);
                    break;

                case PipeInstallEvent.P2CheckLengthAndAngle:
                    // length or angle is wrong, re-command robot
                    // update event

                    ShowModalDialog(false);
                    
                    var ev3 = gameObject.GetComponent<P2CommandRobotEvent>();
                    ev3.Execute();
                    break;

                case PipeInstallEvent.P2CheckLevel:
                    // use RPC to send check result
                    ShowModalDialog(false);
                    var ev4 = gameObject.GetComponent<P2CheckLevelEvent>();
                    ev4.Initialize(GlobalConstants.DialogSecondButton);
                    ev4.Execute();
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}