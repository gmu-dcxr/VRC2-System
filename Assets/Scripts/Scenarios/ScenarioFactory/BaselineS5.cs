using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS5 : Scenario
    {
        [Header("Config")]
        [Tooltip("Yml file name")]
        public string filename = "BaselineS5.yml";

        [Header("Accident Configure")] public GameObject glass;
        public GameObject unloadedGlass;

        [Header("TruckPositions")]
        private Transform Start1;
        private Transform Finish1;

        public GameObject craneTruck;
        private float speed = 6f;

        private bool backingUp = false;
        private bool movingForward = false;

        [Header("Player")] public GameObject player;



        private void Start()
        {
            InitFromFile(filename);

            IncidentStart += OnIncidentStart;
            IncidentFinish += OnIncidentFinish;

            CheckIncidentsCallbacks();

            //Find positions
            Start1 = GameObject.Find("Start").transform;
            Finish1 = GameObject.Find("Finish").transform;

            unloadedGlass.SetActive(false);

            
        }

        private void Update()
        {
            if (backingUp)
            {
                craneTruck.transform.position = Vector3.MoveTowards(craneTruck.transform.position, Finish1.transform.position, speed * Time.deltaTime);
            }
            if (movingForward)
            {
                craneTruck.transform.position = Vector3.MoveTowards(craneTruck.transform.position, Start1.transform.position, speed * Time.deltaTime);
            }
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

        IEnumerator partialTipTruck()
        {
            print("StartedCoroutine");
            yield return new WaitForSeconds(15f);
            craneTruck.transform.eulerAngles = new Vector3(craneTruck.transform.eulerAngles.x, craneTruck.transform.eulerAngles.y, craneTruck.transform.eulerAngles.z + 30);
            yield return new WaitForSeconds(15f);
            craneTruck.transform.eulerAngles = new Vector3(craneTruck.transform.eulerAngles.x, craneTruck.transform.eulerAngles.y, craneTruck.transform.eulerAngles.z - 30);
            yield break;
        }

        IEnumerator tipTruck()
        {
            print("StartedCoroutine");
            yield return new WaitForSeconds(15f);
            craneTruck.transform.eulerAngles = new Vector3(craneTruck.transform.eulerAngles.x, craneTruck.transform.eulerAngles.y, craneTruck.transform.eulerAngles.z + 90);
            glass.SetActive(false);
            unloadedGlass.SetActive(true);
            yield break;
        }


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

            movingForward = false;
            backingUp = true;
            glass.SetActive(true);
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


            backingUp = false;
            movingForward = true;
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

            movingForward = false;
            backingUp = true;
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