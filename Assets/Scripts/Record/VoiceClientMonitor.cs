using System;
using System.Collections.Generic;
using Fusion;
using Photon.Voice.Unity;
using UnityEngine;

namespace VRC2.Record
{
    [RequireComponent(typeof(UnityVoiceClient))]
    public class VoiceClientMonitor : MonoBehaviour
    {
        [Header("Network")] public NetworkRunner runner;

        private UnityVoiceClient _unityVoiceClient;

        // private VoiceConnection;
        private List<RemoteVoiceLink> voiceLinks;
        private bool voiceLinkDetermined = false;
        public System.Action OnVoiceLinkDetermined;

        [HideInInspector] public RemoteVoiceLink supervisorVoiceLink;
        [HideInInspector] public RemoteVoiceLink safetyManagerVoiceLink;
        
        private void Start()
        {
            _unityVoiceClient = GetComponent<UnityVoiceClient>();

            voiceLinks = _unityVoiceClient.CachedRemoteVoices;
        }

        private void Update()
        {
            if (voiceLinkDetermined) return;

            if (runner != null && runner.isActiveAndEnabled)
            {
                var count = voiceLinks.Count;
                if (count < 3)
                {
                    Debug.LogError("Supervisor or SafetyManager not joined");
                    voiceLinkDetermined = true;
                }
                else
                {
                    if (count > 3)
                    {
                        supervisorVoiceLink = voiceLinks[2];
                        safetyManagerVoiceLink = voiceLinks[3];
                    }
                    else if (count == 3)
                    {
                        supervisorVoiceLink = voiceLinks[2];
                        safetyManagerVoiceLink = null;
                    }

                    voiceLinkDetermined = true;

                    if (OnVoiceLinkDetermined != null)
                    {
                        OnVoiceLinkDetermined();
                    }
                }
            }
        }
    }
}