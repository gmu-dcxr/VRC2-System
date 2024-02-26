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
using VRC2.ScenariosV2.Adaptor;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS8 : Scenario
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
            print("Start Normal Incident Baseline S8");
            normal.SetActive(true);
            EXCAV.GetComponent<ExcavAnimPlayer>().start_2();
            TRUCK.GetComponent<TruckAnimPlayer>().Backup1();
        }

        public void On_BaselineS8_1_Start()
        {

        }

        public void On_BaselineS8_1_Finish()
        {

        }

        public void On_BaselineS8_2_Start()
        {
            print("On_BaselineS8_2_Start");
            normal.SetActive(false);
            EXCAV.GetComponent<ExcavAnimPlayer_2>().start_2();
            TRUCK.GetComponent<TruckAnimPlayer>().Backup1();

        }

        public void On_BaselineS8_2_Finish()
        {
            // An excavator is digging next to the participants.
        }

        public void On_BaselineS8_3_Start()
        {
            print("On_BaselineS8_3_Start");
            normal.SetActive(false);
            EXCAV.GetComponent<ExcavAnimPlayer>().start_3();
            TRUCK.GetComponent<TruckAnimPlayer>().Backup2();
        }

        public void On_BaselineS8_3_Finish()
        {
            // The excavator is digging into the working zone.
        }

        public void On_BaselineS8_4_Start()
        {
            print("On_BaselineS8_4_Start");
            normal.SetActive(false);
            EXCAV.GetComponent<ExcavAnimPlayer>().start_4();
            TRUCK.GetComponent<TruckAnimPlayer>().Backup3();
        }

        public void On_BaselineS8_4_Finish()
        {
            // The excavator is digging more into the working zone.
        }


        public void On_BaselineS8_5_Start()
        {

        }

        public void On_BaselineS8_5_Finish()
        {

        }

        #endregion


    }
}
