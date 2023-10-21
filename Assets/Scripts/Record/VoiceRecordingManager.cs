using System;
using UnityEngine;

namespace VRC2.Record
{
    public class VoiceRecordingManager : MonoBehaviour
    {
        public VoiceClientMonitor monitor;


        private void Start()
        {
            monitor.OnVoiceLinkDetermined += OnVoiceLinkDetermined;
        }

        private void OnVoiceLinkDetermined()
        {
            print($"OnVoiceLinkDetermined: Supervisor {monitor.supervisorVoiceLink.PlayerId} {monitor.supervisorVoiceLink.VoiceId}");
        }
    }
}