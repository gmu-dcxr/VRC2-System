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
        }

        #endregion
    }
}