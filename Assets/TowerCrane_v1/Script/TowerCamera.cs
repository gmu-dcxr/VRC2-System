using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCamera : MonoBehaviour
{
    public TowerControllerCrane scriptTCC;
    private SwitchingBetweenVehicles scriptSBV;
    public KeyCode switchCamera = KeyCode.R;
    [Space(20)]
    public KeyCode movingCamera_press = KeyCode.LeftShift;
    public float speedMoveCamera = 1;
    public float smoothMoveCamera = 0.6f;
    [Tooltip("How high can you raise the camera")]
    public float maxUpCamera = -1f;
    [Space(10)]
    public float speedZoomCamera = 1;
    public float smoothZoomCamera = 0.6f;
    private float offsetposY = 0;
    private float offsetMovePosY = 0;
    private float offsetZoomCamera = 0;
    [HideInInspector]
    public float maxDownCamera = 0;
    private bool pressButton = true;
    private Transform targetCam;
    [HideInInspector]
    public float distance = 6f;
    private float horizontal = 0f;
    private float h = 0f;
    private float vertical = 0f;
    private float v = 0f;
    [Space(20)]
    public float speedRotationCamera = 2;
    public float smoothRotationCamera = 0.5f;
    private float yMinLi = 0f;
    private float yMaxLi = 90f;
    public float minDistanceX = 2.5f;
    public float maxDistanceX = 50f;
    private int pointCamera = 1;
    public Transform pointCameraCabin;
    public Transform pointCameraBoomCart;
    public Transform pointCameraCrane;
    public Transform pointCameraHook;
    public float startDistancePointCrane = 4;

    void Start()
    {
        scriptSBV = scriptTCC.GetComponent<SwitchingBetweenVehicles>();
        targetCam = pointCameraCrane;
        maxDownCamera = 0;
    }
    void Update()
    {
        if (scriptTCC.blockControllerCrane == true)
        {
            if (Input.GetKeyDown(switchCamera))
            {
                pointCamera += 1;
                if (pointCamera > 4)
                {
                    pointCamera = 1;
                }
                SwitchPoint();
            }
        }     
    }
    void LateUpdate()
    {
        if (scriptSBV.blockCamera_Bool == true)
        {
            h += Input.GetAxis("Mouse X") * speedRotationCamera;
            horizontal = Mathf.Lerp(horizontal, h, Time.deltaTime * speedRotationCamera / smoothRotationCamera);
            v -= Input.GetAxis("Mouse Y") * speedRotationCamera;
            vertical = Mathf.Lerp(vertical, v, Time.deltaTime * speedRotationCamera / smoothRotationCamera);
            vertical = Mathf.Clamp(vertical, yMinLi, yMaxLi);
            Quaternion rotation = Quaternion.Euler(vertical, horizontal, 0);
            Vector3 position = rotation * new Vector3(0f, offsetMovePosY, -offsetZoomCamera) + targetCam.position;
            if (pointCamera == 1 || pointCamera == 2)
            {
                transform.rotation = rotation;
            }
            transform.position = position;
            if (pointCamera == 1)
            {
                if (Input.GetKey(movingCamera_press))
                {
                    pressButton = false;
                }
                else if (Input.GetKeyUp(movingCamera_press))
                {
                    pressButton = true;
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    if (pressButton == true) //Zoom Camera
                    {
                        --distance;
                    }
                    else // Moving Camera
                    {
                        ++offsetposY;
                    }
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (pressButton == true) //Zoom Camera
                    {
                        ++distance;
                    }
                    else // Moving Camera
                    {
                        --offsetposY;
                    }
                }
                if (pointCamera == 1)
                {
                    offsetMovePosY = Mathf.Lerp(offsetMovePosY, offsetposY, Time.deltaTime * speedMoveCamera / smoothMoveCamera);
                    offsetZoomCamera = Mathf.Lerp(offsetZoomCamera, distance, Time.deltaTime * speedZoomCamera / smoothZoomCamera);
                    offsetposY = Mathf.Clamp(offsetposY, maxDownCamera, maxUpCamera);
                    distance = Mathf.Clamp(distance, minDistanceX, maxDistanceX);
                }
            }
        }
    }
    public void SwitchPoint()
    {
        if (pointCamera == 1)
        {
            targetCam = pointCameraCrane;
            distance = startDistancePointCrane;
            yMinLi = 0f;
            yMaxLi = 90;
        }
        if (pointCamera == 2)
        {
            targetCam = pointCameraCabin;
            transform.rotation = Quaternion.Euler(0, -90, 0);
            offsetZoomCamera = 0;
            offsetposY = 0;
            offsetMovePosY = 0;
            h = 0;
            yMinLi = -30f;
            yMaxLi = 60;
        }
        if (pointCamera == 3)
        {
            targetCam = pointCameraBoomCart;
            transform.localRotation = Quaternion.Euler(90, 0, 0);
            offsetZoomCamera = 0;
            offsetposY = 0;
            offsetMovePosY = 0;
            yMinLi = 0f;
            yMaxLi = 90;
        }
        if(pointCamera == 4)
        {
            targetCam = pointCameraHook;
            transform.localRotation = Quaternion.Euler(90, 0, 0);
            offsetZoomCamera = 0;
            offsetposY = 0;
            offsetMovePosY = 0;
            yMinLi = 0f;
            yMaxLi = 90;
        }
    }
}
