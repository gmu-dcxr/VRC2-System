using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace VRC2.Animations
{
    public class RobotDogInputReplay : BaseInputReplay
    {
        [Space(30)] [Header("Filename")] public string forwardFile;
        public string leftFile;
        public string rightFile;
        public string leftTurnFile;
        public string rightTurnFile;
        public string pickupFile;
        public string dropoffFile;
        public string stopFile;

        [Space(30)] [Header("Settings")] public float rotationOffset = 90; // pipe.y - dog.y
        public float positionOffset = 1; // pos.z - dog.z

        #region Traces

        private InputEventTrace forwardET;
        private InputEventTrace leftET;
        private InputEventTrace rightET;
        private InputEventTrace leftTurnET;
        private InputEventTrace rightTurnET;
        private InputEventTrace pickupET;
        private InputEventTrace dropoffET;
        private InputEventTrace stopET;

        #endregion

        #region Controllers

        private InputEventTrace.ReplayController forwardController;
        private InputEventTrace.ReplayController leftController;
        private InputEventTrace.ReplayController rightController;
        private InputEventTrace.ReplayController leftTurnController;
        private InputEventTrace.ReplayController rightTurnController;
        private InputEventTrace.ReplayController pickupController;
        private InputEventTrace.ReplayController dropoffController;
        private InputEventTrace.ReplayController stopController;



        #endregion

        #region Override functions

        public override void InitTraces()
        {
            InitTrace(ref forwardET, forwardFile);
            InitTrace(ref leftET, leftFile);
            InitTrace(ref rightET, rightFile);
            InitTrace(ref leftTurnET, leftTurnFile);
            InitTrace(ref rightTurnET, rightTurnFile);
            InitTrace(ref pickupET, pickupFile);
            InitTrace(ref dropoffET, dropoffFile);
            InitTrace(ref stopET, stopFile);
        }

        public override void InitControllers()
        {
            forwardController = InitController(ref forwardET, OnFinished, OnEvent);
            leftController = InitController(ref leftET, OnFinished, OnEvent);
            rightController = InitController(ref rightET, OnFinished, OnEvent);
            leftTurnController = InitController(ref leftTurnET, OnFinished, OnEvent);
            rightTurnController = InitController(ref rightTurnET, OnFinished, OnEvent);
            pickupController = InitController(ref pickupET, OnFinished, OnEvent);
            dropoffController = InitController(ref dropoffET, OnFinished, OnEvent);
            stopController = InitController(ref stopET, OnFinished, OnEvent);
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

        public void Forward(bool loop = false, bool stop = false)
        {
            StartReplay(ref forwardController, loop, stop);
        }

        public void Left(bool loop = false, bool stop = false)
        {
            StartReplay(ref leftController, loop, stop);
        }

        public void Right(bool loop = false, bool stop = false)
        {
            StartReplay(ref rightController, loop, stop);
        }

        public void LeftTurn(bool loop = false, bool stop = false)
        {
            StartReplay(ref leftTurnController, loop, stop);
        }

        public void RightTurn(bool loop = false, bool stop = false)
        {
            StartReplay(ref rightTurnController, loop, stop);
        }

        public void Stop(bool loop = false)
        {
            StartReplay(ref stopController, loop, false);
        }

        public void Pickup()
        {
            StartReplay(ref pickupController);
        }

        public void Dropoff()
        {
            StartReplay(ref dropoffController);
        }

        #endregion

        #region Debug

        private void OnGUI()
        {
            // if (GUI.Button(new Rect(10, 10, 100, 50), "Start"))
            // {
            //     StartReplay(ref pickupController);
            // }
        }

        #endregion
    }
}