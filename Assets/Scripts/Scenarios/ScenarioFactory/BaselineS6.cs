using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

using UnityTimer;
using Timer = UnityTimer.Timer;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS6 : Scenario
    {
        private Transform _pipeParent;

        private Vector3 _pipeLocalPos;
        private Quaternion _pipeLocalRot;

        [Header("Animator")] public Animator animator;
        public float yaw;
        public float dolly;
        public float hook;

        [Header("GameObjects")] public GameObject pipeStack;

        public GameObject unpackedPipe;


        private float originalDolly;
        private float originalHook;

        private float randomYawIncrease;

        private GameObject crane;
        private GameObject pipeDolly;

        private GameObject player;


        private bool triggered = false;
        private bool backward = false;

        private float wind1 = 1.0f;
        private float wind2 = 5.0f;
        private float wind3 = 10.0f;

        private float yawOffset = 20;

        private float currentWind = 0;
        private Timer _timer;
        private float swingFreq = 0.3f; // every 0.1 second

        private float initYaw = 0f; // yaw of incident start
        private float initDolly = 0f;

        private void Start()
        {

            base.Start();

            player = localPlayer;

            crane = animator.gameObject;
            randomYawIncrease = 5; // Random.Range(1, 10);
            // make it rotate at the start
            triggered = true;
            backward = false;

            initYaw = CalculateRawBetweenCranePlayer(crane, player);
            initDolly = dolly;


            unpackedPipe.SetActive(false);
        }

        void StartTimer()
        {
            CancelTimer();

            _timer = Timer.Register(swingFreq, () => { SwingDolly(currentWind); }, isLooped: true, useRealTime: true);
        }

        void CancelTimer()
        {
            if (_timer != null)
            {
                Timer.Cancel(_timer);
            }
        }

        private void Update()
        {
            if (triggered)
            {
                var v = randomYawIncrease;
                if (backward)
                {
                    v *= -1;
                }

                yaw += Time.deltaTime * v;
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

            if (backward)
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


        void SwingDolly(float value)
        {
            var r = Random.Range(0, value);

            dolly = initDolly + r;
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

        #region Accident Events Callbacks

        void TriggerEvent(float wind, bool hasload, bool back)
        {
            yaw = initYaw;

            currentWind = wind;

            pipeStack.SetActive(hasload);

            triggered = true;
            backward = back;

            StartTimer();
        }

        public void On_BaselineS6_1_Start()
        {

        }

        public void On_BaselineS6_1_Finish()
        {
        }

        public void On_BaselineS6_2_Start()
        {
            print("On_BaselineS6_2_Start");
            // A load is passing overhead, and it swings a little bit due to the wind.
            // get incident
            var incident = GetIncident(2);
            var warning = incident.Warning;
            print(warning);

            TriggerEvent(wind1, true, false);
        }

        public void On_BaselineS6_2_Finish()
        {
            // A load is passing overhead, and it swings a little bit due to the wind.
        }

        public void On_BaselineS6_3_Start()
        {
            print("On_BaselineS6_3_Start");
            // The hook without a load is passing in the opposite direction.
            // get incident
            var incident = GetIncident(3);

            TriggerEvent(wind1, false, true);
        }

        public void On_BaselineS6_3_Finish()
        {
            // The hook without a load is passing in the opposite direction.
        }

        public void On_BaselineS6_4_Start()
        {
            print("On_BaselineS6_4_Start");
            // Another load is passing overhead, and it swings bigger due to the sudden wind. The load stops until it is static.
            // get incident
            var incident = GetIncident(4);
            var warning = incident.Warning;
            print(warning);

            TriggerEvent(wind2, true, false);
        }

        public void On_BaselineS6_4_Finish()
        {
            // Another load is passing overhead, and it swings bigger due to the sudden wind. The load stops until it is static.
        }

        public void On_BaselineS6_5_Start()
        {
            print("On_BaselineS6_5_Start");
            // The hook without a load is passing in the opposite direction..
            // get incident
            var incident = GetIncident(5);

            TriggerEvent(wind2, false, true);
        }

        public void On_BaselineS6_5_Finish()
        {
            // The hook without a load is passing in the opposite direction..
        }

        public void On_BaselineS6_6_Start()
        {
            print("On_BaselineS6_6_Start");
            // Another load is passing overhead, it swings even stronger due to the sudden wind, and is about to hit the power line.
            // get incident
            var incident = GetIncident(6);
            var warning = incident.Warning;

            TriggerEvent(wind3, true, false);
        }

        public void On_BaselineS6_6_Finish()
        {
            // Another load is passing overhead, it swings even stronger due to the sudden wind, and is about to hit the power line.
        }

        public void On_BaselineS6_7_Start()
        {
            print("On_BaselineS6_7_Start");

            CancelTimer();
            // stop
            triggered = false;

            //SAGAT query
            ShowSAGAT();
        }

        public void On_BaselineS6_7_Finish()
        {
            HideSAGAT();
        }

        #endregion

    }
}