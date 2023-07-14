using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [System.Serializable]
    public class ForkliftController : MonoBehaviour
    {
        public float forksVerticalSpeed = 0.3f;
        public float forksHorizontalSpeed = 0.5f;
        public float mastTiltSpeed = 0.3f;

        [SerializeField] private bool _isEngineOn = true;

        [SerializeField] public RotatingMechanicalPart mainMast;
        [SerializeField] public MovingMechanicalPart secondaryMast;
        [SerializeField] public MovingMechanicalPart forksCylinders;
        [SerializeField] public MovingMechanicalPart forks;
        [SerializeField] public Transform forksVerticalLever;
        [SerializeField] public Transform forksHorizontalLever;
        [SerializeField] public Transform mastTiltLever;
        [SerializeField] public AudioSource forkMovingSFX;
        [SerializeField] public AudioSource forkStartMovingSFX;
        [SerializeField] public AudioSource forkStopMovingSFX;

        private float _verticalLeverAngle = 0;
        private float _horizontalLeverAngle = 0;
        private float _tiltLeverAngle = 0;

        [Range(0f, 1f)] private float _forksVertical;
        [Range(0f, 1f)] private float _secondaryMastVertical;
        [Range(0f, 1f)] private float _forksHorizontal;
        [Range(0f, 1f)] private float _mastTilt;

        public float ForksVertical { get { return _forksVertical; } set { _forksVertical = value; } }
        public float ForksHorizontal { get { return _forksHorizontal; } set { _forksHorizontal = value; } }
        public float MastTilt { get { return _mastTilt; } set { _mastTilt = value; } }

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

        /// <summary>
        /// Initialize forklift
        /// </summary>
        private void Start()
        {
            _mastTilt = mainMast.MovementInput;
            _forksHorizontal = forksCylinders.MovementInput;
            _forksVertical = forks.MovementInput;
            _secondaryMastVertical = secondaryMast.MovementInput;

            forksVerticalSpeed = Mathf.Abs(forksVerticalSpeed);
            forksHorizontalSpeed = Mathf.Abs(forksHorizontalSpeed);
            mastTiltSpeed = Mathf.Abs(mastTiltSpeed);
        }

        /// <summary>
        /// 
        /// </summary>
        private void LateUpdate()
        {
            if (_isEngineOn)
            {
                bool forksMoving = forks.IsMoving || secondaryMast.IsMoving || mainMast.IsMoving || forksCylinders.IsMoving;
                ForksMovementSFX(forksMoving); //Should be called on late update to track SFX correctly
            }
        }

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
        /// Handles forks vertical movement
        /// </summary>
        /// <param name="verticalInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveForksVertically(int verticalInput)
        {
            if (_isEngineOn)
            {
                _forksVertical += _secondaryMastVertical <= 0 ? (verticalInput * Time.deltaTime * forksVerticalSpeed) : 1f;
                _forksVertical = Mathf.Clamp01(_forksVertical);

                _secondaryMastVertical += _forksVertical >= 1 ? (verticalInput * Time.deltaTime * forksVerticalSpeed) : 0f;
                _secondaryMastVertical = Mathf.Clamp01(_secondaryMastVertical);

                forks.MovementInput = _forksVertical;
                secondaryMast.MovementInput = _secondaryMastVertical; 
            }
        }

        /// <summary>
        /// Handles forks horizontal movement
        /// </summary>
        /// <param name="horizontalInput">-1 = left | 0 = none | 1 = right</param>
        public void MoveForksHorizontally(int horizontalInput)
        {
            if (_isEngineOn)
            {
                _forksHorizontal += (horizontalInput * Time.deltaTime * forksHorizontalSpeed);
                _forksHorizontal = Mathf.Clamp01(_forksHorizontal);
                forksCylinders.MovementInput = _forksHorizontal; 
            }
        }

        /// <summary>
        /// Handles mast rotation
        /// </summary>
        /// <param name="direction">-1 = backwards | 0 = none | 1 = forward</param>
        public void RotateMast(int direction)
        {
            if (_isEngineOn)
            {
                _mastTilt += (direction * Time.deltaTime * mastTiltSpeed);
                _mastTilt = Mathf.Clamp01(_mastTilt);
                mainMast.MovementInput = _mastTilt; 
            }
        }

        /// <summary>
        /// Animate levers accordingly to player's input
        /// </summary>
        /// <param name="forkVerticalInput"></param>
        /// <param name="forkHorizontalInput"></param>
        /// <param name="mastTiltInput"></param>
        public void UpdateLevers(int forkVerticalInput, int forkHorizontalInput, int mastTiltInput)
        {
            if (_isEngineOn)
            {
                _verticalLeverAngle = Mathf.MoveTowards(_verticalLeverAngle, forkVerticalInput * -10f, 70f * Time.deltaTime);
                _horizontalLeverAngle = Mathf.MoveTowards(_horizontalLeverAngle, forkHorizontalInput * 10f, 70f * Time.deltaTime);
                _tiltLeverAngle = Mathf.MoveTowards(_tiltLeverAngle, mastTiltInput * 10f, 70f * Time.deltaTime);

                if (forksVerticalLever != null) forksVerticalLever.localEulerAngles = new Vector3(_verticalLeverAngle, 0f, 0f);
                if (forksHorizontalLever != null) forksHorizontalLever.localEulerAngles = new Vector3(_horizontalLeverAngle, 0f, 0f);
                if (mastTiltLever != null) mastTiltLever.localEulerAngles = new Vector3(_tiltLeverAngle, 0f, 0f); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="play"></param>
        private void ForksMovementSFX(bool forksMoving)
        {
            if (_isEngineOn && forkMovingSFX != null)
            {
                if (!forkMovingSFX.isPlaying && forksMoving)
                {
                    forkMovingSFX.Play();

                    if (forkStartMovingSFX != null && !forkStartMovingSFX.isPlaying)
                        forkStartMovingSFX.Play();
                }
                else if (forkMovingSFX.isPlaying && !forksMoving)
                {
                    forkMovingSFX.Stop();

                    if (forkStopMovingSFX != null && !forkStopMovingSFX.isPlaying)
                        forkStopMovingSFX.Play();
                }
            }
        }
    }
}
