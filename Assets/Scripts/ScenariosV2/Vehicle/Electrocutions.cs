using System;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Electrocutions : Base
    {
        #region Baselines
        [Header("Adaption")] private BaselineS8 _baselineS8;

        public BaselineS8 baselineS8
        {
            get
            {
                if (_baselineS8 == null)
                {
                    _baselineS8 = FindObjectOfType<BaselineS8>();
                }

                return _baselineS8;
            }
        }        
        #endregion

        #region Callbacks

        public void Electrocutions_normals_1(object[] parameters)
        {
            print("Invoked Electrocutions_normals_1");
            baselineS8.ShowWarning(2);
            baselineS8.On_BaselineS8_2_Start();
        }

        public void Electrocutions_normals_2(object[] parameters)
        {
            print("Invoked Electrocutions_normals_2");
        }

        public void Electrocutions_accidents_1(object[] parameters)
        {
            print("Invoked Electrocutions_normals_1");
            baselineS8.ShowWarning(2);
            baselineS8.On_BaselineS8_2_Start();
        }

        public void Electrocutions_accidents_2(object[] parameters)
        {
            print("Invoked Electrocutions_accidents_2");
        }

        public void Electrocutions_accidents_3(object[] parameters)
        {
            print("Invoked Electrocutions_accidents_3");
            baselineS8.ShowWarning(3);
            baselineS8.On_BaselineS8_3_Start();
        }

        public void Electrocutions_accidents_4(object[] parameters)
        {
            print("Invoked Electrocutions_accidents_4");
        }

        public void Electrocutions_accidents_5(object[] parameters)
        {
            print("Invoked Electrocutions_accidents_5");
            baselineS8.ShowWarning(4);
            baselineS8.On_BaselineS8_4_Start();
        }

        public void Electrocutions_accidents_6(object[] parameters)
        {
            print("Invoked Electrocutions_accidents_6");
        }   
        #endregion
    }
}