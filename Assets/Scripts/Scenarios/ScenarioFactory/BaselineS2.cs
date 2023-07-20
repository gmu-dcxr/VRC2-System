using Assets.OVR.Scripts;
using TMPro;
using UnityEngine;


namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS2 : Scenario
    {
        [Header("Config")]
        [Tooltip("Yml file name")]
        public string filename = "BaselineS2.yml";

        [Header("Drone")] public GameObject drone;
        public float speed;
        public GameObject endPosition;


        private void Start()
        {
            InitFromFile(filename);

            IncidentStart += OnIncidentStart;
            IncidentFinish += OnIncidentFinish;

            CheckIncidentsCallbacks();

        }

        private void Update()
        {
            droneMove();
        }

        private void OnIncidentFinish(int obj)
        {
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Finish);

            print($"[{ClsName}] Callback: {name}");

            Invoke(name, 0);
        }

        private void OnIncidentStart(int obj)
        {
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Start);

            print($"[{ClsName}] Callback: {name}");
            Invoke(name, 0);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Start"))
            {
                var ts = Helper.SecondNow();
                Execute(ts);              
            }
        }

        public void droneMove()
        {
            drone.transform.position = Vector3.MoveTowards(drone.transform.position, endPosition.transform.position, speed * Time.deltaTime);
        }


        #region Accident Events Callbacks

        public void On_BaselineS2_1_Start()
        {
            print("On_BaselineS2_1_Start");

            drone.transform.position = Vector3.MoveTowards(drone.transform.position, endPosition.transform.position, speed * Time.deltaTime);
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

            drone.transform.position = Vector3.MoveTowards(drone.transform.position, endPosition.transform.position, speed * Time.deltaTime);

        }

        public void On_BaselineS2_2_Finish()
        {
            // A supervising drone approaches the participants, and informs the second participant of a plan of installment change order. Gives the second participant a new sheet of requirements.
        }

        public void On_BaselineS2_3_Start()
        {
            print("On_BaselineS2_3_Start");
            // SAGAT query
            // get incident
            var incident = GetIncident(3);
        }

        public void On_BaselineS2_3_Finish()
        {
            // SAGAT query.
        }


        #endregion
    }
}