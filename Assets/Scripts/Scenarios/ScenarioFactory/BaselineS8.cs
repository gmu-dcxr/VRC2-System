using NodeCanvas.BehaviourTrees;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS8 : Scenario
    {
        [Header("TruckPositions")] private Transform Start1;
        private Transform Finish1;

        public GameObject Truck;
        public GameObject waterLeak;
        public GameObject liveWire;
        private float speed = 6f;

        private bool backingUp = false;
        private bool moveFoward = false;
        private int trips = 0;
        private int maxTrips = 5;


        [Header("Player")] public GameObject player;



        private void Start()
        {
            base.Start();
            //Find positions
            Start1 = GameObject.Find("Start").transform;
            Finish1 = GameObject.Find("Finish").transform;
            waterLeak.SetActive(false);
        }

        private void Update()
        {
            if (trips < maxTrips)
            {
                if (backingUp)
                {
                    //back up until each finish, then move foward, waterLeak increases each time    
                    if (Truck.transform.position.z != (Finish1.position.z))
                    {
                        Truck.transform.position = Vector3.MoveTowards(Truck.transform.position, Finish1.transform.position,
                            speed * Time.deltaTime);
                    }
                    if (Truck.transform.position.z == (Finish1.position.z))
                    {
                        backingUp = false;
                        moveFoward = true;
                        //leakWater(waterLeak);
                    }
                }

                if (moveFoward)
                {
                    if (Truck.transform.position.z != Start1.position.z)
                    {
                        Truck.transform.position = Vector3.MoveTowards(Truck.transform.position, Start1.transform.position,
                            speed * Time.deltaTime);
                    }
                    if (Truck.transform.position.z == (Start1.position.z))
                    {
                        moveFoward = false;
                        backingUp = true;
                        leakWater(waterLeak);
                        trips++;
                    }
                }
            }
        }
        //runs every time the truck reaches a position (simple)
        private void leakWater(GameObject waterLeak)
        {
            float yValue = waterLeak.transform.localScale.y;
            Renderer r = waterLeak.GetComponent<Renderer>();
            Material m = r.material;

            if (!waterLeak.activeSelf) 
            { 
                waterLeak.SetActive(true); 
            }
            // increase waterLeak size and scale texture
            else 
            {
                waterLeak.transform.localScale += new Vector3(1.0f, 0.0f, 0.0f);
                m.SetTextureScale("_MainTex", new Vector2(waterLeak.transform.localScale.x, waterLeak.transform.localScale.y));
            }
        }


        #region Accident Events Callbacks

        public void On_BaselineS8_1_Start()
        {

        }

        public void On_BaselineS8_1_Finish()
        {

        }

        public void On_BaselineS8_2_Start()
        {
            print("On_BaselineS8_2_Start");
            // A truck backs up to carry the mud and leaves, and the water leaks along the path the truck goes.Repeat this multiple times, so the water stain on the ground grows bigger and it is approaching the wire on the ground.
            // get incident
            var incident = GetIncident(2);

            backingUp = true;

        }

        public void On_BaselineS8_2_Finish()
        {
            // A truck backs up to carry the mud and leaves, and the water leaks along the path the truck goes.Repeat this multiple times, so the water stain on the ground grows bigger and it is approaching the wire on the ground.
        }

        public void On_BaselineS8_3_Start()
        {
            print("On_BaselineS8_3_Start");
            // Repeat every time when participants are passing the road.
            // get incident
            var incident = GetIncident(3);
            var warning = incident.Warning;
            print(warning);
        }

        public void On_BaselineS8_3_Finish()
        {
            // Repeat every time when participants are passing the road.

        }


        public void On_BaselineS8_4_Start()
        {
            print("On_BaselineS8_4_Start");
            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS8_4_Finish()
        {
            // SAGAT query
            HideSAGAT();
        }

        #endregion

    }
}