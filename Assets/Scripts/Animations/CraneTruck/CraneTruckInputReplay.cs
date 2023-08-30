using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace VRC2.Animations.CraneTruck
{
    public class CraneTruckInputReplay : BaseInputReplay
    {
        [Space(30)] [Header("Filename")] public string forwardFile;
        public string backwardFile;
        public string leftFile;
        public string rightFile;
        public string stopFile;
        public string dropoffFile;

        #region Traces

        private InputEventTrace forwardET;
        private InputEventTrace backwardET;
        private InputEventTrace leftET;
        private InputEventTrace rightET;
        private InputEventTrace stopET;
        private InputEventTrace dropoffET;

        #endregion

        #region Controllers

        private InputEventTrace.ReplayController forwardController;
        private InputEventTrace.ReplayController backwardController;
        private InputEventTrace.ReplayController leftController;
        private InputEventTrace.ReplayController rightController;
        private InputEventTrace.ReplayController stopController;
        private InputEventTrace.ReplayController dropoffController;



        #endregion

        #region Override functions

        public override void InitTraces()
        {
            print("InitTraces");
            InitTrace(ref forwardET, forwardFile);
            InitTrace(ref backwardET, backwardFile);
            InitTrace(ref leftET, leftFile);
            InitTrace(ref rightET, rightFile);
            InitTrace(ref stopET, stopFile);
            InitTrace(ref dropoffET, dropoffFile);
        }

        public override void InitControllers()
        {
            print("InitControllers");
            forwardController = InitController(ref forwardET, OnFinished, OnEvent);
            backwardController = InitController(ref backwardET, OnFinished, OnEvent);
            leftController = InitController(ref leftET, OnFinished, OnEvent);
            rightController = InitController(ref rightET, OnFinished, OnEvent);
            stopController = InitController(ref stopET, OnFinished, OnEvent);
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

        #region Replay events

        public void Forward(bool loop = false)
        {
            StartReplay(ref forwardController, loop);
        }

        public void Backward(bool loop = false)
        {
            StartReplay(ref backwardController, loop);
        }

        public void Stop()
        {
            StartReplay(ref stopController);
        }

        public void LeftForward(bool loop = false)
        {
            StartReplay(ref forwardController, loop);
            StartReplay(ref leftController, loop);
        }

        public void RightForward(bool loop = false)
        {
            StartReplay(ref forwardController, loop);
            StartReplay(ref rightController, loop);
        }

        public void Dropoff()
        {
            StartReplay(ref dropoffController);
        }

        #endregion
    }
}