using UnityEngine;
using VRC2.Animations;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;
using static VRC2.Scenarios.ScenarioFactory.BaselineS7;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class Background : Scenario
    {
        [Header("Excavator")] public ExcavatorController excavatorController;

        [Header("Forklift")] public CustomForkLiftController customForkLiftController;
        public WSMVehicleController ForkLiftMovementController;
        public ForkliftController ForkLiftController;

        [Header("Hammer")] public HammerController hammerController;

        

        internal enum ForkliftStage
        {
            Stop = 0,
            Forward = 1,
            Backward = 2,
            Lift = 3,
            Left = 4,
            Right = 5,
        }

        [Space(30)]
        [Header("Recording/Replay")]
        public GameObject forklift;

        public ForkliftInputRecording recording;


        public ForkliftInputReplay replay;

        public Transform startPoint;
        public Transform liftPoint;

        private bool isLifting = false;
        private float distanceThreshold = 3.0f;

        private ForkliftStage _stage;

        private void Start()
        {
            base.Start();
            _stage = ForkliftStage.Stop;
        }

        private void Update()
        {
            switch (_stage)
            {
                case ForkliftStage.Stop:
                    //replay.Stop();
                    if (ReachDestination(liftPoint.position) && (recording.vehicleController.AccelerationInput == 0))
                    {
                        isLifting = false;
                        // time to lift
                        _stage = ForkliftStage.Lift;
                    }

                    break;
                case ForkliftStage.Forward:
                    //replay.Forward(true);
                    if (ReachDestination(startPoint.position))
                    {
                        _stage = ForkliftStage.Stop;
                    }

                    break;
                case ForkliftStage.Backward:
                    if (ReachDestination(liftPoint.position))
                    {
                        _stage = ForkliftStage.Stop;
                    }

                    break;

                case ForkliftStage.Lift:
                    if (!isLifting)
                    {
                        isLifting = true;
                    }
                    else
                    {
                        if (replay.PickupFinished())
                        {
                            _stage = ForkliftStage.Forward;
                        }
                    }

                    break;
            }
        }

        bool ReachDestination(Vector3 des)
        {
            var t = forklift.transform.position;

            // use the same y
            des.y = 0;
            t.y = 0;
            var distance = Vector3.Distance(t, des);

            if (distance < distanceThreshold)
            {
                return true;
            }

            return false;
        }

        #region Accident Events Callbacks

        public void On_Background_1_Start()
        {
            excavatorController.Animate();
        }

        public void On_Background_1_Finish()
        {

        }

        public void On_Background_2_Start()
        {
            customForkLiftController.Animate();
        }

        public void On_Background_2_Finish()
        {

        }

        public void On_Background_3_Start()
        {
            hammerController.Animate();
        }

        public void On_Background_3_Finish()
        {

        }

        #endregion
    }
}