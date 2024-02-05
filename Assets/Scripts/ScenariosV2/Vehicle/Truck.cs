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

                return _baselineS3;
            }
        }

        #region Callbacks

        public void Truck_normals_1()
        {
            print("Invoked");
        }

        public void Truck_normals_2()
        {

        }

        public void Truck_accidents_1()
        {
            print("Invoked Truck_accidents_1");
            // show warning controller
            if (baselineS3 == null)
                print("sssssssssssss");
            print("555555");
            baselineS3.ShowWarning(2);
            print("we showed the warning!!!");
            baselineS3.On_BaselineS3_2_Start();
        }

        public void Truck_accidents_2()
        {
            print("Invoked Truck_accidents_2");

            baselineS3.ShowWarning(3);
            baselineS3.On_BaselineS3_3_Start();
        }

        public void Truck_accidents_3()
        {
            print("Invoked Truck_accidents_3");

            baselineS3.ShowWarning(4);
            baselineS3.On_BaselineS3_4_Start();
        }

        public void Truck_accidents_4()
        {
            print("Invoked Truck_accidents_4");

            baselineS3.ShowWarning(5);
            baselineS3.On_BaselineS3_5_Start();
        }

        public void Truck_accidents_5()
        {
            print("Invoked Truck_accidents_5");

            baselineS3.ShowWarning(6);
            baselineS3.On_BaselineS3_6_Start();
        }

        public void Truck_accidents_6()
        {
            print("Invoked Truck_accidents_6");

            baselineS3.ShowWarning(7);
            baselineS3.On_BaselineS3_7_Start();
        }

        public void Truck_accidents_7()
        {
            print("Invoked Truck_accidents_7");

            baselineS3.ShowWarning(8);
            baselineS3.On_BaselineS3_7_Start();
        }

        public void Truck_accidents_8()
        {
            print("Invoked Truck_accidents_8");

            baselineS3.ShowWarning(8);
            baselineS3.On_BaselineS3_8_Start();
        }





        #endregion
    }
}