using UnityEngine;
using VRC2.Events;
using VRC2.Task;

namespace VRC2.ScenariosV2.Scenario
{
    public class Scenario9 : Base
    {

        [Header("Incorrect Instruction")] private Mistask _mistask;

        public Mistask mistask
        {
            get
            {
                if (_mistask == null)
                {
                    _mistask = GameObject.FindObjectOfType<Mistask>();
                }

                return _mistask;
            }
        }

        private InstructionSheetGrabbingCallback _instructionCallBack;

        public InstructionSheetGrabbingCallback instructionCallBack
        {
            get
            {
                if (_instructionCallBack == null)
                {
                    _instructionCallBack = FindObjectOfType<InstructionSheetGrabbingCallback>();
                }

                return _instructionCallBack;
            }
        }

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
            SetIncorrectInstruction();
        }

        public override void OnScenarioFinish()
        {
            base.OnScenarioFinish();
        }

        public void SetIncorrectInstruction()
        {
            // update instruction
            mistask.UpdateTableInstruction(roleChecker.IsP1());
        }

        // After they report it to the supervisor(Task>Incorrect instructions), they will receive a correct plan.
        // This method is invoked via reflection.
        // It's defined in `SupervisorMenu.yml` L31 `action: ["VRC2.ScenariosV2.Scenario.Scenario9", "RestoreInstruction"] # [class name (including namespace), method]`
        public void RestoreInstruction()
        {
            taskBase.UpdateTableInstruction(roleChecker.IsP1());
        }
    }
}