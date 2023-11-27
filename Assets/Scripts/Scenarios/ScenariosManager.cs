using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using VRC2.Conditions;
using ScenarioBase = VRC2.ScenariosV2.Scenario.Base;

namespace VRC2.Scenarios
{
    public class ScenariosManager : NetworkBehaviour
    {
        private int startTimestamp = -1;

        public bool isTraining = true;

        public ConditionNumber conditionNumber;

        public List<ScenarioBase> scenarios = null;

        public System.Action OnStart;
        public System.Action OnFinish;

        private Condition _condition;

        public Condition condition
        {
            get
            {
                if (_condition == null)
                {
                    _condition = Condition.GetCondition(conditionNumber);
                }

                return _condition;
            }
        }

        private void Start()
        {
            CheckScenariosCallbacks();

            Debug.LogWarning($"Use Condition: {condition.ToString()}");
        }

        public void Execute()
        {
        }

        private void OnGUI()
        {
            var runner = GameObject.FindObjectOfType<NetworkRunner>();
            if (runner != null && runner.IsRunning)
            {
                var no = gameObject.GetComponent<NetworkObject>();
                if (no.HasStateAuthority)
                {
                    // host, show GUI
                    if (GUI.Button(new Rect(500, 10, 150, 50), "Start"))
                    {
                        RPC_SendMessage();
                    }
                }
            }
            else // local mode
            {
                if (GUI.Button(new Rect(500, 10, 150, 50), "Start"))
                {
                    StartScenarios();
                }
            }
        }

        #region Start at the same time

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendMessage(RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {
                StartScenarios();
            }
            else
            {
                StartScenarios();
            }

            Debug.LogWarning(message);
        }



        #endregion


        void StartScenarios()
        {
            var count = scenarios.Count;

            for (int i = 1; i < count; i++)
            {
                var s = scenarios[i];

                var s0 = scenarios[i - 1];
                s.OverrideStartEnd(s.startInSec + s0.endInSec, s.endInSec + s0.endInSec);
            }

            startTimestamp = Helper.SecondNow();
            // start all event
            foreach (var scenario in scenarios)
            {
                scenario.Execute(startTimestamp);
            }
        }

        public void CheckScenariosCallbacks()
        {
            bool pass = true;

            var ClsName = GetType().Name;

            var @namespace = "VRC2.Scenarios";
            var myClassType = Type.GetType($"{@namespace}.{ClsName}");

            foreach (var scenario in scenarios)
            {
                var _id = scenario.ID;
                var name1 = Helper.GetScenarioCallbackName(_id, ScenarioCallback.Start);
                var name2 = Helper.GetScenarioCallbackName(_id, ScenarioCallback.Finish);

                if (myClassType.GetMethod(name1) == null)
                {
                    pass = false;
                    Debug.LogWarning($"[{ClsName}] missing method: {name1}");
                }

                if (myClassType.GetMethod(name2) == null)
                {
                    pass = false;
                    Debug.LogWarning($"[{ClsName}] missing method: {name2}");
                }
            }

            Debug.LogWarning($"{ClsName} Check Scenarios Callbacks Result: {pass}");
        }

        #region Debug

        void TestScenarioManager()
        {
            scenarios = new List<ScenarioBase>();

            var s1 = gameObject.AddComponent<ScenarioBase>();
            s1.startInSec = 0;
            s1.endInSec = 10;
            s1.name = "S1";

            var s2 = gameObject.AddComponent<ScenarioBase>();
            s2.startInSec = 0;
            s2.endInSec = 5;
            s2.name = "S2";

            var s3 = gameObject.AddComponent<ScenarioBase>();
            s3.startInSec = 0;
            s3.endInSec = 15;
            s3.name = "S3";

            scenarios.Add(s1);
            scenarios.Add(s2);
            scenarios.Add(s3);

            StartScenarios();
        }

        #endregion

        #region Update Instruction

        public void UpdateInstruction(int start, int end)
        {
            print("UpdateInstruction");
            var s = start;
            var e = end;

            // use mapping
            s = Math.Min(s, e);

            // 1-5, 2-6, 3-7, 4-8
            e = s + 4;

            UpdateWallBoxes(s, e);

            Texture2D texture = null;
            string folder = "";
            string filename = "";


            if (isTraining)
            {
                (texture, folder, filename) = GlobalConstants.loadTrainingInstruction();
            }
            else
            {
                (texture, folder, filename) = GlobalConstants.loadTaskInstruction(s, e);
            }

            UpdateInstructionPanel(texture);
            UpdateWallBackground(texture, folder, filename);
        }

        void UpdateInstructionPanel(Texture2D texture)
        {
            var qim = GameObject.FindWithTag(GlobalConstants.instructionTag).GetComponent<QuadImageManager>();
            qim.SetTexture(texture);
        }

        void UpdateWallBackground(Texture2D texture, string folder, string filename)
        {
            var qim = GameObject.FindWithTag(GlobalConstants.wallTag).GetComponent<QuadImageManager>();
            qim.SetTexture(texture);
            qim.UpdateFolderFilename(folder, filename);
        }

        void UpdateWallBoxes(int s, int e)
        {
            var boxes = GameObject.FindWithTag(GlobalConstants.wallBoxesTag);

            var count = boxes.transform.childCount;

            if (isTraining)
            {
                // load E0 only
                s = 0;
                e = 0;
            }

            for (var i = 0; i < count; i++)
            {
                if (i == s || i == e)
                {
                    // enable it
                    boxes.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    boxes.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        #endregion
    }
}