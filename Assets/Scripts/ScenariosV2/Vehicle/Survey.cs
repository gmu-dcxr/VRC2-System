using System;
using UnityEngine;
using VRC2.SAGAT;
using VRC2.Scenarios.ScenarioFactory;
using VRC2.ScenariosV2.Tool;

namespace VRC2.ScenariosV2.Vehicle
{
    public class Survey : Base
    {
        [Header("Url")] public string surveyUrl;


        #region Browser-based answering

        private GameObject SAGATRoot;

        private SurveyController _surveyController;

        [HideInInspector]
        public SurveyController browserSurvey
        {
            get
            {
                if (SAGATRoot == null)
                {
                    SAGATRoot = GameObject.FindWithTag(GlobalConstants.SAGATTag);
                    _surveyController = SAGATRoot.GetComponent<SurveyController>();
                }

                return _surveyController;
            }
        }

        #endregion

        #region UI-based answering

        private SagatController _sagatController;

        public SagatController uiSurvey
        {
            get
            {
                if (_sagatController == null)
                {
                    _sagatController = FindFirstObjectByType<SagatController>();
                }

                return _sagatController;
            }
        }


        #endregion

        #region Callbacks

        public void Survey_normals_1(object[] parameters)
        {
            // show browser
            BluePrint("Invoked Survey_normals_1");
            browserSurvey.Show(surveyUrl);
        }

        public void Survey_normals_2(object[] parameters)
        {
            // hide browser
            BluePrint("Invoked Survey_normals_2");
            browserSurvey.Hide();
        }

        public void Survey_normals_3(object[] parameters)
        {
            // show UI
            BluePrint("Invoked Survey_normals_3");
            uiSurvey.StartSAGAT();
        }

        public void Survey_normals_4(object[] parameters)
        {
            // hide UI
            BluePrint("Invoked Survey_normals_4");
            uiSurvey.StopSAGAT();
        }

        #endregion
    }
}