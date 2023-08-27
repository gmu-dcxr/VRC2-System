using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using VRC2.Animations;
using VRC2.Pipe;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS1 : Scenario
    {
        private Transform _pipeParent;

        private Vector3 _pipeLocalPos;
        private Quaternion _pipeLocalRot;

        public CraneInputRecording recording;
        public CraneInputReplay replay;

        public float startAngle = 180f;
        public float endAngle = 120f;

        public float startBoomcart = -22.40558f; //x
        public float endBoomcart = -10;

        // [Header("Animator")] public Animator animator;
        // public float yaw;
        // public float dolly;
        // public float hook;

        private float yawOffset = 20;

        private float randomYawIncrease;

        private GameObject crane;

        private GameObject player; // local player

        [Header("GameObjects")] public GameObject playerIndicator;
        public GameObject pipeStack;

        public GameObject unpackedPipe;


        private bool triggered = false;
        private bool clockWise = false;

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

            BackupPipeLocalTransform();

            triggered = false;
            clockWise = false;

            SetActiveness(true, false);
        }

        private void Update()
        {
            if (triggered)
            {
                RotateCrane(clockWise);
            }
        }

        void ResetCraneRotation(float angle)
        {
            recording.ForceUpdateRotation(angle);
        }

        void ResetBoomCart(float x)
        {
            recording.ForceUpdateBoomCart(x);
        }

        float CalculateRawBetweenCranePlayer(GameObject crane, GameObject player)
        {
            var cranepos = crane.transform.position;
            cranepos.y = 0;

            var playerpos = player.transform.position;
            playerpos.y = 0;

            Vector3 dir = (playerpos - cranepos).normalized;
            var forward = this.crane.transform.forward;

            var angle = Vector3.SignedAngle(dir, forward, Vector3.up);

            if (clockWise)
            {
                angle += yawOffset;
            }
            else
            {
                angle += -yawOffset;
            }

            if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }

        void BackupPipeLocalTransform()
        {
            var t = unpackedPipe.transform;
            _pipeLocalPos = t.localPosition;
            _pipeLocalRot = t.localRotation;
            _pipeParent = t.parent;
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
        }

        void SetActiveness(bool pipestack, bool unpackedpipe)
        {
            pipeStack.SetActive(pipestack);
            unpackedPipe.SetActive(unpackedpipe);
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
        }

        #region Accident Events Callbacks

        // TODO: When scenario ends, start the normal event

        // normal event
        public override void StartNormalIncident()
        {
            print("Start Normal Incident Baseline S1");
            triggered = true;
        }

        public void On_BaselineS1_1_Start()
        {
            ResetCraneRotation(startAngle);
            ResetBoomCart(startBoomcart);

            replay.Pickup();
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

            ResetCraneRotation(startAngle);
            SetActiveness(true, false);

            triggered = true;
            clockWise = false;
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

            ResetCraneRotation(endAngle);
            SetActiveness(false, false);

            triggered = true;
            clockWise = true;
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

            ResetCraneRotation(startAngle);
            SetActiveness(true, true);

            triggered = true;
            clockWise = false;
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

            ResetCraneRotation(endAngle);
            SetActiveness(false, false);

            triggered = true;
            clockWise = true;
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

            Reset();
            ResetCraneRotation(startAngle);
            SetActiveness(true, true);

            triggered = true;
            clockWise = false;
        }

        public void On_BaselineS1_6_Finish()
        {
            // A load with an unpacked pipe is passing overhead.
        }

        public void On_BaselineS1_7_Start()
        {
            print("On_BaselineS1_7_Start");
            // The unpacked pipe drops next to the participants. And the load is still passing overhead.
            // get incident
            var incident = GetIncident(7);

            ResetCraneRotation(startAngle);
            SetActiveness(true, true);

            // update pipe position so it'll drop from the player's head
            var pos = player.transform.position;
            pos.y = unpackedPipe.transform.position.y;

            unpackedPipe.transform.position = pos;

            // release it
            unpackedPipe.transform.parent = null;
            // add rigid body
            PipeHelper.EnsureRigidBody(ref unpackedPipe);
            // it will automatically fall
        }

        public void On_BaselineS1_7_Finish()
        {
            // The unpacked pipe drops next to the participants. And the load is still passing overhead.
        }

        public void On_BaselineS1_8_Start()
        {
            print("On_BaselineS1_8_Start");
            // A load with an unpacked pipe is passing overhead.
            // get incident
            var incident = GetIncident(8);

            Reset();
            ResetCraneRotation(endAngle);
            SetActiveness(true, true);

            var warning = incident.Warning;
            print(warning);
        }

        public void On_BaselineS1_8_Finish()
        {
            // A load with an unpacked pipe is passing overhead.
        }

        public void On_BaselineS1_9_Start()
        {
            print("On_BaselineS1_9_Start");
            // SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS1_9_Finish()
        {
            // SAGAT query
            triggered = false;
            HideSAGAT();
        }

        #endregion

    }
}