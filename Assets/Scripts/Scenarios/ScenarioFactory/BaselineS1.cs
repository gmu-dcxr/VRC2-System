using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using VRC2.Animations;
using VRC2.Pipe;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{

    internal enum CraneStatus
    {
        Idle = 0,
        Pickup = 1,
        Rotate = 2,
        Dropoff = 3,
        // refactor
        Init,
        DownHookPickup,
        SeizePickup,
        PickupUpHook,
        RotateLeft,
        DownHookDropoff,
        SeizeDropoff,
        DropoffUpHook,
        RotateRight,
    }

    public class BaselineS1 : Scenario
    {
        private Transform _pipeParent;

        private Vector3 _pipeLocalPos;
        private Quaternion _pipeLocalRot;

        public CraneInputRecording recording;
        public CraneInputReplay replay;

        // public float startAngle = 180f;
        public float endAngle = 120f;
        public float dropAngle = 150f; // when to drop the pipe

        public float startBoomcart = -22.40558f; //x
        public float endBoomcart = -10;

        private GameObject crane;

        private GameObject player; // local player

        [Header("GameObjects")] public GameObject playerIndicator;
        public GameObject pipeStack;

        public GameObject unpackedPipe;

        public GameObject craneGroup;
        public GameObject hook;


        private Vector3 pipeStackPos;
        private Quaternion pipeStackRot;

        [Header("Animators")]
        public Animator craneAnim;
        public Animator hookAnim;
        public RecordTransformHierarchy recorder;

        // private bool triggered = false;
        private bool clockWise = false;

        // private bool enableDrop = false;

        private CraneStatus _craneStatus;

        // the start rotation of the crane
        private float startAngle
        {
            get => recording.startRotation;
        }

        // threshold to decide whether reaching the destination
        private float rotationThreshold = 2f;

        private bool normalCondition = false;

        private void Awake() 
        {
            
        }
        private void Start()
        {
            base.Start();
           
            if (localPlayer != null)
            {
                player = localPlayer;
            }
            else
            {
                player = playerIndicator;
            }

            _craneStatus = CraneStatus.Idle;

            BackupTransforms();

            clockWise = false;

            SetActiveness(true, false);

            // override cargo to avoid error
            recording.backupCargo = pipeStack;

            normalCondition = false;
        }

        private void Update()
        {
            
            switch (_craneStatus)
            {
                case CraneStatus.Pickup:
                    replay.Pickup();
                    if (replay.PickupFinished())
                    {
                        _craneStatus = CraneStatus.Rotate;
                    }

                    break;
                case CraneStatus.Rotate:
                    RotateCrane(clockWise);
                    if (NeedStoppingRotation())
                    {
                        // force update the crane rotation
                        ResetCraneRotation(startAngle);

                        recording.StopRotating = true;

                        if (normalCondition)
                        {
                            StopRotating();
                            StartCrane(true, false, reset: true, unpackedpipe: false);
                        }
                        else
                        {
                            _craneStatus = CraneStatus.Idle;
                        }
                    }

                    break;
                case CraneStatus.Dropoff:
                    replay.Dropoff();
                    if (replay.DropoffFinished())
                    {
                        clockWise = true;
                        if (normalCondition)
                        {
                            StartCrane(false, true, rot2end: true);
                        }
                        else
                        {
                            _craneStatus = CraneStatus.Idle;
                        }
                    }

                    break;
                case CraneStatus.Idle:
                    StopRotating();

                    break;
            }
        }
 
        bool NeedStoppingRotation()
        {
            // stop when rotate back
            return clockWise && Math.Abs(recording.GetCraneRotation() - startAngle) < 1f;
        }

        void ResetCraneRotation(float angle)
        {
            recording.ForceUpdateRotation(angle);
        }

        void ResetBoomCart(float x)
        {
            recording.ForceUpdateBoomCart(x);
        }

        void BackupTransforms()
        {
            var t = unpackedPipe.transform;
            _pipeLocalPos = t.localPosition;
            _pipeLocalRot = t.localRotation;
            _pipeParent = t.parent;

            t = pipeStack.transform;
            pipeStackPos = t.position;
            pipeStackRot = t.rotation;
        }

        private void Reset()
        {
            if (unpackedPipe.transform.parent == null)
            {
                PipeHelper.EnsureNoRigidBody(ref unpackedPipe);

                // reset parent
                unpackedPipe.transform.parent = _pipeParent;
                // update local position and rotation
                unpackedPipe.transform.localPosition = _pipeLocalPos;
                unpackedPipe.transform.localRotation = _pipeLocalRot;
            }

            pipeStack.transform.position = pipeStackPos;
            pipeStack.transform.rotation = pipeStackRot;
        }

        void SetActiveness(bool pipestack, bool unpackedpipe)
        {
            pipeStack.GetComponent<MeshRenderer>().enabled = pipestack;
            unpackedPipe.SetActive(unpackedpipe);
        }

        void StopRotating()
        {
            replay.Left(true);
            replay.Right(true);
        }

        void RotateCrane(bool clockWise)
        {
            if (clockWise)
            {
                replay.Left(true);
                replay.Right(false, true);
            }
            else
            {
                replay.Right(true);
                replay.Left(false, true);
            }

            var angle = Math.Abs(recording.GetCraneRotation() - endAngle);
            if (_craneStatus == CraneStatus.Rotate && !clockWise && angle < rotationThreshold)
            {
                // rewind dropoff
                replay.RewindDropoff();

                // force align the rotation
                ResetCraneRotation(endAngle);
                _craneStatus = CraneStatus.Dropoff;
            }
        }

        #region Accident Events Callbacks

        // TODO: When scenario ends, start the normal event
        void StartCrane(bool pickup, bool clockwise, bool reset = false, bool unpackedpipe = false,
            bool rot2end = false, bool rot2start = false)
        {
            if (reset)
            {
                Reset();
            }

            recording.StopRotating = false;

            if (rot2end)
            {
                ResetCraneRotation(endAngle);
            }

            if (rot2start)
            {
                ResetCraneRotation(startAngle);
            }

            SetActiveness(true, unpackedpipe);

            if (pickup)
            {
                replay.RewindPickup();
                _craneStatus = CraneStatus.Pickup;
            }
            else
            {
                _craneStatus = CraneStatus.Rotate;
            }

            clockWise = clockwise;
        }


        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S1");
            // repeat #2 and #3
            normalCondition = true;
            ResetCraneRotation(startAngle);
            StartCrane(true, false, reset: true, unpackedpipe: false);
        }

        public void On_BaselineS1_1_Start()
        {

        }

        public void On_BaselineS1_1_Finish()
        {
        }

        public void On_BaselineS1_2_Start()
        {
            print("On_BaselineS1_2_Start");
            // A load is passing overhead.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;
           
           craneAnim.SetTrigger("CounterClockWise");
           

            //StartCrane(true, false);
        }

        public void On_BaselineS1_2_Finish()
        {
            // A load is passing overhead.
        }

        public void On_BaselineS1_3_Start()
        {
            print("On_BaselineS1_3_Start");
            // A hook (without a load) is passing overhead in the opposite direction.
            // get incident
            var incident = GetIncident(3);
            craneAnim.SetTrigger("ClockWise");
            //StartCrane(false, true, rot2end: true);
        }

        public void On_BaselineS1_3_Finish()
        {
            // A hook (without a load) is passing overhead in the opposite direction.
        }

        public void On_BaselineS1_4_Start()
        {
            print("On_BaselineS1_4_Start");
            // A load with an unpacked pipe is passing overhead.
            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);
            SetActiveness(true, true);
            craneAnim.SetTrigger("CounterClockWise");
            //StartCrane(true, false, true, true, rot2start: true);
        }

        public void On_BaselineS1_4_Finish()
        {
            // A load with an unpacked pipe is passing overhead.
        }

        public void On_BaselineS1_5_Start()
        {
            print("On_BaselineS1_5_Start");
            // A hook (without a load) is passing overhead in the opposite direction.
            // get incident
            var incident = GetIncident(5);
            craneAnim.SetTrigger("ClockWise");
            // StartCrane(false, true, rot2end: true);
        }

        public void On_BaselineS1_5_Finish()
        {
            // A hook (without a load) is passing overhead in the opposite direction.
        }

        public void On_BaselineS1_6_Start()
        {
            print("On_BaselineS1_6_Start");
            // A load with an unpacked pipe is passing overhead.
            // get incident
            var incident = GetIncident(6);
            var warning = incident.Warning;
            print(warning);

            SetActiveness(true, true);
            craneAnim.SetTrigger("pipeFall");
            StartCrane(true, false, true, true, rot2start: true);
        }

        public void On_BaselineS1_6_Finish()
        {
            // A load with an unpacked pipe is passing overhead.
        }

        void DropOffPipe()
        {
            // release it
            unpackedPipe.transform.parent = null;
            // add rigid body
            PipeHelper.EnsureRigidBody(ref unpackedPipe);
            // it will automatically fall

            // enableDrop = false;
        }

        public void On_BaselineS1_7_Start()
        {
            print("On_BaselineS1_7_Start");
            // The unpacked pipe drops next to the participants. And the load is still passing overhead.
            // get incident
            var incident = GetIncident(7);

            DropOffPipe();
        }

        public void On_BaselineS1_7_Finish()
        {
            // The unpacked pipe drops next to the participants. And the load is still passing overhead.
        }

        public void On_BaselineS1_8_Start()
        {
            print("On_BaselineS1_8_Start");
            // A hook (without a load) is passing overhead in the opposite direction.
            // get incident
            var incident = GetIncident(8);
            var warning = incident.Warning;
            print(warning);
            craneAnim.SetTrigger("ClockWise");
            // StartCrane(false, true, rot2end: true);
        }

        public void On_BaselineS1_8_Finish()
        {

        }

        public void On_BaselineS1_9_Start()
        {
            // A load with an unpacked pipe is passing overhead.
            var incident = GetIncident(9);
            var warning = incident.Warning;
            print(warning);
            SetActiveness(true, true);
            craneAnim.SetTrigger("CounterClockWise");
            //StartCrane(true, false, true, true, rot2start: true);
        }

        public void On_BaselineS1_9_Finish()
        {

        }

        public void On_BaselineS1_10_Start()
        {
            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS1_10_Finish()
        {
            // SAGAT query
            HideSAGAT();
        }

        #endregion

    }
}