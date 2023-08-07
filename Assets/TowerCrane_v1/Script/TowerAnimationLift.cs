using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationLift : MonoBehaviour
{
    [HideInInspector]
    public TowerPlatformInstallation scriptTPL;
    private TowerMarkerElement scriptTME;
    public KeyCode openCapture = KeyCode.P;
    private bool openCapture_Bool = true;
    public KeyCode loadElement = KeyCode.O;
    public KeyCode loadUnloadElement = KeyCode.L;
    [Tooltip("Specifies how long the key prompt will turn off")]
    public float timerOffInfo = 3.2f;
    private int movementSwitching = 0;
    private AudioSource soundLift;
    private Animator towerAnim;
    public AudioClip startDV;
    public AudioClip stopDV;
    public AudioClip motor;
    [Space(10)]
    public string descriptionInstal;
    public string descriptionLoad;
    [Space(10)]
    [Tooltip("The maximum number of elements of the type 'Rack_type of B'")]
    public int maxElementType_B = 9;
    [HideInInspector]
    [Space(10)]
    public bool blockAnimationOpenCapture = false;
    [HideInInspector]
    public bool blockLoadElement_Bool = false;
    public Transform liftCrane;
    public Transform capture;
    public GameObject captureDetali;
    public BoxCollider captureTrigger;
    public Transform elementType_BUp;
    public Transform elementType_BDown;
    [HideInInspector]
    public Transform elementCrane_Capture;
    private Vector3 pointElementCapture;
    private Vector3 openMove;
    private Vector3 closeMove;
    private Vector3 loadMove;
    [HideInInspector]
    public Vector3 startPointElementUp;
    [HideInInspector]
    public Vector3 startPointElementDown;
    private bool blockMove = false;
    public float speedMoveCaoture = 0.41f;
    public float speedAnimationLift = 0.1f;
    private int timerSwitching = 0;
    [HideInInspector]
    public bool blockKeyCapture = true; // If the Element is attached to a grip then the grip animation will be blocked
    private bool blockAnimationLoadElement = true;
    [Tooltip("Ray finds the previous element and adds it to the list")]
    public Transform pointRaycastCheckElement;
    private bool blockRacastCheck = false;
  

    void Start()
    {
        towerAnim = gameObject.GetComponent<Animator>();
        soundLift = gameObject.GetComponent<AudioSource>();
        if (gameObject.GetComponent<TowerAnimationLift>() != null)
        {
            scriptTME = gameObject.GetComponent<TowerMarkerElement>();
        }
        pointElementCapture = new Vector3(-0.021f, 0.487f, 0);
        soundLift.mute = true;
        openMove = new Vector3(-2.641f, capture.localPosition.y, capture.localPosition.z);
        closeMove = new Vector3(-0.91f, capture.localPosition.y, capture.localPosition.z);
        loadMove = new Vector3(-0.001f, capture.localPosition.y, capture.localPosition.z);
        startPointElementUp = elementType_BUp.localPosition;
        startPointElementDown = elementType_BDown.localPosition;
    }
     void Update()
    {
        if(elementCrane_Capture && elementCrane_Capture.TryGetComponent(typeof(HingeJoint),out Component com))
        {
            if (capture.childCount > 1)
            {
                elementCrane_Capture.parent = null;
                blockKeyCapture = true;
                captureTrigger.enabled = true;
                captureDetali.SetActive(false);
                scriptTPL.windowKey.enabled = false;
                scriptTPL.description.text = "";
                scriptTPL.textKey.text = "";
                elementType_BDown.gameObject.layer = 2;
                elementType_BUp.gameObject.layer = 2;
            }
        }
        if (blockAnimationOpenCapture == true)
        {
            OpenCapture();
            LoadElement();
            LoadUnloadElement();
            MoveCapture();
            if (blockRacastCheck == true)
            {
                Debug.DrawRay(pointRaycastCheckElement.position, Vector3.left * 2, Color.green);
                RaycastHit hitCheck;
                int layerCargo = (1 << 10);
                Ray rayCheck = new Ray(pointRaycastCheckElement.position, Vector3.left);
                if(Physics.Raycast(rayCheck,out hitCheck, 2, layerCargo))
                {
                    elementType_BDown = hitCheck.collider.transform;
                    blockRacastCheck = false;
                }
            }
        }
    }
    public void OpenCapture()
    {
        if (Input.GetKeyDown(openCapture) && openCapture_Bool == true && !soundLift.isPlaying && blockKeyCapture == true)
        {
            if (soundLift.mute == true)
            {
                soundLift.mute = false;
            }
            blockMove = true;
            StartCoroutine("SoundMotor");
            gameObject.layer = 2;
            openCapture_Bool = false;
        }
        if (Input.GetKeyDown(openCapture) && openCapture_Bool == false && !soundLift.isPlaying && blockKeyCapture == true && capture.GetComponentInChildren<TowerMarkerElement>()== null)
        {
            StartCoroutine("SoundMotor");
            blockMove = true;
            openCapture_Bool = true;
        }

        if (openCapture_Bool == false && blockMove == true)
        {
            capture.localPosition = Vector3.MoveTowards(capture.localPosition, openMove, speedMoveCaoture * Time.deltaTime);
            if(capture.localPosition.x== openMove.x)
            {
                soundLift.Stop();
                soundLift.PlayOneShot(stopDV,0.5f);
                captureTrigger.enabled = true;
                blockMove = false;
            }
        }
        if(openCapture_Bool == true && blockMove == true)
        {
            capture.localPosition = Vector3.MoveTowards(capture.localPosition, closeMove, speedMoveCaoture * Time.deltaTime);
            if (capture.localPosition.x == closeMove.x)
            {
                soundLift.Stop();
                soundLift.PlayOneShot(stopDV, 0.5f);
                captureTrigger.enabled = false;
                blockMove = false;
                gameObject.layer = 10;
            }
        }
    }
    public void LoadElement()
    {
        if (Input.GetKeyDown(loadElement) && blockLoadElement_Bool == true)
        {
            scriptTME.Action();
            elementCrane_Capture.SetParent(capture);
            elementCrane_Capture.localRotation = Quaternion.Euler(0, 0, 0);
            elementCrane_Capture.localPosition = pointElementCapture;
            blockKeyCapture = false;
            captureTrigger.enabled = false;
            captureDetali.SetActive(true);
            scriptTPL.windowKey.enabled = true;
            scriptTPL.description.text = descriptionLoad;
            scriptTPL.textKey.text = loadUnloadElement.ToString();
            blockLoadElement_Bool = false;
            StartCoroutine("EnabledInfoKey");
        }
    }
    public void LoadUnloadElement()
    {
        if (Input.GetKeyDown(loadUnloadElement) && blockAnimationLoadElement == true && capture.childCount > 1)
        {
            StopCoroutine("EnabledInfoKey");
            StartCoroutine("SoundMotor");
            towerAnim.Play("Animation Lift Up");
            towerAnim.speed = speedAnimationLift;
            scriptTPL.windowKey.enabled = false;
            scriptTPL.description.text = "";
            scriptTPL.textKey.text = "";
            elementCrane_Capture.gameObject.layer = 2;
            scriptTPL.numberElement_RackType_B += 1;
            scriptTPL.numberElementAll += 1;
            scriptTPL.scriptSBV.towerCrane.GetComponent<TowerCamera>().maxDownCamera -= 5;
            blockAnimationLoadElement = false;
        }
        else if (Input.GetKeyDown(loadUnloadElement) && scriptTPL.numberElement_RackType_B > 2 && blockAnimationLoadElement == true && capture.childCount == 1)
        {
            StartCoroutine("SoundMotor");
            towerAnim.Play("Animation Lift Down");
            towerAnim.speed = speedAnimationLift;
            scriptTPL.numberElement_RackType_B -= 1;
            scriptTPL.numberElementAll -= 1;
            blockAnimationLoadElement = false;
            scriptTPL.scriptSBV.towerCrane.GetComponent<TowerCamera>().maxDownCamera += 5;
            timerSwitching = 3;
        }
    }
    public void MoveCapture()
    {
        if (movementSwitching == 1)
        {
            capture.localPosition = Vector3.MoveTowards(capture.localPosition, loadMove, speedMoveCaoture * Time.deltaTime);
            if (capture.localPosition.x == loadMove.x)
            {
                    soundLift.Stop();
                    if (!soundLift.isPlaying)
                    {
                        soundLift.PlayOneShot(stopDV, 0.5f);
                    }
                    timerSwitching = 1;
                    StartCoroutine("TimerStartAnimation");
                    movementSwitching = 0;
            }
        }
        if(movementSwitching == 2)
        {
            capture.localPosition = Vector3.MoveTowards(capture.localPosition, openMove, speedMoveCaoture * Time.deltaTime);
            if (!soundLift.isPlaying)
            {
                StartCoroutine("SoundMotor");
            }
            if (capture.localPosition.x == openMove.x)
            {
                soundLift.Stop();
                soundLift.PlayOneShot(stopDV, 0.5f);
                StartCoroutine("SoundMotor");
                towerAnim.speed = speedAnimationLift;
                movementSwitching = 0;
                timerSwitching = 0;
                blockKeyCapture = true;
                captureTrigger.enabled = true;
            //    blockAnimationLoadElement = true;
                gameObject.layer = 10;
            }
        }
        if(movementSwitching == 3)
        {
            capture.localPosition = Vector3.MoveTowards(capture.localPosition, loadMove, speedMoveCaoture * Time.deltaTime);
            if (capture.localPosition.x == loadMove.x)
            {
                soundLift.Stop();
                soundLift.PlayOneShot(stopDV, 0.5f);
                elementType_BUp.SetParent(capture);
                captureDetali.SetActive(true);
                elementType_BUp.gameObject.layer = 10;
                elementType_BUp = elementType_BDown;
                StartCoroutine("TimerStartAnimation");
                timerSwitching = 4;
                movementSwitching = 0;
            }
        }
        if (movementSwitching == 4)
        {
            capture.localPosition = Vector3.MoveTowards(capture.localPosition, openMove, speedMoveCaoture * Time.deltaTime);
            if (capture.localPosition.x == openMove.x)
            {
                soundLift.Stop();
                soundLift.PlayOneShot(stopDV, 0.5f);
                StartCoroutine("TimerStartAnimation");
                timerSwitching = 6;
                gameObject.layer = 10;
                movementSwitching = 0;
            }
        }
    }
    public void EventStop()
    {
        towerAnim.speed = 0;
        soundLift.Stop();
        soundLift.PlayOneShot(stopDV, 0.5f);
    }
    public void EventStopAnimation()
    {
        towerAnim.speed = 0;
        soundLift.Stop();
        soundLift.PlayOneShot(stopDV, 0.5f);
        StartCoroutine("TimerStartAnimation");
    }
    public void EventStopAnimation1()
    {
        towerAnim.speed = 0;
        soundLift.Stop();
        soundLift.PlayOneShot(stopDV, 0.5f);
        liftCrane.SetParent(null);
        transform.SetParent(null);
        transform.position = liftCrane.position;
        liftCrane.SetParent(transform);
        transform.SetParent(scriptTME.platformCrane);
        if (timerSwitching == 6)
        {
            blockRacastCheck = true;
            timerSwitching = 0;
        }
    }
    public void EventBlockLoad()
    {
        blockAnimationLoadElement = true;
    }
    IEnumerator SoundMotor()
    {
        soundLift.PlayOneShot(startDV);
        yield return new WaitForSeconds(0.3f);
        soundLift.clip = motor;
        soundLift.Play();
        soundLift.loop = true;
    }
    IEnumerator EnabledInfoKey()
    {
        yield return new WaitForSeconds(timerOffInfo);
        scriptTPL.windowKey.enabled = false;
        scriptTPL.description.text = "";
        scriptTPL.textKey.text = "";
    }
    IEnumerator TimerStartAnimation()
    {
        yield return new WaitForSeconds(0.5f);
            if (timerSwitching == 0)
            {
                soundLift.Stop();
                StartCoroutine("SoundMotor");
                movementSwitching = 1;
            }
            if (timerSwitching == 1)
            {
                soundLift.Stop();
                if (!soundLift.isPlaying)
                {
                    StartCoroutine("SoundMotor");
                }
                towerAnim.speed = speedAnimationLift;
                timerSwitching = 2;
            }
            else if (timerSwitching == 2)
            {
                elementCrane_Capture.SetParent(scriptTME.platformCrane);
                elementType_BDown.gameObject.layer = 10;
                elementType_BDown = elementType_BUp;
                elementType_BUp = elementCrane_Capture;
                captureDetali.SetActive(false);
                movementSwitching = 2;
            }
        if (timerSwitching == 3)
        {
            StartCoroutine("SoundMotor");
            movementSwitching = 3;
        }
        if (timerSwitching == 4)
        {
            StartCoroutine("SoundMotor");
            towerAnim.speed = speedAnimationLift;
            timerSwitching = 5;
        }
        else if (timerSwitching == 5)
        {
            StartCoroutine("SoundMotor");
            movementSwitching = 4;
        }
        if (timerSwitching == 6)
        {
            StartCoroutine("SoundMotor");
            towerAnim.speed = speedAnimationLift;
         //   blockAnimationLoadElement = true;
        }
    }
}
