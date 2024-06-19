using System.Collections;
using System.Collections.Generic;
using SimpleWebBrowser;
using UnityEngine;
using UnityEngine.UI;
using VRC2.WebView;

namespace VRC2
{
    public class SurveyController : MonoBehaviour
    {
        // [Header("Root canvas")] public GameObject rootCanvas;
        //
        // // [Header("UIHelper")] public GameObject UIHelper;
        //
        //
        // [Header("Confirm/Clear")] public Button confirmButton;
        //
        // public Button clearButton;
        //
        // [Header("Text")] public InputField inputField;
        //
        // [Header("Browser")] public WebBrowser2D browser2D;
        //
        // private Canvas _canvas
        // {
        //     get => rootCanvas.GetComponent<Canvas>();
        // }

        [Header("SurveyView")] public SurveyWebView webView;

        [Header("Event System")] public GameObject currentES;
        public GameObject webviewES;


        [HideInInspector]
        public bool showing
        {
            get => webView.enabled;
        }

        // Start is called before the first frame update
        void Start()
        {
            // confirmButton.onClick.AddListener(OnConfirmed);
            // clearButton.onClick.AddListener(OnCleared);

            webView.OnInitialized += OnInitialized;
        }

        private void OnInitialized()
        {
            Hide();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Show()
        {
            SetEventSystem(false);
            webView.SetVisibility(true);
            GlobalConstants.SetLaserPointer(true);
        }

        public void Hide()
        {
            SetEventSystem(true);
            webView.SetVisibility(false);
            GlobalConstants.SetLaserPointer(false);
        }

        public void Show(string url)
        {
            webView.LoadUrl(url);
            Show();
        }

        void SetEventSystem(bool current)
        {
            currentES.SetActive(current);
            webviewES.SetActive(!current);
        }

        // void OnConfirmed()
        // {
        //     var text = inputField.text;
        //     browser2D.InputText(text);
        // }
        //
        // void OnCleared()
        // {
        //     browser2D.ClearSelection();
        // }
    }
}