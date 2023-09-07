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

        // public string pickupFile;
        public string dropoffFile;
        public string stopFile;

        [Space(30)] [Header("Pickup / Dropoff")]
        public string pickupEFile;

        public string pickupWFile;
        public string pickupSFile;
        public string pickupQFile;
        public string pickupCloseFile;
        public string pickupReleaseFile;

        [Space(30)] [Header("Settings")] public float rotationOffset = 90; // pipe.y - dog.y
        public float positionOffset = 1; // pos.z - dog.z

        #region Traces

        private InputEventTrace forwardET;
        private InputEventTrace leftET;
        private InputEventTrace rightET;
        private InputEventTrace leftTurnET;

        private InputEventTrace rightTurnET;

        // private InputEventTrace pickupET;
        private InputEventTrace dropoffET;
        private InputEventTrace stopET;

        private InputEventTrace pickupEET;
        private InputEventTrace pickupWET;
        private InputEventTrace pickupSET;
        private InputEventTrace pickupQET;
        private InputEventTrace pickupCloseET;
        private InputEventTrace pickupReleaseET;

        #endregion

        #region Controllers

        private InputEventTrace.ReplayController forwardController;
        private InputEventTrace.ReplayController leftController;
        private InputEventTrace.ReplayController rightController;
        private InputEventTrace.ReplayController leftTurnController;

        private InputEventTrace.ReplayController rightTurnController;

        // private InputEventTrace.ReplayController pickupController;
        private InputEventTrace.ReplayController dropoffController;
        private InputEventTrace.ReplayController stopController;

        private InputEventTrace.ReplayController pickupEController;
        private InputEventTrace.ReplayController pickupWController;
        private InputEventTrace.ReplayController pickupSController;
        private InputEventTrace.ReplayController pickupQController;
        private InputEventTrace.ReplayController pickupCloseController;
        private InputEventTrace.ReplayController pickupReleaseController;

        #endregion

        #region Pickup / Dropoff
        
        // initial rotation, part 1 (0,270,0)      , part 2 (0,0,0)
        // pickup rotation , part 1 (0,270,170) - W, part 2 (0,0,90) - E 
        // holding rotation, part 1 (0,270,200) - S, part 2 (0,0,70) - Q

        private float part1Pickup = 170;
        private float part2Pickup = 90;
        private float part1Holding = 200;
        private float part2Holding = 70;



        #endregion


        #region Override functions

        public override void InitTraces()
        {
            InitTrace(ref forwardET, forwardFile);
            InitTrace(ref leftET, leftFile);
            InitTrace(ref rightET, rightFile);
            InitTrace(ref leftTurnET, leftTurnFile);
            InitTrace(ref rightTurnET, rightTurnFile);
            // InitTrace(ref pickupET, pickupFile);
            InitTrace(ref dropoffET, dropoffFile);
            InitTrace(ref stopET, stopFile);

            InitTrace(ref pickupEET, pickupEFile);
            InitTrace(ref pickupWET, pickupWFile);
            InitTrace(ref pickupSET, pickupSFile);
            InitTrace(ref pickupQET, pickupQFile);
            InitTrace(ref pickupCloseET, pickupCloseFile);
            InitTrace(ref pickupReleaseET, pickupReleaseFile);
        }

        public override void InitControllers()
        {
            forwardController = InitController(ref forwardET, OnFinished, OnEvent);
            leftController = InitController(ref leftET, OnFinished, OnEvent);
            rightController = InitController(ref rightET, OnFinished, OnEvent);
            leftTurnController = InitController(ref leftTurnET, OnFinished, OnEvent);
            rightTurnController = InitController(ref rightTurnET, OnFinished, OnEvent);
            // pickupController = InitController(ref pickupET, OnFinished, OnEvent);
            dropoffController = InitController(ref dropoffET, OnFinished, OnEvent);
            stopController = InitController(ref stopET, OnFinished, OnEvent);

            pickupEController = InitController(ref pickupEET, OnFinished, OnEvent);
            pickupWController = InitController(ref pickupWET, OnFinished, OnEvent);
            pickupQController = InitController(ref pickupQET, OnFinished, OnEvent);
            pickupSController = InitController(ref pickupSET, OnFinished, OnEvent);
            pickupCloseController = InitController(ref pickupCloseET, OnFinished, OnEvent);
            pickupReleaseController = InitController(ref pickupReleaseET, OnFinished, OnEvent);
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
            // StartReplay(ref pickupController);
        }

        public void RewindPickup()
        {
            // ForceRewind(ref pickupController);
        }

        public bool PickupDone()
        {
            // return IsFinished(ref pickupController, true);
        }

        public bool DropoffDone()
        {
            return IsFinished(ref dropoffController, true);
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