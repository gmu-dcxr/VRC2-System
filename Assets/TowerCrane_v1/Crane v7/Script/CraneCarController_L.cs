using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraneCarController_L : MonoBehaviour
{
    [HideInInspector]
    public SwitchingBetweenVehicles scriptSwitch;
    [Header("Pover Crane")]
    public KeyCode pover = KeyCode.Y;
    [HideInInspector]
    public bool pover_Bool = true;
    [Header("Switching Crane to Car")]
    public KeyCode startCrane_Key = KeyCode.Tab;
    [HideInInspector]
    public bool startCrane_Bool = true;
    private UIPanelCrane_L mScriptUI;
    private CraneController_L mScriptCrane;
    private CraneBigHook_L mScriptBigHook;
    private CraneSmallHook_L mScriptSmallHook;
    [HideInInspector]
    public CameraCrane_L mScriptCamera;
    [HideInInspector]
    public Rigidbody rigCar;
    [HideInInspector]
    public AudioSource soundEngine;
    private AudioSource soundTurnSignal;
    public AudioClip startEngine;
    public AudioClip stopEngine;
    public AudioClip engineClip;
    public WheelCollider[] _wheelCollider;
    public Transform[] _wheelTransform;
    public Transform _centerOfMass;
    [Header("Tuning Car Crane")]
    public float speedCrane = 210;
    private float _speed = 0f;
    public float _maxSpeed = 0f;
    public float steerForwardWheel = 0f;
    public float steerBackWheel = 0f;
    private float m_Horizontal = 0f;
    private float m_Vertical = 0f;
    //Check in which direction the car is moving
    private Vector3 checkMoving;
    private int checkMovint_Int = 0;
    [Header("Turning the rear wheels")]
    public KeyCode turningWheels_Key;
    private bool turningWheels_Bool = false;
    [Header("Overdrive")]
    public KeyCode overdriveKey;
    private bool overdrive_Bool = true;
    public float overdrive_Speed = 0f;
    [Header("Rear axle connection")]
    public KeyCode rearAxle_Key;
    private bool rearAxle_Bool = false;
    [Header("Braking force with the key pressed")]
    public float p = 0;
    [Header("Braking force when coasting")]
    public float c = 0;
    [Header("Sound Engine")]
    public float pitchSoundCrane = 0.75f;
    public float maxPitch = 1.38f;
    private float maxEngineRPM = 6000.0f;
    private float minEngineRPM = 1000.0f;
    private float engineRPM = 0f;
    private int currentGear = 0;
    private float gearShiftRate = 19;
    public float smoohtPitch = 3;
    public Light[] lightCrane;
    //TurnSignals
    public KeyCode turnSignals_Key;
    private bool turnSignals_Bool = true;
    public KeyCode leftTurnSignal_Key;
    public KeyCode RightTurnSignal_Key;
    private bool leftTurnSignal_Bool = true;
    private bool RightTurnSignal_Bool = true;
    //Headlights
    public KeyCode headlights_Key;
    private bool headlights_Bool = true;
    [Header("Speedometer Setting")]
    public float g = 0f;
    [Header("switch Camera Crane")]
    public KeyCode _camera = KeyCode.R;
    [Header("Look Around Camera")]
    public KeyCode lookCamera = KeyCode.LeftControl;
    public Transform pointCameraCabin;
    private float currentValuesCamera = 0;
    [HideInInspector]
    public bool _camera_Bool = true;
    private int blockCrane_Int_Crane = 0;

    private void Awake()
    {
        mScriptUI = gameObject.GetComponent<UIPanelCrane_L>();
    }
    void Start()
    {
        mScriptCamera = transform.GetComponentInChildren<CameraCrane_L>();
        mScriptCrane = transform.GetComponent<CraneController_L>();
        mScriptBigHook = gameObject.GetComponentInChildren<CraneBigHook_L>();
        mScriptSmallHook = gameObject.GetComponentInChildren<CraneSmallHook_L>();
      rigCar = gameObject.GetComponent<Rigidbody>();
      rigCar.centerOfMass = _centerOfMass.localPosition;
      soundEngine = gameObject.GetComponent<AudioSource>();
      soundTurnSignal = this.transform.Find("Sound TurnSignal").GetComponent<AudioSource>();
        if (scriptSwitch != null)
        {
            blockCrane_Int_Crane = scriptSwitch.blockCrane_Int;
        }else
        {
            blockCrane_Int_Crane = 1;
        }
    }
    void Update()
    {
        // Pover Crane
        if (Input.GetKeyDown(pover) && blockCrane_Int_Crane == 1)
        {
            PoverCrane();
            if(startCrane_Bool == false)
            {
                StartCrane();
            }
        }
        //Switching Crane to Car
        if (Input.GetKeyDown(startCrane_Key))
        {
            StartCrane();
            if(mScriptCrane.onHook_Bool == false)
            {
                if(mScriptCrane.numberHook == 1)
                {
                    mScriptBigHook.decayHookPoint.gameObject.SetActive(true);
                }
                if(mScriptCrane.numberHook == 2)
                {
                    mScriptSmallHook.decayHookPoint.gameObject.SetActive(true);
                }
            }
        }
        SoundEngine();
            //Steer & Engine Wheel Car
            if (startCrane_Bool == true && pover_Bool == false)
            {
                m_Horizontal = Input.GetAxis("Horizontal");
                m_Vertical = Input.GetAxis("Vertical");
                UpdateWheelPoses();
                SteerWheel();
                Motor();
                Brake();
                //Turn Signal
                if (Input.GetKeyDown(turnSignals_Key) && turnSignals_Bool == true)
                {
                    if (leftTurnSignal_Bool == false || RightTurnSignal_Bool == false)
                    {
                        StopCoroutine("RightTurnSignal");
                        lightCrane[1].enabled = false;
                        lightCrane[3].enabled = false;
                        RightTurnSignal_Bool = true;
                        StopCoroutine("LeftTurnSignal");
                        lightCrane[0].enabled = false;
                        lightCrane[2].enabled = false;
                        leftTurnSignal_Bool = true;
                        mScriptUI.StopCoroutine("LeftTurnSignal_Image");
                        mScriptUI.StopCoroutine("RightTurnSignal_Image");
                    }
                    mScriptUI.StartCoroutine("TurnSignal_Image");
                    StartCoroutine("TrunSignalIE");
                    soundTurnSignal.Play();
                    turnSignals_Bool = false;
                }
                else if (Input.GetKeyDown(turnSignals_Key) && turnSignals_Bool == false)
                {
                    mScriptUI.StopCoroutine("TurnSignal_Image");
                    mScriptUI.icon_S6.color = new Color32(255, 255, 255, 36);
                    mScriptUI.icon_S5Left.color = new Color32(255, 255, 255, 36);
                    mScriptUI.icon_S5Right.color = new Color32(255, 255, 255, 36);
                    StopCoroutine("TrunSignalIE");
                    lightCrane[0].enabled = false;
                    lightCrane[1].enabled = false;
                    lightCrane[2].enabled = false;
                    lightCrane[3].enabled = false;
                    soundTurnSignal.Stop();
                    turnSignals_Bool = true;
                }
                //Turn Signal Left
                if (Input.GetKeyDown(leftTurnSignal_Key) && leftTurnSignal_Bool == true && turnSignals_Bool == true)
                {
                    if (RightTurnSignal_Bool == false)
                    {
                        StopCoroutine("RightTurnSignal");
                        lightCrane[1].enabled = false;
                        lightCrane[3].enabled = false;
                        RightTurnSignal_Bool = true;
                        mScriptUI.StopCoroutine("RightTurnSignal_Image");
                        mScriptUI.icon_S5Right.color = new Color32(255, 255, 255, 36);
                    }
                    mScriptUI.StartCoroutine("LeftTurnSignal_Image");
                    StartCoroutine("LeftTurnSignal");
                    soundTurnSignal.Play();
                    leftTurnSignal_Bool = false;
                }
                else if (Input.GetKeyDown(leftTurnSignal_Key) && leftTurnSignal_Bool == false)
                {
                    mScriptUI.StopCoroutine("LeftTurnSignal_Image");
                    StopCoroutine("LeftTurnSignal");
                    lightCrane[0].enabled = false;
                    lightCrane[2].enabled = false;
                    mScriptUI.icon_S5Left.color = new Color32(255, 255, 255, 36);
                    soundTurnSignal.Stop();
                    leftTurnSignal_Bool = true;
                }
                //Turn Signal Right
                if (Input.GetKeyDown(RightTurnSignal_Key) && RightTurnSignal_Bool == true && turnSignals_Bool == true)
                {
                    if (leftTurnSignal_Bool == false)
                    {
                        StopCoroutine("LeftTurnSignal");
                        lightCrane[0].enabled = false;
                        lightCrane[2].enabled = false;
                        leftTurnSignal_Bool = true;
                        mScriptUI.StopCoroutine("LeftTurnSignal_Image");
                        mScriptUI.icon_S5Left.color = new Color32(255, 255, 255, 36);
                    }
                    mScriptUI.StartCoroutine("RightTurnSignal_Image");
                    StartCoroutine("RightTurnSignal");
                    soundTurnSignal.Play();
                    RightTurnSignal_Bool = false;
                }
                else if (Input.GetKeyDown(RightTurnSignal_Key) && RightTurnSignal_Bool == false)
                {
                    mScriptUI.StopCoroutine("RightTurnSignal_Image");
                    StopCoroutine("RightTurnSignal");
                    lightCrane[1].enabled = false;
                    lightCrane[3].enabled = false;
                    mScriptUI.icon_S5Right.color = new Color32(255, 255, 255, 36);
                    soundTurnSignal.Stop();
                    RightTurnSignal_Bool = true;
                }
                //Headlights
                if (Input.GetKeyDown(headlights_Key) && headlights_Bool == true)
                {
                    for (int i = 8; i < lightCrane.Length; i++)
                    {
                        lightCrane[i].enabled = true;
                    }
                    mScriptUI.Headlights();
                    headlights_Bool = false;
                }
                else if (Input.GetKeyDown(headlights_Key) && headlights_Bool == false)
                {
                    for (int i = 8; i < lightCrane.Length; i++)
                    {
                        lightCrane[i].enabled = false;
                    }
                    mScriptUI.Headlights();
                    headlights_Bool = true;
                }
                //Reversing lights
                if (checkMovint_Int < 0)
                {
                    lightCrane[4].enabled = true;
                    lightCrane[5].enabled = true;
                }
                else
                {
                    lightCrane[4].enabled = false;
                    lightCrane[5].enabled = false;
                }
                if (Input.GetKeyDown(_camera) && _camera_Bool == true)
                {
                currentValuesCamera = mScriptCamera.h;
                mScriptCamera.transform.SetParent(pointCameraCabin);
                mScriptCamera.h = 0;
                mScriptCamera.v = 0;
                mScriptCamera.transform.localPosition = new Vector3(0, 0, 0);
                mScriptCamera.transform.localRotation = pointCameraCabin.localRotation;
                mScriptCamera.switchCamera = false;
                _camera_Bool = false;
                }
                else if (Input.GetKeyDown(_camera) && _camera_Bool == false)
                {
                mScriptCamera.transform.SetParent(transform);
                pointCameraCabin.transform.localRotation = Quaternion.Euler(0, 0, 0);
                mScriptCamera.distance = 8;
                mScriptCamera.yMinLi = 0f;
                mScriptCamera.yMaxLi = 90f;
                mScriptCamera.h = currentValuesCamera;
                mScriptCamera.switchCamera = true;
                _camera_Bool = true;
                }
                SpeedEngine();
                //Check AKPP
                if (checkMovint_Int == 0 && m_Vertical == 0)
                {
                    mScriptUI.textAkpp.text = "P";
                }
                else if (checkMovint_Int > 0 && m_Vertical > 0)
                {
                    mScriptUI.textAkpp.text = "D";
                }
                else if (checkMovint_Int < 0 && m_Vertical < 0)
                {
                    mScriptUI.textAkpp.text = "R";
                }
            }
        }
    //Pover Crane
    public void PoverCrane()
    {
        if(pover_Bool == true)
        {
            StartCoroutine("Pover");
            pover_Bool = false;
        }
        else if(pover_Bool == false)
        {
            soundEngine.Stop();
            soundEngine.loop = false;
            soundEngine.PlayOneShot(stopEngine, 0.8f);
            pover_Bool = true;
            mScriptUI.PoverCraneUI();
            mScriptUI.InfoKey_Int = 0;
        }
    }
    public void StartCrane()
    {
        if(startCrane_Bool == true && pover_Bool == false)
        {
            mScriptUI.icon_S4.color = new Color32(255, 87, 54, 255);
            mScriptUI.OnMenuCrane();
            if (mScriptCamera.transform.parent == pointCameraCabin)
            {
                mScriptCamera.transform.SetParent(transform);
                mScriptCamera.switchCamera = true;
                mScriptCamera.distance = 8;
                _camera_Bool = true;
            }
            startCrane_Bool = false;
        }
        else if(startCrane_Bool == false)
        {
            mScriptUI.OnMenuCrane();
            mScriptUI.icon_S4.color = new Color32(255, 255, 255, 36);
            if (mScriptCamera.transform.parent == mScriptCrane.pointCameraCabinCrane)
            {
                mScriptCamera.transform.SetParent(transform);
                mScriptCamera.switchCamera = true;
                mScriptCamera.distance = 8;
                _camera_Bool = true;
            }
            startCrane_Bool = true;
        }
    }
    //Pover Crane
    IEnumerator Pover()
    {
        soundEngine.PlayOneShot(startEngine, 0.7f);
        yield return new WaitForSeconds(0.23f);
        mScriptUI.PoverCraneUI();
        soundEngine.clip = engineClip;
        soundEngine.loop = true;
        soundEngine.Play();
    }
    //Turn Signals
    IEnumerator TrunSignalIE()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            lightCrane[0].enabled = true;
            lightCrane[1].enabled = true;
            lightCrane[2].enabled = true;
            lightCrane[3].enabled = true;
            yield return new WaitForSeconds(0.3f);
            lightCrane[0].enabled = false;
            lightCrane[1].enabled = false;
            lightCrane[2].enabled = false;
            lightCrane[3].enabled = false;
        }
    }
    //Turn Signal Left
    IEnumerator LeftTurnSignal ()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            lightCrane[0].enabled = true;
            lightCrane[2].enabled = true;
            yield return new WaitForSeconds(0.3f);
            lightCrane[0].enabled = false;
            lightCrane[2].enabled = false;
        }
    }
    //Turn Signal Right
    IEnumerator RightTurnSignal()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            lightCrane[1].enabled = true;
            lightCrane[3].enabled = true;
            yield return new WaitForSeconds(0.3f);
            lightCrane[1].enabled = false;
            lightCrane[3].enabled = false;
        }
    }
    void FixedUpdate()
    {
        //Check in which direction the car is moving
        checkMoving = rigCar.transform.InverseTransformDirection(rigCar.velocity);
        checkMovint_Int = (Mathf.RoundToInt(checkMoving.z));
        //  Speedometer
        if (pover_Bool == false)
        {
            UISpeedCrane_L.ShowSpeed(rigCar.velocity.magnitude, 0, g);
        }
    }
    private void SteerWheel()
    {
        _wheelCollider[0].steerAngle = m_Horizontal * steerForwardWheel;
        _wheelCollider[1].steerAngle = m_Horizontal * steerForwardWheel;
        _wheelCollider[2].steerAngle = m_Horizontal * steerForwardWheel;
        _wheelCollider[3].steerAngle = m_Horizontal * steerForwardWheel;
        if(turningWheels_Bool == true)
        {
            _wheelCollider[4].steerAngle = m_Horizontal * -steerBackWheel;
            _wheelCollider[5].steerAngle = m_Horizontal * -steerBackWheel;
            _wheelCollider[6].steerAngle = m_Horizontal * -steerBackWheel;
            _wheelCollider[7].steerAngle = m_Horizontal * -steerBackWheel;
        }
        //Turning the rear wheels
        if(Input.GetKeyDown(turningWheels_Key) && turningWheels_Bool == false)
        {
          mScriptUI.TurningWheels();
          turningWheels_Bool = true;
            if (rearAxle_Bool == false)
            {
                mScriptUI.RearAxle();
                rearAxle_Bool = false;
            }
        }
        else if(Input.GetKeyDown(turningWheels_Key) && turningWheels_Bool == true)
        {
            mScriptUI.TurningWheels();
            _wheelCollider[4].steerAngle = 0;
            _wheelCollider[5].steerAngle = 0;
            _wheelCollider[6].steerAngle = 0;
            _wheelCollider[7].steerAngle = 0;
            turningWheels_Bool = false;
            mScriptUI.RearAxle();
            rearAxle_Bool = false;
        }
    }
    private void UpdateWheel(WheelCollider wcol, Transform wTran)
    {
        Vector3 _pos = wTran.position;
        Quaternion _quat = wTran.rotation;
        wcol.GetWorldPose(out _pos, out _quat);
        wTran.transform.position = _pos;
        wTran.transform.rotation = _quat;
    }
    public void UpdateWheelPoses()
    {
        UpdateWheel(_wheelCollider[0], _wheelTransform[0]);
        UpdateWheel(_wheelCollider[1], _wheelTransform[1]);
        UpdateWheel(_wheelCollider[2], _wheelTransform[2]);
        UpdateWheel(_wheelCollider[3], _wheelTransform[3]);
        UpdateWheel(_wheelCollider[4], _wheelTransform[4]);
        UpdateWheel(_wheelCollider[5], _wheelTransform[5]);
        UpdateWheel(_wheelCollider[6], _wheelTransform[6]);
        UpdateWheel(_wheelCollider[7], _wheelTransform[7]);
    }
    private void Motor()
    {
        //Rear axle connection
        if(Input.GetKeyDown(rearAxle_Key) && rearAxle_Bool == true && turningWheels_Bool == false)
        {
            mScriptUI.RearAxle();
            rearAxle_Bool = false;
        }
        else if(Input.GetKeyDown(rearAxle_Key)&& rearAxle_Bool == false && turningWheels_Bool == false)
        {
            mScriptUI.RearAxle();
            rearAxle_Bool = true;
        }
        if (Input.GetKeyDown(overdriveKey)&& overdrive_Bool == true)
        {
            mScriptUI.Overdrive();
            overdrive_Bool = false;
        }else if(Input.GetKeyDown(overdriveKey) && overdrive_Bool == false)
        {
            mScriptUI.Overdrive();
            overdrive_Bool = true;
        }
        if (overdrive_Bool == true)
        {
            _wheelCollider[0].motorTorque = m_Vertical * speedCrane;
            _wheelCollider[1].motorTorque = m_Vertical * speedCrane;
            _wheelCollider[2].motorTorque = m_Vertical * speedCrane;
            _wheelCollider[3].motorTorque = m_Vertical * speedCrane;
            if(rearAxle_Bool == true)
            {
                _wheelCollider[4].motorTorque = m_Vertical * speedCrane;
                _wheelCollider[5].motorTorque = m_Vertical * speedCrane;
                _wheelCollider[6].motorTorque = m_Vertical * speedCrane;
                _wheelCollider[7].motorTorque = m_Vertical * speedCrane;
            }
        }
        else if(overdrive_Bool == false)
        {
            _wheelCollider[0].motorTorque = m_Vertical * overdrive_Speed;
            _wheelCollider[1].motorTorque = m_Vertical * overdrive_Speed;
            _wheelCollider[2].motorTorque = m_Vertical * overdrive_Speed;
            _wheelCollider[3].motorTorque = m_Vertical * overdrive_Speed;
            if (rearAxle_Bool == true)
            {
                _wheelCollider[4].motorTorque = m_Vertical * overdrive_Speed;
                _wheelCollider[5].motorTorque = m_Vertical * overdrive_Speed;
                _wheelCollider[6].motorTorque = m_Vertical * overdrive_Speed;
                _wheelCollider[7].motorTorque = m_Vertical * overdrive_Speed;
            }
        }
        rigCar.velocity = Vector3.ClampMagnitude(rigCar.velocity, _maxSpeed);

    }
    private void SpeedEngine()
    {
        if (checkMovint_Int < 3 && m_Horizontal == 0)
        {
            float d1 = speedCrane;
            _speed = d1 * 3;
        }
        else
            _speed = speedCrane;
    }
    private void Brake()
    {
        if (checkMovint_Int > 0 && m_Vertical < 0)
        {
            _wheelCollider[0].brakeTorque = (p) * (Mathf.Abs(m_Vertical));
            _wheelCollider[1].brakeTorque = (p) * (Mathf.Abs(m_Vertical));
            _wheelCollider[2].brakeTorque = (p) * (Mathf.Abs(m_Vertical));
            _wheelCollider[3].brakeTorque = (p) * (Mathf.Abs(m_Vertical));
            lightCrane[6].enabled = true;
            lightCrane[7].enabled = true;
        }
        else if (checkMovint_Int < 0 && m_Vertical > 0)
        {
            _wheelCollider[0].brakeTorque = (p) * (Mathf.Abs(m_Vertical));
            _wheelCollider[1].brakeTorque = (p) * (Mathf.Abs(m_Vertical));
            _wheelCollider[2].brakeTorque = (p) * (Mathf.Abs(m_Vertical));
            _wheelCollider[3].brakeTorque = (p) * (Mathf.Abs(m_Vertical));
            lightCrane[6].enabled = true;
            lightCrane[7].enabled = true;
        }
        else if (m_Vertical == 0)
        {
            _wheelCollider[0].brakeTorque = (c) * (Mathf.Abs(1));
            _wheelCollider[1].brakeTorque = (c) * (Mathf.Abs(1));
            _wheelCollider[2].brakeTorque = (c) * (Mathf.Abs(1));
            _wheelCollider[3].brakeTorque = (c) * (Mathf.Abs(1));
        }
        else if (checkMovint_Int == 0)
        {
            _wheelCollider[0].brakeTorque = 0;
            _wheelCollider[1].brakeTorque = 0;
            _wheelCollider[2].brakeTorque = 0;
            _wheelCollider[3].brakeTorque = 0;
            lightCrane[6].enabled = false;
            lightCrane[7].enabled = false;
        }
    }
    private void SoundEngine()
    {
        engineRPM = Mathf.Clamp((((Mathf.Abs((_wheelCollider[1].rpm + _wheelCollider[4].rpm)) * gearShiftRate) + minEngineRPM)) / (float)(currentGear + 1), minEngineRPM, maxEngineRPM);
        soundEngine.pitch = Mathf.Lerp(soundEngine.pitch, Mathf.Lerp(pitchSoundCrane, maxPitch, (engineRPM - minEngineRPM / 1.82f) / (maxEngineRPM + minEngineRPM)), Time.deltaTime * smoohtPitch);
    }
}
