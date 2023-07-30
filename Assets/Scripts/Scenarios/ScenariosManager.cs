using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using VRC2.Conditions;

namespace VRC2.Scenarios
{
    public class ScenariosManager : MonoBehaviour
    {
        private int startTimestamp = -1;

        public ConditionNumber conditionNumber;

        public List<Scenario> scenarios = null;

        public System.Action OnStart;
        public System.Action OnFinish;

        private Condition _condition;

        private void Start()
        {
            CheckScenariosCallbacks();
            
            // get condition
            _condition = Condition.GetCondition(conditionNumber);
            Debug.LogWarning($"Use Condition: {_condition.ToString()}");
        }

        public void Execute()
        {
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 100, 150, 50), "Test"))
            {
                TestScenarioManager();
            }
        }


        void StartScenarios()
        {
            var count = scenarios.Count;

            for (int i = 1; i < count; i++)
            {
                var s0 = scenarios[i - 1];
                var s = scenarios[i];
                s.OverrideID(i);
                s.OverrideStartEnd(s.startInSec + s0.endInSec, s.endInSec + s0.endInSec);
            }

            startTimestamp = Helper.SecondNow();
            // start all event
            foreach (var scenario in scenarios)
            {
                scenario.Execute(startTimestamp);
            }
        }

        public void CheckScenariosCallbacks()
        {
            bool pass = true;

            var ClsName = GetType().Name;

            var @namespace = "VRC2.Scenarios";
            var myClassType = Type.GetType($"{@namespace}.{ClsName}");

            foreach (var scenario in scenarios)
            {
                var _id = scenario.ID;
                var name1 = Helper.GetScenarioCallbackName(_id, ScenarioCallback.Start);
                var name2 = Helper.GetScenarioCallbackName(_id, ScenarioCallback.Finish);

                if (myClassType.GetMethod(name1) == null)
                {
                    pass = false;
                    Debug.LogError($"[{ClsName}] missing method: {name1}");
                }

                if (myClassType.GetMethod(name2) == null)
                {
                    pass = false;
                    Debug.LogError($"[{ClsName}] missing method: {name2}");
                }
            }

            Debug.LogWarning($"{ClsName} Check Scenarios Callbacks Result: {pass}");
        }

        #region Debug

        void TestScenarioManager()
        {
            scenarios = new List<Scenario>();

            var s1 = gameObject.AddComponent<Scenario>();
            s1.startInSec = 0;
            s1.endInSec = 10;
            s1.name = "S1";

            var s2 = gameObject.AddComponent<Scenario>();
            s2.startInSec = 0;
            s2.endInSec = 5;
            s2.name = "S2";

            var s3 = gameObject.AddComponent<Scenario>();
            s3.startInSec = 0;
            s3.endInSec = 15;
            s3.name = "S3";

            scenarios.Add(s1);
            scenarios.Add(s2);
            scenarios.Add(s3);

            StartScenarios();
        }

        #endregion
    }
}