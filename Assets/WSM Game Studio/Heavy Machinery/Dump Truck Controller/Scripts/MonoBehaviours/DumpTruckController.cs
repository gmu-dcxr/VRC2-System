using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    public class DumpTruckController : MonoBehaviour
    {
        #region VARIABLES
        public float dumpBedSpeed = 0.2f;

        private float _dumpBedLeverAngle = 0f;
        private bool _tailgateLocked = true;

        [SerializeField] private bool _isEngineOn = true;

        [SerializeField] public RotatingMechanicalPart dumpBed;
        [SerializeField] public Transform dumpBedLever;
        [SerializeField] public HingeJoint tailgateJoint;
        [SerializeField] public Transform tailgateAnchor;
        [SerializeField] public AudioSource partsMovingSFX;
        [SerializeField] public AudioSource partsStartMovingSFX;
        [SerializeField] public AudioSource partsStopMovingSFX;

        [Range(0f, 1f)] private float _dumpBedTilt;
        #endregion

        #region PROPERTIES
        public float DumpBedTilt { get { return _dumpBedTilt; } set { _dumpBedTilt = value; } }

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
        /// Initialize wheel loader
        /// </summary>
        private void Start()
        {
            if (dumpBed != null) _dumpBedTilt = dumpBed.MovementInput;

            dumpBedSpeed = Mathf.Abs(dumpBedSpeed);
        }

        /// <summary>
        /// Updates each frame
        /// </summary>
        private void Update()
        {
            if (_isEngineOn)
            {
                TailgatePhysics();
            }
        }

        /// <summary>
        /// Late Update
        /// </summary>
        private void LateUpdate()
        {
            if (_isEngineOn && dumpBed != null)
            {
                bool isMoving = dumpBed.IsMoving;
                DumpbedMovementSFX(isMoving); //Should be called on late update to track SFX correctly
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
        /// Handles dump bed movement
        /// </summary>
        /// <param name="horizontalInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveDumpBed(int dumpBedInput)
        {
            if (_isEngineOn && dumpBed != null)
            {
                _dumpBedTilt += (dumpBedInput * Time.deltaTime * dumpBedSpeed);
                _dumpBedTilt = Mathf.Clamp01(_dumpBedTilt);
                dumpBed.MovementInput = _dumpBedTilt;
            }
        }

        /// <summary>
        /// Animate levers accordingly to player's input
        /// </summary>
        /// <param name="dumpBedInput"></param>
        public void UpdateLevers(int dumpBedInput)
        {
            if (_isEngineOn)
            {
                _dumpBedLeverAngle = Mathf.MoveTowards(_dumpBedLeverAngle, dumpBedInput * -15f, 80f * Time.deltaTime);

                if (dumpBedLever != null) dumpBedLever.localEulerAngles = new Vector3(_dumpBedLeverAngle, 0f, 0f);
            }
        }

        /// <summary>
        /// Handles mechanical parts sfx
        /// </summary>
        /// <param name="frameMoving"></param>
        private void DumpbedMovementSFX(bool frameMoving)
        {
            if (IsEngineOn && partsMovingSFX != null)
            {
                if (!partsMovingSFX.isPlaying && frameMoving)
                {
                    partsMovingSFX.Play();

                    if (partsStartMovingSFX != null && !partsStartMovingSFX.isPlaying)
                        partsStartMovingSFX.Play();
                }
                else if (partsMovingSFX.isPlaying && !frameMoving)
                {
                    partsMovingSFX.Stop();

                    if (partsStopMovingSFX != null && !partsStopMovingSFX.isPlaying)
                        partsStopMovingSFX.Play();
                }
            }
        }

        /// <summary>
        /// Handles tailgate physics
        /// </summary>
        private void TailgatePhysics()
        {
            if (!_tailgateLocked && tailgateJoint != null && tailgateAnchor != null)
            {
                tailgateJoint.connectedAnchor = tailgateJoint.connectedBody.transform.InverseTransformPoint(tailgateAnchor.position);

                float minLimit = Mathf.Lerp(dumpBed.Min.x, dumpBed.Max.x, dumpBed.MovementInput);

                JointLimits limits = tailgateJoint.limits;
                limits.min = minLimit;
                tailgateJoint.limits = limits;
            }
        }

        /// <summary>
        /// Unlocks dumpbed tailgate
        /// </summary>
        public void UnlockTailgate()
        {
            _tailgateLocked = false;

            JointLimits limits = tailgateJoint.limits;
            limits.max = 90;
            tailgateJoint.limits = limits;
        }

        /// <summary>
        /// Locks dumpbed tailgate
        /// </summary>
        public void LockTailgate()
        {
            _tailgateLocked = true;

            JointLimits limits = tailgateJoint.limits;
            limits.min = 0;
            limits.max = 0;
            tailgateJoint.limits = limits;
        }

        #endregion
    }
}
