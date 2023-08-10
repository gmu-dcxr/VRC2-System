using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [System.Serializable]
    public class BackhoeController : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField] public LevelingMode levelingMode;
        public float loaderFrameSpeed = 0.5f;
        public float frontBucketSpeed = 0.3f;
        public float loaderSelfLevelingSpeed = 0.22f;
        public float swingFrameSpeed = 0.5f;
        public float boomSpeed = 0.5f;
        public float armSpeed = 0.5f;
        public float rearBucketSpeed = 0.3f;
        public float stabilizerLegSpeed = 0.3f;

        //Loader
        private float _loaderFrameLeverAngle = 0f;

        private float _frontBucketLeverAngle = 0f;

        //Backhoe
        private float _swingFrameLeverAngle = 0;
        private float _boomLeverAngle = 0;
        private float _armLeverAngle = 0;
        private float _rearBucketLeverAngle = 0f;

        [SerializeField] private bool _isEngineOn = true;

        [SerializeField] public RotatingMechanicalPart loaderFrame;
        [SerializeField] public RotatingMechanicalPart frontBucket;
        [SerializeField] public RotatingMechanicalPart swingFrame;
        [SerializeField] public RotatingMechanicalPart boom;
        [SerializeField] public RotatingMechanicalPart arm;
        [SerializeField] public RotatingMechanicalPart rearBucket;
        [SerializeField] public RotatingMechanicalPart leftStabilizerLeg;
        [SerializeField] public RotatingMechanicalPart rightStabilizerLeg;
        [SerializeField] public Transform loaderFrameLever;
        [SerializeField] public Transform frontBucketLever;
        [SerializeField] public Transform swingFrameLever;
        [SerializeField] public Transform boomLever;
        [SerializeField] public Transform armLever;
        [SerializeField] public Transform rearBucketLever;
        [SerializeField] public AudioSource partsMovingSFX;
        [SerializeField] public AudioSource partsStartMovingSFX;
        [SerializeField] public AudioSource partsStopMovingSFX;

        //Loader
        [Range(0f, 1f)] private float _loaderFrameTilt;
        [Range(0f, 1f)] private float _frontBucketTilt;

        [Range(0f, 1f)] private float _bellCrankTilt;

        //Backhoe
        [Range(0f, 1f)] private float _swingFrameTilt;
        [Range(0f, 1f)] private float _boomTilt;
        [Range(0f, 1f)] private float _armTilt;
        [Range(0f, 1f)] private float _rearBucketTilt;
        [Range(0f, 1f)] private float _stabilizerLegsTilt;

        #endregion

        #region PROPERTIES

        //Loader
        public float LoaderFrameTilt
        {
            get { return _loaderFrameTilt; }
            set { _loaderFrameTilt = value; }
        }

        public float FrontBucketTilt
        {
            get { return _frontBucketTilt; }
            set { _frontBucketTilt = value; }
        }

        //Backhoe
        public float SwingFrameTilt
        {
            get { return _swingFrameTilt; }
            set { _swingFrameTilt = value; }
        }

        public float BoomTilt
        {
            get { return _boomTilt; }
            set { _boomTilt = value; }
        }

        public float ArmTilt
        {
            get { return _armTilt; }
            set { _armTilt = value; }
        }

        public float RearBucketTilt
        {
            get { return _rearBucketTilt; }
            set { _rearBucketTilt = value; }
        }

        public float StabilizerLegsTilt
        {
            get { return _stabilizerLegsTilt; }
            set { _stabilizerLegsTilt = value; }
        }

        public bool IsEngineOn
        {
            get { return _isEngineOn; }
            set
            {
                if (!_isEngineOn && value)
                    StartEngine();
                else if (_isEngineOn && !value)
                    StopEngine();
            }
        }

        #endregion

        #region UNITY METHODS

        /// <summary>
        /// Initialize Backhoe
        /// </summary>
        private void Start()
        {
            if (loaderFrame != null) _loaderFrameTilt = loaderFrame.MovementInput;
            if (frontBucket != null) _frontBucketTilt = frontBucket.MovementInput;
            if (swingFrame != null) _swingFrameTilt = swingFrame.MovementInput;
            if (boom != null) _boomTilt = boom.MovementInput;
            if (arm != null) _armTilt = arm.MovementInput;
            if (rearBucket != null) _rearBucketTilt = rearBucket.MovementInput;
            if (leftStabilizerLeg != null) _stabilizerLegsTilt = leftStabilizerLeg.MovementInput;

            loaderFrameSpeed = Mathf.Abs(loaderFrameSpeed);
            frontBucketSpeed = Mathf.Abs(frontBucketSpeed);
            loaderSelfLevelingSpeed = Mathf.Abs(loaderSelfLevelingSpeed);
            armSpeed = Mathf.Abs(armSpeed);
            rearBucketSpeed = Mathf.Abs(rearBucketSpeed);
        }

        /// <summary>
        /// Late Update
        /// </summary>
        private void LateUpdate()
        {
            if (_isEngineOn)
            {
                bool isMoving =
                    (loaderFrame == null ? false : loaderFrame.IsMoving) ||
                    (frontBucket == null ? false : frontBucket.IsMoving) ||
                    (boom == null ? false : boom.IsMoving) ||
                    (arm == null ? false : arm.IsMoving) ||
                    (rearBucket == null ? false : rearBucket.IsMoving) ||
                    (leftStabilizerLeg == null ? false : leftStabilizerLeg.IsMoving) ||
                    (rightStabilizerLeg == null ? false : rightStabilizerLeg.IsMoving);
                MechanicalMovementSFX(isMoving); //Should be called on late update to track SFX correctly
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Starts vehicle engine
        /// </summary>
        public void StartEngine()
        {
            _isEngineOn = true;
        }

        /// <summary>
        /// Stop vehicle engine
        /// </summary>
        public void StopEngine()
        {
            _isEngineOn = false;
        }

        /// <summary>
        /// Handles loader frame movement
        /// </summary>
        /// <param name="horizontalInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveLoaderFrame(int frameInput)
        {
            if (_isEngineOn && loaderFrame != null)
            {
                _loaderFrameTilt += (frameInput * Time.deltaTime * loaderFrameSpeed);
                _loaderFrameTilt = Mathf.Clamp01(_loaderFrameTilt);
                loaderFrame.MovementInput = _loaderFrameTilt;
            }
        }

        /// <summary>
        /// Handles loader frame movement
        /// </summary>
        /// <param name="horizontalInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveFrontBucket(int bucketInput, int frameInput)
        {
            if (_isEngineOn && frontBucket != null)
            {
                float speed = frontBucketSpeed;

                if (levelingMode == LevelingMode.SelfLeveling && bucketInput == 0)
                {
                    if (loaderFrame.IsMoving)
                    {
                        bucketInput = -frameInput;
                        speed = loaderSelfLevelingSpeed;
                    }
                }

                _frontBucketTilt += (bucketInput * Time.deltaTime * speed);
                _frontBucketTilt = Mathf.Clamp01(_frontBucketTilt);
                frontBucket.MovementInput = _frontBucketTilt;
            }
        }

        /// <summary>
        /// Handles swing frame movement
        /// </summary>
        /// <param name="swingFrameInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveSwingFrame(int swingFrameInput)
        {
            if (_isEngineOn && swingFrame != null)
            {
                _swingFrameTilt += (swingFrameInput * Time.deltaTime * swingFrameSpeed);
                _swingFrameTilt = Mathf.Clamp01(_swingFrameTilt);
                swingFrame.MovementInput = _swingFrameTilt;
            }
        }

        /// <summary>
        /// Handles boom movement
        /// </summary>
        /// <param name="boomInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveBoom(int boomInput)
        {
            if (_isEngineOn && boom != null)
            {
                _boomTilt += (boomInput * Time.deltaTime * boomSpeed);
                _boomTilt = Mathf.Clamp01(_boomTilt);
                boom.MovementInput = _boomTilt;
            }
        }

        /// <summary>
        /// Handles arm movement
        /// </summary>
        /// <param name="armInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveArm(int armInput)
        {
            if (_isEngineOn && arm != null)
            {
                _armTilt += (armInput * Time.deltaTime * armSpeed);
                _armTilt = Mathf.Clamp01(_armTilt);
                arm.MovementInput = _armTilt;
            }
        }

        /// <summary>
        /// Handles rear bucket movement
        /// </summary>
        /// <param name="bucketInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveRearBucket(int bucketInput)
        {
            if (_isEngineOn && rearBucket != null)
            {
                _rearBucketTilt += (bucketInput * Time.deltaTime * rearBucketSpeed);
                _rearBucketTilt = Mathf.Clamp01(_rearBucketTilt);
                rearBucket.MovementInput = _rearBucketTilt;
            }
        }

        /// <summary>
        /// Handles stabilizer legs movement
        /// </summary>
        /// <param name="stabilizerLegsInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveStabilizerLegs(int stabilizerLegsInput)
        {
            if (_isEngineOn)
            {
                _stabilizerLegsTilt += (stabilizerLegsInput * Time.deltaTime * stabilizerLegSpeed);
                _stabilizerLegsTilt = Mathf.Clamp01(_stabilizerLegsTilt);
                if (leftStabilizerLeg != null) leftStabilizerLeg.MovementInput = _stabilizerLegsTilt;
                if (rightStabilizerLeg != null) rightStabilizerLeg.MovementInput = _stabilizerLegsTilt;
            }
        }

        /// <summary>
        /// Animate levers accordingly to player's input
        /// </summary>
        /// <param name="frameInput"></param>
        /// <param name="frontBucketInput"></param>
        /// <param name="swingFrameInput"></param>
        /// <param name="boomInput"></param>
        /// <param name="armInput"></param>
        /// <param name="rearBucketInput"></param>
        public void UpdateLevers(int frameInput, int frontBucketInput, int swingFrameInput, int boomInput, int armInput,
            int rearBucketInput)
        {
            if (_isEngineOn)
            {
                _loaderFrameLeverAngle =
                    Mathf.MoveTowards(_loaderFrameLeverAngle, frameInput * -15f, 80f * Time.deltaTime);
                _frontBucketLeverAngle =
                    Mathf.MoveTowards(_frontBucketLeverAngle, frontBucketInput * -15f, 80f * Time.deltaTime);
                _swingFrameLeverAngle =
                    Mathf.MoveTowards(_swingFrameLeverAngle, swingFrameInput * -10f, 80f * Time.deltaTime);
                _boomLeverAngle = Mathf.MoveTowards(_boomLeverAngle, boomInput * -10f, 80f * Time.deltaTime);
                _armLeverAngle = Mathf.MoveTowards(_armLeverAngle, armInput * -10f, 80f * Time.deltaTime);
                _rearBucketLeverAngle =
                    Mathf.MoveTowards(_rearBucketLeverAngle, rearBucketInput * -10f, 80f * Time.deltaTime);

                if (loaderFrameLever != null)
                    loaderFrameLever.localEulerAngles = new Vector3(_loaderFrameLeverAngle, 0f, 0f);
                if (frontBucketLever != null)
                    frontBucketLever.localEulerAngles = new Vector3(_frontBucketLeverAngle, 0f, 0f);
                if (swingFrameLever != null)
                    swingFrameLever.localEulerAngles = new Vector3(_swingFrameLeverAngle, 0f, 0f);
                if (boomLever != null) boomLever.localEulerAngles = new Vector3(_boomLeverAngle, 0f, 0f);
                if (armLever != null) armLever.localEulerAngles = new Vector3(_armLeverAngle, 0f, 0f);
                if (rearBucketLever != null)
                    rearBucketLever.localEulerAngles = new Vector3(_rearBucketLeverAngle, 0f, 0f);
            }
        }

        /// <summary>
        /// Handles mechanical parts SFX
        /// </summary>
        /// <param name="moving"></param>
        private void MechanicalMovementSFX(bool moving)
        {
            if (IsEngineOn && partsMovingSFX != null)
            {
                if (!partsMovingSFX.isPlaying && moving)
                {
                    partsMovingSFX.Play();

                    if (partsStartMovingSFX != null && !partsStartMovingSFX.isPlaying)
                        partsStartMovingSFX.Play();
                }
                else if (partsMovingSFX.isPlaying && !moving)
                {
                    partsMovingSFX.Stop();

                    if (partsStopMovingSFX != null && !partsStopMovingSFX.isPlaying)
                        partsStopMovingSFX.Play();
                }
            }
        }

        #endregion

        #region HACK - Reset


        public void ResetBackhoe()
        {
            if (loaderFrameLever != null) loaderFrameLever.localEulerAngles = new Vector3(0, 0f, 0f);
            if (frontBucketLever != null) frontBucketLever.localEulerAngles = new Vector3(0, 0f, 0f);
            if (swingFrameLever != null) swingFrameLever.localEulerAngles = new Vector3(0, 0f, 0f);
            if (boomLever != null) boomLever.localEulerAngles = new Vector3(0, 0f, 0f);
            if (armLever != null) armLever.localEulerAngles = new Vector3(0, 0f, 0f);
            if (rearBucketLever != null) rearBucketLever.localEulerAngles = new Vector3(0, 0f, 0f);

            if (leftStabilizerLeg != null) leftStabilizerLeg.ResetToMin();
            if (rightStabilizerLeg != null) rightStabilizerLeg.ResetToMin();
        }


        #endregion
    }
}
