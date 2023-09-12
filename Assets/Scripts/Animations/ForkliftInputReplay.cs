using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace VRC2.Animations
{
    public class ForkliftInputReplay : BaseInputReplay
    {
        [Space(30)][Header("Filename")] public string forwardFile;
        public string backwardFile;
        public string liftFile;
        public string leftFile;
        public string rightFile;

        #region Traces

        private InputEventTrace forwardET;
        private InputEventTrace backwardET;
        private InputEventTrace liftET;
        private InputEventTrace leftET;
        private InputEventTrace rightET;

        #endregion

        #region Controllers

        private InputEventTrace.ReplayController forwardController;
        private InputEventTrace.ReplayController backwardController;
        private InputEventTrace.ReplayController liftController;
        private InputEventTrace.ReplayController leftController;
        private InputEventTrace.ReplayController rightController;



        #endregion

        #region Override functions

        public override void InitTraces()
        {
            print("InitTraces");
            InitTrace(ref forwardET, forwardFile);
            InitTrace(ref backwardET, backwardFile);
            InitTrace(ref liftET, liftFile);
            InitTrace(ref leftET, leftFile);
            InitTrace(ref rightET, rightFile);
        }

        public override void InitControllers()
        {
            print("InitControllers");
            forwardController = InitController(ref forwardET, OnFinished, OnEvent);
            backwardController = InitController(ref backwardET, OnFinished, OnEvent);
            liftController = InitController(ref liftET, OnFinished, OnEvent);
            leftController = InitController(ref leftET, OnFinished, OnEvent);
            rightController = InitController(ref rightET, OnFinished, OnEvent);
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

        public void Forward(bool loop = false)
        {
            StartReplay(ref forwardController, loop: loop);
        }

        public void Backward(bool loop = false)
        {
            StartReplay(ref backwardController, loop: loop);
        }

        public void Left(bool stop = false, bool loop = false)
        {
            StartReplay(ref leftController, loop: loop, stop: stop);
        }

        public void Right(bool stop = false, bool loop = false)
        {
            StartReplay(ref rightController, loop: loop, stop: stop);
        }

        public void Lift(bool loop = false)
        {
            StartReplay(ref liftController, loop: loop);
        }

        public void RewindPickup()
        {
            ForceRewind(ref liftController);
        }

        public bool PickupFinished()
        {
            return IsFinished(ref liftController, true);
        }



        #endregion

        #region Debug

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Start"))
            {
                //StartReplay(ref forwardController);
            }
        }

        #endregion
    }
}
