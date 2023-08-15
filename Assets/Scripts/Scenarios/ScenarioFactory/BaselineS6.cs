using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

using UnityTimer;
using Timer = UnityTimer.Timer;


namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS6 : Scenario
    {

        //Actaully Using
        public GameObject pipes;
        public GameObject crane;
        public GameObject hook;
        public Wind scriptWind;
        private ConfigurableJoint _jointHook;

        private ConfigurableJoint jointHook
        {
            get
            {
                if (_jointHook == null)
                {
                    _jointHook = hook.GetComponent<ConfigurableJoint>();
                }

                return _jointHook;
            }
        }

        private TowerControllerCrane scriptTCC;
        public Transform startingRotation;
        public Transform endingRotation;
        private Transform boomCart;
        private bool clockWise; //From above
        private bool canRotate;
        public float rotationSpeed = 60f;
        public float hookOffset = 5f;
        public float windSpeed = 1f;
        private float boomCartPositionX;
        //end Actually Using        

        private GameObject player;

        private Transform _craneSwivel;

        private Transform craneSwivel
        {
            get
            {
                if (_craneSwivel == null)
                {
                    _craneSwivel = crane.GetComponent<TowerControllerCrane>().rotationElementCrane;
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
            // Handle Roatating Crane
            if (canRotate)
            {
                print("can rotate");
                if (clockWise)
                {
                    print("clockWise");
                    craneSwivel.rotation = Quaternion.RotateTowards(craneSwivel.rotation, endingRotation.rotation,
                        rotationSpeed * Time.deltaTime);
                }
                else
                {
                    print("!clockWise");
                    craneSwivel.rotation = Quaternion.RotateTowards(craneSwivel.rotation, startingRotation.rotation,
                        rotationSpeed * Time.deltaTime);
                }
            }

            jointHook.anchor = new Vector3(0, hookOffset, 0);

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

            //TriggerEvent(wind1, true, false);
            clockWise = false;
            canRotate = true;
            scriptWind.windForce = 100f;
            pipes.SetActive(true);
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

            //TriggerEvent(wind1, false, true);
            clockWise = true;
            pipes.SetActive(false);
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

            //TriggerEvent(wind2, true, false);
            clockWise = false;
            pipes.SetActive(true);
            scriptWind.windForce = 1000f;
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

            //TriggerEvent(wind2, false, true);
            clockWise = true;
            pipes.SetActive(false);
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

            //TriggerEvent(wind3, true, false);
            clockWise = false;
            pipes.SetActive(true);
            scriptWind.windForce = 10000f;
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