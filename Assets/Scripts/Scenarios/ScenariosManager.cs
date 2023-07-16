using System;
using System.Collections.Generic;
using UnityEngine;

using Fusion;

namespace VRC2.Scenarios
{
    public class ScenariosManager : MonoBehaviour
    {
        private int startTimestamp = -1;

        public List<Scenario> scenarios = null;

        public System.Action OnStart;
        public System.Action OnFinish;

        private void Start()
        {
            startTimestamp = Helper.SecondNow();
        }

        public void Execute()
        {
        }

        void StartScenarios()
        {
            foreach (var scenario in scenarios)
            {
                scenario.Execute(startTimestamp);
            }
        }
    }
}