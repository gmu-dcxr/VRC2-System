using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC2.Events;

namespace VRC2
{
    public class ModalDialog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;

        [SerializeField] private TextMeshProUGUI _content;

        [SerializeField] private TextMeshPro _OKButton;

        [SerializeField] private TextMeshPro _CancelButton;

        public PointableUnityEventWrapper button1Events;
        public PointableUnityEventWrapper button2Events;

        // store event
        [HideInInspector]public PipeInstallEvent currentEvent = PipeInstallEvent.EmptyEvent;

        // store check result
        [HideInInspector]public bool checkResult = false;

        public string title
        {
            get { return _title.text; }
            set { _title.text = value; }
        }

        public string content
        {
            get { return _content.text; }
            set { _content.text = value; }
        }

        public string button1
        {
            get { return _OKButton.text; }
            set { _OKButton.text = value; }
        }

        public string button2
        {
            get { return _CancelButton.text; }
            set { _CancelButton.text = value; }
        }

        public void UpdateDialog(string title, string content, string btn1, string btn2,
            PipeInstallEvent ev = PipeInstallEvent.EmptyEvent)
        {
            this.title = title;
            this.content = content;
            this.button1 = btn1;
            this.button2 = btn2;

            // hide button if text is null
            if (btn1 == null)
            {
                this._OKButton.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                this._OKButton.gameObject.transform.parent.gameObject.SetActive(true);
            }

            if (btn2 == null)
            {
                this._CancelButton.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                this._CancelButton.gameObject.transform.parent.gameObject.SetActive(true);
            }

            // update event
            if (ev != PipeInstallEvent.EmptyEvent)
            {
                this.currentEvent = ev;
            }
        }
    }
}