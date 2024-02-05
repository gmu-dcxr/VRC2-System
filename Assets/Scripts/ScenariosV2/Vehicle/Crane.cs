using System;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Crane : Base
    {
        [Header("Adaption")] private BaselineS1 _baselineS1;

        public BaselineS1 baselineS1
        {
            get
            {
                if (_baselineS1 == null)
                {
                    _baselineS1 = FindObjectOfType<BaselineS1>();
                }

                return _baselineS1;
            }
        }

        #region Callbacks

        public void Crane_normals_1()
        {
            print("Invoked");
        }

        public void Crane_normals_2()
        {

        }

        public void Crane_accidents_1()
        {
            print("Invoked Crane_accidents_1");
            // show warning controller
            baselineS1.ShowWarning(2);
            baselineS1.On_BaselineS1_2_Start();
        }

        public void Crane_accidents_2()
        {
            print("Invoked Crane_accidents_2");

            baselineS1.ShowWarning(3);
            baselineS1.On_BaselineS1_3_Start();
        }

        public void Crane_accidents_3()
        {
            print("Invoked Crane_accidents_3");

            baselineS1.ShowWarning(4);
            baselineS1.On_BaselineS1_4_Start();
        }

        public void Crane_accidents_4()
        {
            print("Invoked Crane_accidents_4");

            baselineS1.ShowWarning(5);
            baselineS1.On_BaselineS1_5_Start();
        }

        public void Crane_accidents_5()
        {
            print("Invoked Crane_accidents_5");

            baselineS1.ShowWarning(6);
            baselineS1.On_BaselineS1_6_Start();
        }

        public void Crane_accidents_6()
        {
            print("Invoked Crane_accidents_6");

            baselineS1.ShowWarning(7);
            baselineS1.On_BaselineS1_7_Start();
        }

        public void Crane_accidents_7()
        {
            print("Invoked Crane_accidents_7");

            baselineS1.ShowWarning(8);
            baselineS1.On_BaselineS1_8_Start();
        }

        public void Crane_accidents_8()
        {
            print("Invoked Crane_accidents_8");

            baselineS1.ShowWarning(9);
            baselineS1.On_BaselineS1_9_Start();
        }

        public void Crane_accidents_9()
        {
            print("Invoked Crane_accidents_9");

            baselineS1.ShowWarning(10);
            baselineS1.On_BaselineS1_10_Start();
        }

        #endregion
    }
}
