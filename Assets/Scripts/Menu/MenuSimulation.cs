using System;
using UnityEngine;

namespace VRC2.Menu
{
    public class MenuSimulation : MonoBehaviour
    {
        [SerializeField] private PipeMenuHandler _handler;

        [Space(30)] [Header("Pipe Spawning")] public Transform spawnPoint;

        private void Start()
        {
        }

        private void Update()
        {

        }

        private void OnGUI()
        {
            if (!GlobalConstants.GameStarted)
                return;

            GUILayout.BeginVertical();
            if (GlobalConstants.Checker)
            {
                if (GUILayout.Button("1 - Give Instruction"))
                {
                    _handler.OnGiveInstruction();
                }

                if (GUILayout.Button("2 - Size & Color"))
                {
                    _handler.OnCheckPipeSizeColor();
                }

                if (GUILayout.Button("3 - Measure Distance"))
                {
                    _handler.OnMeasureDistance();
                }

                if (GUILayout.Button("4 - Command Robot"))
                {
                    _handler.OnCommandRobot();
                }

                if (GUILayout.Button("5 - Length & Angle"))
                {
                    _handler.OnCheckLengthAngle();
                }

                if (GUILayout.Button("6 - Check Level"))
                {
                    _handler.OnCheckLevel();
                }
            }
            else
            {
                if (GUILayout.Button("1 - Simulate AI Drone"))
                {
                    _handler.Simulate_AIDroneDeliver();
                }

                if (GUILayout.Button("2 - Spawn Pipe"))
                {
                    _handler.Simulate_SpawnPipe(spawnPoint);
                }

                if (GUILayout.Button("3 - Simulate Robot"))
                {
                    _handler.Simulate_RobotBendCut();
                }

                if (GUILayout.Button("4 - Refill Clamp"))
                {
                    _handler.Simulate_RefillClamp();
                }

                if (GUILayout.Button("5 - Refill Glue"))
                {
                    _handler.Simulate_RefillGlue();
                }

                if (GUILayout.Button("6 - Report Dog"))
                {
                    _handler.Simulate_OnReportDog();
                }
            }

            GUILayout.EndVertical();
        }
    }
}