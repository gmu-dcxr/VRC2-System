// fix meta movement sdk losing tracking sometimes in the editor
// refer: https://forum.unity.com/threads/stop-and-restart-xr-plugin-management.1145918/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.XR.Management;

namespace VRC2.Hack
{
    public class OVRPatcher : MonoBehaviour
    {
        private static bool started = false;

        private void OnApplicationQuit()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                DisableXR();
            }
        }

        private void Start()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (started == false)
                {
                    Application.targetFrameRate = 90;
                    Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(90);
                    started = true;
                }
            }

            //Application.targetFrameRate = 90;
            // if (OVRManager.display.displayFrequenciesAvailable.Contains(90f))
            // {
            //     OVRManager.display.displayFrequency = 90f;
            // }

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (started == false)
                {
                    StartCoroutine(EnableXRCoroutine());
                }
            }
        }


        public static IEnumerator EnableXRCoroutine()
        {
            yield return new WaitWhile(() =>
            {
                return XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.Manager == null;
            });

            // Make sure the XR is disabled and properly disposed. It can happen that there is an activeLoader left
            // from the previous run.
            if (XRGeneralSettings.Instance.Manager.activeLoader ||
                XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                DisableXR();
                yield return null;
            }

            // Enable XR
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (!XRGeneralSettings.Instance.Manager.activeLoader ||
                !XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                // Something went wrong, XR is not enabled
                yield break;
            }

            XRGeneralSettings.Instance.Manager.StartSubsystems();

            started = true;

            yield return null;

            // // Not that OVRBody and OVRFaceExpressions components will not enable themselves automatically.
            // // You will have to do that manually
            // OVRPlugin.StartBodyTracking();
            // OVRPlugin.StartFaceTracking();
        }

        public static void DisableXR()
        {
            if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                // OVRPlugin.StopBodyTracking();
                // OVRPlugin.StopFaceTracking();
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
        }

    }
}