using System;
// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using VRC2.Animations;
using VRC2.Pipe;
using UnityTimer;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{

    internal enum CraneStatus
    {
        Init,
        DownHookPickup,
        SeizePickup,
        ActionPickup,
        PickupUpHook,
        RotateLeft,
        DownHookDropoff,
        SeizeDropoff,
        ActionDropoff,
        DropoffUpHook,
        Waiting, // waiting for next action
        RotateRight,
        LoopDone,
    }

    public class BaselineS1 : Scenario
    {
        private Transform _pipeParent;

        private Vector3 _pipeLocalPos;
        private Quaternion _pipeLocalRot;

        public CraneInputRecording recording;

        private GameObject crane;

        private GameObject player; // local player

        [Header("GameObjects")] public GameObject playerIndicator;
        public GameObject pipeStack;

        public GameObject unpackedPipe;


        private Vector3 pipeStackPos;
        private Quaternion pipeStackRot;

        private CraneStatus craneStatus;

        // threshold to decide whether reaching the destination
        private float rotationThreshold = 2f;

        private bool normalCondition = false;

        #region Crane action control

        // private string initDistanceCart;
        private string distanceHook = "5.4";
        private int initRotationCrane = 176;


        [Space(30)] [Header("Markers")] public float pickupDownHook = 20.5f; // pick up at this distance
        public float dropoffDownHook = 6.9f; // drop off at this distance
        public float dropoffRotation = 100f; // drop off at this rotation

        string Distance2String(float d)
        {
            return d.ToString("0.0");
        }

        string Rotation2String(float r)
        {
            return Mathf.RoundToInt(r).ToString();
        }

        #endregion

        #region Timer to make a pause for the animation

        [Space(30)] [Header("Pauses in second")]
        public int beforePickup = 3;

        public int afterPickup = 3;
        public int beforeDropOff = 3;
        public int afterDropoff = 3;

        private Timer _timer;

        private bool _timerRunning = false;

        void StartTimer(int second, Action oncomplete)
        {
            if (_timerRunning) return;
            // if (_timer != null)
            // {
            //     Timer.Cancel(_timer);
            // }
            _timer = Timer.Register(second, oncomplete, isLooped: false, useRealTime: true);

            _timerRunning = true;
        }

        #endregion

        #region Debug control

        private bool debugPrint = false;

        void DebugPrint(string msg)
        {
            if (debugPrint)
            {
                print(msg);
            }
        }

        #endregion


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

            craneStatus = CraneStatus.Waiting;

            BackupTransforms();

            SetActiveness(true, false);

            // override cargo to avoid error
            recording.backupCargo = pipeStack;

            normalCondition = false;
        }

        private void Update()
        {
            switch (craneStatus)
            {
                case CraneStatus.Init:
                    // if (WaitUntilHookSteady())
                    // {
                    //     craneStatus = CraneStatus.DownHookPickup;
                    // }
                    craneStatus = CraneStatus.DownHookPickup;

                    break;
                case CraneStatus.DownHookPickup:
                    if (DownHookUntil(pickupDownHook))
                    {
                        // add a timer to make it wait for a while
                        StartTimer(beforePickup, () =>
                        {
                            _timerRunning = false;
                            craneStatus = CraneStatus.ActionPickup;
                        });
                    }

                    break;

                case CraneStatus.ActionPickup:
                    // connect cargo
                    recording.SeizeCargo();
                    craneStatus = CraneStatus.SeizePickup;

                    break;

                case CraneStatus.SeizePickup:
                    StartTimer(afterPickup, () =>
                    {
                        _timerRunning = false;
                        craneStatus = CraneStatus.PickupUpHook;
                    });

                    break;
                case CraneStatus.PickupUpHook:
                    if (UpHookUntilInit())
                    {
                        craneStatus = CraneStatus.RotateLeft;
                    }

                    break;
                case CraneStatus.RotateLeft:
                    if (RotateLeftUntilDropoff())
                    {
                        craneStatus = CraneStatus.DownHookDropoff;
                    }

                    break;

                case CraneStatus.DownHookDropoff:
                    if (DownHookUntil(dropoffDownHook))
                    {
                        StartTimer(beforeDropOff, () =>
                        {
                            _timerRunning = false;
                            craneStatus = CraneStatus.ActionDropoff;
                        });
                    }

                    break;

                case CraneStatus.ActionDropoff:
                    // release cargo
                    recording.SeizeCargo();
                    craneStatus = CraneStatus.SeizeDropoff;

                    break;

                case CraneStatus.SeizeDropoff:
                    StartTimer(afterDropoff, () =>
                    {
                        _timerRunning = false;
                        craneStatus = CraneStatus.DropoffUpHook;
                    });

                    break;
                case CraneStatus.DropoffUpHook:
                    if (UpHookUntilInit())
                    {
                        //update distance hook
                        distanceHook = Distance2String(recording.DistanceHook);

                        if (normalCondition)
                        {
                            // directly into the next state
                            craneStatus = CraneStatus.RotateRight;
                        }
                        else
                        {
                            // waiting for next event
                            craneStatus = CraneStatus.Waiting;
                        }
                    }

                    break;
                case CraneStatus.RotateRight:
                    if (RotateRightUntilInit())
                    {
                        if (normalCondition)
                        {
                            // auto loop
                            SetActiveness(true, false);

                            // start over
                            craneStatus = CraneStatus.Init;
                        }
                        else
                        {
                            craneStatus = CraneStatus.LoopDone;
                        }
                    }

                    break;
                case CraneStatus.LoopDone:
                    break;

                case CraneStatus.Waiting:
                    break;

                default:
                    break;
            }
        }

        bool DownHookUntil(float target)
        {
            var hook = recording.DistanceHook;
            DebugPrint($"DownHookUntil: {hook} {target}");
            if (hook < target)
            {
                recording.DownHook();
                return false;
            }

            return true;
        }

        bool WaitUntilHookSteady()
        {
            var s1 = Distance2String(recording.DistanceHook);
            return s1 == distanceHook;
        }

        bool UpHookUntilInit()
        {
            var hook = recording.DistanceHook;
            DebugPrint($"UpHookUntilInit: {recording.DistanceHook} {distanceHook}");

            if (Distance2String(hook) == distanceHook)
            {
                return true;
            }
            else
            {
                if (hook > float.Parse(distanceHook))
                {
                    recording.UpHook();
                }
                else
                {
                    recording.DownHook();
                }

                return false;
            }
        }

        bool RotateLeftUntilDropoff()
        {
            var rotation = recording.RotationCrane;
            DebugPrint($"RotateLeftUntilDropoff: {rotation} {Rotation2String(dropoffRotation)}");
            if (rotation > dropoffRotation)
            {
                recording.TurnLeft();
                return false;
            }

            return WaitUntilHookSteady();
        }

        bool RotateRightUntilInit()
        {
            var rotation = recording.RotationCrane;
            DebugPrint($"RotateRightUntilInit: {rotation} {initRotationCrane}");
            if (rotation < initRotationCrane)
            {
                recording.TurnRight();
                return false;
            }

            return WaitUntilHookSteady();
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

        void SetActiveness(bool reset = false, bool unpackedpipe = false)
        {
            if (reset)
            {
                Reset();
            }

            // pipeStack.GetComponent<MeshRenderer>().enabled = pipestack;
            unpackedPipe.SetActive(unpackedpipe);
        }

        #region Accident Events Callbacks

        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S1");
            // repeat #2 and #3
            normalCondition = true;
            craneStatus = CraneStatus.Init;
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

            normalCondition = false;

            craneStatus = CraneStatus.Init;
            SetActiveness(true, false);
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

            normalCondition = false;
            craneStatus = CraneStatus.RotateRight;
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

            normalCondition = false;

            craneStatus = CraneStatus.Init;

            SetActiveness(true, true);
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

            normalCondition = false;

            craneStatus = CraneStatus.RotateRight;
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

            normalCondition = false;
            craneStatus = CraneStatus.Init;

            SetActiveness(true, true);
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

            normalCondition = false;
            craneStatus = CraneStatus.RotateRight;
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

            // make it auto return back
            normalCondition = true;
            craneStatus = CraneStatus.Init;
            SetActiveness(true, true);
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