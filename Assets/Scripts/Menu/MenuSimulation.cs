using System;
using UnityEngine;

namespace VRC2.Menu
{
    [RequireComponent(typeof(PipeMenuHandler))]
    public class MenuSimulation : MonoBehaviour
    {
        private PipeMenuHandler _handler;

        private void Start()
        {
            _handler = gameObject.GetComponent<PipeMenuHandler>();
        }

        private void Update()
        {
            if (GlobalConstants.Checker)
            {
                if (Input.GetKeyUp(KeyCode.Keypad1))
                {
                    _handler.OnCheckPipeSizeColor();
                }

                if (Input.GetKeyUp(KeyCode.Keypad2))
                {
                    _handler.OnMeasureDistance();
                }

                if (Input.GetKeyUp(KeyCode.Keypad3))
                {
                    _handler.OnCommandRobot();
                }

                if (Input.GetKeyUp(KeyCode.Keypad4))
                {
                    _handler.OnCheckLengthAngle();
                }
                
                if (Input.GetKeyUp(KeyCode.Keypad5))
                {
                    _handler.OnCheckLevel();
                }
            }
        }

        private void OnGUI()
        {
            if (GlobalConstants.Checker)
            {
                GUI.Label(new Rect(10, 10, 200, 30), "1 - Size & Color");
                GUI.Label(new Rect(10, 60, 200, 30), "2 - Measure Distance");
                GUI.Label(new Rect(10, 110, 200, 30), "3 - Command Robot");
                GUI.Label(new Rect(10, 160, 200, 30), "4 - Length & Angle");
                GUI.Label(new Rect(10, 210, 200, 30), "5 - Check Level");
            }
        }
    }
}