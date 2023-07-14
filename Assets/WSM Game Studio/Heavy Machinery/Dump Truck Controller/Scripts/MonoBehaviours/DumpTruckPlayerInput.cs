using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.HeavyMachinery
{
    public class DumpTruckPlayerInput : MonoBehaviour
    {
        public bool enablePlayerInput = true;
        public DumpTruckInputSettings inputSettings;
        public UnityEvent[] customEvents;

        private DumpTruckController _dumpTruckController;

        private int _dumpBedTilt = 0;

        /// <summary>
        /// Initializing references
        /// </summary>
        void Start()
        {
            _dumpTruckController = GetComponent<DumpTruckController>();
        }

        /// <summary>
        /// Handling player input
        /// </summary>
        void Update()
        {
            if (enablePlayerInput)
            {
                if (inputSettings == null) return;

                #region Wheel Loader Controls

                if (Input.GetKeyDown(inputSettings.toggleEngine))
                    _dumpTruckController.IsEngineOn = !_dumpTruckController.IsEngineOn;

                _dumpBedTilt = Input.GetKey(inputSettings.dumpBedUp) ? 1 : (Input.GetKey(inputSettings.dumpBedDown) ? -1 : 0);

                _dumpTruckController.MoveDumpBed(_dumpBedTilt);
                _dumpTruckController.UpdateLevers(_dumpBedTilt);

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
