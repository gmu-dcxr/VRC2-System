using UnityEngine.InputSystem;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;

namespace VRC2.Animations
{
    public class ForkliftInputRecording : BaseInputRecording
    {

        public ForkliftController forkliftController;
        public WSMVehicleController vehicleController;
        public ForkliftInputActions forkliftIA;

        private InputAction liftHorizontal;
        private InputAction liftVertical;
        private InputAction move;


        public override void InitInputActions()
        {
            forkliftIA = new ForkliftInputActions();
            forkliftIA.Enable();

            liftHorizontal = forkliftIA.Forklift.LiftHorizontal;
            liftVertical = forkliftIA.Forklift.LiftVertical;
            move = forkliftIA.Forklift.Move;
        }

        public override void DisposeInputActions()
        {
            forkliftIA.Dispose();
        }

        public override void UpdateLogic()
        {
            //liftHorizontal right
            if (liftHorizontal.ReadValue<Vector2>().x > 0)
            {
                forkliftController.MoveForksHorizontally(1);
            }
            //liftHorizontal left
            if (liftHorizontal.ReadValue<Vector2>().x < 0)
            {
                forkliftController.MoveForksHorizontally(-1);
            }
            //liftHorizontal stop
            if (liftHorizontal.ReadValue<Vector2>().x == 0)
            {
                forkliftController.MoveForksHorizontally(0);
            }
            //liftVertical up
            if (liftVertical.ReadValue<Vector2>().y > 0)
            {
                forkliftController.MoveForksVertically(1);
            }
            //liftVertical down
            if (liftVertical.ReadValue<Vector2>().y < 0)
            {
                forkliftController.MoveForksVertically(-1);
            }
            //liftVertical stop
            if (liftVertical.ReadValue<Vector2>().y == 0)
            {
                forkliftController.MoveForksVertically(0);
            }
            //move forward
            if (move.ReadValue<Vector2>().y > 0)
            {
                vehicleController.AccelerationInput = 1f;
                vehicleController.BrakesInput = 0f;
            }
            //move backward
            if (move.ReadValue<Vector2>().y < 0)
            {
                vehicleController.AccelerationInput = -1f;
                vehicleController.BrakesInput = 0f;
            }
            //move right
            if (move.ReadValue<Vector2>().x > 0)
            {
                vehicleController.SteeringInput = 1f;
                vehicleController.BrakesInput = 0f;
            }
            //move left
            if (move.ReadValue<Vector2>().x < 0)
            {
                vehicleController.SteeringInput = -1f;
                vehicleController.BrakesInput = 0f;
            }
            //move straight
            if (move.ReadValue<Vector2>().x == 0)
            {
                vehicleController.SteeringInput = 0f;
                vehicleController.BrakesInput = 0f;
            }
            //stop move
            if (move.ReadValue<Vector2>().x == 0 && move.ReadValue<Vector2>().y == 0)
            {
                vehicleController.SteeringInput = 0f;
                vehicleController.AccelerationInput = 0f;
                vehicleController.BrakesInput = 1f;
            }
        }
    }
}
