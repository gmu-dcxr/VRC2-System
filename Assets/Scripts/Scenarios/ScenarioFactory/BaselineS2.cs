using Assets.OVR.Scripts;
using TMPro;
using UnityEngine;


namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS2 : Scenario
    {
        [Header("Drone")] public GameObject drone;
        public float speed;
        public GameObject endPosition;

        private GameObject player;

        void Start()
        {
            base.Start();

            player = localPlayer;
        }

        private void Update()
        {
            droneMove();
        }
        
        public void droneMove()
        {
            drone.transform.position = Vector3.MoveTowards(drone.transform.position, endPosition.transform.position,
                speed * Time.deltaTime);
        }


        #region Accident Events Callbacks

        public void On_BaselineS2_1_Start()
        {
            print("On_BaselineS2_1_Start");

            drone.transform.position = Vector3.MoveTowards(drone.transform.position, endPosition.transform.position,
                speed * Time.deltaTime);
        }

        public void On_BaselineS2_1_Finish()
        {

        }

        public void On_BaselineS2_2_Start()
        {
            print("On_BaselineS2_2_Start");
            // A supervising drone approaches the participants, and informs the second participant of a plan of installment change order. Gives the second participant a new sheet of requirements.

            // get incident
            var incident = GetIncident(2);
            var warning = incident;
            print(warning);

            drone.transform.position = Vector3.MoveTowards(drone.transform.position, endPosition.transform.position,
                speed * Time.deltaTime);

        }

        public void On_BaselineS2_2_Finish()
        {
            // A supervising drone approaches the participants, and informs the second participant of a plan of installment change order. Gives the second participant a new sheet of requirements.
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