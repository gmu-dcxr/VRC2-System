using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRC2
{

    public class ModalDialog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;

        [SerializeField] private TextMeshProUGUI _content;

        [SerializeField] private TextMeshPro _OKButton;

        [SerializeField] private TextMeshPro _CancelButton;
        
        public System.Action OnButton1Clicked;
        public System.Action OnButton2Clicked;

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

        public void Button1Clicked()
        {
            Debug.Log("Button 1 clicked");
            if (OnButton1Clicked != null)
            {
                OnButton1Clicked();
            }
        }

        public void Button2Clicked()
        {
            Debug.Log("Button 2 clicked");
            
            if (OnButton2Clicked != null)
            {
                OnButton2Clicked();
            }
        }
    }
}