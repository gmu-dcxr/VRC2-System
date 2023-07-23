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

        // Start is called before the first frame update
        void Start()
        {
            confirmButton.onClick.AddListener(OnConfirmed);
            clearButton.onClick.AddListener(OnCleared);
        }

        private void Browser2DOnOnJSQuery(string query)
        {
            Debug.Log("Javascript query:" + query);
            browser2D.RespondToJSQuery("My response: OK");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show()
        {
            rootCanvas.SetActive(true);
            UIHelper.SetActive(true);
        }

        public void Hide()
        {
            rootCanvas.SetActive(false);
            UIHelper.SetActive(false);
        }

        void OnConfirmed()
        {
            var text = inputField.text;
            print(text);
            browser2D.InputText(text);
        }

        void OnCleared()
        {
            browser2D.ClearSelection();
        }
    }
}