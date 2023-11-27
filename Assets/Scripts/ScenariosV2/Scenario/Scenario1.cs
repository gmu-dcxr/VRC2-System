using System;

namespace VRC2.ScenariosV2.Scenario
{
    public class Scenario1: Base
    {
        public void Start()
        {
            base.Start();
            // update the private id variable
            _id = 1;
        }
    }
}