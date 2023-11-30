using System;
using UnityEngine;

namespace VRC2.Record
{
    [RequireComponent(typeof(AudioSource))]
    public class MicrophoneRecorder : MonoBehaviour
    {
        private bool connected = false;

        private int minFrequency;
        private int maxFrequency;

        private AudioSource audioSource;

        private void Start()
        {
            //Check if there is at least one microphone connected    
            if (Microphone.devices.Length <= 0)
            {
                //Throw a warning message at the console if there isn't    
                Debug.LogWarning("Microphone not connected!");
            }
            else //At least one microphone is present    
            {

                var count = Microphone.devices.Length;
                for (var i = 0; i < count; i++)
                {
                    print($"microphone device: {i} - {Microphone.devices[i]}");
                }
                
                //Set our flag 'micConnected' to true    
                connected = true;

                //Get the default microphone recording capabilities    
                Microphone.GetDeviceCaps(null, out minFrequency, out maxFrequency);

                //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...    
                if (minFrequency == 0 && maxFrequency == 0)
                {
                    //...meaning 44100 Hz can be used as the recording sampling rate    
                    maxFrequency = 44100;
                }

                //Get the attached AudioSource component    
                audioSource = this.GetComponent<AudioSource>();
            }
        }

        void OnGUI()
        {
            //If there is a microphone    
            if (connected)
            {
                //If the audio from any microphone isn't being captured    
                if (!Microphone.IsRecording(null))
                {
                    //Case the 'Record' button gets pressed    
                    if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Record"))
                    {
                        //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource    
                        audioSource.clip = Microphone.Start(null, true, 20, maxFrequency);
                    }
                }
                else //Recording is in progress    
                {
                    //Case the 'Stop and Play' button gets pressed    
                    if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Stop and Play!"))
                    {
                        Microphone.End(null); //Stop the audio recording    
                        audioSource.Play(); //Playback the recorded audio    
                    }

                    GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 25, 200, 50),
                        "Recording in progress...");
                }
            }
            else // No microphone    
            {
                //Print a red "Microphone not connected!" message at the center of the screen    
                GUI.contentColor = Color.red;
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50),
                    "Microphone not connected!");
            }

        }
    }
}