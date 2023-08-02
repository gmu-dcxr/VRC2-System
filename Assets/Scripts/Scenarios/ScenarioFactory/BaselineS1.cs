using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using VRC2.Pipe;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS1 : Scenario
    {
        private Transform _pipeParent;

        private Vector3 _pipeLocalPos;
        private Quaternion _pipeLocalRot;

        [Header("Animator")] public Animator animator;
        public float yaw;
        public float dolly;
        public float hook;

        private float randomYawIncrease;

        private GameObject crane;

        [Header("GameObjects")] public GameObject player;
        [FormerlySerializedAs("pipeDolly")] public GameObject pipeStack;
        public GameObject unpackedPipe;


        private bool triggered = false;
        private bool backwardsTriggered = false;

        private void Start()
        {
            base.Start();
            
            BackupPipeLocalTransform();

            crane = animator.gameObject;
            randomYawIncrease = Random.Range(1, 10);
            // make it rotate at the start
            triggered = true;

            SetActiveness(true, false);
        }

        private void Update()
        {
            if (triggered)
            {
                yaw += Time.deltaTime * randomYawIncrease;
                UpdateAnimator(yaw, dolly, hook);
            }

            if (backwardsTriggered)
            {
                yaw += Time.deltaTime * -randomYawIncrease;
                UpdateAnimator(yaw, dolly, hook);
            }
        }

        float CalculateRawBetweenCranePlayer(GameObject crane, GameObject player)
        {
            var cranepos = crane.transform.position;
            cranepos.y = 0;

            var playerpos = player.transform.position;
            playerpos.y = 0;

            Vector3 dir = (playerpos - cranepos).normalized;
            var forward = animator.gameObject.transform.forward;

            var angle = Vector3.SignedAngle(dir, forward, Vector3.up);

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
                // reset parent
                unpackedPipe.transform.parent = _pipeParent;
                // update local position and rotation
                unpackedPipe.transform.localPosition = _pipeLocalPos;
                unpackedPipe.transform.localRotation = _pipeLocalRot;
            }
        }

        void UpdateAnimator(float y, float d, float h)
        {
            if (y >= 0)
            {
                animator.SetFloat("Rotate_YAW", Mathf.Abs(y) % 360);
            }

            if (d >= 0)
            {
                animator.SetFloat("dolly", d);
            }

            if (h >= 0)
            {
                animator.SetFloat("hook", h);
            }
        }

        void SetActiveness(bool pipestack, bool unpackedpipe)
        {
            pipeStack.SetActive(pipestack);
            unpackedPipe.SetActive(unpackedpipe);
        }

        #region Accident Events Callbacks

        public void On_BaselineS1_1_Start()
        {
            triggered = true;
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

            SetActiveness(true, false);

            // get yaw
            yaw = CalculateRawBetweenCranePlayer(crane, player);
            triggered = true;
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

            SetActiveness(false, false);

            yaw = CalculateRawBetweenCranePlayer(crane, player);
            triggered = false;
            backwardsTriggered = true;
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

            yaw = CalculateRawBetweenCranePlayer(crane, player);

            backwardsTriggered = false;
            triggered = true;
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
            SetActiveness(false, false);

            yaw = CalculateRawBetweenCranePlayer(crane, player);
            backwardsTriggered = true;
            triggered = false;
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

            yaw = CalculateRawBetweenCranePlayer(crane, player);
            backwardsTriggered = false;
            triggered = true;
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

            SetActiveness(true, true);

            yaw = CalculateRawBetweenCranePlayer(crane, player);
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

            SetActiveness(true, true);

            yaw = CalculateRawBetweenCranePlayer(crane, player);
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