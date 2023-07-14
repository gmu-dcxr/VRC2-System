using UnityEngine;
using UnityEngine.UI;
using WSMGameStudio.Vehicles;

namespace WSMGameStudio.HeavyMachinery
{
    public class DumpTruckDemo : MonoBehaviour
    {
        public GameObject dumpTruck;
        public Text txtControls;

        private DumpTruckPlayerInput _dumpTruckInput;
        private WSMVehiclePlayerInput _vehicleInput;

        private bool _showControlsText = false;
        private const string _defaultText = "Show/Hide Controls: Tab";
        private string _controlsText = string.Empty;

        // Use this for initialization
        void Start()
        {
            if (dumpTruck != null)
            {
                _dumpTruckInput = dumpTruck.GetComponent<DumpTruckPlayerInput>();
                _vehicleInput = dumpTruck.GetComponent<WSMVehiclePlayerInput>();

                FormatControlsText();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _showControlsText = !_showControlsText;

                if (_showControlsText)
                    txtControls.text = _controlsText;
                else
                    txtControls.text = _defaultText;
            }
        }

        private void FormatControlsText()
        {
            _controlsText = string.Format("{0}{1}", _defaultText, System.Environment.NewLine);

            //Backhoe
            _controlsText += string.Format("{0}DUMP TRUCK{0}", System.Environment.NewLine);
            _controlsText += string.Format("Dump Bed up/down: {0}/{1}{2}", _dumpTruckInput.inputSettings.dumpBedUp, _dumpTruckInput.inputSettings.dumpBedDown, System.Environment.NewLine);
            //Vehicle
            _controlsText += string.Format("{0}VEHICLE{0}", System.Environment.NewLine);
            _controlsText += string.Format("Vehicle's Engine On/Off: {0}{1}", _vehicleInput.inputSettings.toggleEngine, System.Environment.NewLine);
            _controlsText += string.Format("Acceleration/Reverse: {0}/{1}{2}", _vehicleInput.inputSettings.acceleration, _vehicleInput.inputSettings.reverse, System.Environment.NewLine);
            _controlsText += string.Format("Steering Left/Right: {0}/{1}{2}", _vehicleInput.inputSettings.turnLeft, _vehicleInput.inputSettings.turnRight, System.Environment.NewLine);
            _controlsText += string.Format("Brakes/Handbrake: {0}/{1}{2}", _vehicleInput.inputSettings.brakes, _vehicleInput.inputSettings.handbrake, System.Environment.NewLine);
            _controlsText += string.Format("Clutch: {0}{1}", _vehicleInput.inputSettings.clutch, System.Environment.NewLine);
            _controlsText += string.Format("Horn: {0}{1}", _vehicleInput.inputSettings.horn, System.Environment.NewLine);
            _controlsText += string.Format("Headlights: {0}{1}", _vehicleInput.inputSettings.headlights, System.Environment.NewLine);
            _controlsText += string.Format("Left Signal Light: {0}{1}", _vehicleInput.inputSettings.leftSignalLights, System.Environment.NewLine);
            _controlsText += string.Format("Right Signal Light: {0}{1}", _vehicleInput.inputSettings.rightSignalLights, System.Environment.NewLine);
            if (_vehicleInput.inputSettings.customEventTriggers != null && _vehicleInput.inputSettings.customEventTriggers.Length >= 2)
            {
                _controlsText += string.Format("Left Door: {0}{1}", _vehicleInput.inputSettings.customEventTriggers[0], System.Environment.NewLine);
                _controlsText += string.Format("Right Door: {0}{1}", _vehicleInput.inputSettings.customEventTriggers[1], System.Environment.NewLine);
            }
            //Camera
            _controlsText += string.Format("{0}CAMERA{0}", System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Right: {0}{1}", _vehicleInput.inputSettings.cameraLookRight, System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Right: {0}{1}", _vehicleInput.inputSettings.cameraLookLeft, System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Back: {0}{1}", _vehicleInput.inputSettings.cameraLookBack, System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Up: {0}{1}", _vehicleInput.inputSettings.cameraLookUp, System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Down: {0}{1}", _vehicleInput.inputSettings.cameraLookDown, System.Environment.NewLine);
            _controlsText += string.Format("Toggle Camera: {0}{1}", _vehicleInput.inputSettings.toggleCamera, System.Environment.NewLine);
        }
    } 
}
