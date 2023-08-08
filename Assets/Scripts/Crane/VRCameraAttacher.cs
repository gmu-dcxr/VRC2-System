using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCameraAttacher : MonoBehaviour
{
    // this SHOULD be the parent of OVRCameraRig
    public GameObject camRig;
    
    public Camera towerCamera;

    public Transform target;

    public GameObject canvas;

    [Header("Offset")] public Vector3 localPos = Vector3.zero;

    public Vector3 localRot = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // disable tower camera in VR
        towerCamera.enabled = false;
        
        // disable canvas
        canvas.SetActive(false);
        
        // set parent
        camRig.transform.parent = target.transform;

        camRig.transform.localPosition = localPos;
        camRig.transform.localRotation = Quaternion.Euler(localRot);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
