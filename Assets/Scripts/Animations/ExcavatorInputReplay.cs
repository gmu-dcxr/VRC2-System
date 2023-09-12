using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace VRC2.Animations
{
    public class ExcavatorInputReplay : BaseInputReplay
    {
        [Space(30)] [Header("Filename")] public string forwardFile;
        public string backwardFile;
        public string digFile;
        public string turnFile;
        public string stopFile;
        public string turnRFile;
        public string startFile;
        public string dumpFile;

        #region Traces

        private InputEventTrace forwardET;
        private InputEventTrace backwardET;
        private InputEventTrace digET;
        private InputEventTrace turnET;
        private InputEventTrace stopET;
        private InputEventTrace turnRET;
        private InputEventTrace startET;
        private InputEventTrace dumpET;

        #endregion

        #region Controllers

        private InputEventTrace.ReplayController forwardController;
        private InputEventTrace.ReplayController backwardController;
        private InputEventTrace.ReplayController digController;
        private InputEventTrace.ReplayController turnController;
        private InputEventTrace.ReplayController stopController;
        private InputEventTrace.ReplayController turnRController;
        private InputEventTrace.ReplayController startController;
        private InputEventTrace.ReplayController dumpController;



        #endregion

        #region Override functions

        public override void InitTraces()
        {
            print("InitTraces");
            InitTrace(ref forwardET, forwardFile);
            InitTrace(ref backwardET, backwardFile);
            InitTrace(ref digET, digFile);
            InitTrace(ref turnET, turnFile);
            InitTrace(ref stopET, stopFile);
            InitTrace(ref turnRET, turnRFile);
            InitTrace(ref startET, startFile);
            InitTrace(ref dumpET, dumpFile);
        }

        public override void InitControllers()
        {
            print("InitControllers");
            forwardController = InitController(ref forwardET, OnFinished, OnEvent);
            backwardController = InitController(ref backwardET, OnFinished, OnEvent);
            digController = InitController(ref digET, OnFinished, OnEvent);
            turnController = InitController(ref turnET, OnFinished, OnEvent);
            turnRController = InitController(ref turnRET, OnFinished, OnEvent);
            stopController = InitController(ref stopET, OnFinished, OnEvent);
            startController = InitController(ref startET, OnFinished, OnEvent);
            dumpController = InitController(ref dumpET, OnFinished, OnEvent);
        }

        #endregion

        private void Start()
        {
            
        }

        #region Controllers' callbacks

        private void OnEvent(InputEventPtr obj)
        {

        }

        void OnFinished()
        {

        }

        #endregion

        #region APIs

        public void Stop()
        {
            StartReplay(ref stopController, true);
            // stop forward and backward if applicable
            StopReplay(ref forwardController, true);
            StopReplay(ref backwardController, true);
        }

        public void Forward(bool loop = false)
        {
            StartReplay(ref forwardController, loop: loop);
        }

        public void Backward(bool loop = false)
        {
            StartReplay(ref backwardController, loop: loop);
        }

        public void Turn(bool loop = false)
        {
            StartReplay(ref turnController, loop: loop);
        }

        public void TurnR(bool loop = false)
        {
            StartReplay(ref turnRController, loop: loop);
        }

        public void Start(bool loop = false)
        {
            StartReplay(ref startController, loop: loop);
        }

        public void Dump(bool loop = false)
        {
            StartReplay(ref dumpController, loop: loop);
        }



        public void Dig(bool loop = false)
        {
            //StopReplay(ref stopController, true);
            print("digdig***");
            //StartReplay(ref digController);
            StopReplay(ref stopController, true);
            StartReplay(ref digController);

        }

        public bool DigFinished()
        {
            return IsFinished(ref digController, true);
        }

        public bool DumpDone()
        {
            return IsFinished(ref dumpController, true);
        }



        #endregion

        #region Debug

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Start"))
            {
                StartReplay(ref forwardController);
            }
        }

        #endregion
    }
}
