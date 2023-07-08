using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VRC2.Events;
using PipeMaterialColor = VRC2.Pipe.PipeConstants.PipeMaterialColor;

namespace VRC2
{
    [RequireComponent(typeof(ModalDialogManager))]
    public class PipeMenuHandler : MonoBehaviour
    {
        private ModalDialog modalDialog;

        private ModalDialogManager dialogManager;

        private DirectMessage directMessage;

        internal void ShowModalDialog(bool flag)
        {
            dialogManager.Show(flag);
        }

        private void Start()
        {
            // initialize modal dialog
            dialogManager = gameObject.GetComponent<ModalDialogManager>();
            modalDialog = dialogManager.modalDialog;

            // initialize direct message
            directMessage = gameObject.GetComponent<DirectMessage>();

            // disable modal dialog first
            ShowModalDialog(false);

            // add event listener for dialog window
            modalDialog.button1Events.WhenRelease.AddListener(() => { DialogButton1Clicked(); });
            modalDialog.button2Events.WhenRelease.AddListener(() => { DialogButton2Clicked(); });
        }

        #region Menu Button Events

        /*
         * P1 menu event flows:
         *  0. OnVoiceControl()
         *  1. OnCheckStorage() - P1CheckStorage - {<true>P1PickupPipeEvent, <false> P1CommandAIDrone
         *  2. OnPickupPipe() - P1PickupPipeEvent
         * 
         * P2 menu event flows:
         *  0. OnVoiceControl()
         *  1. OnGiveInstruction() - P2GiveInstruction - P1GetInstruction
         *  2. OnCheckPipeSizeColor() - P2CheckSizeAndColor - P1GetSizeAndColorResult
         *  3. OnMeasureDistance() - P2MeasureDistance - P2MeasureDistanceResult
         *  4. OnCommandRobot() - P2CommandRobotBendOrCut - 
         *  5. OnCheckLengthAngle() - P2CheckLengthAndAngle - {DirectMessage<true>(glue), P2CommandRobotBendOrCut<false>}
         *  6. OnCheckLevel() - P2CheckLevel - {DirectMessage<true>(clamp), DirectMessage<false>(adjust)}
         */

        public void OnVoiceControl()
        {
            // enable or disable voice
            Debug.Log("You clicked Voice Control");

            dialogManager.UpdateDialog("Voice Control", "Enable/Disable Voice?", "Enable", "Disable",
                PipeInstallEvent.VoiceControl);
            ShowModalDialog(true);
        }

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
            
            // change the logic, show the menu directly
            var ev0 = gameObject.GetComponent<P1CommandAIDroneEvent>();
            ev0.Execute();

            // dialogManager.UpdateDialog("Check Storage", "Is the storage enough?", "Enough", "Lack",
            //     PipeInstallEvent.P1CheckStorage);
            // ShowModalDialog(true);
        }

        public void OnDeprecate()
        {
            if (!GlobalConstants.lastSpawned.IsValid) return;

            // p1 deprecate current pipe
            dialogManager.UpdateDialog("Warning", "Deprecate this pipe?", "Yes", "No",
                PipeInstallEvent.P1Deprecate);
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

            // get size and color
            var (size, color) = P2CheckSizeAndColorEvent.GetPipeSizeAndColor();

            if (size < 0)
            {
                // not valid, maybe network is not ready, or no spawned pipe
                Debug.LogWarning("Not found a spawned pipe");
                return;
            }

            dialogManager.UpdateDialog("Tip",
                $"Size: {size} Color: {Utils.GetDisplayName<PipeMaterialColor>(color)}\n" +
                $"All Correct?", "Yes", "No",
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

        public void OnCheckGlue()
        {
            Debug.Log("You clicked check glue");
            // dialogManager.UpdateDialog("Check Glue", "Is glue enough?", "Yes", "no",
            //     PipeInstallEvent.P1CheckGlue);
            // ShowModalDialog(true);
            // change logic, directly refill glue
            var cge = gameObject.GetComponent<P1CheckGlueEvent>();
            cge.Execute();
        }

        public void OnCheckClamp()
        {
            Debug.Log("You clicked check clamp");
            // dialogManager.UpdateDialog("Check Glue", "Is clamp enough?", "Yes", "no",
            //     PipeInstallEvent.P1CheckClamp);
            // ShowModalDialog(true);
            // change logic
            var cce = gameObject.GetComponent<P1CheckClampEvent>();
            cce.Execute();
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

            // get water level value
            var value = P2CheckLevelEvent.GetWaterLevelValue();
            if (value < 0)
            {
                // not valid, maybe network is not ready, or no spawned pipe
                Debug.LogWarning("Not found a spawned pipe");
                return;
            }

            dialogManager.UpdateDialog("Check Level", $"Level value:{value}\nIs it correct?", "Yes", "No",
                PipeInstallEvent.P2CheckLevel);
            ShowModalDialog(true);
        }

        #endregion

        #region Participants' Actions

        void SendDirectMessage(string title, string content)
        {
            directMessage.title = title;
            directMessage.content = content;
            directMessage.Execute();
        }

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

        void P1DeprecateAction()
        {
            if (!GlobalConstants.IsNetworkReady())
            {
                Debug.LogError("Runner or localPlayer is none");
                return;
            }

            if (GlobalConstants.lastSpawned.IsValid)
            {
                var runner = GlobalConstants.networkRunner;
                var obj = runner.FindObject(GlobalConstants.lastSpawned);
                runner.Despawn(obj);
            }
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
                case PipeInstallEvent.VoiceControl:
                    ShowModalDialog(false);
                    // enable voice
                    var vce = gameObject.GetComponent<VoiceControlEvent>();
                    vce.enableVoice = true;
                    vce.Execute();
                    break;

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

                case PipeInstallEvent.P1Deprecate:
                    // yes
                    ShowModalDialog(false);
                    P1DeprecateAction();
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

                case PipeInstallEvent.P1CheckGlue:
                    // enough   
                    ShowModalDialog(false);
                    break;

                case PipeInstallEvent.P1CheckClamp:
                    // enough
                    ShowModalDialog(false);
                    break;

                case PipeInstallEvent.P2CheckLengthAndAngle:
                    // pass: nothing to do on P2 side, P1 may start glue
                    ShowModalDialog(false);
                    // send message
                    SendDirectMessage("From P2", "You may glue it.");
                    break;

                case PipeInstallEvent.P1Glue:
                    P1MayStartPlace();
                    break;

                case PipeInstallEvent.P2CheckLevel:
                    ShowModalDialog(false);

                    // one way is to send message via RPC
                    // use RPC to send check result
                    // var ev4 = gameObject.GetComponent<P2CheckLevelEvent>();
                    // ev4.Initialize(GlobalConstants.DialogFirstButton);
                    // ev4.Execute();

                    // another way is to send direct message
                    // pass
                    SendDirectMessage("From P2", "You may clamp it.");
                    break;

                case PipeInstallEvent.P1GetLevelResult:
                    // Unused

                    break;

                default:
                    ShowModalDialog(false);
                    break;
            }
        }

        void DialogButton2EventHandler()
        {
            // when the 2nd button on the dialog was pressed
            var ev = modalDialog.currentEvent;
            switch (ev)
            {
                case PipeInstallEvent.VoiceControl:
                    ShowModalDialog(false);
                    // enable voice
                    var vce = gameObject.GetComponent<VoiceControlEvent>();
                    vce.enableVoice = false;
                    vce.Execute();
                    break;

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

                case PipeInstallEvent.P1CheckGlue:
                    // not enough   
                    ShowModalDialog(false);
                    var cge = gameObject.GetComponent<P1CheckGlueEvent>();
                    cge.Execute();
                    break;

                case PipeInstallEvent.P1CheckClamp:
                    // not enough
                    ShowModalDialog(false);
                    var cce = gameObject.GetComponent<P1CheckClampEvent>();
                    cce.Execute();
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
                    SendDirectMessage("From P2", "You may adjust it.");
                    break;

                default:
                    ShowModalDialog(false);
                    break;
            }
        }

        #endregion
    }
}