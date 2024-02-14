using System;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Drone : Base
    {
        #region Baselines
        [Header("Adaption")] private BaselineS2 _baselineS2;
        private BaselineS4 _baselineS4;

        public BaselineS2 baselineS2
        {
            get
            {
                if (_baselineS2 == null)
                {
                    _baselineS2 = FindObjectOfType<BaselineS2>();
                }

                return _baselineS2;
            }
        }        

        public BaselineS4 baselineS4
        {
            get
            {
                if (_baselineS4 == null)
                {
                    _baselineS4 = FindObjectOfType<BaselineS4>();
                }

                return _baselineS4;
            }
        }
        #endregion

        #region Callbacks

        public void Drone_normals_1(object[] parameters)
        {
            print("Invoked Drone_normals_1");
            baselineS4.ShowWarning(2);
            baselineS4.On_BaselineS4_2_Start();
        }

        public void Drone_normals_2(object[] parameters)
        {
            print("Invoked Drone_normals_2");
            baselineS4.ShowWarning(3);
            baselineS4.On_BaselineS4_3_Start();
        }

        public void Drone_normals_3(object[] parameters) 
        {
            print("Invoked Drone_normals_3");
            baselineS2.ShowWarning(2);
            baselineS2.On_BaselineS2_2_Start();
        } 
        
        public void Drone_normals_4(object[] parameters) 
        {
            print("Invoked Drone_normals_4");
            baselineS2.ShowWarning(3);
            baselineS2.On_BaselineS2_3_Start();
        }

        public void Drone_accidents_1(object[] parameters)
        {
            print("Invoked Drone_accidents_1");
            baselineS4.ShowWarning(2);
            baselineS4.On_BaselineS4_2_Start();
        }

        public void Drone_accidents_2(object[] parameters)
        {
            print("Invoked Drone_accidents_2");
            baselineS4.ShowWarning(3);
            baselineS4.On_BaselineS4_3_Start();
        }

        public void Drone_accidents_3(object[] parameters)
        {
            print("Invoked Drone_accidents_4");
            baselineS4.ShowWarning(4);
            baselineS4.On_BaselineS4_4_Start();
        }

        public void Drone_accidents_4(object[] parameters)
        {
            print("Invoked Drone_accidents_4");
            baselineS4.ShowWarning(5);
            baselineS4.On_BaselineS4_5_Start();
        }

        public void Drone_accidents_5(object[] parameters)
        {
            print("Invoked Drone_accidents_5");
            baselineS4.ShowWarning(6);
            baselineS4.On_BaselineS4_6_Start();
        }

        public void Drone_accidents_6(object[] parameters)
        {
            print("Invoked Drone_accidents_6");
            baselineS4.ShowWarning(7);
            baselineS4.On_BaselineS4_7_Start();
        }

        public void Drone_accidents_7(object[] parameters)
        {
            print("Invoked Drone_accidents_7");
            baselineS4.ShowWarning(6);
            baselineS4.On_BaselineS4_6_Start();
        }

        public void Drone_accidents_8(object[] parameters)
        {
            print("Invoked Drone_accidents_8");
            baselineS4.ShowWarning(7);
            baselineS4.On_BaselineS4_7_Start();
        }

        #endregion
    }
}