using System;
using UnityEngine;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Forklift : Base
    {
        #region Baselines
        [Header("Adaption")] private Background _background;

        public Background background
        {
            get
            {
                if (_background == null)
                {
                    _background = FindObjectOfType<Background>();
                }

                return _background;
            }
        }                
        #endregion

        #region Callbacks

        public void Forklift_normals_1(object[] parameters)
        {
            BluePrint("Invoked Forklift_normals_1");
            background.ShowWarning(2, audioSource);
            background.On_Background_2_Start();
        }

        public void Forklift_normals_2(object[] parameters)
        {
            BluePrint("Invoked Forklift_normals_2");
            background.On_Background_2_Finish();
        }
        #endregion
    }
}