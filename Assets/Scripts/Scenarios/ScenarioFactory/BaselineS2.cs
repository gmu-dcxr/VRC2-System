using Assets.OVR.Scripts;
using PathCreation.Examples;
using TMPro;
using UnityEngine;
using UnityTimer;
using VRC2.Events;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS2 : Scenario
    {
        [Header("Drone")] public GameObject drone;
        public float speed;

        [Space(30)] [Header("Instruction Sheet")]
        public GameObject sheetObject;

        public InstructionSheetGrabbingCallback _sheetCallback;
        public string title;
        public string instruction;

        private GameObject player;

        private Vector3 dronePosition;

        private bool moving = false;
        private bool goBack = false;

        private Timer _timer;

        private AudioSource _audioSource;

        private PathFollower _pathFollower;

        private Vector3 targetPosition
        {
            get
            {
                var pos = player.transform.position;
                pos += player.transform.forward;
                pos.y += 2.0f; // minimal height
                return pos;
            }
        }

        void Start()
        {
            base.Start();

            player = localPlayer;

            dronePosition = drone.transform.position;

            moving = false;
            goBack = false;

            drone.SetActive(true);

            _pathFollower = drone.GetComponent<PathFollower>();

            // disable at the beginning
            _pathFollower.enabled = false;

            _audioSource = GetComponent<AudioSource>();

            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }

        void StartTimer(int second)
        {
            if (_timer != null)
            {
                Timer.Cancel(_timer);
            }

            _timer = Timer.Register(second, () =>
            {
                moving = true;
                goBack = true;
            }, isLooped: false, useRealTime: true);

        }

        private void Update()
        {
            // droneMove();
            if (moving)
            {
                if (goBack)
                {
                    drone.transform.position = Vector3.MoveTowards(drone.transform.position, dronePosition,
                        speed * Time.deltaTime);

                    if (Vector3.Distance(drone.transform.position, dronePosition) < 0.1f)
                    {
                        // reach the destination
                        moving = false;

                        drone.SetActive(false);
                    }
                }
                else
                {
                    drone.transform.position = Vector3.MoveTowards(drone.transform.position, targetPosition,
                        speed * Time.deltaTime);

                    if (Vector3.Distance(drone.transform.position, targetPosition) < 0.1f)
                    {
                        // reach the target
                        UpdateInstruction();

                        moving = false;
                        // wait a moment
                        StartTimer(5);
                    }
                }
            }
        }


        #region Accident Events Callbacks

        void UpdateInstruction()
        {
            print("The plan of installment order changed");

            // update instruction sheet title and content
            _sheetCallback.title = title;
            _sheetCallback.content = instruction;
            sheetObject.SetActive(true);

            _audioSource.Play();
        }

        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S2");
            _pathFollower.enabled = true;
        }

        public void On_BaselineS2_1_Start()
        {
            print("On_BaselineS2_1_Start");
        }

        public void On_BaselineS2_1_Finish()
        {

        }

        public void On_BaselineS2_2_Start()
        {
            print("On_BaselineS2_2_Start");
            // A supervising drone approaches the participants, and informs the second participant of a plan of installment change order. Gives the second participant a new sheet of requirements.

            // disable it
            _pathFollower.enabled = false;

            // get incident
            var incident = GetIncident(2);
            var warning = incident;
            print(warning);

            drone.SetActive(true);

            moving = true;
            goBack = false;
        }

        public void On_BaselineS2_2_Finish()
        {
            // A supervising drone approaches the participants, and informs the second participant of a plan of installment change order. Gives the second participant a new sheet of requirements.
            moving = false;
        }

        public void On_BaselineS2_3_Start()
        {
            print("On_BaselineS2_3_Start");
            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS2_3_Finish()
        {
            // SAGAT query.
            HideSAGAT();
        }


        #endregion
    }
}