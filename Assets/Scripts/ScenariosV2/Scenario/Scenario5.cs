using VRC2.Conditions;

namespace VRC2.ScenariosV2.Scenario
{
    public class Scenario5 : Base
    {
        private Format _format;

        public void Start()
        {
            base.Start();
            // update the private id variable
            _id = 5;
        }

        public override void OnScenarioStart()
        {
            base.OnScenarioStart();
            // backup format
            _format = scenariosManager.condition.Format;
            // change to both
            scenariosManager.condition.Format = Format.Both;
            
            print($"Scenario 5 OnScenarioStart {_format}");
        }

        public override void OnScenarioFinish()
        {
            base.OnScenarioFinish();
            // restore format
            scenariosManager.condition.Format = _format;
            print($"Scenario 5 OnScenarioFinish {_format}");
        }
    }
}