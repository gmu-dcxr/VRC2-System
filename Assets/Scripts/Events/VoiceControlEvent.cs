using UnityEngine;
using Fusion;
using Photon.Voice.Unity;

namespace VRC2.Events
{
    public class VoiceControlEvent : BaseEvent
    {

        private Recorder _recorder;

        public bool enableVoice { get; set; }

        void Start()
        {
            base.Start();
            // find voice recorder
            var go = GameObject.FindWithTag(GlobalConstants.voiceRecorderTag);
            _recorder = go.GetComponent<Recorder>();
        }

        public override void Execute()
        {
            _recorder.TransmitEnabled = enableVoice;
        }
    }
}