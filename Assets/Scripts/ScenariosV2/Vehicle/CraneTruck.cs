using System;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class CraneTruck : Base
    {
        #region Callbacks

        public void CraneTruck_normals_1()
        {
            print("Invoked CraneTruck_normals_1");
        }

        public void CraneTruck_normals_2()
        {
            print("Invoked CraneTruck_normals_2");
        }

        public void CraneTruck_accidents_1()
        {
            print("Invoked CraneTruck_accidents_1");
            // TODO: this is only for demonstration.
            // When the previous implementation BaselineSx is called, it will automatically show the expected warning.
            warningController.ShowVisualOnly();
        }

        public void CraneTruck_accidents_2()
        {
            print("Invoked CraneTruck_accidents_2");
        }

        public void CraneTruck_accidents_3()
        {
            print("Invoked CraneTruck_accidents_3");
        }

        public void CraneTruck_accidents_4()
        {
            print("Invoked CraneTruck_accidents_4");
        }

        public void CraneTruck_accidents_5()
        {
            print("Invoked CraneTruck_accidents_5");
        }

        public void CraneTruck_accidents_6()
        {
            print("Invoked CraneTruck_accidents_6");
        }

        #endregion
    }
}