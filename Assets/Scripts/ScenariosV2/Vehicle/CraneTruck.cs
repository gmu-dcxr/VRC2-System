using System;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class CraneTruck : Base
    {
        [Header("Adaption")] private BaselineS5 _baselineS5;

        public BaselineS5 baselineS5
        {
            get
            {
                if (_baselineS5 == null)
                {
                    _baselineS5 = FindObjectOfType<BaselineS5>();
                }

                return _baselineS5;
            }
        }

        #region Callbacks

        // TODO: WARNING: warning sound defined in cranetruck rather than in BaselineS5

        private void ShowWarning(int idx)
        {
            baselineS5.ShowWarning(idx, audioSource);
        }

        public void CraneTruck_normals_1(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_normals_1");
            // baselineS5.ShowWarning(2);
            PlayAudioOnly(true, 1);
            baselineS5.On_BaselineS5_2_Start();
        }

        public void CraneTruck_normals_2(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_normals_2");
            // crane truck unloads
            PlayAudioOnly(true, 2);
        }

        public void CraneTruck_normals_3(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_normals_3");
            ShowWarning(3);
            baselineS5.On_BaselineS5_3_Start();
        }

        public void CraneTruck_accidents_1(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_accidents_1");
            // TODO: this is only for demonstration.
            // When the previous implementation BaselineSx is called, it will automatically show the expected warning.
            // warningController.ShowVisualOnly();
            // baselineS5.ShowWarning(2);
            PlayAudioOnly(false, 1);
            baselineS5.On_BaselineS5_2_Start();
        }

        public void CraneTruck_accidents_2(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_accidents_2");
            //crane truck unloads
            PlayAudioOnly(false, 2);
        }

        public void CraneTruck_accidents_3(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_accidents_3");
            ShowWarning(3);
            baselineS5.On_BaselineS5_3_Start();
        }

        public void CraneTruck_accidents_4(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_accidents_4");
            // baselineS5.ShowWarning(4);
            PlayAudioOnly(false, 4);
            baselineS5.On_BaselineS5_4_Start();
        }

        public void CraneTruck_accidents_5(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_accidents_5");
            //cranetruck unloads
            PlayAudioOnly(false, 5);
        }

        public void CraneTruck_accidents_6(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_accidents_6");
            ShowWarning(5);
            baselineS5.On_BaselineS5_5_Start();
        }

        public void CraneTruck_accidents_7(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_accidents_7");
            // baselineS5.ShowWarning(6);
            PlayAudioOnly(false, 7);
            baselineS5.On_BaselineS5_6_Start();
        }

        public void CraneTruck_accidents_8(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_accidents_8");
            //cranetruck unloads
            PlayAudioOnly(false, 8);
        }

        public void CraneTruck_accidents_9(object[] parameters)
        {
            BluePrint("Invoked CraneTruck_accidents_9");
            ShowWarning(7);
            baselineS5.On_BaselineS5_7_Start();
        }

        #endregion
    }
}