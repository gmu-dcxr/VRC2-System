using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraneLiftController_L : MonoBehaviour
{
    private UIPanelCrane_L mScriptLift_S;
    private CraneCarController_L mScriptCar_S;
    public float _speed = 0f;
    public float pitchSupport = 0f;
    [Header("Support lift Forward/Back & Up/Down")]
    public KeyCode a_Key;
    public KeyCode b_Key;
    public KeyCode ab_Key_For_Back;
    public KeyCode ab_Key_Up_Down;
    [Header("Support FL_A")]
    public Transform supportArrow_FL_A;
    public float FL_Support_A_Min_ = 0f;
    public float FL_Support_A_Max_ = 0f;
    private Vector3 FL_Support_A_Min;
    private Vector3 FL_Support_A_Max;
    [Header("Support FL_B")]
    public Transform supportArrow_FL_B;
    public float FL_Support_B_Min_ = 0f;
    public float FL_Support_B_Max_ = 0f;
    private Vector3 FL_Support_B_Min;
    private Vector3 FL_Support_B_Max;
    [Header("Support FL_Lift")]
    public Transform support_Lift_FL;
    public float FL_Support_Lift_Min_;
    public float FL_Support_Lift_Max_;
    private Vector3 FL_Support_Lift_Min;
    private Vector3 FL_Support_Lift_Max;
    [Header("Support FR_A")]
    public Transform supportArrow_FR_A;
    public float FR_Support_A_Min_;
    public float FR_Support_A_Max_;
    private Vector3 FR_Support_A_Min;
    private Vector3 FR_Support_A_Max;
    [Header("Support FR_B")]
    public Transform supportArrow_FR_B;
    public float FR_Support_B_Min_;
    public float FR_Support_B_Max_;
    private Vector3 FR_Support_B_Min;
    private Vector3 FR_Support_B_Max;
    [Header("Support FR_Lift")]
    public Transform support_Lift_FR;
    public float FR_Support_Lift_Min_;
    public float FR_Support_Lift_Max_;
    private Vector3 FR_Support_Lift_Min;
    private Vector3 FR_Support_Lift_Max;
    [Header("Support BL_A")]
    public Transform supportArrow_BL_A;
    public float BL_Support_A_Min_;
    public float BL_Support_A_Max_;
    private Vector3 BL_Support_A_Min;
    private Vector3 BL_Support_A_Max;
    [Header("Support BL_B")]
    public Transform supportArrow_BL_B;
    public float BL_Support_B_Min_;
    public float BL_Support_B_Max_;
    private Vector3 BL_Support_B_Min;
    private Vector3 BL_Support_B_Max;
    [Header("Support BL_Lift")]
    public Transform support_Lift_BL;
    public float BL_Support_Lift_Min_;
    public float BL_Support_Lift_Max_;
    private Vector3 BL_Support_Lift_Min;
    private Vector3 BL_Support_Lift_Max;
    [Header("Support BR_A")]
    public Transform supportArrow_BR_A;
    public float BR_Support_A_Min_;
    public float BR_Support_A_Max_;
    private Vector3 BR_Support_A_Min;
    private Vector3 BR_Support_A_Max;
    [Header("Support BR_B")]
    public Transform supportArrow_BR_B;
    public float BR_Support_B_Min_;
    public float BR_Support_B_Max_;
    private Vector3 BR_Support_B_Min;
    private Vector3 BR_Support_B_Max;
    [Header("Support BR_Lift")]
    public Transform support_Lift_BR;
    public float BR_Support_Lift_Min_;
    public float BR_Support_Lift_Max_;
    private Vector3 BR_Support_Lift_Min;
    private Vector3 BR_Support_Lift_Max;
    public KeyCode supportFL_Key;
    public KeyCode supportFR_Key;
    public KeyCode supportBL_Key;
    public KeyCode supportBR_Key;
    //Check Distance Support Forward Back
    private GameObject _support_Fl_Distance_A;
    private GameObject _support_Fl_Distance_B;
    private GameObject _support_FR_Distance_A;
    private GameObject _support_FR_Distance_B;
    private GameObject _support_Bl_Distance_A;
    private GameObject _support_Bl_Distance_B;
    private GameObject _support_BR_Distance_A;
    private GameObject _support_BR_Distance_B;
    private GameObject _support_FL_Distance_Up;
    private GameObject _support_FR_Distance_Up;
    private GameObject _support_BL_Distance_Up;
    private GameObject _support_BR_Distance_Up;
    private float dis_FL = 0f;
    private float dis_FR = 0f;
    private float dis_BL = 0f;
    private float dis_BR = 0f;
    private float dis_FL_Up = 0f;
    private float dis_FR_Up = 0f;
    private float dis_BL_Up = 0f;
    private float dis_BR_Up = 0f;

    void Start()
    {
        mScriptLift_S = gameObject.GetComponent<UIPanelCrane_L>();
        mScriptCar_S = gameObject.GetComponent<CraneCarController_L>();
        //Check Distance Support Forward Back
        //FL
        GameObject support_Fl_Distance_A = new GameObject("FL Dis_A");
        _support_Fl_Distance_A = support_Fl_Distance_A;
        _support_Fl_Distance_A.transform.localPosition = support_Lift_FL.position;
        _support_Fl_Distance_A.transform.SetParent(supportArrow_FL_A);
        GameObject support_Fl_Distance_B = new GameObject("FL Dis_B");
        _support_Fl_Distance_B = support_Fl_Distance_B;
        _support_Fl_Distance_B.transform.localPosition = support_Lift_FL.position;
        _support_Fl_Distance_B.transform.SetParent(transform);
        //FR
        GameObject support_FR_Distance_A = new GameObject("FR Dis_A");
        _support_FR_Distance_A = support_FR_Distance_A;
        _support_FR_Distance_A.transform.localPosition = support_Lift_FR.position;
        _support_FR_Distance_A.transform.SetParent(supportArrow_FR_A);
        GameObject support_FR_Distance_B = new GameObject("FR Dis_B");
        _support_FR_Distance_B = support_FR_Distance_B;
        _support_FR_Distance_B.transform.localPosition = support_Lift_FR.position;
        _support_FR_Distance_B.transform.SetParent(transform);
        //BL
        GameObject support_BL_Distance_A = new GameObject("BL Dis_A");
        _support_Bl_Distance_A = support_BL_Distance_A;
        _support_Bl_Distance_A.transform.localPosition = support_Lift_BL.position;
        _support_Bl_Distance_A.transform.SetParent(supportArrow_BL_A);
        GameObject support_BL_Distance_B = new GameObject("BL Dis_B");
        _support_Bl_Distance_B = support_BL_Distance_B;
        _support_Bl_Distance_B.transform.localPosition = support_Lift_BL.position;
        _support_Bl_Distance_B.transform.SetParent(transform);
        //BR
        GameObject support_BR_Distance_A = new GameObject("BR Dis_A");
        _support_BR_Distance_A = support_BR_Distance_A;
        _support_BR_Distance_A.transform.localPosition = support_Lift_BR.position;
        _support_BR_Distance_A.transform.SetParent(supportArrow_BR_A);
        GameObject support_BR_Distance_B = new GameObject("BR Dis_B");
        _support_BR_Distance_B = support_BR_Distance_B;
        _support_BR_Distance_B.transform.localPosition = support_Lift_BR.position;
        _support_BR_Distance_B.transform.SetParent(transform);
        //Check Distance Support Up Down
        //FL
        GameObject support_FL_Up = new GameObject("FL_Up");
        _support_FL_Distance_Up = support_FL_Up;
        _support_FL_Distance_Up.transform.localPosition = support_Lift_FL.position;
        _support_FL_Distance_Up.transform.SetParent(support_Lift_FL);
        //FR
        GameObject support_FR_Up = new GameObject("FR_Up");
        _support_FR_Distance_Up = support_FR_Up;
        _support_FR_Distance_Up.transform.localPosition = support_Lift_FR.position;
        _support_FR_Distance_Up.transform.SetParent(support_Lift_FR);
        //BL
        GameObject support_BL_Up = new GameObject("BL_Up");
        _support_BL_Distance_Up = support_BL_Up;
        _support_BL_Distance_Up.transform.localPosition = support_Lift_BL.position;
        _support_BL_Distance_Up.transform.SetParent(support_Lift_BL);
        //FR
        GameObject support_BR_Up = new GameObject("BR_Up");
        _support_BR_Distance_Up = support_BR_Up;
        _support_BR_Distance_Up.transform.localPosition = support_Lift_BR.position;
        _support_BR_Distance_Up.transform.SetParent(support_Lift_BR);
        //FL A
        FL_Support_A_Min = new Vector3(FL_Support_A_Min_,supportArrow_FL_A.localPosition.y, supportArrow_FL_A.localPosition.z);
        FL_Support_A_Max = new Vector3(FL_Support_A_Max_, supportArrow_FL_A.localPosition.y, supportArrow_FL_A.localPosition.z);
        //FL B
        FL_Support_B_Min = new Vector3(FL_Support_B_Min_, supportArrow_FL_B.localPosition.y, supportArrow_FL_B.localPosition.z);
        FL_Support_B_Max = new Vector3(FL_Support_B_Max_, supportArrow_FL_B.localPosition.y, supportArrow_FL_B.localPosition.z);
        //FR A
        FR_Support_A_Min = new Vector3(FR_Support_A_Min_, supportArrow_FR_A.localPosition.y, supportArrow_FR_A.localPosition.z);
        FR_Support_A_Max = new Vector3(FR_Support_A_Max_, supportArrow_FR_A.localPosition.y, supportArrow_FR_A.localPosition.z);
        //FR B
        FR_Support_B_Min = new Vector3(FR_Support_B_Min_, supportArrow_FR_B.localPosition.y, supportArrow_FR_B.localPosition.z);
        FR_Support_B_Max = new Vector3(FR_Support_B_Max_, supportArrow_FR_B.localPosition.y, supportArrow_FR_B.localPosition.z);
        //BL A
        BL_Support_A_Min = new Vector3(BL_Support_A_Min_, supportArrow_BL_A.localPosition.y, supportArrow_BL_A.localPosition.z);
        BL_Support_A_Max = new Vector3(BL_Support_A_Max_, supportArrow_BL_A.localPosition.y, supportArrow_BL_A.localPosition.z);
        //BL B
        BL_Support_B_Min = new Vector3(BL_Support_B_Min_, supportArrow_BL_B.localPosition.y, supportArrow_BL_B.localPosition.z);
        BL_Support_B_Max = new Vector3(BL_Support_B_Max_, supportArrow_BL_B.localPosition.y, supportArrow_BL_B.localPosition.z);
        //BR A
        BR_Support_A_Min = new Vector3(BR_Support_A_Min_, supportArrow_BR_A.localPosition.y, supportArrow_BR_A.localPosition.z);
        BR_Support_A_Max = new Vector3(BR_Support_A_Max_, supportArrow_BR_A.localPosition.y, supportArrow_BR_A.localPosition.z);
        //BR B
        BR_Support_B_Min = new Vector3(BR_Support_B_Min_, supportArrow_BR_B.localPosition.y, supportArrow_BR_B.localPosition.z);
        BR_Support_B_Max = new Vector3(BR_Support_B_Max_, supportArrow_BR_B.localPosition.y, supportArrow_BR_B.localPosition.z);
        //FL Lift
        FL_Support_Lift_Min = new Vector3(support_Lift_FL.localPosition.x, FL_Support_Lift_Min_, support_Lift_FL.localPosition.z);
        FL_Support_Lift_Max = new Vector3(support_Lift_FL.localPosition.x, FL_Support_Lift_Max_, support_Lift_FL.localPosition.z);
        //FR Lift
        FR_Support_Lift_Min = new Vector3(support_Lift_FR.localPosition.x, FR_Support_Lift_Min_, support_Lift_FR.localPosition.z);
        FR_Support_Lift_Max = new Vector3(support_Lift_FR.localPosition.x, FR_Support_Lift_Max_, support_Lift_FR.localPosition.z);
        //BL Lift
        BL_Support_Lift_Min = new Vector3(support_Lift_BL.localPosition.x, BL_Support_Lift_Min_, support_Lift_BL.localPosition.z);
        BL_Support_Lift_Max = new Vector3(support_Lift_BL.localPosition.x, BL_Support_Lift_Max_, support_Lift_BL.localPosition.z);
        //BR Lift
        BR_Support_Lift_Min = new Vector3(support_Lift_BR.localPosition.x, BR_Support_Lift_Min_, support_Lift_BR.localPosition.z);
        BR_Support_Lift_Max = new Vector3(support_Lift_BR.localPosition.x, BR_Support_Lift_Max_, support_Lift_BR.localPosition.z);
    }
    void Update()
    {
        if (mScriptLift_S._toggleMenuCrane == 0 && mScriptCar_S.startCrane_Bool == false)
        {
            //FL Support
            if (Input.GetKeyDown(supportFL_Key))
            {
                mScriptLift_S.SupportImageFL();
            }
            //FR Support
            if (Input.GetKeyDown(supportFR_Key))
            {
                mScriptLift_S.SupportImageFR();
            }
            //BL Support
            if (Input.GetKeyDown(supportBL_Key))
            {
                mScriptLift_S.SupportImageBL();
            }
            //BR Support
            if (Input.GetKeyDown(supportBR_Key))
            {
                mScriptLift_S.SupportImageBR();
            }
            SupportForward();
            SupportDown();
        }
    }
    public void SupportForward()
    {
        if (Input.GetKey(ab_Key_For_Back)&& Input.GetKey(a_Key))
        {
            if(mScriptLift_S.icon_G4FL_Bool == false)
            {
                supportArrow_FL_B.localPosition = Vector3.MoveTowards(supportArrow_FL_B.localPosition, FL_Support_B_Max, _speed * Time.deltaTime);
                supportArrow_FL_A.localPosition = Vector3.MoveTowards(supportArrow_FL_A.localPosition, FL_Support_A_Max, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4FR_Bool == false)
            {
                supportArrow_FR_B.localPosition = Vector3.MoveTowards(supportArrow_FR_B.localPosition, FR_Support_B_Max, _speed * Time.deltaTime);
                supportArrow_FR_A.localPosition = Vector3.MoveTowards(supportArrow_FR_A.localPosition, FR_Support_A_Max, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4BL_Bool == false)
            {
                supportArrow_BL_B.localPosition = Vector3.MoveTowards(supportArrow_BL_B.localPosition, BL_Support_B_Max, _speed * Time.deltaTime);
                supportArrow_BL_A.localPosition = Vector3.MoveTowards(supportArrow_BL_A.localPosition, BL_Support_A_Max, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4BR_Bool == false)
            {
                supportArrow_BR_B.localPosition = Vector3.MoveTowards(supportArrow_BR_B.localPosition, BR_Support_B_Max, _speed * Time.deltaTime);
                supportArrow_BR_A.localPosition = Vector3.MoveTowards(supportArrow_BR_A.localPosition, BR_Support_A_Max, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4FL_Bool == false || mScriptLift_S.icon_G4FR_Bool == false || mScriptLift_S.icon_G4BL_Bool == false || mScriptLift_S.icon_G4BR_Bool == false)
            {
                SoundPitchCrane();
                mScriptLift_S.icon_G3.color = new Color32(255, 214, 0, 255);
            }
            CheckDistanceSupport();
        }
        else if(Input.GetKey(ab_Key_For_Back)&& Input.GetKey(b_Key))
        {
            if (mScriptLift_S.icon_G4FL_Bool == false)
            {
                supportArrow_FL_B.localPosition = Vector3.MoveTowards(supportArrow_FL_B.localPosition, FL_Support_B_Min, _speed * Time.deltaTime);
                supportArrow_FL_A.localPosition = Vector3.MoveTowards(supportArrow_FL_A.localPosition, FL_Support_A_Min, _speed * Time.deltaTime);
        }
            if (mScriptLift_S.icon_G4FR_Bool == false)
            {
                supportArrow_FR_B.localPosition = Vector3.MoveTowards(supportArrow_FR_B.localPosition, FR_Support_B_Min, _speed * Time.deltaTime);
                supportArrow_FR_A.localPosition = Vector3.MoveTowards(supportArrow_FR_A.localPosition, FR_Support_A_Min, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4BL_Bool == false)
            {
                supportArrow_BL_B.localPosition = Vector3.MoveTowards(supportArrow_BL_B.localPosition, BL_Support_B_Min, _speed * Time.deltaTime);
                supportArrow_BL_A.localPosition = Vector3.MoveTowards(supportArrow_BL_A.localPosition, BL_Support_A_Min, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4BR_Bool == false)
            {
                supportArrow_BR_B.localPosition = Vector3.MoveTowards(supportArrow_BR_B.localPosition, BR_Support_B_Min, _speed * Time.deltaTime);
                supportArrow_BR_A.localPosition = Vector3.MoveTowards(supportArrow_BR_A.localPosition, BR_Support_A_Min, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4FL_Bool == false || mScriptLift_S.icon_G4FR_Bool == false || mScriptLift_S.icon_G4BL_Bool == false || mScriptLift_S.icon_G4BR_Bool == false)
            {
                SoundPitchCrane();
                mScriptLift_S.icon_G3.color = new Color32(255, 214, 0, 255);
            }
            CheckDistanceSupport();
        }
        else
            mScriptLift_S.icon_G3.color = new Color32(255, 255, 255, 29);
    }
    public void SupportDown()
    {
        if (Input.GetKey(ab_Key_Up_Down) && Input.GetKey(a_Key))
        {
            if (mScriptLift_S.icon_G4FL_Bool == false)
            {
                support_Lift_FL.localPosition = Vector3.MoveTowards(support_Lift_FL.localPosition, FL_Support_Lift_Max, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4FR_Bool == false)
            {
                support_Lift_FR.localPosition = Vector3.MoveTowards(support_Lift_FR.localPosition, FR_Support_Lift_Max, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4BL_Bool == false)
            {
                support_Lift_BL.localPosition = Vector3.MoveTowards(support_Lift_BL.localPosition, BL_Support_Lift_Max, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4BR_Bool == false)
            {
                support_Lift_BR.localPosition = Vector3.MoveTowards(support_Lift_BR.localPosition, BR_Support_Lift_Max, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4FL_Bool == false || mScriptLift_S.icon_G4FR_Bool == false || mScriptLift_S.icon_G4BL_Bool == false || mScriptLift_S.icon_G4BR_Bool == false)
            {
                SoundPitchCrane();
                mScriptLift_S.icon_G2.color = new Color32(255, 214, 0, 255);
            }
            CheckDistanceSupport();
        }
        else if (Input.GetKey(ab_Key_Up_Down) && Input.GetKey(b_Key))
        {
            if (mScriptLift_S.icon_G4FL_Bool == false)
            {
                support_Lift_FL.localPosition = Vector3.MoveTowards(support_Lift_FL.localPosition, FL_Support_Lift_Min, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4FR_Bool == false)
            {
                support_Lift_FR.localPosition = Vector3.MoveTowards(support_Lift_FR.localPosition, FR_Support_Lift_Min, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4BL_Bool == false)
            {
                support_Lift_BL.localPosition = Vector3.MoveTowards(support_Lift_BL.localPosition, BL_Support_Lift_Min, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4BR_Bool == false)
            {
                support_Lift_BR.localPosition = Vector3.MoveTowards(support_Lift_BR.localPosition, BR_Support_Lift_Min, _speed * Time.deltaTime);
            }
            if (mScriptLift_S.icon_G4FL_Bool == false || mScriptLift_S.icon_G4FR_Bool == false || mScriptLift_S.icon_G4BL_Bool == false || mScriptLift_S.icon_G4BR_Bool == false)
            {
                SoundPitchCrane();
                mScriptLift_S.icon_G2.color = new Color32(255, 214, 0, 255);
            }
            CheckDistanceSupport();
        }
        else
            mScriptLift_S.icon_G2.color = new Color32(255, 255, 255, 29);
    }
    public void SoundPitchCrane()
    {
        mScriptCar_S.soundEngine.pitch = Mathf.Lerp(mScriptCar_S.soundEngine.pitch,pitchSupport,Time.deltaTime * 0.8f);
    }
    public void CheckDistanceSupport()
    {
        //Forward Back
        dis_FL = Vector3.Distance(_support_Fl_Distance_A.transform.position, _support_Fl_Distance_B.transform.position);
        dis_FR = Vector3.Distance(_support_FR_Distance_A.transform.position, _support_FR_Distance_B.transform.position);
        dis_BL = Vector3.Distance(_support_Bl_Distance_A.transform.position, _support_Bl_Distance_B.transform.position);
        dis_BR = Vector3.Distance(_support_BR_Distance_A.transform.position, _support_BR_Distance_B.transform.position);
        //Up Down
        dis_FL_Up = Vector3.Distance(_support_FL_Distance_Up.transform.position, _support_Fl_Distance_A.transform.position);
        dis_FR_Up = Vector3.Distance(_support_FR_Distance_Up.transform.position, _support_FR_Distance_A.transform.position);
        dis_BL_Up = Vector3.Distance(_support_BL_Distance_Up.transform.position, _support_Bl_Distance_A.transform.position);
        dis_BR_Up = Vector3.Distance(_support_BR_Distance_Up.transform.position, _support_BR_Distance_A.transform.position);
        SupportPanelInterest();
    }
    private void SupportPanelInterest()
    {
        // Support Text Forward Back
        //FL
        float _FL = Mathf.InverseLerp(0f, 100.0f, dis_FL);
       float FL = _FL * 2575.0f;
        mScriptLift_S.text_FL_A.text = Mathf.RoundToInt(FL).ToString();
        //FR
        float _FR = Mathf.InverseLerp(0f, 100.0f, dis_FR);
        float FR = _FR * 2575.0f;
        mScriptLift_S.text_FR_A.text = Mathf.RoundToInt(FR).ToString();
        //BL
        float _BL = Mathf.InverseLerp(0f, 100.0f, dis_BL);
        float BL = _BL * 2575.0f;
        mScriptLift_S.text_BL_A.text = Mathf.RoundToInt(BL).ToString();
        //BR
        float _BR = Mathf.InverseLerp(0f, 100.0f, dis_BR);
        float BR = _BR * 2575.0f;
        mScriptLift_S.text_BR_A.text = Mathf.RoundToInt(BR).ToString();
        // Support Text Up Down
        //FL
        float _FL_lift = Mathf.InverseLerp(0f, 100.0f, dis_FL_Up);
        float FL_lift = _FL_lift * 9790.0f;
        mScriptLift_S.text_FL_B.text = Mathf.RoundToInt(FL_lift).ToString();
        //FR
        float _FR_lift = Mathf.InverseLerp(0f, 100.0f, dis_FR_Up);
        float FR_lift = _FR_lift * 9790.0f;
        mScriptLift_S.text_FR_B.text = Mathf.RoundToInt(FR_lift).ToString();
        //BL
        float _BL_lift = Mathf.InverseLerp(0f, 100.0f, dis_BL_Up);
        float BL_lift = _BL_lift * 9790.0f;
        mScriptLift_S.text_BL_B.text = Mathf.RoundToInt(BL_lift).ToString();
        //BR
        float _BR_lift = Mathf.InverseLerp(0f, 100.0f, dis_BR_Up);
        float BR_lift = _BR_lift * 9790.0f;
        mScriptLift_S.text_BR_B.text = Mathf.RoundToInt(BR_lift).ToString();

    }
}
