using System;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using VRC2.Events;
using WSMGameStudio.Vehicles;

namespace VRC2.Animations.CraneTruck
{
    public class CraneTruckInputRecording : BaseInputRecording
    {

        [Header("Output Settings")] public string filename = "CraneTruckInputRecording";

        #region Truck

        [Space(30)] 
        [Header("Truck")]
        public WSMVehicleController truckController;

        public ControllerTruck craneController;
        
        private float _acceleration = 0f;
        private float _steering = 0f;

        #endregion

        #region Input Actions

        private CraneTruckInputActions craneTIA;

        private InputAction truckMoveInput;
        private InputAction truckBrakeInput;
        private InputAction truckSwithInput;

        private InputAction craneMoveInput;
        private InputAction craneArrowInput;
        private InputAction craneSeizeInput;

        #endregion

        [HideInInspector]public bool truckMode = true;

        public override void InitInputActions()
        {
            craneTIA = new CraneTruckInputActions();
            craneTIA.Enable();

            truckMoveInput = craneTIA.Truck.Move;
            truckBrakeInput = craneTIA.Truck.Brake;
            truckSwithInput = craneTIA.Truck.Switch;

            craneMoveInput = craneTIA.Crane.Move;
            craneArrowInput = craneTIA.Crane.Arrow;
            craneSeizeInput = craneTIA.Crane.Seize;

            truckMode = true;
        }

        public override void DisposeInputActions()
        {
            craneTIA.Dispose();
        }

        public override void UpdateLogic()
        {
        }

        public override string GetFilename()
        {
            return filename;
        }

        void Start()
        {
        }

        void Update()
        {
            if (truckSwithInput.triggered)
            {
                truckMode = !truckMode;
            }
            if (truckMode)
            {
                _acceleration = truckMoveInput.ReadValue<Vector2>().y > 0 ? 1f : 0;
                _acceleration = truckMoveInput.ReadValue<Vector2>().y < 0 ? _acceleration - 1 : _acceleration;
                truckController.AccelerationInput = _acceleration;

                _steering = 0f;
                _steering = truckMoveInput.ReadValue<Vector2>().x > 0 ? _steering + 1 : _steering;
                _steering = truckMoveInput.ReadValue<Vector2>().x < 0 ? _steering - 1 : _steering;
                truckController.SteeringInput = _steering;

                truckController.BrakesInput = truckBrakeInput.triggered ? 1f : 0f;   
            }
            else
            {
                craneController.TriggerCrane();
            }
        }

    }
}