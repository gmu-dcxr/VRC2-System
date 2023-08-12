using System;
using UnityEngine;

using UnityTimer;
using VRC2.Pipe;

using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;

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

        [Header("Error")] public bool enableError = false;

        // target angle
        private PipeBendAngles _angle;

        private Timer _timer;
        // for input to output 
        private float duration = 3.0f;

        private void Start()
        {
            robotBendCut.ReadyToOperate += OnReadyToOperate;
        }

        private void OnReadyToOperate(PipeBendAngles angle)
        {
            // play noise 
            audioSource.Play();

            // update pipe
            var pipe = GlobalConstants.lastSpawnedPipe;
            pipe.transform.position = pipeInput.position;
            pipe.transform.rotation = pipeInput.rotation;
            pipe.transform.parent = null;

            // update image
            imageManager.UpdateFilename(GetImageName());
            
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
            var no = robotBendCut.SpawnPipe();
            // update spawned pipe transform
            no.transform.position = pipeOutput.transform.position;
            no.transform.rotation = pipeOutput.transform.rotation;
            
            // start a new timer to let robot deliver
            SetTimer(() =>
            {
                robotBendCut.Deliver();
            });
        }

        private void Update()
        {

        }

        string GetImageName()
        {
            var s = Utils.GetDisplayName<PipeBendAngles>(_angle);
            return s.Split('_')[1];
        }
    }
}