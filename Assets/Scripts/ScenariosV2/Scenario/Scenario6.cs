using System;

namespace VRC2.ScenariosV2.Scenario
{
    public class Scenario6 : Base
    {
        public void Start()
        {
            base.Start();
            // update the private id variable
            _id = 6;
        }

        public override void OnScenarioStart()
        {
            base.OnScenarioStart();

            warningController.StartCounterDown(endInSec);
        }

        public override void OnScenarioFinish()
        {
            base.OnScenarioFinish();

            warningController.StopCounterDown();
        }
    }
}