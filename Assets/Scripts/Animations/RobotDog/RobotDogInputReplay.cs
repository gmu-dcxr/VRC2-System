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

        [Space(30)] [Header("Settings")] public float rotationOffset = 90; // pipe.y - dog.y
        public float positionOffset = 1; // pos.y - dog.y

        #region Traces

        private InputEventTrace forwardET;
        private InputEventTrace leftET;
        private InputEventTrace rightET;
        private InputEventTrace leftTurnET;
        private InputEventTrace rightTurnET;
        private InputEventTrace pickupET;
        private InputEventTrace dropoffET;

        #endregion

        #region Controllers

        private InputEventTrace.ReplayController forwardController;
        private InputEventTrace.ReplayController leftController;
        private InputEventTrace.ReplayController rightController;
        private InputEventTrace.ReplayController leftTurnController;
        private InputEventTrace.ReplayController rightTurnController;
        private InputEventTrace.ReplayController pickupController;
        private InputEventTrace.ReplayController dropoffController;



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

        #endregion

        #region Debug

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Start"))
            {
                StartReplay(ref pickupController);
            }
        }

        #endregion
    }
}