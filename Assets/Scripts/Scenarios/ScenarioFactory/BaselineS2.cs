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

        private float hoveringThreshold = 0.1f;

        private float normalHeight = 7f;
        private float changeOrderHeight = 6f;

        private bool moving = false;
        private bool goBack = false;

        private Timer _timer;

        private AudioSource _audioSource;

        private PathFollower _pathFollower;

        // resolve the conflicts between of BaselineS2 and BaselineS4
        [HideInInspector] public bool controllingDrone = true;

        [HideInInspector] public float leaveAfter = 30f; // leave after 30 second

        [HideInInspector] public bool changeOrder = false; // indicate if it is change order

        private Vector3 targetPosition
        {
            get
            {
                var pos = player.transform.position;
                pos += player.transform.forward;
                pos.y += changeOrderHeight; // minimal height
                return pos;
            }
        }

        void Start()
        {
            base.Start();

            player = CenterEyeTransform.gameObject;

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

        void StartTimer(float second)
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
            if (!controllingDrone) return;

            // droneMove();
            if (moving)
            {
                if (goBack)
                {
                    drone.transform.position = Vector3.MoveTowards(drone.transform.position, dronePosition,
                        speed * Time.deltaTime);

                    if (Vector3.Distance(drone.transform.position, dronePosition) < hoveringThreshold)
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

                    if (Vector3.Distance(drone.transform.position, targetPosition) < hoveringThreshold)
                    {
                        if (changeOrder)
                        {
                            // reach the target
                            UpdateInstruction();   
                        }

                        moving = false;
                        // wait a moment, leave after 20 seconds
                        // this is defined in Supervising Drone normals 4
                        StartTimer(leaveAfter);
                    }
                }
            }
        }

        void UpdateDrone(float heightoffset)
        {
            // enable it
            drone.SetActive(true);

            // calculate height
            var pos = player.transform.position;
            pos.y += heightoffset;

            // update drone height
            var dronePos = drone.transform.position;
            dronePos.y = pos.y;

            drone.transform.position = dronePos;
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
            drone.SetActive(true);
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

            if (changeOrder)
            {
                UpdateDrone(changeOrderHeight);   
            }
            else
            {
                // set normal height
                UpdateDrone(normalHeight);
            }

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
            // start normal event
            StartNormalIncident();

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