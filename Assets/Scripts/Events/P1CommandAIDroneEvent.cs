using Fusion;
using UnityEngine;
using VRC2.Animations;
using VRC2.Pipe;

namespace VRC2.Events
{
    public class P1CommandAIDroneEvent : BaseEvent
    {
        public AIDroneMenuController _controller;
        private PipeConstants.PipeParameters _parameters;

        // Start is called before the first frame update
        void Start()
        {
            _controller.OnConfirmed += OnConfirmed;
        }

        private void OnConfirmed()
        {
            var parameter = _controller.result;
            Debug.Log("AI drone is going to deliver.");
            Debug.Log(parameter.ToString());

            if (!GlobalConstants.IsNetworkReady())
            {
                Debug.LogError("Runner or localPlayer is none");
                return;
            }

            if (Runner != null && Runner.isActiveAndEnabled && Runner.IsClient)
            {
                // change to P2 command AI drone
                RPC_SendMessage(parameter);
            }
            else
            {
                var go = GameObject.Find(GlobalConstants.AIDroneDeliver);
                var add = go.GetComponent<AIDroneDeliver>();
                add.InitParameters(parameter);
                add.Execute();
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(PipeConstants.PipeParameters parameters, RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
            {

            }
            else
            {
                var go = GameObject.Find(GlobalConstants.AIDroneDeliver);
                var add = go.GetComponent<AIDroneDeliver>();
                add.InitParameters(parameters);
                add.Execute();
            }

            Debug.LogWarning(message);
        }

        // Update is called once per frame
        void Update()
        {
        }

        //TODO: devise AI Drone logic
        public override void Execute()
        {
            Debug.Log("AI drone is going to deliver pipes");

            if (_controller.showing)
            {
                _controller.Hide();
            }
            else
            {
                _controller.Show();
            }
        }
    }
}