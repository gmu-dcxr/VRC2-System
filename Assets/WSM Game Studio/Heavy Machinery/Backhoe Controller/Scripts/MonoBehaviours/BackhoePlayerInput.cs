using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.HeavyMachinery
{
    [RequireComponent(typeof(BackhoeController))]
    public class BackhoePlayerInput : MonoBehaviour
    {
        public bool enablePlayerInput = true;
        public BackhoeInputSettings inputSettings;
        public UnityEvent[] customEvents;

        private BackhoeController _backhoeController;

        private int _loaderFrameTilt = 0;
        private int _frontBucketTilt = 0;
        private int _swingFrameTilt = 0;
        private int _boomTilt = 0;
        private int _armTilt = 0;
        private int _rearBucketTilt = 0;
        private int _stabilizerLegsTilt = 0;

        /// <summary>
        /// Initializing references
        /// </summary>
        void Start()
        {
            _backhoeController = GetComponent<BackhoeController>();
        }

        /// <summary>
        /// Handling player input
        /// </summary>
        void Update()
        {
            if (inputSettings == null) return;

            #region Backhoe Controls

            if (Input.GetKeyDown(inputSettings.toggleEngine))
                _backhoeController.IsEngineOn = !_backhoeController.IsEngineOn;

            //Loader
            _loaderFrameTilt = Input.GetKey(inputSettings.loaderFrameUp) ? 1 : (Input.GetKey(inputSettings.loaderFrameDown) ? -1 : 0);
            _frontBucketTilt = Input.GetKey(inputSettings.frontBucketUp) ? 1 : (Input.GetKey(inputSettings.frontBucketDown) ? -1 : 0);
            //Backhoe
            _swingFrameTilt = Input.GetKey(inputSettings.swingFrameRight) ? 1 : (Input.GetKey(inputSettings.swingFrameLeft) ? -1 : 0);
            _boomTilt = Input.GetKey(inputSettings.boomForwards) ? 1 : (Input.GetKey(inputSettings.boomBackwards) ? -1 : 0);
            _armTilt = Input.GetKey(inputSettings.armForwards) ? 1 : (Input.GetKey(inputSettings.armBackwards) ? -1 : 0);
            _rearBucketTilt = Input.GetKey(inputSettings.rearBucketUp) ? 1 : (Input.GetKey(inputSettings.rearBucketDown) ? -1 : 0);
            _stabilizerLegsTilt = Input.GetKey(inputSettings.stabilizerLegsUp) ? 1 : (Input.GetKey(inputSettings.stabilizerLegsDown) ? -1 : 0);

            //Loader
            _backhoeController.MoveLoaderFrame(_loaderFrameTilt);
            _backhoeController.MoveFrontBucket(_frontBucketTilt, _loaderFrameTilt);
            //Backhoe
            _backhoeController.MoveSwingFrame(_swingFrameTilt);
            _backhoeController.MoveBoom(_boomTilt);
            _backhoeController.MoveArm(_armTilt);
            _backhoeController.MoveRearBucket(_rearBucketTilt);
            _backhoeController.MoveStabilizerLegs(_stabilizerLegsTilt);
            //Levers
            _backhoeController.UpdateLevers(_loaderFrameTilt, _frontBucketTilt, _swingFrameTilt, _boomTilt, _armTilt, _rearBucketTilt);

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
