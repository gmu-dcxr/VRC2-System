﻿// refer: Photon.Voice.Unity.Demos.DemoVoiceUI.MicrophoneSelector

using System;
using System.Collections.Generic;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VRC2.SAGAT;
using UtilityScripts = Photon.Voice.Unity.UtilityScripts;

namespace VRC2.Record
{
    public struct MicRef
    {
        public readonly Recorder.MicType MicType;
        public readonly DeviceInfo Device;

        public MicRef(Recorder.MicType micType, DeviceInfo device)
        {
            this.MicType = micType;
            this.Device = device;
        }

        public override string ToString()
        {
            return string.Format("Mic reference: {0}", this.Device.Name);
        }
    }

    public class MicrophoneSelector : VoiceComponent
    {
        public class MicrophoneSelectorEvent : UnityEvent<Recorder.MicType, DeviceInfo>
        {
        }

        public MicrophoneSelectorEvent onValueChanged = new MicrophoneSelectorEvent();

        private List<MicRef> micOptions;

#pragma warning disable 649
        [SerializeField] private Dropdown micDropdown;
        [SerializeField] private Slider micLevelSlider;

        [SerializeField] private Recorder recorder;

        [SerializeField] [FormerlySerializedAs("RefreshButton")]
        private GameObject refreshButton;

        [Space(30)] [Header("Recording Control")] [SerializeField]
        private GameObject startRecordingButton;

        [SerializeField] private GameObject stopRecordingButton;
        [SerializeField] private SaveLocalVoiceStream saver;

        [Space(30)] [Header("Question Control")] [SerializeField]
        private SAGATSurvey survey;

        [SerializeField] private GameObject prevQuestionButton;

        [SerializeField] private GameObject nextQuestionButton;

        [Space(30)] [Header("VR Control")] [SerializeField]
        private GameObject resumeButton;

        [Space(30)] [Header("Question")] [SerializeField]
        Text scenarioTitle;

        [SerializeField] private Text title;

        [SerializeField] private Text question;


        private Image fillArea;
        private Color defaultFillColor = Color.white;
        private Color speakingFillColor = Color.green;

        public System.Action RequestReturnToVR;


#pragma warning restore 649

        private IDeviceEnumerator unityMicEnum;
        // private IDeviceEnumerator photonMicEnum;

        protected override void Awake()
        {
            base.Awake();
            unityMicEnum = new AudioInEnumerator(this.Logger);
            // photonMicEnum = Platform.CreateAudioInEnumerator(this.Logger);
            this.RefreshMicrophones();
            this.refreshButton.GetComponentInChildren<Button>().onClick.AddListener(RefreshMicrophones);
            this.startRecordingButton.GetComponentInChildren<Button>().onClick.AddListener(StartRecording);
            this.stopRecordingButton.GetComponentInChildren<Button>().onClick.AddListener(StopRecording);

            this.prevQuestionButton.GetComponentInChildren<Button>().onClick.AddListener(PrevQuestion);
            this.nextQuestionButton.GetComponentInChildren<Button>().onClick.AddListener(NextQuestion);

            this.resumeButton.GetComponentInChildren<Button>().onClick.AddListener(ResumeVR);

            this.fillArea = this.micLevelSlider.fillRect.GetComponent<Image>();

            this.defaultFillColor = this.fillArea.color;
        }

        private void Update()
        {
            if (this.recorder != null)
            {
                this.micLevelSlider.value = this.recorder.LevelMeter.CurrentPeakAmp;
                this.fillArea.color = this.speakingFillColor;
                // this.fillArea.color = this.recorder.IsCurrentlyTransmitting
                //     ? this.speakingFillColor
                //     : this.defaultFillColor;
            }
        }

        private void OnEnable()
        {
            UtilityScripts.MicrophonePermission.MicrophonePermissionCallback += this.OnMicrophonePermissionCallback;
        }

        private void OnMicrophonePermissionCallback(bool granted)
        {
            this.RefreshMicrophones();
        }

        private void OnDisable()
        {
            UtilityScripts.MicrophonePermission.MicrophonePermissionCallback -= this.OnMicrophonePermissionCallback;
        }

        private void SetupMicDropdown()
        {
            this.micDropdown.ClearOptions();

            this.micOptions = new List<MicRef>();
            List<string> micOptionsStrings = new List<string>();

            this.micOptions.Add(new MicRef(Recorder.MicType.Unity, DeviceInfo.Default));
            micOptionsStrings.Add(string.Format("[Default]"));

            foreach (var d in this.unityMicEnum)
            {
                this.micOptions.Add(new MicRef(Recorder.MicType.Unity, d));
                micOptionsStrings.Add(string.Format("{0}", d));
            }

            // this.micOptions.Add(new MicRef(Recorder.MicType.Photon, DeviceInfo.Default));
            // micOptionsStrings.Add(string.Format("[Photon] [Default]"));
            //
            // foreach (var d in this.photonMicEnum)
            // {
            //     this.micOptions.Add(new MicRef(Recorder.MicType.Photon, d));
            //     micOptionsStrings.Add(string.Format("[Photon] {0}", d));
            // }

            this.micDropdown.AddOptions(micOptionsStrings);
            this.micDropdown.onValueChanged.RemoveAllListeners();
            this.micDropdown.onValueChanged.AddListener(delegate
            {
                this.MicDropdownValueChanged(this.micOptions[this.micDropdown.value]);
            });
        }

        private void MicDropdownValueChanged(MicRef mic)
        {
            this.recorder.MicrophoneType = mic.MicType;
            this.recorder.MicrophoneDevice = mic.Device;

            onValueChanged?.Invoke(mic.MicType, mic.Device);
        }

        private void SetCurrentValue()
        {
            if (this.micOptions == null)
            {
                Debug.LogWarning("micOptions list is null");
                return;
            }

            this.micDropdown.gameObject.SetActive(true);
            this.refreshButton.SetActive(true);
            for (int valueIndex = 0; valueIndex < this.micOptions.Count; valueIndex++)
            {
                MicRef val = this.micOptions[valueIndex];
                if (this.recorder.MicrophoneType == val.MicType)
                {
                    if (this.recorder.MicrophoneType == val.MicType &&
                        this.recorder.MicrophoneDevice == val.Device)
                    {
                        this.micDropdown.value = valueIndex;

                        return;
                    }
                }
            }
        }

        /*public void PhotonMicToggled(bool on)
        {
            this.micDropdown.gameObject.SetActive(!on);
            this.refreshButton.SetActive(!on);
            if (on)
            {
                this.recorder.MicrophoneType = Recorder.MicType.Photon;
            }
            else
            {
                this.recorder.MicrophoneType = Recorder.MicType.Unity;
            }
            this.voiceConnection.Client.LocalPlayer.SetMic(this.recorder.MicrophoneType);
        }*/

        public void RefreshMicrophones()
        {
            this.unityMicEnum.Refresh();
            // this.photonMicEnum.Refresh();
            this.SetupMicDropdown();
            this.SetCurrentValue();
        }

        // sync. UI in case a change happens from the Unity Editor Inspector
        private void PhotonVoiceCreated()
        {
            this.RefreshMicrophones();
        }

        #region Recording control

        public void StartRecording()
        {
            saver.StartRecording("Question");
        }

        public void StopRecording()
        {
            saver.StopRecording();
        }

        public void SetScenarioText(string s)
        {
            scenarioTitle.text = s;
        }

        #endregion

        #region Question control

        private void Start()
        {
            // var filename = "Scenario1.yml";
            // survey.LoadFile(filename);
            // // fill the 1st question
            // InitQuestion();
        }

        public void LoadForClass(string name)
        {
            var filename = $"{name}.yml";
            survey.LoadFile(filename);
            InitQuestion();
        }

        public void InitQuestion()
        {
            var q = survey.First();
            if (q.IsNone())
            {
                UpdateQuestion("Label", "Question", null);
            }
            else
            {
                UpdateQuestion(q.label, q.question, q.options);
            }
        }

        public void PrevQuestion()
        {
            var q = survey.PrevQuestion();
            UpdateQuestion(q.label, q.question, q.options);
        }

        public void NextQuestion()
        {
            if (!survey.IsLast())
            {
                // insert separator
                saver.InsertSeparator();
            }

            var q = survey.NextQuestion();
            UpdateQuestion(q.label, q.question, q.options);


        }

        private List<string> GenerateList(List<string> options)
        {

            var length = options.Count;
            List<string> letters = new List<string>();

            for (int i = 0; i < length; i++)
            {
                char letter = (char)('a' + i);
                letters.Add($"<color=blue>{letter}.</color>\t{options[i]}");
            }

            return letters;
        }

        private void UpdateQuestion(string t, string q, List<string> options)
        {
            title.text = t;

            // concatenate q and options
            var s = q;
            if (options != null)
            {
                var list = GenerateList(options);
                s += "\n\n" + String.Join("\n", list);
            }

            question.text = s;
        }

        public void ResumeVR()
        {
            // return to vr, basically start the next scenario
            if (RequestReturnToVR != null)
            {
                RequestReturnToVR();
            }
        }

        #endregion
    }
}