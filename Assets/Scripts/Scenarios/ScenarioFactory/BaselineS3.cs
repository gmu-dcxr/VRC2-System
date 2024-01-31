using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform.Models;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using VRC2.Animations;
using WSMGameStudio.Vehicles;
using Random = UnityEngine.Random;
using Timer = UnityTimer.Timer;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS3 : Scenario
    {

        public GameObject TRUCK;
        public GameObject EXCAV;

        public GameObject normal;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Accident Events Callbacks
        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S3");
            normal.SetActive(true);
            EXCAV.GetComponent<ExcavAnimPlayer>().start_2();
            TRUCK.GetComponent<TruckAnimPlayer>().Backup1();
        }

        public void On_BaselineS3_1_Start()
        {
            print("Starting BaselineS3 1");
        }

        public void On_BaselineS3_1_Finish()
        {
            print("Ending BaselineS3 1");
        }

        public void On_BaselineS3_2_Start()
        {
            print("On_BaselineS3_2_Start");
            normal.SetActive(false);
            EXCAV.GetComponent<ExcavAnimPlayer>().start_2();
            TRUCK.GetComponent<TruckAnimPlayer>().Backup1();

        }

        public void On_BaselineS3_2_Finish()
        {
            print("Ending BaselineS3 2");
            // An excavator is digging next to the participants.
        }

        public void On_BaselineS3_3_Start()
        {
            print("On_BaselineS3_3_Start");
            normal.SetActive(false);
            EXCAV.GetComponent<ExcavAnimPlayer>().start_3();
            TRUCK.GetComponent<TruckAnimPlayer>().Backup2();
        }

        public void On_BaselineS3_3_Finish()
        {
            // The excavator is digging into the working zone.
            print("Ending BaselineS3 3");
        }

        public void On_BaselineS3_4_Start()
        {
            print("On_BaselineS3_4_Start");
            normal.SetActive(false);
            EXCAV.GetComponent<ExcavAnimPlayer>().start_4();
            TRUCK.GetComponent<TruckAnimPlayer>().Backup3();
        }

        public void On_BaselineS3_4_Finish()
        {
            // The excavator is digging more into the working zone.
            print("Ending BaselineS3 4");
        }


        public void On_BaselineS3_5_Start()
        {

        }

        public void On_BaselineS3_5_Finish()
        {

        }

        public void On_BaselineS3_6_Start()
        {

        }

        public void On_BaselineS3_6_Finish()
        {

        }

        public void On_BaselineS3_7_Start()
        {

        }

        public void On_BaselineS3_7_Finish()
        {

        }

        public void On_BaselineS3_8_Start()
        {

        }

        public void On_BaselineS3_8_Finish()
        {

        }

        #endregion


    }
}
