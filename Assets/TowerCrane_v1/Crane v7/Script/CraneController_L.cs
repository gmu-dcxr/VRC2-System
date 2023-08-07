using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController_L : MonoBehaviour
{
    private UIPanelCrane_L _mScriptUI;
    private CraneCarController_L _mScriptCar;
    private CraneCounterweight_L _mScriptCounter;
    private CraneDopArrow_L _mScriptDop;
    private CameraCrane_L _mScriptCam;
    private CraneCable_L _mScriptCable;
    private CraneBigHook_L _mScriptBH;
    private CraneSmallHook_L _mScriptSH;
    private CraneErrorCodesInfoPanel_L _mScriptError;
    [Header("On Off Hook")]
    public KeyCode onHook_Key;
    [Header("Toggle Hook")]
    public KeyCode toggleHook_Key;
    [HideInInspector]
    public int numberHook = 0;
    [HideInInspector]
    public bool onHook_Bool = true;
    [Header("Point Camera Cabin Crane")]
    public Transform pointCameraCabinCrane;
    private float currentValuesCamera = 0;
    [Header("Light Crane")]
    public KeyCode lightCrane_Key;
    private bool lightCrane_Bool = true;
    public Light[] lightCrane;
    [Header("Rotation Cabin Crane")]
    public Transform cabinCrane;
    public KeyCode leftRotationCabin;
    public KeyCode rightRotationCabin;
    public float speedCabin = 0f;
    public float smoothRotationCabin = 1.2f;
    [HideInInspector]
    public float floatRotCabin = 0;
    private bool rotationCabin_Bool = true;
    [Header("Up Down Arrow Crane")]
    public Transform arrow_1;
    public KeyCode upArrow;
    public KeyCode downArrow;
    public float speedUpArrow = 0f;
    public float smoothArrowUp = 0.5f;
    public float minValueArrow = 0f;
    public float maxValueArrow = 0f;
    private bool arrowUpDown_Bool = true;
    [HideInInspector]
    public float floatArrow = 0f;
    [HideInInspector]
    public bool blockingAnimation = false;
    [Header("Arrow Forward Setting")]
    public KeyCode forwardArrow;
    public KeyCode backArrow;
    public float speedArrowFor = 0;
    private bool arrowFor_Bool = true;
    [Header("Arrow 2")]
    public Transform arrow_2;
    private Vector3 minArrow_2;
    private Vector3 maxArrow_2;
    public float min_Arrow2F = 0;
    public float max_Arrow2F = 0;
    [Header("Arrow 3")]
    public Transform arrow_3;
    private Vector3 minArrow_3;
    private Vector3 maxArrow_3;
    public float min_Arrow3F = 0;
    public float max_Arrow3F = 0;
    [Header("Arrow 4")]
    public Transform arrow_4;
    private Vector3 minArrow_4;
    private Vector3 maxArrow_4;
    public float min_Arrow4F = 0;
    public float max_Arrow4F = 0;
    [Header("Arrow 5")]
    public Transform arrow_5;
    private Vector3 minArrow_5;
    private Vector3 maxArrow_5;
    public float min_Arrow5F = 0;
    public float max_Arrow5F = 0;
    [Header("Arrow 6")]
    public Transform arrow_6;
    private Vector3 minArrow_6;
    private Vector3 maxArrow_6;
    public float min_Arrow6F = 0;
    public float max_Arrow6F = 0;
    public Transform cabRotor;
    public Transform mn1DetaliArrow_6;
    [Header("Wheel Cable")]
    public float speedWheelCable = 16.5f;
    public Transform ropeReel_A;
    public Transform ropeReel_B;
    public Transform piston_A;
    public Transform piston_B;
    private Transform _piston_A;
    private Transform _piston_B;
    public float pitchSupportCabin = 0;
    [Header("Check Distance Foward Arrow")]
    public Transform checkDisArrowFor_A;
    public Transform checkDisArrowFor_B;
    private float dis = 0f;
    public bool blocksForArrow = true;
    public Transform craneObject_5;
    public Transform wheelArrowBig_1;
    public Transform wheelArrowBig_2;
    public Transform wheelArrowBig_3;
    public Transform hookFastening;
    private Transform _wheelArrowBig_3;
    private Transform _hookFastening;
    private Transform _pointBigHook;
    private Transform _pointSmallHook;
    private Transform _mn1DetalyArrow_6;
    //Distance Arrow / Hook
    [HideInInspector]
    public float distanceAnchorBigHook = 0f;
    [HideInInspector]
    public float distanceAnchorSmallHook = 0f;
    private float Wheel_Float = 0f;
    [HideInInspector]
    public int errorCodesPanel = 0;
    [Header("If the load collides with the collider, the downward movement of the arrow will be blocked")]
    public bool blocksCargoDown = true;
    [HideInInspector]
    public bool _blocksC = true;

    void Start()
    {
        _mScriptSH = transform.GetComponentInChildren<CraneSmallHook_L>();
        _mScriptBH = transform.GetComponentInChildren<CraneBigHook_L>();
        _mScriptCar = transform.GetComponent<CraneCarController_L>();
        _mScriptUI = transform.GetComponent<UIPanelCrane_L>();
        _mScriptCounter = transform.GetComponent<CraneCounterweight_L>();
        _mScriptDop = transform.GetComponent<CraneDopArrow_L>();
        _mScriptCam = transform.GetComponentInChildren<CameraCrane_L>();
        _mScriptCable = transform.GetComponent<CraneCable_L>();
        _mScriptError = transform.GetComponent<CraneErrorCodesInfoPanel_L>();
        //Start Position end Rotation Piston
        GameObject p_A = new GameObject("PistonEmpty_A");
        _piston_A = p_A.transform;
        _piston_A.transform.localPosition = piston_A.position;
        _piston_A.SetParent(arrow_1);
        GameObject p_B = new GameObject("PistonEmpty_B");
        _piston_B = p_B.transform;
        _piston_B.transform.localPosition = piston_B.position;
        _piston_B.SetParent(cabinCrane);
        _piston_A.LookAt(_piston_B.position, _piston_A.up);
        _piston_B.LookAt(_piston_A.position, _piston_B.up);
        piston_A.SetParent(_piston_A);
        piston_B.SetParent(_piston_B);
        //Start Position end Rotation Big Hook
        GameObject w = new GameObject("_wheelArrowBig_3");
        _wheelArrowBig_3 = w.transform;
        _wheelArrowBig_3.SetParent(arrow_6);
        _wheelArrowBig_3.localPosition = wheelArrowBig_3.localPosition;
        GameObject h = new GameObject("_hookFastening");
        _hookFastening = h.transform;
        _hookFastening.SetParent(transform);
        _hookFastening.localPosition = new Vector3(0.05078924f, -1.348531f, 4.813746f);
        _wheelArrowBig_3.LookAt(_hookFastening.position, _wheelArrowBig_3.up);
        _hookFastening.LookAt(_wheelArrowBig_3.position, _hookFastening.up);
        hookFastening.SetParent(_hookFastening);
        GameObject empty = new GameObject("_pointBigHook");
        _pointBigHook = empty.transform;
        _pointBigHook.SetParent(_hookFastening, false);
        _pointBigHook.localPosition = new Vector3(0, -0.289f, 0.207f);
        _pointBigHook.LookAt(_wheelArrowBig_3.position, _pointBigHook.up);
        _mScriptBH.bigHook.SetParent(_pointBigHook);
        _mScriptBH.bigHook.localPosition = new Vector3(0.003f, -0.029f, 0.736f);
        _mScriptBH.bigHook.localRotation = Quaternion.Euler(92.28699f, 0, 0);
        //Start Position end Rotation Small Hook
        GameObject ps = new GameObject("_pointSmallHook");
        _pointSmallHook = ps.transform;
        _pointSmallHook.SetParent(_mScriptBH.bigHook);
        _pointSmallHook.localPosition = new Vector3(0, -0.117f, 0.337f);
        _pointSmallHook.LookAt(wheelArrowBig_2.position, _pointSmallHook.up);
        _mScriptSH.smallHook.SetParent(_pointSmallHook);
        _mScriptSH.smallHook.localPosition = new Vector3(0.012f, -0.055f, 0.706f);
        _mScriptSH.smallHook.localRotation = Quaternion.Euler(95.45001f, -9.80899f, -11.98099f);
        //Start Position end Rotation Mn_1_Arrow_6
        GameObject mn = new GameObject("_mn1DetalyArrow_6");
        _mn1DetalyArrow_6 = mn.transform;
        _mn1DetalyArrow_6.SetParent(arrow_6);
        _mn1DetalyArrow_6.localPosition = mn1DetaliArrow_6.localPosition;
        _mn1DetalyArrow_6.LookAt(_mScriptCable.pointLineWheelCableL2.position, _mn1DetalyArrow_6.up);
        _mScriptCable.pointLineWheelCableL2.LookAt(_mn1DetalyArrow_6.position, _mScriptCable.pointLineWheelCableL2.up);
        mn1DetaliArrow_6.SetParent(_mn1DetalyArrow_6);
        //Arrow Forward
        minArrow_2 = new Vector3(arrow_2.localPosition.x, arrow_2.localPosition.y, min_Arrow2F);
        maxArrow_2 = new Vector3(arrow_2.localPosition.x, arrow_2.localPosition.y, max_Arrow2F);
        minArrow_3 = new Vector3(arrow_3.localPosition.x, arrow_3.localPosition.y, min_Arrow3F);
        maxArrow_3 = new Vector3(arrow_3.localPosition.x, arrow_3.localPosition.y, max_Arrow3F);
        minArrow_4 = new Vector3(arrow_4.localPosition.x, arrow_4.localPosition.y, min_Arrow4F);
        maxArrow_4 = new Vector3(arrow_4.localPosition.x, arrow_4.localPosition.y, max_Arrow4F);
        minArrow_5 = new Vector3(arrow_5.localPosition.x, arrow_5.localPosition.y, min_Arrow5F);
        maxArrow_5 = new Vector3(arrow_5.localPosition.x, arrow_5.localPosition.y, max_Arrow5F);
        minArrow_6 = new Vector3(arrow_6.localPosition.x, arrow_6.localPosition.y, min_Arrow6F);
        maxArrow_6 = new Vector3(arrow_6.localPosition.x, arrow_6.localPosition.y, max_Arrow6F);
        lightCrane[0].transform.localRotation = Quaternion.Euler(37.995f, 180f,0f);
        lightCrane[1].transform.localRotation = Quaternion.Euler(37.995f, 180f, 0f);
    }
    void Update()
    {
            //Rotation Arrow
            if (_mScriptCar.startCrane_Bool == false && _mScriptCar.pover_Bool == false)
            {
                PanelError();
                if (Input.GetKey(rightRotationCabin) && floatArrow > 8 && onHook_Bool == false && _mScriptBH.bigHook.gameObject.activeInHierarchy == true && _mScriptCounter.blocedKey == true)
                {
                    InfoPosCabin();
                    floatRotCabin += Time.deltaTime * speedCabin;
                    rotationCabin_Bool = true;
                }
                else if (Input.GetKey(leftRotationCabin) && floatArrow > 8 && onHook_Bool == false && _mScriptBH.bigHook.gameObject.activeInHierarchy == true && _mScriptCounter.blocedKey == true)
                {
                    InfoPosCabin();
                    floatRotCabin -= Time.deltaTime * speedCabin;
                    rotationCabin_Bool = false;
                }
                RotationCabin();
                //Up Down Arrow
                if (_mScriptBH.bigHook.gameObject.activeInHierarchy == true)
                {
                    if (Input.GetKey(upArrow))
                    {
                        floatArrow += speedUpArrow * Time.deltaTime;
                        ropeReel_A.Rotate(Vector3.left * speedWheelCable * Time.deltaTime);
                        ropeReel_B.Rotate(Vector3.left * speedWheelCable * Time.deltaTime);
                        OffsetCable();
                        arrowUpDown_Bool = true;
                    }
                    else if (Input.GetKey(downArrow) && _blocksC == true)
                    {
                        floatArrow -= speedUpArrow * Time.deltaTime;
                        ropeReel_A.Rotate(Vector3.left * -speedWheelCable * Time.deltaTime);
                        ropeReel_B.Rotate(Vector3.left * -speedWheelCable * Time.deltaTime);
                        OffsetCable();
                        arrowUpDown_Bool = false;
                    }
                }
                ArrowUpDown();
                //Arrow Forward Back
                if (_mScriptBH.bigHook.gameObject.activeInHierarchy == true)
                {
                    if (Input.GetKey(forwardArrow))
                    {
                        arrowFor_Bool = false;
                        ArrowForward();
                    }
                    else if (Input.GetKey(backArrow))
                    {
                        arrowFor_Bool = true;
                        ArrowForward();
                    }
                }
                if (Input.GetKeyDown(_mScriptCar._camera) && _mScriptCar._camera_Bool == true && _mScriptCar.startCrane_Bool == false)
                {
                currentValuesCamera = _mScriptCam.h;
                _mScriptCam.transform.SetParent(pointCameraCabinCrane);
                _mScriptCam.h = 0;
                _mScriptCam.transform.localPosition = new Vector3(0, 0, 0);
                _mScriptCam.transform.localRotation = pointCameraCabinCrane.localRotation;
                _mScriptCam.switchCamera = false;
                _mScriptCar._camera_Bool = false;
                }
                else if (Input.GetKeyDown(_mScriptCar._camera) && _mScriptCar._camera_Bool == false && _mScriptCar.startCrane_Bool == false)
                {
                _mScriptCam.transform.SetParent(transform);
                pointCameraCabinCrane.transform.localRotation = Quaternion.Euler(0, 0, 0);
                _mScriptCam.distance = 8;
                _mScriptCam.yMinLi = 0f;
                _mScriptCam.yMaxLi = 90f;
                _mScriptCam.h = currentValuesCamera;
                _mScriptCam.switchCamera = true;
                _mScriptCar._camera_Bool = true;
            }
                //On Off Hook
                if (Input.GetKeyDown(onHook_Key))
                {
                    OnOffHook();
                }
                if (Input.GetKeyDown(toggleHook_Key) && onHook_Bool == false && _mScriptDop.spreadTheArrow_Bool == true && _mScriptBH.connected_Bool == true && _mScriptSH.connected_Bool == true)
                {
                    ToggleHook();
                }
                if (blocksCargoDown == true)
                {
                    if (_mScriptBH.hookOnCollision_Bool == false || _mScriptSH.hookOnCollision_Bool == false)
                    {
                        _blocksC = false;
                    }
                    else _blocksC = true;
                }

            }
            //Light Crane
            if (Input.GetKeyDown(lightCrane_Key) && lightCrane_Bool == true)
            {
                for (int i = 0; i < lightCrane.Length; i++)
                {
                    lightCrane[i].enabled = true;
                }
                lightCrane_Bool = false;
            }
            else if (Input.GetKeyDown(lightCrane_Key) && lightCrane_Bool == false)
            {
                for (int i = 0; i < lightCrane.Length; i++)
                {
                    lightCrane[i].enabled = false;
                }
                lightCrane_Bool = true;
            }
        }
    void LateUpdate()
    {
        if (_mScriptCar.startCrane_Bool == false)
        {
            if (_piston_A != null && _piston_B != null)
            {
                _piston_A.LookAt(_piston_B.position, _piston_A.up);
                _piston_B.LookAt(_piston_A.position, _piston_B.up);
            }
            if (onHook_Bool == true)
            {
                //LookAt Big Hook
                if (_hookFastening != null && _wheelArrowBig_3 != null)
                {
                    _hookFastening.LookAt(_wheelArrowBig_3.position, _hookFastening.up);
                    _wheelArrowBig_3.LookAt(_hookFastening.position, _wheelArrowBig_3.up);
                }
                if (_pointBigHook != null && _wheelArrowBig_3 != null)
                {
                    _pointBigHook.LookAt(_wheelArrowBig_3.position, _pointBigHook.up);
                    _wheelArrowBig_3.LookAt(_pointBigHook.position, _wheelArrowBig_3.up);
                }
                //LookAt Small Hook
                if (_pointSmallHook != null && wheelArrowBig_2 != null)
                {
                    _pointSmallHook.LookAt(wheelArrowBig_2.position, _pointSmallHook.up);
                }
            }
            //LookAt Mn1DetalyArrow_6
            if (_mScriptDop.blocksMN == true)
                if (_mScriptCable.pointLineWheelCableL2 != null && _mn1DetalyArrow_6 != null)
                {
                    _mScriptCable.pointLineWheelCableL2.LookAt(_mn1DetalyArrow_6.position, _mScriptCable.pointLineWheelCableL2.up);
                    _mn1DetalyArrow_6.LookAt(_mScriptCable.pointLineWheelCableL2.position, _mn1DetalyArrow_6.up);
                }
        }
    }
    private void RotationCabin()
    {
        var cab = Quaternion.AngleAxis(floatRotCabin, Vector3.up);
        cabinCrane.localRotation = Quaternion.Lerp(cabinCrane.localRotation, cab, Time.deltaTime * speedCabin / smoothRotationCabin);
    }
    private void InfoPosCabin()
    {
        if (rotationCabin_Bool == true)
        {
            cabRotor.Rotate(Vector3.up * -speedCabin * Time.deltaTime);
        }
        else if (rotationCabin_Bool == false)
        {
            cabRotor.Rotate(Vector3.up * speedCabin * Time.deltaTime);
        }
        _mScriptUI.textRotationArrow.text = (Mathf.RoundToInt(cabinCrane.localEulerAngles.y).ToString());
        _mScriptUI.textPositionCargo.text = _mScriptUI.textRotationArrow.text;
        //Text Position Cargo
        if (_mScriptUI.textPositionCargo.text == "180")
        {
            _mScriptUI.icon_N3.color = new Color32(88, 255, 95, 255);
            blockingAnimation = true;
        }
        else
        {
            _mScriptUI.icon_N3.color = new Color32(255, 88, 88, 255);
            blockingAnimation = false;
        }
        SoundPitchCraneCabin();
    }
    private void ArrowUpDown()
    {
        floatArrow = Mathf.Clamp(floatArrow, minValueArrow, maxValueArrow);
        var arrow = Quaternion.AngleAxis(floatArrow, Vector3.left);
        arrow_1.localRotation = Quaternion.Lerp(arrow_1.localRotation, arrow, Time.deltaTime * speedUpArrow / smoothArrowUp);
    }
    private void ArrowForward()
    {
        if (arrowFor_Bool == true)
        {
            if (min_Arrow5F == arrow_5.localPosition.z)
            {
                arrow_6.localPosition = Vector3.MoveTowards(arrow_6.localPosition, minArrow_6, speedArrowFor * Time.deltaTime);
            }
            if (min_Arrow4F == arrow_4.localPosition.z)
            {
                arrow_5.localPosition = Vector3.MoveTowards(arrow_5.localPosition, minArrow_5, speedArrowFor * Time.deltaTime);
            }
            if (min_Arrow3F == arrow_3.localPosition.z)
            {
                arrow_4.localPosition = Vector3.MoveTowards(arrow_4.localPosition, minArrow_4, speedArrowFor * Time.deltaTime);
            }
            if (min_Arrow2F == arrow_2.localPosition.z)
            {
                arrow_3.localPosition = Vector3.MoveTowards(arrow_3.localPosition, minArrow_3, speedArrowFor * Time.deltaTime);
            }
            arrow_2.localPosition = Vector3.MoveTowards(arrow_2.localPosition, minArrow_2, speedArrowFor * Time.deltaTime);

            ropeReel_A.Rotate(Vector3.left * speedWheelCable * Time.deltaTime);
            ropeReel_B.Rotate(Vector3.left * speedWheelCable * Time.deltaTime);
        }
        else if (arrowFor_Bool == false)
        {
            arrow_6.localPosition = Vector3.MoveTowards(arrow_6.localPosition, maxArrow_6, speedArrowFor * Time.deltaTime);
            if (max_Arrow6F == arrow_6.localPosition.z)
            {
                arrow_5.localPosition = Vector3.MoveTowards(arrow_5.localPosition, maxArrow_5, speedArrowFor * Time.deltaTime);
            }
            if (max_Arrow5F == arrow_5.localPosition.z)
            {
                arrow_4.localPosition = Vector3.MoveTowards(arrow_4.localPosition, maxArrow_4, speedArrowFor * Time.deltaTime);
            }
            if (max_Arrow4F == arrow_4.localPosition.z)
            {
                arrow_3.localPosition = Vector3.MoveTowards(arrow_3.localPosition, maxArrow_3, speedArrowFor * Time.deltaTime);
            }
            if (max_Arrow3F == arrow_3.localPosition.z)
            {
                arrow_2.localPosition = Vector3.MoveTowards(arrow_2.localPosition, maxArrow_2, speedArrowFor * Time.deltaTime);
            }
            ropeReel_A.Rotate(Vector3.left * -speedWheelCable * Time.deltaTime);
            ropeReel_B.Rotate(Vector3.left * -speedWheelCable * Time.deltaTime);
        }
        //Check Distance Forward Arrow
        dis = Vector3.Distance(checkDisArrowFor_A.position, checkDisArrowFor_B.position);
        float disA = Mathf.InverseLerp(0, 100, dis);
        float disM = disA * 241;
        _mScriptUI.textForArrow.text = Mathf.RoundToInt(disM).ToString();
        SoundPitchCraneCabin();
        if (min_Arrow6F == arrow_6.localPosition.z)
        {
            blocksForArrow = true;
        }
        else
            blocksForArrow = false;
    }
    public void SoundPitchCraneCabin()
    {
        _mScriptCar.soundEngine.pitch = Mathf.Lerp(_mScriptCar.soundEngine.pitch, pitchSupportCabin, Time.deltaTime * 0.8f);
    }
    public void OnOffHook()
    {
        if (_mScriptUI.textRotationArrow.text == "0" && blocksForArrow == true)
        {
            if (onHook_Bool == true)
            {
                //   _mScriptBH.bigHook.SetParent(transform);
                //   _mScriptSH.smallHook.SetParent(transform);
                _mScriptBH.bigHook.parent = null;
                _mScriptSH.smallHook.parent = null;
                _mScriptBH.AddPhysicsHookBig();
                _mScriptSH.AddPhysicsHookSmall();
                _mScriptUI.textBigCargoMaxKg.text = _mScriptBH.maxMassHook.ToString();
                _mScriptUI.textBigCargoKg.text = _mScriptBH.massHook.ToString();
                _mScriptBH.decayHookPoint.gameObject.SetActive(true);
                numberHook = 1;
                _mScriptCar.rigCar.freezeRotation = true;
                onHook_Bool = false;
            }
            else if (onHook_Bool == false && _mScriptBH.connected_Bool == true && _mScriptSH.connected_Bool == true && _mScriptBH.bigHook.gameObject.activeInHierarchy == true && floatArrow == 0 && _mScriptUI.textRotationArrow.text == "0" && blocksForArrow == true)
            {
                    //Big Hook
                    _mScriptBH.bigHook.SetParent(_pointBigHook);
                    _mScriptBH.bigHook.localPosition = new Vector3(0.003f, -0.029f, 0.736f);
                    _mScriptBH.bigHook.localRotation = Quaternion.Euler(92.28699f, 0, 0);
                    //Smal Hook
                    _mScriptSH.smallHook.SetParent(_pointSmallHook);
                    _mScriptSH.smallHook.localPosition = new Vector3(0.012f, -0.055f, 0.706f);
                    _mScriptSH.smallHook.localRotation = Quaternion.Euler(95.45001f, -9.80899f, -11.98099f);
                    _mScriptBH.AddPhysicsHookBig();
                    _mScriptSH.AddPhysicsHookSmall();
                    _mScriptUI.textSmallCargoMaxKg.text = "0";
                    _mScriptUI.textSmallCargoKg.text = "0";
                    _mScriptUI.textBigCargoMaxKg.text = "0";
                    _mScriptUI.textBigCargoKg.text = "0";
                    if (numberHook == 1)
                    {
                        _mScriptBH.decayHookPoint.gameObject.SetActive(false);
                    }
                    if (numberHook == 2)
                    {
                        _mScriptSH.decayHookPoint.gameObject.SetActive(false);
                    }
                _mScriptCar.rigCar.freezeRotation = false;
                onHook_Bool = true;
                }
            }
        }
    //Offset of cables when lifting the boom
    public void OffsetCable()
    {
        if (arrowUpDown_Bool == false)
        {
            Wheel_Float += speedUpArrow * Time.deltaTime;
            Wheel_Float = Mathf.Clamp(Wheel_Float, -180, 0);
            wheelArrowBig_2.localRotation = Quaternion.AngleAxis(Wheel_Float, Vector3.left);
            wheelArrowBig_3.localRotation = Quaternion.AngleAxis(Wheel_Float, Vector3.left);
        }
        else
        {
            Wheel_Float -= speedUpArrow * Time.deltaTime;
            Wheel_Float = Mathf.Clamp(Wheel_Float, -180, 0);
            wheelArrowBig_2.localRotation = Quaternion.AngleAxis(Wheel_Float, Vector3.left);
            wheelArrowBig_3.localRotation = Quaternion.AngleAxis(Wheel_Float, Vector3.left);
        }
        SoundPitchCraneCabin();
        _mScriptUI.ArrowUp();
        if (floatArrow > 7)
        {
            _mScriptCable.pointLineWheelCableA2.SetParent(craneObject_5);
            _mScriptCable.pointLineWheelCableB2.SetParent(craneObject_5);
            _mScriptCable.pointLineWheelCableA2.localPosition = _mScriptCable.pointLineWheelCableA1.localPosition;
            _mScriptCable.pointLineWheelCableB2.localPosition = _mScriptCable.pointLineWheelCableB1.localPosition;
        }
        else if (floatArrow < 7)
        {
            _mScriptCable.pointLineWheelCableA2.SetParent(arrow_1);
            _mScriptCable.pointLineWheelCableB2.SetParent(arrow_1);
            _mScriptCable.pointLineWheelCableA2.localPosition = new Vector3(0.07181949f, 0.6936059f, 0.6072743f);
            _mScriptCable.pointLineWheelCableB2.localPosition = new Vector3(-0.07518177f, 0.6936059f, 0.6072743f);
        }
    }
    public void ToggleHook()
    {
        if (numberHook == 1)
        {
            _mScriptUI.textSmallCargoMaxKg.text = _mScriptSH.maxMassHook.ToString();
            _mScriptUI.textSmallCargoKg.text = _mScriptSH.massHook.ToString();
            _mScriptUI.textBigCargoMaxKg.text = "0";
            _mScriptUI.textBigCargoKg.text = "0";
            _mScriptBH.decayHookPoint.gameObject.SetActive(false);
            _mScriptSH.decayHookPoint.gameObject.SetActive(true);
            numberHook = 2;
        }
        else if (numberHook == 2)
        {
            _mScriptUI.textBigCargoMaxKg.text = _mScriptBH.maxMassHook.ToString();
            _mScriptUI.textBigCargoKg.text = _mScriptBH.massHook.ToString();
            _mScriptUI.textSmallCargoMaxKg.text = "0";
            _mScriptUI.textSmallCargoKg.text = "0";
            _mScriptSH.decayHookPoint.gameObject.SetActive(false);
            _mScriptBH.decayHookPoint.gameObject.SetActive(true);
            numberHook = 1;
        }
    }
    public void StartPointHookRay()
    {
        Debug.DrawRay(wheelArrowBig_3.position, -Vector3.up * 100f, Color.red);
        Ray rayBigH = new Ray(wheelArrowBig_3.position, -Vector3.up);
        Ray raySmallH = new Ray(wheelArrowBig_2.position, -Vector3.up);
        RaycastHit hit;
            if (Physics.Raycast(rayBigH, out hit, 100f))
            {
                Vector3 startPointBigHook = hit.point;
                _mScriptBH.bigHook.position = new Vector3(startPointBigHook.x, 1.6f, startPointBigHook.z);
            }
            if (Physics.Raycast(raySmallH, out hit, 100f))
            {
                Vector3 startPointSmallHook = hit.point;
                _mScriptSH.smallHook.position = new Vector3(startPointSmallHook.x, 2.1f, startPointSmallHook.z);
            }
        }
    //Check at what distance the hook is from the boom / transmits data about the distance ConfigurableJoint
    public void CheckDistanceHook()
    {
        distanceAnchorBigHook = Vector3.Distance(_mScriptBH.bigHook.position, wheelArrowBig_3.position);
        distanceAnchorSmallHook = Vector3.Distance(_mScriptSH.smallHook.position, wheelArrowBig_2.position);
    }
    public void PanelError()
    {
        if (_mScriptError.blocksKeyErroe == true)
        {
            if (floatArrow < 8)
            {
                if (Input.GetKeyDown(leftRotationCabin) || Input.GetKeyDown(rightRotationCabin))
                {
                    errorCodesPanel = 1;
                  _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (onHook_Bool == true && floatArrow > 8)
            {
                if (Input.GetKeyDown(leftRotationCabin) || Input.GetKeyDown(rightRotationCabin))
                {
                    errorCodesPanel = 2;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (_mScriptBH.bigHook.gameObject.activeInHierarchy == false && floatArrow > 8)
            {
                if (Input.GetKeyDown(leftRotationCabin) || Input.GetKeyDown(rightRotationCabin))
                {
                    errorCodesPanel = 3;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (_mScriptCounter.blocedKey == false)
            {
                if (Input.GetKeyDown(leftRotationCabin) || Input.GetKeyDown(rightRotationCabin))
                {
                    errorCodesPanel = 4;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (_mScriptUI.textRotationArrow.text != "0")
            {
                if (Input.GetKeyDown(onHook_Key))
                {
                    errorCodesPanel = 5;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (blocksForArrow == false)
            {
                if (Input.GetKeyDown(onHook_Key))
                {
                    errorCodesPanel = 6;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (floatArrow > 0)
            {
                if (Input.GetKeyDown(onHook_Key))
                {
                    errorCodesPanel = 7;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (floatArrow > 0)
            {
                if (Input.GetKeyDown(_mScriptDop.spreadTheArrow))
                {
                    errorCodesPanel = 8;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (blocksForArrow == false)
            {
                if (Input.GetKeyDown(_mScriptDop.spreadTheArrow))
                {
                    errorCodesPanel = 9;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (_mScriptDop.spreadTheArrow_Bool == false)
            {
                if (Input.GetKeyDown(onHook_Key))
                {
                    errorCodesPanel = 10;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if (onHook_Bool == true)
            {
                if (Input.GetKeyDown(_mScriptDop.spreadTheArrow))
                {
                    errorCodesPanel = 11;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if(_mScriptBH.bigHook.gameObject.activeInHierarchy == false)
            {
                if(Input.GetKeyDown(upArrow) || Input.GetKeyDown(downArrow) || Input.GetKeyDown(forwardArrow) || Input.GetKeyDown(backArrow))
                {
                    errorCodesPanel = 10;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if(_mScriptDop.floatArrow != 0)
            {
                if (Input.GetKeyDown(_mScriptDop.spreadTheArrow))
                {
                    errorCodesPanel = 12;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
            if(_mScriptBH.connected_Bool == false || _mScriptSH.connected_Bool == false)
            {
                if (Input.GetKeyDown(onHook_Key))
                {
                    errorCodesPanel = 13;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
                if (Input.GetKeyDown(_mScriptDop.spreadTheArrow))
                {
                    errorCodesPanel = 13;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
                if (Input.GetKeyDown(toggleHook_Key))
                {
                    errorCodesPanel = 13;
                    _mScriptError.StartCoroutine("ErroeCodes");
                }
            }
        }
    }
}




