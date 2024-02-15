using UnityEngine;
using VRC2.Events;

namespace VRC2.ScenariosV2.Scenario
{
    public class Scenario9 : Base
    {

        [Header("Incorrect Instruction")] public string folder;
        public string filename;

        // backup
        private string backupFolder;
        private string backupFilename;

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

            // backup
            (backupFolder, backupFilename) = instructionCallBack.BackupInstruction();
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
            // backup first
            instructionCallBack.UpdateInstruction(folder, filename);
        }

        // TODO: trigger after reporting
        // After they report it to the supervisor(Task>Incorrect instructions), they will receive a correct plan.
        public void RestoreInstruction()
        {
            instructionCallBack.UpdateInstruction(backupFolder, backupFilename);
        }
    }
}