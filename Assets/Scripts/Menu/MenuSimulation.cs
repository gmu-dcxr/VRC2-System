using System;
using UnityEngine;

namespace VRC2.Menu
{
    public class MenuSimulation : MonoBehaviour
    {
        [SerializeField] private PipeMenuHandler _handler;

        private void Start()
        {
        }

        private void Update()
        {
            if (!GlobalConstants.GameStarted)
                return;

            if (GlobalConstants.Checker)
            {
                if (Input.GetKeyUp(KeyCode.Keypad1))
                {
                    _handler.OnGiveInstruction();
                }

                if (Input.GetKeyUp(KeyCode.Keypad2))
                {
                    _handler.OnCheckPipeSizeColor();
                }

                if (Input.GetKeyUp(KeyCode.Keypad3))
                {
                    _handler.OnMeasureDistance();
                }

                if (Input.GetKeyUp(KeyCode.Keypad4))
                {
                    _handler.OnCommandRobot();
                }

                if (Input.GetKeyUp(KeyCode.Keypad5))
                {
                    _handler.OnCheckLengthAngle();
                }

                if (Input.GetKeyUp(KeyCode.Keypad6))
                {
                    _handler.OnCheckLevel();
                }
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Keypad1))
                {
                    _handler.Simulate_AIDroneDeliver();
                }

                if (Input.GetKeyUp(KeyCode.Keypad2))
                {
                    _handler.OnPickupPipe();
                }

                if (Input.GetKeyUp(KeyCode.Keypad3))
                {
                    _handler.Simulate_RobotBendCut();
                }
            }
        }

        private void OnGUI()
        {
            if (!GlobalConstants.GameStarted)
                return;

            if (GlobalConstants.Checker)
            {
                GUI.Button(new Rect(10, 10, 200, 30), "1 - Give Instruction");
                GUI.Button(new Rect(10, 60, 200, 30), "2 - Size & Color");
                GUI.Button(new Rect(10, 110, 200, 30), "3 - Measure Distance");
                GUI.Button(new Rect(10, 160, 200, 30), "4 - Command Robot");
                GUI.Button(new Rect(10, 210, 200, 30), "5 - Length & Angle");
                GUI.Button(new Rect(10, 260, 200, 30), "6 - Check Level");
            }
            else
            {
                GUI.Button(new Rect(10, 10, 200, 30), "1 - Simulate AI Drone");
                GUI.Button(new Rect(10, 60, 200, 30), "2 - Pickup Pipe");
                GUI.Button(new Rect(10, 110, 200, 30), "3 - Simulate Robot");
            }
        }
    }
}