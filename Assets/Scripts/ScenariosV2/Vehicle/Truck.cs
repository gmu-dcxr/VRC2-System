using System;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Truck : Base
    {
        [Header("Adaption")] private BaselineS3 _baselineS3;

        public BaselineS3 baselineS3
        {
            get
            {
                if (_baselineS3 == null)
                {
                    _baselineS3 = FindObjectOfType<BaselineS3>();
                }

                if (_baselineS3 == null)
                {
                    BluePrint("BAD: baselines3 is null");
                }

                return _baselineS3;
            }
        }

        #region Callbacks

        public void Truck_normals_1(object[] parameters)
        {
            BluePrint("Invoked Truck_normals_1");
            PlayAudioOnly(true, 1);
            baselineS3.On_BaselineS3_2_Start();
        }

        public void Truck_normals_2(object[] parameters)
        {
            BluePrint("Invoked Truck_normals_2");
            //baselineS3.On_BaselineS3_2_Start();
        }

        public void Truck_accidents_1(object[] parameters)
        {
            BluePrint("Invoked Truck_accidents_1");
            // show warning controller
            // baselineS3.ShowWarning(2);
            PlayAudioOnly(false, 1);
            baselineS3.On_BaselineS3_2_Start();
        }

        public void Truck_accidents_2(object[] parameters)
        {
            BluePrint("Invoked Truck_accidents_2");

            //baselineS3.On_BaselineS3_3_Start();
            PlayAudioOnly(false, 2);
        }

        public void Truck_accidents_3(object[] parameters)
        {
            BluePrint("Invoked Truck_accidents_3");

            //baselineS3.ShowWarning(4);
            PlayAudioOnly(false, 3);
            baselineS3.On_BaselineS3_4_Start();
        }

        public void Truck_accidents_4(object[] parameters)
        {
            BluePrint("Invoked Truck_accidents_4");

            //baselineS3.ShowWarning(5);
            //baselineS3.On_BaselineS3_5_Start();
        }

        public void Truck_accidents_5(object[] parameters)
        {
            BluePrint("Invoked Truck_accidents_5");

            //baselineS3.ShowWarning(6);
            PlayAudioOnly(false, 5);
            baselineS3.On_BaselineS3_6_Start();
        }

        public void Truck_accidents_6(object[] parameters)
        {
            BluePrint("Invoked Truck_accidents_6");

            //baselineS3.ShowWarning(7);
            //baselineS3.On_BaselineS3_7_Start();
        }

        public void Truck_accidents_7(object[] parameters)
        {
            BluePrint("Invoked Truck_accidents_7");

            //baselineS3.ShowWarning(8);
            PlayAudioOnly(false, 7);
            baselineS3.On_BaselineS3_7_Start();
        }

        public void Truck_accidents_8(object[] parameters)
        {
            BluePrint("Invoked Truck_accidents_8");

            //baselineS3.ShowWarning(8);
            //baselineS3.On_BaselineS3_8_Start();
        }

        #endregion
    }
}