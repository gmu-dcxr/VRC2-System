using UnityEngine;
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

            var go = GameObject.Find(GlobalConstants.AIDroneDeliver);
            var add = go.GetComponent<AIDroneDeliver>();
            add.InitParameters(parameter);
            add.Execute();
        }

        // Update is called once per frame
        void Update()
        {
        }

        //TODO: devise AI Drone logic
        public override void Execute()
        {
            Debug.Log("AI drone is going to deliver pipes");
            _controller.Show();
        }
    }
}