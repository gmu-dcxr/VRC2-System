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
        public GameObject unpackedPipe;
        public GameObject crane;
        public GameObject hook;
        public GameObject windZone;
        private ConfigurableJoint jointHook;
        private TowerControllerCrane scriptTCC;
        private wind scriptWind;
        public Transform startingRotation;
        public Transform endingRotation;
        private Transform craneSwivel;
        private Transform boomCart;
        private bool clockWise; //From above
        private bool canRotate;
        public float rotationSpeed = 60f;
        public float hookOffset = 5f;
        public float boomCartOffset = -8f;
        public float windSpeed = 1f;
        private float boomCartPositionX;
        //end Actually Using        

        private GameObject player;

        private void Start()
        {

            base.Start();

            player = localPlayer;

            scriptTCC = crane.GetComponent<TowerControllerCrane>();
            scriptWind = windZone.GetComponent<wind>();
            jointHook = hook.GetComponent<ConfigurableJoint>();
            boomCart = scriptTCC.boomCart;
            clockWise = true;
            canRotate = true;
            jointHook.anchor = new Vector3(0, hookOffset, 0);
            boomCart.localPosition += new Vector3(boomCartOffset, 0f, 0f);
            boomCartPositionX = boomCart.localPosition.x;
            craneSwivel = scriptTCC.rotationElementCrane.transform;           

            unpackedPipe.SetActive(false);
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
            unpackedPipe.SetActive(true);
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
            unpackedPipe.SetActive(false);
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
            unpackedPipe.SetActive(true);
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
            unpackedPipe.SetActive(false);
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
            unpackedPipe.SetActive(true);
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