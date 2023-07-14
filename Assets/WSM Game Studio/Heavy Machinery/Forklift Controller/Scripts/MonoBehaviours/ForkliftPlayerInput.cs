using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.HeavyMachinery
{
    [RequireComponent(typeof(ForkliftController))]
    public class ForkliftPlayerInput : MonoBehaviour
    {
        public bool enablePlayerInput = true;
        public ForkliftInputSettings inputSettings;
        public UnityEvent[] customEvents;

        private ForkliftController _forkliftController;

        private int _mastTilt = 0;
        private int _forksVertical = 0;
        private int _forksHorizontal = 0;

        /// <summary>
        /// Initializing references
        /// </summary>
        void Start()
        {
            _forkliftController = GetComponent<ForkliftController>();
        }

        /// <summary>
        /// Handling player input
        /// </summary>
        void Update()
        {
            if (enablePlayerInput)
            {
                if (inputSettings == null) return;

                #region Forklift Controls

                if (Input.GetKeyDown(inputSettings.toggleEngine))
                    _forkliftController.IsEngineOn = !_forkliftController.IsEngineOn;

                _mastTilt = Input.GetKey(inputSettings.mastTiltForwards) ? 1 : (Input.GetKey(inputSettings.mastTiltBackwards) ? -1 : 0);
                _forksVertical = Input.GetKey(inputSettings.forksUp) ? 1 : (Input.GetKey(inputSettings.forksDown) ? -1 : 0);
                _forksHorizontal = Input.GetKey(inputSettings.forksRight) ? 1 : (Input.GetKey(inputSettings.forksLeft) ? -1 : 0);

                _forkliftController.RotateMast(_mastTilt);
                _forkliftController.MoveForksVertically(_forksVertical);
                _forkliftController.MoveForksHorizontally(_forksHorizontal);
                _forkliftController.UpdateLevers(_forksVertical, _forksHorizontal, _mastTilt);

                #endregion

                #region Player Custom Events

                for (int i = 0; i < inputSettings.customEventTriggers.Length; i++)
                {
                    if (Input.GetKeyDown(inputSettings.customEventTriggers[i]))
                    {
                        if (customEvents.Length > i)
                            customEvents[i].Invoke();
                    }
                }

                #endregion
            }
        }
    }
}
