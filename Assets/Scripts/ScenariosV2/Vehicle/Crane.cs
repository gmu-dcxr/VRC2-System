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

        private void ShowWarning(int idx)
        {
            baselineS1.ShowWarning(idx, audioSource);
        }

        public void Crane_normals_1(object[] parameters)
        {
            BluePrint("Invoked Crane_normals_1");
            ShowWarning(2);
            baselineS1.On_BaselineS1_2_Start();
        }

        public void Crane_normals_2(object[] parameters)
        {
            BluePrint("Invoked Crane_normals_2");
            ShowWarning(3);
            baselineS1.On_BaselineS1_3_Start();
        }

        public void Crane_accidents_1(object[] parameters)
        {
            BluePrint("Invoked Crane_accidents_1");
            // show warning controller
            if (showWaring(parameters))
            {
                ShowWarning(2);
            }

            baselineS1.On_BaselineS1_2_Start();
        }

        public void Crane_accidents_2(object[] parameters)
        {
            BluePrint("Invoked Crane_accidents_2");

            ShowWarning(3);
            baselineS1.On_BaselineS1_3_Start();
        }

        public void Crane_accidents_3(object[] parameters)
        {
            BluePrint("Invoked Crane_accidents_3");
            if (showWaring(parameters))
            {
                ShowWarning(4);
            }

            baselineS1.On_BaselineS1_4_Start();
        }

        public void Crane_accidents_4(object[] parameters)
        {
            BluePrint("Invoked Crane_accidents_4");

            if (showWaring(parameters))
            {
                ShowWarning(5);
            }

            baselineS1.On_BaselineS1_5_Start();
        }

        public void Crane_accidents_5(object[] parameters)
        {
            BluePrint("Invoked Crane_accidents_5");

            ShowWarning(6);
            baselineS1.On_BaselineS1_6_Start();
        }

        public void Crane_accidents_6(object[] parameters)
        {
            BluePrint("Invoked Crane_accidents_6");

            ShowWarning(7);
            baselineS1.On_BaselineS1_7_Start();
        }

        public void Crane_accidents_7(object[] parameters)
        {
            BluePrint("Invoked Crane_accidents_7");

            ShowWarning(8);
            baselineS1.On_BaselineS1_8_Start();
        }

        public void Crane_accidents_8(object[] parameters)
        {
            BluePrint("Invoked Crane_accidents_8");

            ShowWarning(9);
            baselineS1.On_BaselineS1_9_Start();
        }

        public void Crane_accidents_9(object[] parameters)
        {
            BluePrint("Invoked Crane_accidents_9");

            ShowWarning(10);
            baselineS1.On_BaselineS1_10_Start();
        }

        #endregion
    }
}
