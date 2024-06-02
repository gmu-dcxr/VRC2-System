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
            // this is deprecated
            return;
            
            if (voiceLinkDetermined) return;

            if (runner != null && runner.IsRunning)
            {
                var count = voiceLinks.Count;
                // here the count doesn't consider itself
                if (count < 2)
                {
                    Debug.LogWarning($"Supervisor or SafetyManager not joined [{count}]");
                }
                else
                {
                    if (count > 2)
                    {
                        supervisorVoiceLink = voiceLinks[1];
                        safetyManagerVoiceLink = voiceLinks[2];
                    }
                    else if (count == 2)
                    {
                        supervisorVoiceLink = voiceLinks[1];
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