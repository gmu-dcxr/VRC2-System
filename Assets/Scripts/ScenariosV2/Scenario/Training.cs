using System;
using TaskBase = VRC2.Task.Base;

namespace VRC2.ScenariosV2.Scenario
{
    public class Training : Base
    {
        public void Start()
        {
            base.Start();
            // update the private id variable
            _id = 0;
        }

        public override void LoadTask()
        {
            // special case for training scenario
            // var name = task.Split('.')[0];
            var name = "Task0";
            // find task under VRC2.Task
            var clsName = $"VRC2.Task.{name}";
            print($"Load Task: {clsName}");
            var myClassType = Type.GetType(clsName);

            taskBase = (TaskBase)FindObjectOfType(myClassType);
        }
    }
}