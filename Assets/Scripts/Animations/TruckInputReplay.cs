using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace VRC2.Animations
{
    public class TruckInputReplay : BaseInputReplay
    {
        [Space(30)] [Header("Filename")] public string forwardFile;
        public string backwardFile;
        public string turnLeftFile;
        public string turnRightFile;
        public string brakeFile;

        #region Traces

        private InputEventTrace forwardET;
        private InputEventTrace backwardET;
        private InputEventTrace turnLeftET;
        private InputEventTrace turnRightET;
        private InputEventTrace brakeET;

        #endregion

        #region Controllers

        private InputEventTrace.ReplayController forwardController;
        private InputEventTrace.ReplayController backwardController;
        private InputEventTrace.ReplayController turnLeftController;
        private InputEventTrace.ReplayController turnRightController;
        private InputEventTrace.ReplayController brakeController;


        #endregion

        #region Override functions

        public override void InitTraces()
        {
            print("InitTraces");
            InitTrace(ref forwardET, forwardFile);
            InitTrace(ref backwardET, backwardFile);
            InitTrace(ref turnLeftET, turnLeftFile);
            InitTrace(ref turnRightET, turnRightFile);
            InitTrace(ref brakeET, brakeFile);
        }

        public override void InitControllers()
        {
            print("InitControllers");
            forwardController = InitController(ref forwardET, OnFinished, OnEvent);
            backwardController = InitController(ref backwardET, OnFinished, OnEvent);
            turnLeftController = InitController(ref turnLeftET, OnFinished, OnEvent);
            turnRightController = InitController(ref turnRightET, OnFinished, OnEvent);
            brakeController = InitController(ref brakeET, OnFinished, OnEvent);
        }

        #endregion

        #region Controllers' callbacks

        private void OnEvent(InputEventPtr obj)
        {

        }

        void OnFinished()
        {

        }

        #endregion

        #region APIs

        public void StopAll()
        {
            StopReplay(ref forwardController, true);
            StopReplay(ref backwardController, true);
            StopReplay(ref turnLeftController, true);
            StopReplay(ref turnRightController, true);
            StopReplay(ref brakeController, true);
        }

        public void RewindAll()
        {
            ForceRewind(ref forwardController, true);
            ForceRewind(ref backwardController, true);
            ForceRewind(ref turnLeftController, true);
            ForceRewind(ref turnRightController, true);
            ForceRewind(ref brakeController, true);
        }

        public void Forward(bool loop = false, bool stop = false)
        {
            StartReplay(ref forwardController, loop: loop, stop: stop);
        }

        public void Backward(bool loop = false, bool stop = false)
        {
            StartReplay(ref backwardController, loop: loop, stop: stop);
        }

        public void TurnLeft()
        {
            StartReplay(ref turnLeftController);
        }

        public bool TurnLeftDone()
        {
            return turnLeftController.finished && turnLeftController.position != 0;
        }

        public void TurnRight()
        {
            StartReplay(ref turnRightController);
        }

        public bool TurnRightDone()
        {
            return turnRightController.finished && turnRightController.position != 0;
        }

        public void Brake()
        {
            StartReplay(ref brakeController, loop: true);
        }

        #endregion
    }
}