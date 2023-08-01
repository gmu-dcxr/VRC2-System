using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS6 : Scenario
    {
        [Header("Config")]
        [Tooltip("Yml file name")]
        public string filename = "BaselineS6.yml";

        
        private Transform _pipeParent;

        private Vector3 _pipeLocalPos;
        private Quaternion _pipeLocalRot;

        [Header("Animator")] public Animator animator;
        public float yaw;
        public float dolly;
        public float hook;

        private float originalDolly;
        private float originalHook;

        private float randomYawIncrease;

        private GameObject crane;
        private GameObject pipeDolly;

        [Header("Player")] public GameObject player;


        private bool triggered = false;
        private bool backwardsTriggered = false;

        [SerializeField]
        private float wind1 = 0.1f;
        private float wind2 = 0.3f;
        private float wind3 = 0.5f;

        private void Start()
        {
            InitFromFile(filename);

            IncidentStart += OnIncidentStart;
            IncidentFinish += OnIncidentFinish;

            CheckIncidentsCallbacks();

            crane = animator.gameObject;
            randomYawIncrease = Random.Range(1, 10);
            // make it rotate at the start
            triggered = true;

            //Find pipeDolly Object
            pipeDolly = GameObject.Find("Pipes");

            //Wind calculations
            originalDolly = dolly;
            originalHook = hook;
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

        private void OnIncidentFinish(int obj)
        {
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Finish);

            print($"[{ClsName}] Callback: {name}");

            Invoke(name, 0);
        }

        private void OnIncidentStart(int obj)
        {
            var name = Helper.GetIncidentCallbackName(ClsName, obj, ScenarioCallback.Start);

            print($"[{ClsName}] Callback: {name}");
            Invoke(name, 0);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Start"))
            {
                var ts = Helper.SecondNow();
                Execute(ts);
            }
        }


        private void Reset()
        {
           
        }

        private void Wind(float range)
        {
            if (range != 0.0f)
            {
                float rand = Random.Range(range, -range);
                dolly = dolly * (1.0f + rand);
                hook = hook * (1.0f + rand);
            }
            else
            {
                dolly = originalDolly;
                hook = originalHook;
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

        #region Accident Events Callbacks

        public void On_BaselineS6_1_Start()
        {
            triggered = false;
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


            pipeDolly.SetActive(true);
            Wind(wind1);
            // get yaw
            yaw = CalculateRawBetweenCranePlayer(crane, player);
            triggered = true;
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


            pipeDolly.SetActive(false);
            Wind(0.0f);
            triggered = false;
            backwardsTriggered = true;
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

            pipeDolly.SetActive(true);
            Wind(wind2);
            backwardsTriggered = false;
            triggered = true;
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


            pipeDolly.SetActive(false);
            Wind(0.0f);

            backwardsTriggered = true;
            triggered = false;
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
            pipeDolly.SetActive(true);
            Wind(wind3);
            print(warning);
        }

        public void On_BaselineS6_6_Finish()
        {
            // Another load is passing overhead, it swings even stronger due to the sudden wind, and is about to hit the power line.
            Wind(0.0f);
        }

        public void On_BaselineS6_7_Start()
        {
            print("On_BaselineS6_7_Start");
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