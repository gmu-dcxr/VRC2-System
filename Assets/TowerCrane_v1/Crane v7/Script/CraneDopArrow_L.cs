using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneDopArrow_L : MonoBehaviour
{
    private CraneCable_L _mScriptCable;
    private CraneCarController_L _mScriptC;
    private UIPanelCrane_L _mScriptUI;
    private CraneController_L _mScriptCC;
    private CraneBigHook_L _mScriptBH;
    private CraneSmallHook_L _mScriptSH;
    [Header("Spread the arrow")]
    public KeyCode spreadTheArrow;
    [HideInInspector]
    public bool spreadTheArrow_Bool = true;
    public float speedSpread = 0f;
    [Header("Maximum mass Arrow cargo")]
    public float maxMassCargo = 0;
    [Header("Up Down Dop Arrow control")]
    public KeyCode upArrow;
    public KeyCode dowArrow;
    public KeyCode ab_Key;
    public float minValueDopArrow = 0f;
    public float maxValueDopArrow = 0f;
    [HideInInspector]
    public float floatArrow = 0;
    private bool upArrow_Bool = true;
    public float speedArrow;
    public Transform dopArrow_1;
    public Transform dopArrow_2;
    public Transform dopArrow_3;
    public Transform piston_A;
    public Transform piston_B;
    private Transform _piston_A;
    private Transform _piston_B;
    public Transform detaliArrow_6;
    public Transform mnDopArrow_3;
    private Transform _mnDopArrow_3;
    public Transform wheelArrow_1;
    public Transform wheelArrow_3;
    public Transform lockDopArrow;
    private bool auto_Open_1 = false;
    private bool auto_Open_2 = false;
    private bool auto_Open_3 = false;
    private bool auto_Open_4 = false;
    private bool auto_Closed_1 = false;
    private bool auto_Closed_2 = false;
    private bool auto_Closed_3 = false;
    private bool auto_Closed_4 = false;
    [HideInInspector]
    public bool blicedKeyOpen = true;
    private bool blicedKeyClosed = false;
    private GameObject _emptyRotationDopArrow;
    // Blocks loockAt mn1DetalyArrow_6
    [HideInInspector]
    public bool blocksMN = true;

    void Start()
    {
        _mScriptBH = transform.GetComponentInChildren<CraneBigHook_L>();
        _mScriptSH = transform.GetComponentInChildren<CraneSmallHook_L>();
        _mScriptCable = transform.GetComponent<CraneCable_L>();
        _mScriptC = gameObject.GetComponent<CraneCarController_L>();
        _mScriptUI = gameObject.GetComponent<UIPanelCrane_L>();
        _mScriptCC = gameObject.GetComponent<CraneController_L>();
        GameObject _empty = new GameObject("_emptyRotationDopArrow");
        _emptyRotationDopArrow = _empty;
        _emptyRotationDopArrow.transform.SetParent(_mScriptCC.arrow_6);
        _emptyRotationDopArrow.transform.localPosition = new Vector3(0.3893042f, 0.3555943f, 4.353301f);
        _emptyRotationDopArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        GameObject pis_A = new GameObject("_piston_A");
        _piston_A = pis_A.transform;
        _piston_A.localPosition = piston_A.position;
        _piston_A.SetParent(dopArrow_1);
        GameObject pis_B = new GameObject("_piston_B");
        _piston_B = pis_B.transform;
        _piston_B.localPosition = piston_B.position;
        _piston_B.SetParent(dopArrow_2);
        _piston_A.LookAt(_piston_B.position, _piston_A.up);
        _piston_B.LookAt(_piston_A.position, _piston_B.up);
        piston_A.SetParent(_piston_A);
        piston_B.SetParent(_piston_B);
        GameObject mn = new GameObject("_mnDopArrow_3");
        _mnDopArrow_3 = mn.transform;
        _mnDopArrow_3.SetParent(dopArrow_3);
        _mnDopArrow_3.localPosition = mnDopArrow_3.localPosition;
    }
    void Update()
    {
        if(_mScriptCC.onHook_Bool == false)
        {
            //Spread the arrow
            if (Input.GetKeyDown(spreadTheArrow) && spreadTheArrow_Bool == true)
            {
                if (_mScriptCC.floatArrow == 0 && _mScriptCC.blocksForArrow == true && _mScriptCC.onHook_Bool == false  && blicedKeyOpen == true) {
                    StartCoroutine("OpenAnimationDopArrow");
                    lockDopArrow.gameObject.SetActive(false);
                    spreadTheArrow_Bool = false;
                }
            }
            else if(Input.GetKeyDown(spreadTheArrow) && spreadTheArrow_Bool == false)
            {
                if (_mScriptCC.floatArrow == 0 && _mScriptCC.blocksForArrow == true && blicedKeyClosed == true && floatArrow == 0 && _mScriptBH.connected_Bool == true) {
                    StartCoroutine("ClosedAnimationDopArrow");
                    spreadTheArrow_Bool = true;
                }
            }
            //Up Down Dop Arrow
            if (Input.GetKey(upArrow) && Input.GetKey(ab_Key))
            {
                upArrow_Bool = true;
                UpDownDopArrow();
            }
            else if (Input.GetKey(dowArrow) && Input.GetKey(ab_Key) && _mScriptCC._blocksC == true)
            {
                upArrow_Bool = false;
                UpDownDopArrow();
            }
            //Open Dop Arrow
            if (auto_Open_1 == true)
            {
                   detaliArrow_6.localRotation = Quaternion.Lerp(detaliArrow_6.localRotation, Quaternion.Euler(0, 180, 0), speedSpread * Time.deltaTime);
            }
            if (auto_Open_2 == true)
            {
                dopArrow_1.localRotation = Quaternion.Lerp(dopArrow_1.localRotation, Quaternion.Euler(0, -14.95f, 0), speedSpread * Time.deltaTime);
            }
            if (auto_Open_3 == true)
            {
                _emptyRotationDopArrow.transform.localRotation = Quaternion.Lerp(_emptyRotationDopArrow.transform.localRotation, Quaternion.Euler(0, -165.05f, 0), speedSpread * Time.deltaTime);
            }
            if (auto_Open_4 == true)
            {
                dopArrow_3.localRotation = Quaternion.Lerp(dopArrow_3.localRotation, Quaternion.Euler(0, -180, 0), speedSpread * Time.deltaTime);
            }
            //Closed Dop Arrow
            if (auto_Closed_4 == true)
            {
                dopArrow_3.localRotation = Quaternion.Lerp(dopArrow_3.localRotation, Quaternion.Euler(0, 0, 0), speedSpread * Time.deltaTime);
            }
            if (auto_Closed_3 == true)
            {
                _emptyRotationDopArrow.transform.localRotation = Quaternion.Lerp(_emptyRotationDopArrow.transform.localRotation, Quaternion.Euler(0, 0, 0), speedSpread * Time.deltaTime);
            }
            if (auto_Closed_2 == true)
            {
                dopArrow_1.localRotation = Quaternion.Lerp(dopArrow_1.localRotation, Quaternion.Euler(0, 0, 0), speedSpread * Time.deltaTime);
            }
            if (auto_Closed_1 == true)
            {
                detaliArrow_6.localRotation = Quaternion.Lerp(detaliArrow_6.localRotation, Quaternion.Euler(0, 0, 0), speedSpread * Time.deltaTime);
            }
        }
    }
    void LateUpdate()
    {
        if (spreadTheArrow_Bool == false)
        {
            if (_piston_A != null && _piston_B != null)
            {
                _piston_A.LookAt(_piston_B.position, _piston_A.up);
                _piston_B.LookAt(_piston_A.position, _piston_B.up);
            }
            if (mnDopArrow_3.parent == _mnDopArrow_3)
            {
                if (_mScriptCable.pointLineWheelCableL2 != null && _mnDopArrow_3 != null)
                {
                    _mnDopArrow_3.LookAt(_mScriptCable.pointLineWheelCableL2.position, _mnDopArrow_3.up);
                }
            }
        }
    }
    IEnumerator OpenAnimationDopArrow()
    {
        blocksMN = false;
        if(_mScriptCC.numberHook == 1)
        {
            _mScriptUI.textBigCargoMaxKg.text = "0";
            _mScriptUI.textBigCargoKg.text = "0";
            _mScriptBH.decayHookPoint.gameObject.SetActive(false);
        }
        if (_mScriptCC.numberHook == 2)
        {
            _mScriptUI.textSmallCargoMaxKg.text = "0";
            _mScriptUI.textSmallCargoKg.text = "0";
            _mScriptSH.decayHookPoint.gameObject.SetActive(false);
        }
        _mScriptCC.numberHook = 1;
        _mScriptSH.AddPhysicsHookSmall();
        _mScriptBH.AddPhysicsHookBig();
        _mScriptSH.smallHook.gameObject.SetActive(false);
        _mScriptBH.bigHook.gameObject.SetActive(false);
        _mScriptCable.OnCable_B();
        _mScriptBH.bigHook.SetParent(wheelArrow_3);
        _mScriptBH.bigHook.localPosition = new Vector3(0, -0.879f, 0);
            blicedKeyOpen = false;
            auto_Open_1 = true;
            yield return new WaitForSeconds(4);
            auto_Open_2 = true;
            yield return new WaitForSeconds(6);
            dopArrow_1.SetParent(_emptyRotationDopArrow.transform);
            auto_Open_3 = true;
            yield return new WaitForSeconds(8);
            auto_Open_4 = true;
            yield return new WaitForSeconds(10);
        _mScriptCable.OnCable_A();
        _mScriptBH.bigHook.gameObject.SetActive(true);
            auto_Open_1 = false;
            auto_Open_2 = false;
            auto_Open_3 = false;
            auto_Open_4 = false;
            blicedKeyClosed = true;
        _mScriptBH.AddPhysicsHookBig();
        _mScriptBH.decayHookPoint.gameObject.SetActive(true);
        _mnDopArrow_3.LookAt(_mScriptCable.pointLineWheelCableL2.position, _mnDopArrow_3.up);
        mnDopArrow_3.SetParent(_mnDopArrow_3);
        mnDopArrow_3.localRotation = Quaternion.Euler(12.13f,-13.6f,90f);
        _mScriptUI.textMaxKgDopArrow.text = maxMassCargo.ToString();
        _mScriptUI.textKgCargoDopArrow.text = _mScriptBH.massHook.ToString();
    }
    IEnumerator ClosedAnimationDopArrow()
    {
        _mScriptBH.decayHookPoint.gameObject.SetActive(false);
        _mScriptUI.textMaxKgDopArrow.text = "0";
        _mScriptUI.textKgCargoDopArrow.text = "0";
        _mScriptBH.AddPhysicsHookBig();
        _mScriptSH.smallHook.gameObject.SetActive(false);
        _mScriptBH.bigHook.gameObject.SetActive(false);
        _mScriptCable.OnCable_C();
        blicedKeyClosed = false;
        auto_Closed_4 = true;
        yield return new WaitForSeconds(8);
        auto_Closed_3 = true;
        yield return new WaitForSeconds(8);
        auto_Closed_2 = true;
        yield return new WaitForSeconds(6);
        auto_Closed_1 = true;
        yield return new WaitForSeconds(10);
        dopArrow_1.SetParent(_mScriptCC.arrow_1);
        _mScriptCable.OnCable_D();
        _mScriptCC.StartPointHookRay();
        _mScriptBH.bigHook.SetParent(_mScriptC.transform);
        _mScriptSH.AddPhysicsHookSmall();
        _mScriptBH.AddPhysicsHookBig();
        _mScriptSH.smallHook.gameObject.SetActive(true);
        _mScriptBH.bigHook.gameObject.SetActive(true);
        auto_Closed_1 = false;
        auto_Closed_2 = false;
        auto_Closed_3 = false;
        auto_Closed_4 = false;
        blicedKeyOpen = true;
        lockDopArrow.gameObject.SetActive(true);    
        _mScriptUI.textBigCargoMaxKg.text = _mScriptBH.maxMassHook.ToString();
        _mScriptUI.textBigCargoKg.text = _mScriptBH.massHook.ToString();
        blocksMN = true;      
        mnDopArrow_3.SetParent(dopArrow_3);
        mnDopArrow_3.localRotation = Quaternion.Euler(0,0,0);
        _mScriptBH.decayHookPoint.gameObject.SetActive(true);
    }
    private void UpDownDopArrow()
    {
        if (blicedKeyClosed == true)
        {
            if (upArrow_Bool == true)
            {
                floatArrow += speedArrow;
                floatArrow = Mathf.Clamp(floatArrow, minValueDopArrow, maxValueDopArrow);
                dopArrow_2.localRotation = Quaternion.AngleAxis(floatArrow, Vector3.left);
            }
            else if (upArrow_Bool == false)
            {
                floatArrow -= speedArrow;
                floatArrow = Mathf.Clamp(floatArrow, minValueDopArrow, maxValueDopArrow);
                dopArrow_2.localRotation = Quaternion.AngleAxis(floatArrow, Vector3.left);
            }
            _mScriptCC.SoundPitchCraneCabin();
            _mScriptUI.DopArrowUp();
        }
    }
}
