using UnityEngine;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class Background : Scenario
    {
        [Header("Excavator")] public ExcavatorController excavatorController;

        [Header("Forklift")] public CustomForkLiftController customForkLiftController;
        

        
        private void Start()
        {
            base.Start();
        }

        #region Accident Events Callbacks

        public void On_Background_1_Start()
        {
            excavatorController.Animate();
        }

        public void On_Background_1_Finish()
        {

        }

        public void On_Background_2_Start()
        {
            customForkLiftController.Animate();
        }

        public void On_Background_2_Finish()
        {

        }

        public void On_Background_3_Start()
        {

        }

        public void On_Background_3_Finish()
        {

        }

        #endregion
    }
}