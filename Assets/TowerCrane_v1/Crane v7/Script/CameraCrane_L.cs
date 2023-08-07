using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCrane_L : MonoBehaviour
{
    private CraneCarController_L _mScriptCar;
    private CraneController_L _mScriptCrane;
    private UIPanelCrane_L _mScript;
    private Transform targetCam;
    [HideInInspector]
    public float distance = 6f;
    private float horizontal = 0f;
    [HideInInspector]
    public float h = 0f;
    private float vertical = 0f;
    [HideInInspector]
    public float v = 0f;
    public float speed = 2;
    public float speedZoom = 1;
    public float smoothCamera = 0.5f;
    public float smoothCameraCabin = 1;
    [HideInInspector]
    public float yMinLi = 0f;
    [HideInInspector]
    public float yMaxLi = 90f;
    public float minDistanceX = 8.5f;
    public float maxDistanceX = 50f;
    [HideInInspector]
    public float offsetZoomCamera = 0;
    private Transform lookAtCameraCabinCar;
    [HideInInspector]
    public bool switchCamera = true;
    [Header("Camera Car")]
    public float minHorizontalCamera_CabinCar = -90;
    public float maxHorizontalCamera_CabinCar = 90;
    public float minVerticalCamera_CabinCar = -12;
    public float maxVerticalCamera_CabinCar = 26;
    [Header("Camera Crane")]
    public float minHorizontalCamera_CabinCrane = -115;
    public float maxHorizontalCamera_CabinCrane = 115;
    public float minVerticalCamera_CabinCrane = -86;
    public float maxVerticalCamera_CabinCrane = 26;
    private float offsetRotCamera_h = 0;
    public float offsetRotCamera_v = 0;

    void Start()
    {
        _mScriptCar = transform.GetComponentInParent<CraneCarController_L>();
        _mScriptCrane = transform.GetComponentInParent<CraneController_L>();
        _mScript = transform.GetComponentInParent<UIPanelCrane_L>();
        targetCam = _mScriptCar.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void LateUpdate()
    {
        if (switchCamera == true)
        {
            h += Input.GetAxis("Mouse X") * speed;
            v -= Input.GetAxis("Mouse Y") * speed;
            horizontal = Mathf.Lerp(horizontal, h, Time.deltaTime * speed / smoothCamera);
            vertical = Mathf.Lerp(vertical, v, Time.deltaTime * speed / smoothCamera);
            vertical = Mathf.Clamp(vertical, yMinLi, yMaxLi);
            Quaternion rotation = Quaternion.Euler(vertical, horizontal, 0);
            Vector3 position = rotation * new Vector3(0f, 0f, -offsetZoomCamera) + targetCam.position;
            transform.rotation = rotation;
            transform.position = position;
            //Zoom Camera
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && _mScript.toggleMune_Bool == true && _mScriptCar._camera_Bool == true)
            {
                --distance;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && _mScript.toggleMune_Bool == true && _mScriptCar._camera_Bool == true)
            {
                ++distance;
            }
            if (_mScriptCar._camera_Bool == true)
            {
                offsetZoomCamera = Mathf.Lerp(offsetZoomCamera, distance, Time.deltaTime * speedZoom / smoothCamera);
                distance = Mathf.Clamp(distance, minDistanceX, maxDistanceX);
            }
        }
        else if (switchCamera == false)
        {
            if (Input.GetKey(_mScriptCar.lookCamera) && _mScriptCar.startCrane_Bool == true)
            {
                h += Input.GetAxis("Mouse X") * speed;
                v -= Input.GetAxis("Mouse Y") * speed;
                h = Mathf.Clamp(h, minHorizontalCamera_CabinCar, maxHorizontalCamera_CabinCar);
                v = Mathf.Clamp(v, minVerticalCamera_CabinCar, maxVerticalCamera_CabinCar);
            }
            else if (Input.GetKeyUp(_mScriptCar.lookCamera) && _mScriptCar.startCrane_Bool == true)
            {
                h = 0;
                v = 0;
            }
            offsetRotCamera_h = Mathf.Lerp(offsetRotCamera_h, h, Time.deltaTime * speed / smoothCameraCabin);
            offsetRotCamera_v = Mathf.Lerp(offsetRotCamera_v, v, Time.deltaTime * speed / smoothCameraCabin);
            if (_mScriptCar.startCrane_Bool == true)
            {
                _mScriptCar.pointCameraCabin.localRotation = Quaternion.Euler(offsetRotCamera_v, offsetRotCamera_h, 0);
            }
            else if (_mScriptCar.startCrane_Bool == false)
            {
                h += Input.GetAxis("Mouse X") * speed;
                v -= Input.GetAxis("Mouse Y") * speed;
                h = Mathf.Clamp(h, minHorizontalCamera_CabinCrane, maxHorizontalCamera_CabinCrane);
                v = Mathf.Clamp(v, minVerticalCamera_CabinCrane, maxVerticalCamera_CabinCrane);
                _mScriptCrane.pointCameraCabinCrane.localRotation = Quaternion.Euler(offsetRotCamera_v, offsetRotCamera_h, 0);
            }
        }
    }
}
