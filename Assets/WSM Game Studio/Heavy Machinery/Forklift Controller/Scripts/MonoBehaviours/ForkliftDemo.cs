using UnityEngine;
using UnityEngine.UI;
using WSMGameStudio.Vehicles;

namespace WSMGameStudio.HeavyMachinery
{
    public class ForkliftDemo : MonoBehaviour
    {
        public GameObject forklift;
        public Text txtControls;

        private ForkliftPlayerInput _forkliftInput;
        private WSMVehiclePlayerInput _vehicleInput;

        private bool _showControlsText = false;
        private const string _defaultText = "Show/Hide Controls: Tab";
        private string _controlsText = string.Empty;

        // Use this for initialization
        void Start()
        {
            _forkliftInput = forklift.GetComponent<ForkliftPlayerInput>();
            _vehicleInput = forklift.GetComponent<WSMVehiclePlayerInput>();

            FormatControlsText();
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

            //Forklift
            _controlsText += string.Format("{0}FORKLIFT{0}", System.Environment.NewLine);
            _controlsText += string.Format("Forks Engine On/Off: {0}{1}", _forkliftInput.inputSettings.toggleEngine, System.Environment.NewLine);
            _controlsText += string.Format("Forks up/down: {0}/{1}{2}", _forkliftInput.inputSettings.forksUp, _forkliftInput.inputSettings.forksDown, System.Environment.NewLine);
            _controlsText += string.Format("Forks left/right: {0}/{1}{2}", _forkliftInput.inputSettings.forksLeft, _forkliftInput.inputSettings.forksRight, System.Environment.NewLine);
            _controlsText += string.Format("Mast Tilt Back/Forward: {0}/{1}{2}", _forkliftInput.inputSettings.mastTiltBackwards, _forkliftInput.inputSettings.mastTiltForwards, System.Environment.NewLine);
            //Vehicle
            _controlsText += string.Format("{0}VEHICLE{0}", System.Environment.NewLine);
            _controlsText += string.Format("Vehicle's Engine On/Off: {0}{1}", _vehicleInput.inputSettings.toggleEngine, System.Environment.NewLine);
            _controlsText += string.Format("Acceleration/Reverse: {0}/{1}{2}", _vehicleInput.inputSettings.acceleration, _vehicleInput.inputSettings.reverse, System.Environment.NewLine);
            _controlsText += string.Format("Steering Left/Right: {0}/{1}{2}", _vehicleInput.inputSettings.turnLeft, _vehicleInput.inputSettings.turnRight, System.Environment.NewLine);
            _controlsText += string.Format("Brakes/Handbrake: {0}/{1}{2}", _vehicleInput.inputSettings.brakes, _vehicleInput.inputSettings.handbrake, System.Environment.NewLine);
            _controlsText += string.Format("Clutch: {0}{1}", _vehicleInput.inputSettings.clutch, System.Environment.NewLine);
            _controlsText += string.Format("Horn: {0}{1}", _vehicleInput.inputSettings.horn, System.Environment.NewLine);
            _controlsText += string.Format("Headlights: {0}{1}", _vehicleInput.inputSettings.headlights, System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Right: {0}{1}", _vehicleInput.inputSettings.cameraLookRight, System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Right: {0}{1}", _vehicleInput.inputSettings.cameraLookLeft, System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Back: {0}{1}", _vehicleInput.inputSettings.cameraLookBack, System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Up: {0}{1}", _vehicleInput.inputSettings.cameraLookUp, System.Environment.NewLine);
            _controlsText += string.Format("Camera Look Down: {0}{1}", _vehicleInput.inputSettings.cameraLookDown, System.Environment.NewLine);
            _controlsText += string.Format("Toggle Camera: {0}{1}", _vehicleInput.inputSettings.toggleCamera, System.Environment.NewLine);
        }
    }
}
