using UnityEngine;
using UnityEngine.InputSystem;
using WSMGameStudio.Vehicles;

namespace VRC2.Animations
{
    public class TruckInputRecording : BaseInputRecording
    {
        [Header("Truck")] public WSMVehicleController truckController;

        [Space(30)] [Header("Output Settings")]
        public string filename = "truckForward";

        private TruckInpuActions truckIA;

        private InputAction moveAction;
        private InputAction brakeAction;

        private float _acceleration = 0f;
        private float _steering = 0f;

        public override string GetFilename()
        {
            return filename;
        }

        public override void InitInputActions()
        {
            truckIA = new TruckInpuActions();
            truckIA.Enable();

            moveAction = truckIA.Truck.Move;
            brakeAction = truckIA.Truck.Brake;
        }

        public override void DisposeInputActions()
        {
            truckIA.Dispose();
        }

        public override void UpdateLogic()
        {
            _acceleration = moveAction.ReadValue<Vector2>().y > 0 ? 1f : 0;
            _acceleration = moveAction.ReadValue<Vector2>().y < 0 ? _acceleration - 1 : _acceleration;
            truckController.AccelerationInput = _acceleration;

            _steering = 0f;
            _steering = moveAction.ReadValue<Vector2>().x > 0 ? _steering + 1 : _steering;
            _steering = moveAction.ReadValue<Vector2>().x < 0 ? _steering - 1 : _steering;
            truckController.SteeringInput = _steering;

            truckController.BrakesInput = brakeAction.ReadValue<float>() > 0 ? 1f : 0f;
            // truckController.HandBrakeInput = brakeAction.triggered ? 1f : 0f;
            // truckController.ClutchInput = brakeAction.triggered ? 1f : 0f;
        }

        public void ZeroSpeed()
        {
            _acceleration = 0;
            _steering = 0;
            truckController.AccelerationInput = 0;
            truckController.SteeringInput = 0;
            truckController.ZeroSpeed();
        }
    }
}