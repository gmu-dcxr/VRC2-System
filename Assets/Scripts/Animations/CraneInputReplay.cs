using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace VRC2.Animations
{
    public class CraneInputReplay : BaseInputReplay
    {
        [Space(30)] [Header("Filename")] public string forwardFile;
        public string backwardFile;
        public string pickupFile;
        public string dropoffFile;
        public string leftFile;
        public string rightFile;

        #region Traces

        private InputEventTrace forwardET;
        private InputEventTrace backwardET;
        private InputEventTrace pickupET;
        private InputEventTrace dropoffET;
        private InputEventTrace leftET;
        private InputEventTrace rightET;

        #endregion

        #region Controllers

        private InputEventTrace.ReplayController forwardController;
        private InputEventTrace.ReplayController backwardController;
        private InputEventTrace.ReplayController pickupController;
        private InputEventTrace.ReplayController dropoffController;
        private InputEventTrace.ReplayController leftController;
        private InputEventTrace.ReplayController rightController;



        #endregion

        #region Override functions

        public override void InitTraces()
        {
            print("InitTraces");
            InitTrace(ref forwardET, forwardFile);
            InitTrace(ref backwardET, backwardFile);
            InitTrace(ref pickupET, pickupFile);
            InitTrace(ref dropoffET, dropoffFile);
            InitTrace(ref leftET, leftFile);
            InitTrace(ref rightET, rightFile);
        }

        public override void InitControllers()
        {
            print("InitControllers");
            forwardController = InitController(ref forwardET, OnFinished, OnEvent);
            backwardController = InitController(ref backwardET, OnFinished, OnEvent);
            pickupController = InitController(ref pickupET, OnFinished, OnEvent);
            dropoffController = InitController(ref dropoffET, OnFinished, OnEvent);
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

        public void Pickup(bool loop = false)
        {
            StartReplay(ref pickupController, loop: loop);
        }

        public void RewindPickup()
        {
            ForceRewind(ref pickupController);
        }

        public void RewindDropoff()
        {
            ForceRewind(ref dropoffController);
        }

        public void Dropoff(bool loop = false)
        {
            StartReplay(ref dropoffController, loop: loop);
        }

        public bool PickupFinished()
        {
            return IsFinished(ref pickupController, true);
        }

        public bool DropoffFinished()
        {
            return IsFinished(ref dropoffController, true);
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