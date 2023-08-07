using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS5 : Scenario
    {
        [Header("Accident Configure")] 
        public GameObject glass;
        public GameObject unloadedGlass;

        [Header("TruckPositions")]

        public GameObject destination;
        private Vector3 destinationPos;
        

        public GameObject craneTruck;
        private Vector3 startPos;
        private Quaternion startRotation;
        private WSMVehicleController _vehicleController;

        private float speed = 6f;
        private float rotationSpeed = 0f;

        private bool started = false;
        private bool movingForward = false;
        private bool moving = false;

        public Animator animator;

        private float distanceThreshold = 4.0f;

        [Header("Player")] public GameObject player;



        private void Start()
        {
            base.Start();

            //Find positions
            startPos = craneTruck.transform.position;
            startRotation = craneTruck.transform.rotation;
            destinationPos = destination.transform.position;

            _vehicleController = craneTruck.GetComponent<WSMVehicleController>();
            animator.enabled = false;

            unloadedGlass.SetActive(false);
        }

        private void Update()
        {

            if (!started) return;


            if (!moving)
            {
                print("Not moving");
                //StopVehicle();
            }
            else
            {
                MoveForward(movingForward);
                
            }
        }

        IEnumerator partialTipTruck()
        {
            print("StartedCoroutine");
            yield return new WaitForSeconds(5f);
            animator.enabled = true;
            animator.SetBool("woble", true);
            yield return new WaitForSeconds(2f);
            animator.SetBool("woble", false);
            animator.enabled = false;
            yield break;
        }

        IEnumerator tipTruck()
        {
            print("StartedCoroutine");
            yield return new WaitForSeconds(5f);
            animator.enabled = true;
            animator.SetBool("tipOver", true);
            StopVehicle();
            _vehicleController.StopEngine();
            yield return new WaitForSeconds(2.5f);
            glass.SetActive(false);
            unloadedGlass.SetActive(true);
            yield break;
        }

        #region craneTruck control

        bool craneTruckStopped() {
            return _vehicleController.CurrentSpeed < 0.1f;
        }

        bool ReachedDestination(bool forward) {
            var d = destinationPos; // foward
            if (!forward)
            {
                d = startPos; // back
            }

            // ignore y distance
            var t = craneTruck.transform.position;
            d.y = t.y;
            var distance = Vector3.Distance(t, d);

            if (distance < distanceThreshold)
            {
                return true;
            }

            return false;
        }

       /* bool ReachedPivot(bool forward)
        {
            var d = destinationPos; // backward
            if (forward)
            {
                d = startPos;
            }
            // ignore y distance
            var t = craneTruck.transform.position;
            d.y = t.y;
            
            if (forward) { // want to compare x values
                d.z = t.z; 
            }
            else { // want to compare z values
                d.x = t.x; 
            }
            var distance = Vector3.Distance(t, d);

            if (distance < distanceThreshold)
            {
                return true;
            }
            return false;
        }*/

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
            if (ReachedDestination(forward))
            {
                print("Crane Truck reached destination");
                moving = false;
                return;
            }

           /* if (ReachedPivot(forward)) 
            {
                print("Crane Truck reched pivot");
                movingForward = false;
                rotating = true;
            }*/

            var _acceleration = 1.0f;
            if (!forward)
            {
                _acceleration = -1.0f;
            }

            _vehicleController.AccelerationInput = _acceleration;
        }

       /* void Rotate() 
        {
            var targetDirection = startPos - craneTruck.transform.position;
            if (movingForward)
            {
                targetDirection = destinationPos - craneTruck.transform.position;
            }

            var newDirection = Vector3.RotateTowards(craneTruck.transform.forward, targetDirection,
                   speed * Time.deltaTime, rotationSpeed);

            craneTruck.transform.rotation = Quaternion.LookRotation(newDirection);
            if(startRotation.y - 180f <= craneTruck.transform.rotation.y) { rotating = false; }
        }*/
        #endregion



        #region Accident Events Callbacks

        public void On_BaselineS5_1_Start()
        {

        }

        public void On_BaselineS5_1_Finish()
        {

        }

        public void On_BaselineS5_2_Start()
        {
            print("On_BaselineS5_2_Start");
            // A crane truck loaded with windows parks next to the working zone, and is going to unload the windows next to participants.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;
            print(warning);

            moving = false;
            started = true;
            glass.SetActive(true);
           // craneTruck.transform.position = new Vector3(Finish1.transform.position.x, Finish1.transform.position.y,
               // Finish1.transform.position.z);
            

        }

        public void On_BaselineS5_2_Finish()
        {
            // A crane truck loaded with windows parks next to the working zone, and is going to unload the windows next to participants.
        }

        public void On_BaselineS5_3_Start()
        {
            print("On_BaselineS5_3_Start");
            // The unload finishes and the crane truck leaves.
            // get incident
            var incident = GetIncident(3);


            movingForward = true;
            moving = true;
            glass.SetActive(false);
            unloadedGlass.SetActive(true);
        }

        public void On_BaselineS5_3_Finish()
        {
            // The unload finishes and the crane truck leaves.

        }

        public void On_BaselineS5_4_Start()
        {
            print("On_BaselineS5_4_Start");
            // Another crane truck loaded with heavier windows parks and is going to unload the windows. This time the crane tilts a little bit.

            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);

            craneTruck.transform.position = destinationPos;
            craneTruck.transform.rotation = startRotation;
           // glass.transform.position = destinationPos - new Vector3(2f, 0f, 0f);
            glass.SetActive(true);
            movingForward = false;
            moving = true;
            unloadedGlass.SetActive(false);
            StartCoroutine(partialTipTruck());
        }

        public void On_BaselineS5_4_Finish()
        {
            // Another crane truck loaded with heavier windows parks and is going to unload the windows. This time the crane tilts a little bit.

        }

        public void On_BaselineS5_5_Start()
        {
            print("On_BaselineS5_5_Start");
            // The unload finishes and the crane truck leaves.
            // get incident
            var incident = GetIncident(5);

            craneTruck.transform.position = startPos;
            craneTruck.transform.rotation = startRotation;

            // backingUp = false;
            movingForward = true;
            moving = true;
            glass.SetActive(false);
            unloadedGlass.SetActive(true);
        }

        public void On_BaselineS5_5_Finish()
        {
            // The unload finishes and the crane truck leaves.

        }

        public void On_BaselineS5_6_Start()
        {
            print("On_BaselineS5_6_Start");
            // Another crane truck loaded with even heavier windows parks and is going to unload the windows. This time the crane is about to overturn.

            // get incident
            var incident = GetIncident(6);
            var warning = incident.Warning;
            print(warning);

            craneTruck.transform.position = destinationPos;
            craneTruck.transform.rotation = startRotation;

            movingForward = false;
            moving = true;
            glass.SetActive(true);
            unloadedGlass.SetActive(false);
            StartCoroutine(tipTruck());

        }

        public void On_BaselineS5_6_Finish()
        {
            // Another crane truck loaded with even heavier windows parks and is going to unload the windows. This time the crane is about to overturn.

        }

        public void On_BaselineS5_7_Start()
        {
            print("On_BaselineS5_7_Start");
            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS5_7_Finish()
        {
            // SAGAT query
            HideSAGAT();
        }

        #endregion

    }
}