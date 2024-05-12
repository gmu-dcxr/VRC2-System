using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VRC2.Animations;
using VRC2.Pipe;

using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;
using Random = System.Random;
using Timer = UnityTimer.Timer;

namespace VRC2.Events
{
    public class BendCutMachineController : BaseEvent
    {
        [Header("Monitor")] public QuadImageManager imageManager;
        public AudioSource audioSource;

        [Header("RobotDog")] public RobotDogController robotDog;

        // time of waiting for the cutting
        public float cutDuration = 5.0f;

        // time before going to pickup the result
        public float pickupDuration = 2.0f;

        [Header("Error")] public bool enableError = false;

        private PipeBendAngles _angle;

        private Timer _timer;

        // for generating wrong pipe (angle only)
        private Random random;

        private void Start()
        {
            random = new Random();
            robotDog.ReadyToOperate += OnReadyToOperate;
        }

        private void OnReadyToOperate(PipeBendAngles angle, int amount)
        {
            _angle = angle;

            if (enableError)
            {
                _angle = GetRandomAngle();
                Debug.LogWarning($"Use random angle: {Utils.GetDisplayName<PipeBendAngles>(_angle)}");
            }

            // play noise 
            audioSource.Play();

            var pipeInput = robotDog.bendcutInput;

            // update pipe
            var pipe = GlobalConstants.lastSpawnedPipe;
            // disable update to make it look smooth
            // pipe.transform.position = pipeInput.position;
            // pipe.transform.rotation = pipeInput.rotation;
            pipe.transform.parent = null;

            // update current pipe
            robotDog.currentPipe = pipe;

            RPC_SendMessage(_angle);

            // start timer
            SetTimer(cutDuration, () => { SpawnPipe(amount); });
        }

        void SetTimer(float duration, Action complete)
        {
            if (_timer != null)
            {
                Timer.Cancel(_timer);
            }

            _timer = Timer.Register(duration, complete, isLooped: false, useRealTime: true);
        }

        void SpawnPipe(int amount)
        {
            // stop noise
            audioSource.Stop();

            var pipeOutput = robotDog.bendcutOutput;

            List<GameObject> objs = new List<GameObject>();
            List<Transform> ts = new List<Transform>();

            var (pos, rot) = robotDog.GetCurrentPipeTransform();

            // destroy current pipe
            robotDog.DestroyCurrentPipe();

            // new design
            for (var i = 0; i < amount; i++)
            {
                // spawn pipe
                var no = robotDog.SpawnPipe(_angle, pos, rot);
                // update spawned pipe transform
                no.transform.position = pipeOutput.transform.position;
                no.transform.rotation = pipeOutput.transform.rotation;

                objs.Add(no.gameObject);
                ts.Add(no.transform);
            }

            // start a new timer to let robot deliver
            SetTimer(pickupDuration, () => { robotDog.PickupResult(objs, ts); });
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