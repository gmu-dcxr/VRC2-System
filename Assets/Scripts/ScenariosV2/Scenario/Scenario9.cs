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
            (backupFolder, backupFilename) = taskBase.BackupImageInConfig();
            Debug.LogError($"[Scenario9] backup: {backupFolder} {backupFilename}");
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
            Debug.LogError($"[Scenario9] SetIncorrectInstruction ({folder}, {filename})");
            instructionCallBack.UpdateInstruction(folder, filename);
        }
        
        // After they report it to the supervisor(Task>Incorrect instructions), they will receive a correct plan.
        // This method is invoked via reflection.
        // It's defined in `SupervisorMenu.yml` L31 `action: ["VRC2.ScenariosV2.Scenario.Scenario9", "RestoreInstruction"] # [class name (including namespace), method]`
        public void RestoreInstruction()
        {
            instructionCallBack.UpdateInstruction(backupFolder, backupFilename);
        }
    }
}