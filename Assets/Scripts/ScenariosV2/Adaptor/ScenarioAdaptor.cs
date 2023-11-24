using System.Collections.Generic;
using UnityEngine;
using VRC2.Scenarios;
using YamlDotNet.Serialization;

namespace VRC2.ScenariosV2.Adaptor
{
    public class ScenarioAdaptor : VRC2.Scenarios.Scenario
    {

        public void Start()
        {
            // hide the original debug UI
            showDebugUI = false;

            InitFromFile();
            CheckIncidentsCallbacks();

            this.ScenarioStart += OnScenarioStart;
            this.ScenarioFinish += OnScenarioFinish;
        }

        public virtual void OnIncidentFinish(int obj)
        {
            // hide warning
            // HideWarning();

            // var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Finish);
            // print($"{_name} #{obj} {name}");
            // Invoke(name, 0);
        }

        public virtual void OnIncidentStart(int obj, float? delay)
        {
            // show warning
            // var msg = GetRightMessage(obj, scenariosManager.condition.Context, scenariosManager.condition.Amount);
            // ShowWarning(name, obj, msg, delay);
            // var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Start);
            // print($"{_name} #{obj} {name}");
            // Invoke(name, 0);
        }

        public virtual void OnScenarioStart()
        {
            UpdateInstruction();
        }

        public virtual void OnScenarioFinish()
        {

        }
    }
}