using System.Collections;
using System.Collections.Generic;
using SimpleWebBrowser;
using UnityEngine;
using UnityEngine.UI;

namespace VRC2
{
    public class SurveyController : MonoBehaviour
    {
        [Header("Root canvas")] public GameObject rootCanvas;

        [Header("UIHelper")] public GameObject UIHelper;


        [Header("Confirm/Clear")] public Button confirmButton;

        public Button clearButton;

        [Header("Text")] public InputField inputField;

        [Header("Browser")] public WebBrowser2D browser2D;

        private Canvas _canvas
        {
            get => rootCanvas.GetComponent<Canvas>();
        }

        // Start is called before the first frame update
        void Start()
        {
            confirmButton.onClick.AddListener(OnConfirmed);
            clearButton.onClick.AddListener(OnCleared);
            Hide();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show()
        {
            _canvas.enabled = true;
            UIHelper.SetActive(true);
        }

        public void Hide()
        {
            _canvas.enabled = false;
            UIHelper.SetActive(false);
        }

        void OnConfirmed()
        {
            var text = inputField.text;
            browser2D.InputText(text);
        }

        void OnCleared()
        {
            browser2D.ClearSelection();
        }
    }
}