using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRC2.Scenarios.ScenarioFactory
{
    public class BaselineS1 : Scenario
    {
        [Header("Config")] [Tooltip("Yml file name")]
        public string filename = "BaselineS1.yml";

        [Header("Accident Configure")] public GameObject pipe;

        private Transform _pipeParent;

        private Vector3 _pipeLocalPos;
        private Quaternion _pipeLocalRot;

        [Header("Animator")] public Animator animator;
        public float yaw;
        public float dolly;
        public float hook;

        private float randomYawIncrease;

        private GameObject crane;

        [Header("Player")] public GameObject player;


        private bool triggered = false;

        private void Start()
        {
            base.Start();

            InitFromFile(filename);

            IncidentStart += OnIncidentStart;
            IncidentFinish += OnIncidentFinish;

            CheckIncidentsCallbacks();

            BackupPipeLocalTransform();

            crane = animator.gameObject;
            randomYawIncrease = Random.Range(1, 10);
            // make it rotate at the start
            triggered = true;
        }
        
        private void Update()
        {
            if (triggered)
            {
                yaw += Time.deltaTime * randomYawIncrease;
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

        void BackupPipeLocalTransform()
        {
            var t = pipe.transform;
            _pipeLocalPos = t.localPosition;
            _pipeLocalRot = t.localRotation;
            _pipeParent = t.parent;
        }

        private void Reset()
        {
            if (pipe.transform.parent == null)
            {
                // reset parent
                pipe.transform.parent = _pipeParent;
                // update local position and rotation
                pipe.transform.localPosition = _pipeLocalPos;
                pipe.transform.localRotation = _pipeLocalRot;
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
            print(warning);

            // get yaw
            yaw = CalculateRawBetweenCranePlayer(crane, player);
            triggered = true;
        }

        public void On_BaselineS1_2_Finish()
        {
            // A load is passing overhead.
        }

        #endregion

    }
}