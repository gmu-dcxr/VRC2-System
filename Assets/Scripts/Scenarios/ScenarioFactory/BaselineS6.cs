using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

using UnityTimer;
using VRC2.Animations;
using Timer = UnityTimer.Timer;


namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS6 : Scenario
    {
        [Header("Wind level")] public float level1 = 20;
        public float level2 = 100;
        public float level3 = 300;

        [Space(30)] public CraneInputRecording recording;
        public CraneInputReplay replay;

        public float startAngle = 180f;
        public float endAngle = 120f;

        [Space(30)]
        //Actaully Using
        public GameObject pipes;

        public GameObject crane;
        public GameObject hook;
        public GameObject decayHookOn;
        public Wind scriptWind;


        private bool clockWise; //From above
        private bool canRotate;
        //end Actually Using        

        private GameObject player;

        private Transform _craneSwivel;

        // distance between the cargo and the hook to make the cargo grabbable
        private float cargoOffset = 3.5f; // to make Decay Hook On visible
        private bool _connected = false;

        private MeshRenderer _pipeMeshRenderer;

        private MeshRenderer pipeMeshRenderer
        {
            get
            {
                if (_pipeMeshRenderer == null)
                {
                    _pipeMeshRenderer = pipes.GetComponent<MeshRenderer>();
                }

                return _pipeMeshRenderer;
            }
        }

        private Transform craneSwivel
        {
            get
            {
                if (_craneSwivel == null)
                {
                    _craneSwivel = recording.rotationElementCrane;
                }

                return _craneSwivel;
            }
        }



        private void Start()
        {

            base.Start();
            player = localPlayer;

            clockWise = true;
            canRotate = true;
        }

        private void Update()
        {
            if (recording.Ready && !_connected)
            {
                ConnectCargo(pipes);
            }

            if (_connected && canRotate)
            {
                RotateCrane(clockWise);
            }
        }

        void RotateCrane(bool clockWise)
        {
            if (clockWise)
            {
                replay.Left(true);
                replay.Right(false, true);
            }
            else
            {
                replay.Right(true);
                replay.Left(false, true);
            }
        }

        void SetPipeActiveness(bool enable)
        {
            pipeMeshRenderer.enabled = enable;
        }

        #region Connect Cargo

        void ConnectCargo(GameObject cargo)
        {
            print("connect cargo");

            // move the cargo under the hook
            var pos = hook.transform.position;
            var upward = hook.transform.up;

            cargo.transform.position = pos + upward * -cargoOffset;

            if (decayHookOn.activeSelf)
            {
                recording.ManuallyConnectCargo();
                _connected = true;
            }
        }



        #endregion

        void ResetCraneRotation(float angle)
        {
            // var rot = craneSwivel.transform.localRotation.eulerAngles;
            // rot.y = angle;
            // craneSwivel.transform.localRotation = Quaternion.Euler(rot);

            recording.ForceUpdateRotation(angle);
        }

        float GetCurrentAngle()
        {
            return craneSwivel.transform.localRotation.eulerAngles.y;
        }


        #region Accident Events Callbacks

        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S6");

        }

        public void On_BaselineS6_1_Start()
        {

        }

        public void On_BaselineS6_1_Finish()
        {
        }

        public void On_BaselineS6_2_Start()
        {
            print("On_BaselineS6_2_Start");
            // A load is passing overhead, and it swings a little bit due to the wind.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;
            print(warning);

            ResetCraneRotation(startAngle);

            clockWise = false;
            canRotate = true;
            scriptWind.windForce = level1;
            SetPipeActiveness(true);
        }

        public void On_BaselineS6_2_Finish()
        {
            // A load is passing overhead, and it swings a little bit due to the wind.
        }

        public void On_BaselineS6_3_Start()
        {
            print("On_BaselineS6_3_Start");
            // The hook without a load is passing in the opposite direction.
            // get incident
            var incident = GetIncident(3);

            ResetCraneRotation(endAngle);
            clockWise = true;
            SetPipeActiveness(false);
        }

        public void On_BaselineS6_3_Finish()
        {
            // The hook without a load is passing in the opposite direction.
        }

        public void On_BaselineS6_4_Start()
        {
            print("On_BaselineS6_4_Start");
            // Another load is passing overhead, and it swings bigger due to the sudden wind. The load stops until it is static.
            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);

            ResetCraneRotation(startAngle);
            clockWise = false;
            SetPipeActiveness(true);
            scriptWind.windForce = level2;
        }

        public void On_BaselineS6_4_Finish()
        {
            // Another load is passing overhead, and it swings bigger due to the sudden wind. The load stops until it is static.
        }

        public void On_BaselineS6_5_Start()
        {
            print("On_BaselineS6_5_Start");
            // The hook without a load is passing in the opposite direction..
            // get incident
            var incident = GetIncident(5);

            ResetCraneRotation(endAngle);
            //TriggerEvent(wind2, false, true);
            clockWise = true;
            SetPipeActiveness(false);
        }

        public void On_BaselineS6_5_Finish()
        {
            // The hook without a load is passing in the opposite direction..
        }

        public void On_BaselineS6_6_Start()
        {
            print("On_BaselineS6_6_Start");
            // Another load is passing overhead, it swings even stronger due to the sudden wind, and is about to hit the power line.
            // get incident
            var incident = GetIncident(6);
            var warning = incident.Warning;

            ResetCraneRotation(startAngle);
            clockWise = false;
            SetPipeActiveness(true);
            scriptWind.windForce = level3;
        }

        public void On_BaselineS6_6_Finish()
        {
            // Another load is passing overhead, it swings even stronger due to the sudden wind, and is about to hit the power line.
        }

        public void On_BaselineS6_7_Start()
        {
            print("On_BaselineS6_7_Start");

            //SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS6_7_Finish()
        {
            HideSAGAT();
        }

        #endregion

    }
}