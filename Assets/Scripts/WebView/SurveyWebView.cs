using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Vuplex.WebView;

namespace VRC2.WebView
{

    public class SurveyWebView : MonoBehaviour
    {
        // WebViewPrefab _webViewPrefab;
        // Keyboard _keyboard;

        public System.Action OnInitialized;

        #region WebView Initialization refer to OculusWebViewDemo

        
        // async void Start()
        // {
        //
        //     // Use a desktop User-Agent to request the desktop versions of websites.
        //     // https://developer.vuplex.com/webview/Web#SetUserAgent
        //     Web.SetUserAgent(false);
        //
        //     // Create a 0.6 x 0.4 instance of the prefab.
        //     // https://developer.vuplex.com/webview/WebViewPrefab#Instantiate
        //     _webViewPrefab = WebViewPrefab.Instantiate(0.6f, 0.4f);
        //     _webViewPrefab.transform.SetParent(transform, false);
        //     _webViewPrefab.transform.localPosition = new Vector3(0, 0.2f, 0.6f);
        //     _webViewPrefab.transform.localEulerAngles = new Vector3(0, 180, 0);
        //
        //     // Add an on-screen keyboard under the webview.
        //     // https://developer.vuplex.com/webview/Keyboard
        //     _keyboard = Keyboard.Instantiate();
        //     _keyboard.transform.SetParent(_webViewPrefab.transform, false);
        //     _keyboard.transform.localPosition = new Vector3(0, -0.41f, 0);
        //     _keyboard.transform.localEulerAngles = Vector3.zero;
        //
        //     // Wait for the prefab to initialize because its WebView property is null until then.
        //     // https://developer.vuplex.com/webview/WebViewPrefab#WaitUntilInitialized
        //     await _webViewPrefab.WaitUntilInitialized();
        //
        //     // After the prefab has initialized, you can use the IWebView APIs via its WebView property.
        //     // https://developer.vuplex.com/webview/IWebView
        //     _webViewPrefab.WebView.LoadUrl("https://google.com");
        //
        //     if (OnInitialized != null)
        //     {
        //         OnInitialized();
        //     }
        // }
        
        #endregion

        #region API
        
        public void LoadUrl(string url)
        {
            // _webViewPrefab.WebView.LoadUrl(url);
        }

        public void SetVisibility(bool show)
        {
            // _webViewPrefab.gameObject.SetActive(show);
        }
        
        #endregion
        
    }
}
