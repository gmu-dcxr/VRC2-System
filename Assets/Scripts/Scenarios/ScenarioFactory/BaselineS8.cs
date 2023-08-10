using NodeCanvas.BehaviourTrees;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS8 : Scenario
    {
        public GameObject truck;
        public GameObject muds;

        public GameObject destination;

        public GameObject warningPosition;

        private Vector3 startPos;
        private Quaternion startRotation;
        private Vector3 destinationPos;

        public GameObject waterLeak;
        public GameObject liveWire;
        private float speed = 6f;

        private bool backingUp = false;
        private bool moveFoward = false;
        private int trips = 0;
        private int maxTrips = 5;


        private GameObject player;

        private WSMVehicleController _vehicleController;

        private bool isStarted = false;
        private bool moving = false;
        private bool back = false;

        private float distanceThreshold = 5.0f;
        private float leakWaterThreshold = 5.0f;
        private float warningThreshold = 2.0f;



        private void Start()
        {
            base.Start();

            player = localPlayer;

            startPos = truck.transform.position;
            startRotation = truck.transform.rotation;

            destinationPos = destination.transform.position;

            _vehicleController = truck.GetComponent<WSMVehicleController>();


            waterLeak.SetActive(false);
        }

        private void Update()
        {
            // player approch warning
            PlayerApproachingWarning();

            if (!isStarted) return;

            if (!moving)
            {
                StopVehicle();
                // if stopped
                if (TruckStopped())
                {
                    // change direction
                    moving = true;

                    if (!back)
                    {
                        // forward to the start point
                        // reset truck postion and rotation
                        truck.transform.position = startPos;
                        truck.transform.rotation = startRotation;

                        // increase leak water
                        IncreaseLeakWater();
                    }

                    // change direction
                    back = !back;

                    // only active when moving forward
                    muds.SetActive(!back);
                }
            }
            else
            {
                MoveForward(!back);
            }
        }

        #region Warning Detection

        void PlayerApproachingWarning()
        {
            var t = player.transform.position;
            var w = warningPosition.transform.position;

            // ignore y distance
            t.y = 0;
            w.y = 0;

            if (Vector3.Distance(t, w) < warningThreshold)
            {
                var msg = GetRightMessage(3, scenariosManager.condition.Context, scenariosManager.condition.Amount);
                if (!warningShowing)
                {
                    ShowWarning(ClsName, 3, msg);
                }
            }
            else
            {
                if (warningShowing)
                {
                    // hide warning
                    HideWarning();
                }
            }
        }



        #endregion

        #region Truck Control

        bool TruckStopped()
        {
            return _vehicleController.CurrentSpeed < 0.1f;
        }

        bool ReachDestination(bool forward)
        {
            var d = destinationPos; // backward
            if (forward)
            {
                d = startPos;
            }

            // ignore y distance
            var t = truck.transform.position;
            d.y = t.y;
            var distance = Vector3.Distance(t, d);

            if (distance < distanceThreshold)
            {
                return true;
            }

            return false;
        }

        void ShowLoad(bool flag)
        {
            muds.SetActive(flag);
        }

        void StopVehicle()
        {
            _vehicleController.BrakesInput = 1;
            _vehicleController.HandBrakeInput = 1;
            _vehicleController.ClutchInput = 1;
        }

        void StartVehicle()
        {
            _vehicleController.BrakesInput = 0;
            _vehicleController.HandBrakeInput = 0;
            _vehicleController.ClutchInput = 0;
        }

        void MoveForward(bool forward)
        {
            if (ReachDestination(forward))
            {
                print("Truck reach destination");
                moving = false;
                return;
            }

            var _acceleration = 1.0f;
            if (!forward)
            {
                _acceleration = -1.0f;
            }

            _vehicleController.AccelerationInput = _acceleration;
        }

        #endregion

        //runs every time the truck reaches a position (simple)
        void IncreaseLeakWater()
        {
            if (waterLeak.transform.localScale.x > leakWaterThreshold) return;

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
                m.SetTextureScale("_MainTex",
                    new Vector2(waterLeak.transform.localScale.x, waterLeak.transform.localScale.y));
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

            ShowLoad(false);

            isStarted = true;
            moving = true;
            back = true;
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

            // hide truck
            isStarted = false;
            truck.SetActive(false);

            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS8_4_Finish()
        {
            // SAGAT query
            HideSAGAT();
        }

        #endregion

        #region Warning Override

        public override void OnIncidentStart(int obj)
        {
            print($"");
            // not show warning in this scenario
            // var msg = GetRightMessage(obj, scenariosManager.condition.Context, scenariosManager.condition.Amount);
            // ShowWarning(ClsName, obj, msg);
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Start);
            print($"{ClsName} #{obj} {name}");
            Invoke(name, 0);
        }

        #endregion

    }
}