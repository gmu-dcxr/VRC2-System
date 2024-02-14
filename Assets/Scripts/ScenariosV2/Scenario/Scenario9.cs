namespace VRC2.ScenariosV2.Scenario
{
    public class Scenario9 : Base
    {
        public void Start()
        {
            base.Start();
            // update the private id variable
            _id = 9;
        }
        
        public override void OnScenarioStart()
        {
            base.OnScenarioStart();
            // The plan players receive at the beginning of the experiment is not doable on the construction site.
        }

        public override void OnScenarioFinish()
        {
            base.OnScenarioFinish();
            // After they report it to the supervisor(Task>Incorrect instructions), they will receive a correct plan.
        }

        public void SetIncorrectInstruction()
        {
            
        }
    }
}