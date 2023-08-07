using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPanelCrane_L : MonoBehaviour
{
    private CraneCarController_L _mScriptCrane;
    private CraneController_L _mScriptCC;
    private CraneDopArrow_L _mScriptDop;
    public CraneBigHook_L _mScriptBigHook;
    public CraneSmallHook_L _mScriptSmallHook;
    private bool onMenu_Bool = true;
    [Header("Toggle tap menu Key + Scroll Wheel")]
    public KeyCode toggleMune_Key;
    [HideInInspector]
    public bool toggleMune_Bool = true;
    [HideInInspector]
    public int _toggleMenuCrane = 0;
    public Image icon_S1;
    public Image icon_S2;
    public Image icon_S3;
    public Image icon_S4;
    public Image icon_S5Left;
    public Image icon_S5Right;
    public Image icon_S6;
    public Image icon_S7;
    public Image icon_S8;
    public Image icon_S9;
    public Image icon_S10;
    public Image icon_Od;
    public Text textAkpp;
    public Image icon_S11;
    private bool icon_Od_Bool = true;
    private bool icon_S1_Bool = true;
    private bool icon_S2_Bool = true;
    private bool icon_S7_Bool = true;
    public Image icon_G1;
    public Image icon_G2;
    public Image icon_G3;
    public Image icon_G4FL;
    public Image icon_G4FR;
    public Image icon_G4BL;
    public Image icon_G4BR;
    public Image iconKeyInfo;
    public Text textKeyInfo;
    public Text textKeyDescription;
    [HideInInspector]
    public int InfoKey_Int = 0;
    [HideInInspector]
    public string infoKeyCode;
    [HideInInspector]
    public string infoDescription;
    public Text text_FL_A;
    public Text text_FL_B;
    public Text text_FR_A;
    public Text text_FR_B;
    public Text text_BL_A;
    public Text text_BL_B;
    public Text text_BR_A;
    public Text text_BR_B;
    [HideInInspector]
    public bool icon_G4FL_Bool = true;
    [HideInInspector]
    public bool icon_G4FR_Bool = true;
    [HideInInspector]
    public bool icon_G4BL_Bool = true;
    [HideInInspector]
    public bool icon_G4BR_Bool = true;
    public Image icon_N1;
    public Image icon_N2;
    public Image icon_N3;
    public Image icon_N4;
    public Text textPositionCargo;
    public Text textConectedCargo;
    public Image icon_F1;
    public Text textAngleDopArrpw;
    public Text textKgCargoDopArrow;
    public Text textMaxKgDopArrow;
    public Image icon_K1;
    public Text textSmallCargoMaxKg;
    public Text textBigCargoMaxKg;
    public Text textBigCargoKg;
    public Text textSmallCargoKg;
    public Text textForArrow;
    public Text textUpArrow;
    public Text textRotationArrow;
    public Text textInfoPanel;
    public Text text_F1;
    public Text text_F2;
    public Text text_F3;
    public Text text_F4;
    private bool poverIcon = true;
    
    void Start()
    {
        _mScriptCrane = transform.GetComponent<CraneCarController_L>();
        _mScriptCC = transform.GetComponent<CraneController_L>();
        _mScriptDop = transform.GetComponent<CraneDopArrow_L>();
        _mScriptBigHook = gameObject.GetComponentInChildren<CraneBigHook_L>();
        _mScriptSmallHook = gameObject.GetComponentInChildren<CraneSmallHook_L>();
        poverIcon = false;
        PoverCraneUI();
    }
    void Update()
    {
        if (_mScriptCrane.startCrane_Bool == false)
        {
            if (Input.GetKeyDown(toggleMune_Key) && _mScriptCrane.startCrane_Bool == false)
            {
                toggleMune_Bool = false;
            }
            else if (Input.GetKeyUp(toggleMune_Key))
            {
                toggleMune_Bool = true;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && toggleMune_Bool == false)
            {
                _toggleMenuCrane -= 1;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && toggleMune_Bool == false)
            {
                _toggleMenuCrane += 1;
            }
            if (toggleMune_Bool == false)
            {
                _toggleMenuCrane = Mathf.Clamp(_toggleMenuCrane, 0, 3);
                ToggleMenuCrane();
            }
        }
    }
    public void Overdrive()
    {
        if(icon_Od_Bool == true)
        {
            icon_Od.color = new Color32(255, 160, 0, 214);
            icon_Od_Bool = false;
        }
        else 
        {
            icon_Od.color = new Color32(255, 255, 255, 36);
            icon_Od_Bool = true;
        }
    }
    public void RearAxle()
    {
        if(icon_S2_Bool == true)
        {
            icon_S2.color = new Color32(255, 160, 0, 214);
            icon_S2_Bool = false;
        }
        else
        {
            icon_S2.color = new Color32(255, 255, 255, 36);
            icon_S2_Bool = true;
        }
    }
    public void TurningWheels()
    {
        if(icon_S1_Bool == true)
        {
            icon_S1.color = new Color32(255, 160, 0, 214);
            icon_S1_Bool = false;
        }
        else
        {
            icon_S1.color = new Color32(255, 255, 255, 36);
            icon_S1_Bool = true;
        }
    }
    public void Headlights()
    {
        if(icon_S7_Bool == true)
        {
            icon_S7.color = new Color32(255, 160, 0, 214);
            icon_S7_Bool = false;
        }
        else if(icon_S7_Bool == false)
        {
            icon_S7.color = new Color32(255, 255, 255, 36);
            icon_S7_Bool = true;
        }
    }
    IEnumerator LeftTurnSignal_Image()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            icon_S5Left.color = new Color32(0, 255, 15, 255);
            yield return new WaitForSeconds(0.3f);
            icon_S5Left.color = new Color32(255, 255, 255, 36);
        }
    }
    IEnumerator RightTurnSignal_Image()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            icon_S5Right.color = new Color32(0, 255, 15, 255);
            yield return new WaitForSeconds(0.3f);
            icon_S5Right.color = new Color32(255, 255, 255, 36);
        }
    }
    IEnumerator TurnSignal_Image()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            icon_S6.color = new Color32(0, 255, 15, 255);
            icon_S5Left.color = new Color32(0, 255, 15, 255);
            icon_S5Right.color = new Color32(0, 255, 15, 255);
            yield return new WaitForSeconds(0.3f);
            icon_S6.color = new Color32(255, 255, 255, 36);
            icon_S5Left.color = new Color32(255, 255, 255, 36);
            icon_S5Right.color = new Color32(255, 255, 255, 36);
        }
    }
    public void SupportImageFL()
    {
        if(icon_G4FL_Bool == true)
        {
            icon_G4FL.color = new Color32(255, 214, 0, 255);
            icon_G4FL_Bool = false;
        }
        else
        {
            icon_G4FL.color = new Color32(255, 255, 255, 255);
            icon_G4FL_Bool = true;
        }
    }
    public void SupportImageFR()
    {
        if (icon_G4FR_Bool == true)
        {
            icon_G4FR.color = new Color32(255, 214, 0, 255);
            icon_G4FR_Bool = false;
        }
        else
        {
            icon_G4FR.color = new Color32(255, 255, 255, 255);
            icon_G4FR_Bool = true;
        }
    }
    public void SupportImageBL()
    {
        if (icon_G4BL_Bool == true)
        {
            icon_G4BL.color = new Color32(255, 214, 0, 255);
            icon_G4BL_Bool = false;
        }
        else
        {
            icon_G4BL.color = new Color32(255, 255, 255, 255);
            icon_G4BL_Bool = true;
        }
    }
    public void SupportImageBR()
    {
        if (icon_G4BR_Bool == true)
        {
            icon_G4BR.color = new Color32(255, 214, 0, 255);
            icon_G4BR_Bool = false;
        }
        else
        {
            icon_G4BR.color = new Color32(255, 255, 255, 255);
            icon_G4BR_Bool = true;
        }
    }
    private void ToggleMenuCrane()
    {
        if(_toggleMenuCrane == 0)
        {
            icon_G1.gameObject.SetActive(true);
            icon_N1.gameObject.SetActive(false);
            text_F1.color = new Color32(0, 255, 2, 255);
            text_F2.color = new Color32(197, 197, 197, 255);
        }
        else if(_toggleMenuCrane == 1)
        {
            icon_K1.gameObject.SetActive(false);
            icon_G1.gameObject.SetActive(false);
            icon_N1.gameObject.SetActive(true);
            text_F2.color = new Color32(0, 255, 2, 255);
            text_F1.color = new Color32(197, 197, 197, 255);
            text_F3.color = new Color32(197, 197, 197, 255);
        }
        else if(_toggleMenuCrane == 2)
        {
            icon_F1.gameObject.SetActive(false);
            icon_N1.gameObject.SetActive(false);
            icon_K1.gameObject.SetActive(true);
            text_F3.color = new Color32(0, 255, 2, 255);
            text_F2.color = new Color32(197, 197, 197, 255);
            text_F4.color = new Color32(197, 197, 197, 255);
        }
        else if(_toggleMenuCrane == 3)
        {
            icon_K1.gameObject.SetActive(false);
            icon_F1.gameObject.SetActive(true);
            text_F4.color = new Color32(0, 255, 2, 255);
            text_F3.color = new Color32(197, 197, 197, 255);
        }
    }
    public void OnMenuCrane()
    {
        if(onMenu_Bool == true)
        {
            text_F1.enabled = true;
            text_F2.enabled = true;
            text_F3.enabled = true;
            text_F4.enabled = true;
            ToggleMenuCrane();
            onMenu_Bool = false;
        }
        else if(onMenu_Bool == false)
        {
            text_F1.enabled = false;
            text_F2.enabled = false;
            text_F3.enabled = false;
            text_F4.enabled = false;
            icon_N1.gameObject.SetActive(false);
            icon_F1.gameObject.SetActive(false);
            icon_G1.gameObject.SetActive(false);
            icon_K1.gameObject.SetActive(false);
            onMenu_Bool = true;

        }
    }
    public void ArrowUp()
    {
        textUpArrow.text = Mathf.RoundToInt(_mScriptCC.floatArrow).ToString();
    }
    public void DopArrowUp()
    {
        textAngleDopArrpw.text = Mathf.RoundToInt(_mScriptDop.floatArrow).ToString();
    }
    public void PoverCraneUI()
    {
        if (poverIcon == true)
        {
            icon_S1.enabled = true;
            icon_S2.enabled = true;
            icon_S3.enabled = true;
            icon_S4.enabled = true;
            icon_S5Left.enabled = true;
            icon_S5Right.enabled = true;
            icon_S6.enabled = true;
            icon_S7.enabled = true;
            icon_S8.enabled = true;
            icon_S9.enabled = true;
            icon_S10.enabled = true;
            icon_S11.enabled = true;
            textAkpp.enabled = true;
            icon_Od.enabled = true;
            if (_mScriptCC.numberHook == 1)
            {
                _mScriptBigHook.decayHookPoint.gameObject.SetActive(false);
            }
            if (_mScriptCC.numberHook == 2)
            {
                _mScriptSmallHook.decayHookPoint.gameObject.SetActive(false);
            }
            poverIcon = false;
        }
        else if (poverIcon == false)
        {
            icon_S1.enabled = false;
            icon_S2.enabled = false;
            icon_S3.enabled = false;
            icon_S4.enabled = false;
            icon_S5Left.enabled = false;
            icon_S5Right.enabled = false;
            icon_S6.enabled = false;
            icon_S7.enabled = false;
            icon_S8.enabled = false;
            icon_S9.enabled = false;
            icon_S10.enabled = false;
            icon_S11.enabled = false;
            textAkpp.enabled = false;
            text_F1.enabled = false;
            text_F2.enabled = false;
            text_F3.enabled = false;
            text_F4.enabled = false;
            icon_Od.enabled = false;
            icon_N1.gameObject.SetActive(false);
            icon_F1.gameObject.SetActive(false);
            icon_G1.gameObject.SetActive(false);
            icon_K1.gameObject.SetActive(false);
            if(_mScriptCC.numberHook == 1)
            {
                _mScriptBigHook.decayHookPoint.gameObject.SetActive(false);
            }
            if (_mScriptCC.numberHook == 2)
            {
                _mScriptSmallHook.decayHookPoint.gameObject.SetActive(false);
            }
            poverIcon = true;
        }

    }
    public void InfoKey()
    {
        if(InfoKey_Int == 1)
        {
            if(iconKeyInfo.fillAmount != 1)
            {
                iconKeyInfo.fillAmount += 1.85f * Time.deltaTime;
            }
        }
        else if(InfoKey_Int == 2)
        {
            if (iconKeyInfo.fillAmount > 0)
            {
                iconKeyInfo.fillAmount -= 2f * Time.deltaTime;
            }
        }
        if (iconKeyInfo.fillAmount == 1)
        {
            textKeyInfo.text = infoKeyCode.ToString();
            textKeyDescription.text = infoDescription.ToString();
        }
        else
        {
            textKeyInfo.text = "";
            textKeyDescription.text = "";
        }
    }
}
