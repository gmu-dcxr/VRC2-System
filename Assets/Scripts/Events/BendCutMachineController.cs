﻿using System;
using Fusion;
using UnityEngine;
using VRC2.Pipe;

using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using Random = System.Random;
using Timer = UnityTimer.Timer;

namespace VRC2.Events
{
    public class BendCutMachineController : BaseEvent
    {
        [Header("Markers")] public Transform robotStop;
        public Transform pipeInput;
        public Transform pipeOutput;
        public Transform delivery;

        [Header("Monitor")] public QuadImageManager imageManager;
        public AudioSource audioSource;

        [Header("Executor")] public RobotBendCut robotBendCut;
        public float duration = 5.0f;
        
        [Header("Error")] public bool enableError = false;

        private PipeBendAngles _angle;
        
        private Timer _timer;

        // for generating wrong pipe (angle only)
        private Random random;

        private void Start()
        {
            random = new Random();
            robotBendCut.ReadyToOperate += OnReadyToOperate;
        }

        private void OnReadyToOperate(PipeBendAngles angle)
        {
            _angle = angle;

            if (enableError)
            {
                _angle = GetRandomAngle();
                Debug.LogWarning($"Use random angle: {Utils.GetDisplayName<PipeBendAngles>(_angle)}");
            }

            // play noise 
            audioSource.Play();

            // update pipe
            var pipe = GlobalConstants.lastSpawnedPipe;
            pipe.transform.position = pipeInput.position;
            pipe.transform.rotation = pipeInput.rotation;
            pipe.transform.parent = null;

            RPC_SendMessage(_angle);
            
            // start timer
            SetTimer(SpawnPipe);
        }

        void SetTimer(Action complete)
        {
            if (_timer != null)
            {
                Timer.Cancel(_timer);
            }

            _timer = Timer.Register(duration, complete, isLooped: false, useRealTime: true);
        }

        void SpawnPipe()
        {   
            // stop noise
            audioSource.Stop();
            
            // spawn pipe
            var no = robotBendCut.SpawnPipe(_angle);
            // update spawned pipe transform
            no.transform.position = pipeOutput.transform.position;
            no.transform.rotation = pipeOutput.transform.rotation;
            
            // start a new timer to let robot deliver
            SetTimer(() =>
            {
                robotBendCut.PickupResult(pipeOutput.position);
            });
        }

        private void Update()
        {

        }

        PipeBendAngles GetRandomAngle()
        {
            var i = random.Next(0, 4);
            var src = (int)_angle;
            while (i == src)
            {
                i = random.Next(0, 4);
            }

            return (PipeBendAngles)i;
        }

        string GetImageName(PipeBendAngles angle)
        {
            var s = Utils.GetDisplayName<PipeBendAngles>(angle);
            return s.Split('_')[1];
        }
        
        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(PipeBendAngles angle, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {
                // load side
                imageManager.UpdateFilename(GetImageName(angle));
            }
            else
            {
                // remote side
                imageManager.UpdateFilename(GetImageName(angle));
            }
        }
    }
}