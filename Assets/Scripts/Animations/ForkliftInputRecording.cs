using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;

namespace VRC2.Animations
{
    public class ForkliftInputRecording : BaseInputRecording
    {

        #region Self fields

        public ForkliftController forkliftScript;

        private ForkliftInputActions forkliftIA;

        private InputAction liftVerticalInput;
        private InputAction liftHorizontalInput;
        private InputAction moveInput;

        private Transform forkliftBody
        {
            get => forkliftScript.gameObject.transform;
        }


        #endregion

        public override void InitInputActions()
        {
            forkliftIA = new ForkliftInputActions();
            forkliftIA.Enable();

            liftVerticalInput = forkliftIA.Forklift.LiftVertical;
            liftHorizontalInput = forkliftIA.Forklift.LiftHorizontal;
            moveInput = forkliftIA.Forklift.Move;
        }

        public override void DisposeInputActions()
        {
            forkliftIA.Dispose();
        }
    }
}