﻿using Fusion;
using NodeCanvas.Tasks.Actions;
using UnityEngine;
using VRC2.Animations;
using VRC2.Network;
using VRC2.Pipe;

using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using PipeDiameter = VRC2.Pipe.PipeConstants.PipeDiameter;

namespace VRC2.Events
{
    public class P2CommandRobotEvent : BaseEvent
    {
        public BendCutMenuController _bendCutMenuController;

        public void Start()
        {
            _bendCutMenuController.OnConfirmed += OnConfirmed;

            // hide at the beginning
            _bendCutMenuController.Hide();
        }

        private void OnConfirmed()
        {
            var parameter = _bendCutMenuController.result;
            Debug.Log("Robot is going to bend or cut.");
            Debug.Log(parameter.ToString());

            // // debug only
            // var go = GameObject.Find(GlobalConstants.BendCutRobot);
            // var rbc = go.GetComponent<RobotBendCut>();
            // rbc.InitParameters(parameter.angle, parameter.a, parameter.b);
            // rbc.Execute();

            // return;

            if (!GlobalConstants.IsNetworkReady())
            {
                Debug.LogError("Runner or localPlayer is none");
                return;
            }

            if (_bendCutMenuController.IsNewDesign)
            {
                print("New design for P2CommandRobotEvent");
                RPC_SendMessageNew(parameter.angle, parameter.a, parameter.b, parameter.amount,
                    parameter.connectorDiamter, parameter.connectorAmount);
            }
            else
            {
                // send message to P1 side since P1 has the input authority for the pipe
                // Only angle, a, and b are needed since p2 doesn't have the pipe type and color information
                RPC_SendMessage(parameter.angle, parameter.a, parameter.b);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(PipeBendAngles angle, float a, float b, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
                message = $"You sent {angle} - {a} - {b}\n";
            else
            {
                message = $"Some other player said:  {angle} - {a} - {b}\n";
                // 
                var go = GameObject.Find(GlobalConstants.BendCutRobot);
                var rdc = go.GetComponent<RobotDogController>();
                // update current pipe
                rdc.currentPipe = GlobalConstants.lastSpawnedPipe;

                rdc.InitParameters(angle, a, b);
                rdc.Execute();
            }

            Debug.LogWarning(message);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessageNew(PipeBendAngles angle, float a, float b, int amount, PipeDiameter diameter,
            int camount, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
                message = $"You sent {angle} - {a} - {b} - {amount} - {diameter} - {camount}\n";
            else
            {
                message = $"Some other player said:  {angle} - {a} - {b} - {amount} - {diameter} - {camount}\n";
                // 
                var go = GameObject.Find(GlobalConstants.BendCutRobot);
                var rdc = go.GetComponent<RobotDogController>();
                // update current pipe
                rdc.currentPipe = GlobalConstants.lastSpawnedPipe;

                rdc.InitParameters(angle, a, b, amount, diameter, camount);
                rdc.Execute();
            }

            Debug.LogWarning(message);
        }

        public override void Execute()
        {
            if (_bendCutMenuController.showing)
            {
                _bendCutMenuController.Hide();
            }
            else
            {
                _bendCutMenuController.Show();
            }
        }
    }
}