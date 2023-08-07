using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMarkerElement : MonoBehaviour
{
    private TowerAnimationLift scriptLift;
    private TowerPlatformInstallation scriptTPL;
    private CraneBigHook_L scriptBigH;
    private CraneSmallHook_L scriptSmallH;
    private TowerCounterweightPlatform scriptTCP;
    [Tooltip("Individual object number. Determines which object should be installed where. Do not change this value.")]
    public int numberElement = 0;
    [HideInInspector]
    public Transform platformCrane;
    private Transform pointInstanceElement;
    [HideInInspector]
    public Transform elementCrane;
    [Tooltip("The point from where fires a beam to search for the Elements of the Crane to set the current Element. Not used on some objects.")]
    public Transform pointRaycast;
    [Tooltip("Details of elements are added if they are available for their manipulation")]
    public GameObject[] detali;
    // Counterweight___________________________________________________________
    private Vector3 pointCounterweight_0;
    private Vector3 pointCounterweight_1;
    private Vector3 pointCounterweight_2;
    private Vector3 pointCounterweight_3;
    private Vector3 pointCounterweight_4;
    [Tooltip("Specify a value only for Handrail elements.Checks which object is available")]
    public string descriptionElementHandrail;
    [Tooltip("Specify a value only for Arrow elements.Checks which object is available")]
    public string descriptionElementArrow;
    [HideInInspector]
    public bool blockDisconnectArrow_Center = true;
    private Vector3 pointCableArrow_Forward_0;
    private Vector3 pointCableArrow_Forward_1;
    private Vector3 pointCableArrow_Forward_2_3_4;
    private Vector3 pointLineBoomCart;
    private Vector3 hookStartPosition;

    void Awake()
    {
        scriptTPL = FindObjectOfType<TowerPlatformInstallation>();
        if (scriptTPL == null)
        {
            Debug.Log("Add Gameobject Tower Electronic Platform");
        }
    }
    void Start()
    {
        scriptBigH = scriptTPL.scriptBigHook;
        scriptSmallH = scriptTPL.scriptSmallHook;
        scriptLift = scriptTPL.scriptAnimLift;
        platformCrane = scriptTPL.transform;
        pointCounterweight_0 = new Vector3(9.760001f, 1.818f, -0.01100159f);
        pointCounterweight_1 = new Vector3(10.23f, 1.818f, -0.01100159f);
        pointCounterweight_2 = new Vector3(10.711f, 1.818f, -0.01100159f);
        pointCounterweight_3 = new Vector3(11.178f, 1.818f, -0.01100159f);
        pointCounterweight_4 = new Vector3(11.654f, 1.818f, -0.01100159f);
        if (gameObject.GetComponent<TowerCounterweightPlatform>() != null)
        {
            scriptTCP = gameObject.GetComponent<TowerCounterweightPlatform>();
        }
        pointInstanceElement = scriptTPL.platformCraneBig;
        pointCableArrow_Forward_0 = new Vector3(-0.485f, -0.3884716f, 0.001347844f);
        pointCableArrow_Forward_1 = new Vector3(-0.4903f, -0.3884716f, 0.001347844f);
        pointCableArrow_Forward_2_3_4 = new Vector3(-0.646f, -0.089f, 0.001f);
        pointLineBoomCart = new Vector3(-1.7751f, 0.3662f, -0.0032f);
        hookStartPosition = new Vector3(-0.00399971f, -1.041f, -0.001f);
        if (numberElement == 12 && descriptionElementArrow == "Back")
        {
            GameObject g = new GameObject("PointCheckDis_BoomCartUI");
            g.transform.SetParent(transform);
            g.transform.localRotation = Quaternion.Euler(0, 0, 0);
            g.transform.localPosition = new Vector3(2.71f, -1.222f, -0.001999046f);
            scriptTPL.scriptTCC.pointCheckDistanceUI = g.transform;
        }
        if (numberElement == 1)
        {
            //Collider 1
            detali[1].transform.localPosition = new Vector3(1.761f,0, 1.769f);
            detali[1].transform.localRotation = Quaternion.Euler(54.02f,45f,0f);
            detali[1].GetComponent<BoxCollider>().center = new Vector3(0.007770963f, 0.03844542f, 0.1f);
            detali[1].GetComponent<BoxCollider>().size = new Vector3(0.05f, 0.2044041f, 3.04f);
            //Collider 2
            detali[2].transform.localPosition = new Vector3(1.761f, 0, -1.762001f);
            detali[2].transform.localRotation = Quaternion.Euler(-55.38f, -45f,0f);
            detali[2].GetComponent<BoxCollider>().center = new Vector3(0.01f, 0.01f, -0.03f);
            detali[2].GetComponent<BoxCollider>().size = new Vector3(0.08f, 0.2160432f, 3.04f);
            //Collider 3
            detali[3].transform.localPosition = new Vector3(-1.772462f, 0, 1.774567f);
            detali[3].transform.localRotation = Quaternion.Euler(-55.38f, 134.1f, 0f);
            detali[3].GetComponent<BoxCollider>().center = new Vector3(0f, 0.02f, -0.07f);
            detali[3].GetComponent<BoxCollider>().size = new Vector3(0.08f, 0.2160432f, 3.04f);
            //Collider 4
            detali[4].transform.localPosition = new Vector3(-1.717f, 0, -1.756001f);
            detali[4].transform.localRotation = Quaternion.Euler(54.02f, -135.9f, 0f);
            detali[4].GetComponent<BoxCollider>().center = new Vector3(0.03f, 0.08f, 0.07f);
            detali[4].GetComponent<BoxCollider>().size = new Vector3(0.08f, 0.2160432f, 3.04f);
        }
    }
    void Update()
    {
        if (gameObject.TryGetComponent(typeof(HingeJoint), out Component con))
        {

            if (transform.parent == platformCrane)
            {
                transform.parent = null;
                if (numberElement == 1)
                {
                    detali[0].SetActive(false);
                    platformCrane.gameObject.layer = 10;
                    if (scriptTPL.autoInstallCounterweightDownPlatform == true)
                    {
                        for (int i = 0; i < scriptTPL.counterweightDown.Length; i++)
                        {
                            scriptTPL.counterweightDown[i].gameObject.SetActive(false);
                        }
                    }
                }
                if (numberElement == 2)
                {
                    detali[0].SetActive(false);
                    if (scriptTPL.numberElement_RackType_B > 0)
                    {
                        scriptTPL.numberElement_RackType_B -= 1;
                    }
                    scriptTPL.numberElementAll -= 1;
                    scriptTPL.scriptSBV.towerCrane.GetComponent<TowerCamera>().maxDownCamera += 5;
                }
                if (numberElement == 3)
                {
                    if (scriptTPL.leftCounterweightDown > 0 && elementCrane.GetComponent<TowerCounterweightPlatform>().checkPoint == 1)
                    {
                        scriptTPL.leftCounterweightDown -= 1;
                    }
                    if (scriptTPL.rightCounterweightDown > 0 && elementCrane.GetComponent<TowerCounterweightPlatform>().checkPoint == 2)
                    {
                        scriptTPL.rightCounterweightDown -= 1;
                    }
                    if (scriptTPL.forwardCounterweightDown > 0 && elementCrane.GetComponent<TowerCounterweightPlatform>().checkPoint == 3)
                    {
                        scriptTPL.forwardCounterweightDown -= 1;
                    }
                    if (scriptTPL.backCounterweightDown > 0 && elementCrane.GetComponent<TowerCounterweightPlatform>().checkPoint == 4)
                    {
                        scriptTPL.backCounterweightDown -= 1;
                    }
                    CheckBlockElement_Rack_TypeA();
                }
                if (numberElement == 4)
                {
                    detali[0].SetActive(false);
                    scriptTPL.numberElement_RackType_B -= 2;
                    scriptTPL.numberElementAll -= 2;
                    scriptLift.blockAnimationOpenCapture = false;
                    scriptLift.elementType_BDown.gameObject.layer = 2;
                    scriptLift.elementType_BUp.gameObject.layer = 2;
                    scriptLift.elementType_BUp.SetParent(transform);
                    scriptLift.elementType_BDown.SetParent(transform);
                    scriptLift.elementType_BUp.localPosition = scriptLift.startPointElementUp;
                    scriptLift.elementType_BDown.localPosition = scriptLift.startPointElementDown;
                    scriptLift.elementType_BUp.localRotation = Quaternion.Euler(0, 0, 0);
                    scriptLift.elementType_BDown.localRotation = Quaternion.Euler(0, 0, 0);
                    scriptTPL.scriptSBV.towerCrane.GetComponent<TowerCamera>().maxDownCamera += 10;
                    if (scriptTPL.autoInstallHandrail == true)
                    {
                        for (int i = 0; i < scriptTPL.handrailLiftCrane.Length; i++)
                        {
                            scriptTPL.handrailLiftCrane[i].gameObject.SetActive(false);
                        }
                    }
                    else
                        detali[1].SetActive(false);
                }
                if (numberElement == 5 && scriptTPL.blockConnectionElement == true)
                {
                    detali[2].SetActive(false);
                    scriptTPL.blockConnectionArrow_Back = false;
                    transform.parent = null;
                }
            }
            if (numberElement == 2 && transform.parent == scriptLift.liftCrane)
            {
                scriptTPL.numberElement_Rack_Type_B_UpElementLift -= 1;
                transform.parent = null;
                scriptTPL.numberElementAll -= 1;
            }
            if (numberElement == 5 && transform.parent == scriptLift.liftCrane && scriptTPL.blockConnectionElement == true)
            {
                detali[2].SetActive(false);
                scriptTPL.blockConnectionArrow_Back = false;
                transform.parent = null;
            }
            if (transform.parent == scriptTPL.scriptTCC.rotationElementCrane)
            {
                if (numberElement == 6)
                {
                    detali[0].SetActive(false);
                    detali[1].SetActive(false);
                    detali[2].SetActive(false);
                    scriptTPL.checkSwivelPlatform_Block -= 1;
                    scriptTPL.scriptTCC.blockController_Int -= 1;
                    scriptTPL.blockCable_Bool = false;
                    DestroyCableArrow();
                    if (scriptTPL.numberArrowCenter_Int == 1)
                    {
                        Destroy(scriptTPL.cableArrowCenter.gameObject);
                        scriptTPL.cableArrowCenter = null;
                    }
                    if (scriptTPL.object_2 != null && scriptTPL.object_2.transform.parent == scriptTPL.scriptTCC.rotationElementCrane)
                    {
                        scriptTPL.object_2.detali[0].SetActive(false);
                    }
                    if (scriptTPL.blockArrowIfBoomCartActive == false)
                    {
                        scriptTPL.object_4.detali[0].SetActive(false);
                        scriptTPL.object_4.detali[2].SetActive(false);
                    }
                    ActivationUI_SwitchCrane();
                }
                if (numberElement == 7)
                {
                    detali[0].SetActive(false);
                    detali[1].SetActive(false);
                    detali[2].SetActive(false);
                    detali[3].GetComponent<BoxCollider>().enabled = false;
                    detali[4].SetActive(false);
                    elementCrane.GetComponent<BoxCollider>().size = new Vector3(10.80916f, 0.5192124f, 1.805371f);
                    elementCrane.GetComponent<BoxCollider>().center = new Vector3(-0.02967548f, -0.0648053f, 0);
                    scriptTPL.checkSwivelPlatform_Block -= 1;
                    if (scriptTPL.autoInstallCounterweightUpPlatform == true)
                    {
                        for (int i = 0; i < scriptTPL.counterweightUp.Length; i++)
                        {
                            scriptTPL.counterweightUp[i].gameObject.SetActive(false);
                        }
                    }
                }
                if (numberElement == 8)
                {
                    scriptTPL.object_0.detali[0].SetActive(false);
                    detali[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
                    detali[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
                    scriptTPL.checkSwivelPlatform_Block -= 1;
                    scriptTPL.scriptTCC.blockController_Int -= 1;
                    ActivationUI_SwitchCrane();
                }
                if (numberElement == 9)
                {
                    detali[1].SetActive(false);
                    detali[0].SetActive(false);
                    scriptTPL.checkCounterweightCrane_Block -= 1;
                    scriptTPL.scriptTCC.blockController_Int -= 1;
                    scriptTPL.blockLiftingWinch_Bool = false;
                    if (scriptTPL.object_1 != null && scriptTPL.object_1.transform.parent == scriptTPL.scriptTCC.rotationElementCrane)
                    {
                        scriptTPL.object_1.detali[1].SetActive(false);
                    }
                    if (scriptTPL.blockArrowIfBoomCartActive == false)
                    {
                        scriptTPL.object_4.detali[0].SetActive(false);
                        scriptTPL.object_4.detali[2].SetActive(false);
                    }
                    ActivationUI_SwitchCrane();
                }
                if (numberElement == 10)
                {
                    scriptTPL.numberCounterweight_Int -= 1;
                    if (scriptTPL.scriptTCC.counterweightUI_Int > 0)
                    {
                        scriptTPL.scriptTCC.counterweightUI_Int -= 20;
                        scriptTPL.scriptTCC.counterweightUI.text = scriptTPL.scriptTCC.counterweightUI_Int.ToString() + "%";
                    }
                }
                if (numberElement == 12)
                {
                    if (descriptionElementArrow == "Back")
                    {
                        scriptTPL.blockSwivelPlatform_IfArrowBack = true;
                        scriptTPL.blockConnectionArrow_Center = false;
                        detali[1].SetActive(true);
                        detali[2].GetComponent<BoxCollider>().enabled = false;
                        scriptTPL.object_0.detali[1].SetActive(false);
                        if (scriptTPL.blockLiftingWinch_Bool == true)
                        {
                            scriptTPL.object_2.detali[0].SetActive(false);
                        }
                    }
                    if (descriptionElementArrow == "Center")
                    {
                        detali[0].SetActive(false);
                        detali[1].SetActive(true);
                        detali[2].GetComponent<BoxCollider>().enabled = false;
                        if (scriptTPL.numberArrowCenter_Int == 4)
                        {
                            scriptTPL.object_3[2].blockDisconnectArrow_Center = true;
                            scriptTPL.arrowBoxCollider_Center4 = null;
                        }
                        else if (scriptTPL.numberArrowCenter_Int == 3)
                        {
                            scriptTPL.object_3[1].blockDisconnectArrow_Center = true;
                            Destroy(scriptTPL.cableArrowCenter.gameObject);
                            GameObject cable = Instantiate(Resources.Load("Prefab/Cable Arrow Center _0 Element") as GameObject);
                            scriptTPL.cableArrowCenter = cable.transform;
                            scriptTPL.cableArrowCenter.SetParent(scriptTPL.object_1.transform);
                            scriptTPL.cableArrowCenter.localRotation = Quaternion.Euler(0, 0, 0);
                            scriptTPL.cableArrowCenter.localPosition = pointCableArrow_Forward_0;
                            scriptTPL.arrowBoxCollider_Center3 = null;
                        }
                        else if (scriptTPL.numberArrowCenter_Int == 2)
                        {
                            scriptTPL.object_3[0].blockDisconnectArrow_Center = true;
                            scriptTPL.arrowBoxCollider_Center2 = null;
                        }
                        if (scriptTPL.numberArrowCenter_Int == 1)
                        {
                            Destroy(scriptTPL.cableArrowCenter.gameObject);
                            scriptTPL.cableArrowCenter = null;
                            scriptTPL.arrowBoxCollider_Center1 = null;
                        }
                        scriptTPL.numberArrowCenter_Int -= 1;
                    }
                    if (descriptionElementArrow == "Forward")
                    {
                        detali[0].SetActive(false);
                        detali[1].SetActive(true);
                        detali[2].GetComponent<BoxCollider>().enabled = false;
                        if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().blockDisconnectArrow_Center == false)
                        {
                            scriptTPL.previousElement.GetComponent<TowerMarkerElement>().blockDisconnectArrow_Center = true;
                        }
                        scriptTPL.blockConnectionArrow_Forward = false;
                        DestroyCableArrow();
                    }
                }
                if (numberElement == 13) // Нужно дополниль блокировку снятия элемента если на нем весит груз
                {
                    detali[0].SetActive(false);
                    detali[1].SetActive(false);
                    detali[2].SetActive(false);
                    Destroy(scriptTPL.pointCableBoomCart.gameObject);
                    scriptTPL.blockArrowIfBoomCartActive = true;
                    elementCrane.GetComponent<BoxCollider>().center = new Vector3(-0.01f, -0.5146831f, 0);
                    elementCrane.GetComponent<BoxCollider>().size = new Vector3(1.991649f, 1.048412f, 1.996078f);
                    scriptTPL.scriptTCC.CreateCableHook();
                    detali[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
                    detali[0].transform.localPosition = hookStartPosition;
                    ActivationUI_SwitchCrane();
                }
                transform.parent = null;
            }
            if (numberElement == 11 && transform.parent == scriptLift.liftCrane)
            {
                transform.parent = null;
                scriptTPL.numberHandrail_Int -= 1;
            }
            if (scriptTPL.blockMarker_Bool == true)
            {
                InstanceElement();
            }
        }
    }
    public void InstanceElement()
    {
        if (Input.GetKeyDown(scriptTPL.installCrane))
        {
            if (numberElement == 1 && scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 0)
            {
                Action();
                elementCrane.SetParent(platformCrane);
                elementCrane.localPosition = new Vector3(pointInstanceElement.localPosition.x, scriptTPL.distanceInstallElementHit + 3.966648f, pointInstanceElement.localPosition.z);
                elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                detali[0].SetActive(true);
                if (scriptTPL.autoInstallCounterweightDownPlatform == true)
                {
                    for (int i = 0; i < scriptTPL.counterweightDown.Length; i++)
                    {
                        scriptTPL.counterweightDown[i].gameObject.SetActive(true);
                    }
                }
                platformCrane.gameObject.layer = 2;
            } // Element Rack TypeOf A
            if (numberElement == 2)
            {
                if (scriptTPL.numberElementAll < scriptLift.maxElementType_B)
                {
                    if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 1 && scriptTPL.previousElement.transform.parent == platformCrane)
                    {
                        Action();
                        detali[0].SetActive(true);
                        scriptTPL.numberElement_RackType_B += 1;
                        elementCrane.SetParent(platformCrane);
                        elementCrane.localPosition = new Vector3(pointInstanceElement.localPosition.x, scriptTPL.distanceInstallElementHit + 3.706914f, pointInstanceElement.localPosition.z);
                        elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                        scriptTPL.numberElementAll += 1;
                        scriptTPL.scriptSBV.towerCrane.GetComponent<TowerCamera>().maxDownCamera -= 5;

                    }
                    if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 2)
                    {
                        if (scriptTPL.previousElement.transform.parent == scriptLift.liftCrane)
                        {
                            Action();
                            detali[0].SetActive(true);
                            scriptTPL.numberElement_Rack_Type_B_UpElementLift += 1;
                            elementCrane.SetParent(scriptTPL.elementParentLift);
                            elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                            elementCrane.localPosition = new Vector3(-0.02f, scriptTPL.previousElement.transform.localPosition.y + 3.693f, 0);
                            scriptTPL.numberElementAll += 1;
                            scriptTPL.scriptSBV.towerCrane.GetComponent<TowerCamera>().maxDownCamera -= 5;
                        }
                        if (scriptTPL.previousElement.transform.parent == platformCrane)
                        {
                            Action();
                            detali[0].SetActive(true);
                            scriptTPL.numberElement_RackType_B += 1;
                            elementCrane.SetParent(platformCrane);
                            elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                            elementCrane.localPosition = new Vector3(pointInstanceElement.localPosition.x, scriptTPL.distanceInstallElementHit + 3.693268f, pointInstanceElement.localPosition.z);
                            scriptTPL.numberElementAll += 1;
                            scriptTPL.scriptSBV.towerCrane.GetComponent<TowerCamera>().maxDownCamera -= 5;
                        }
                    }
                    if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 4 && scriptTPL.previousElement.transform.parent == platformCrane)
                    {
                        Action();
                        elementCrane.SetParent(scriptTPL.elementParentLift);
                        elementCrane.localPosition = new Vector3(-0.02f, scriptLift.liftCrane.localPosition.y + 3.702f, 0);
                        elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                        detali[0].SetActive(true);
                        scriptTPL.numberElement_Rack_Type_B_UpElementLift += 1;
                        scriptTPL.numberElementAll += 1;
                        scriptTPL.scriptSBV.towerCrane.GetComponent<TowerCamera>().maxDownCamera -= 5;
                    }
                }
            }// Element Rack TypeOf B
            if (numberElement == 3 && scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 0 || scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 3 && scriptTPL.autoInstallCounterweightDownPlatform == false)
            {
                if (elementCrane.GetComponent<TowerCounterweightPlatform>().checkPoint == 1 && scriptTPL.leftCounterweightDown < 7)
                {
                    Action();
                    elementCrane.SetParent(platformCrane);
                    elementCrane.localRotation = Quaternion.Euler(0, 180, 0);
                    scriptTPL.leftCounterweightDown += 1;
                    if (scriptTPL.leftCounterweightDown == 1)
                    {
                        elementCrane.localPosition = new Vector3(-0.106f, scriptTPL.distanceInstallElementHit + 1.132488f, -6.621f);
                    }
                    if (scriptTPL.leftCounterweightDown > 1)
                    {
                        elementCrane.localPosition = new Vector3(-0.106f, scriptTPL.distanceInstallElementHit + 0.3142636f, -6.621f);
                    }
                    CheckBlockElement_Rack_TypeA();
                }
                if (elementCrane.GetComponent<TowerCounterweightPlatform>().checkPoint == 2 && scriptTPL.rightCounterweightDown < 7)
                {
                    Action();
                    elementCrane.SetParent(platformCrane);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                    scriptTPL.rightCounterweightDown += 1;
                    if (scriptTPL.rightCounterweightDown == 1)
                    {
                        elementCrane.localPosition = new Vector3(3.608f, scriptTPL.distanceInstallElementHit + 1.132488f, -6.637f);
                    }
                    if (scriptTPL.rightCounterweightDown > 1)
                    {
                        elementCrane.localPosition = new Vector3(3.608f, scriptTPL.distanceInstallElementHit + 0.3142636f, -6.637f);
                    }
                    CheckBlockElement_Rack_TypeA();
                }
                if (elementCrane.GetComponent<TowerCounterweightPlatform>().checkPoint == 3 && scriptTPL.forwardCounterweightDown < 7)
                {
                    Action();
                    elementCrane.SetParent(platformCrane);
                    elementCrane.localRotation = Quaternion.Euler(0, -90, 0);
                    scriptTPL.forwardCounterweightDown += 1;
                    if (scriptTPL.forwardCounterweightDown == 1)
                    {
                        elementCrane.localPosition = new Vector3(1.746f, scriptTPL.distanceInstallElementHit + 1.132488f, -4.767f);
                    }
                    if (scriptTPL.forwardCounterweightDown > 1)
                    {
                        elementCrane.localPosition = new Vector3(1.746f, scriptTPL.distanceInstallElementHit + 0.3142636f, -4.767f);
                    }
                    CheckBlockElement_Rack_TypeA();
                }
                if (elementCrane.GetComponent<TowerCounterweightPlatform>().checkPoint == 4 && scriptTPL.backCounterweightDown < 7)
                {
                    Action();
                    elementCrane.SetParent(platformCrane);
                    elementCrane.localRotation = Quaternion.Euler(0, 90, 0);
                    scriptTPL.backCounterweightDown += 1;
                    if (scriptTPL.backCounterweightDown == 1)
                    {
                        elementCrane.localPosition = new Vector3(1.756f, scriptTPL.distanceInstallElementHit + 1.132488f, -8.483f);
                    }
                    if (scriptTPL.backCounterweightDown > 1)
                    {
                        elementCrane.localPosition = new Vector3(1.756f, scriptTPL.distanceInstallElementHit + 0.3142636f, -8.483f);
                    }
                    CheckBlockElement_Rack_TypeA();
                }
            }// Element Counterweight Platform Down
            if (numberElement == 4 && scriptTPL.previousElement.transform.parent == platformCrane)
            {
                Action();
                elementCrane.SetParent(platformCrane);
                if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 1)
                {
                    elementCrane.localPosition = new Vector3(1.775f, scriptTPL.distanceInstallElementHit + 8.022688f, pointInstanceElement.localPosition.z);
                }
                if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 2)
                {
                    elementCrane.localPosition = new Vector3(1.775f, scriptTPL.distanceInstallElementHit + 8.006361f, pointInstanceElement.localPosition.z);
                }
                elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                detali[0].SetActive(true);
                if (scriptTPL.autoInstallHandrail == true)
                {
                    for (int i = 0; i < scriptTPL.handrailLiftCrane.Length; i++)
                    {
                        scriptTPL.handrailLiftCrane[i].gameObject.SetActive(true);
                    }
                }
                else
                    detali[1].SetActive(true);
                scriptTPL.numberElement_RackType_B += 2;
                scriptTPL.numberElementAll += 2;
                scriptLift.blockAnimationOpenCapture = true;
                scriptLift.elementType_BUp.SetParent(platformCrane);
                scriptLift.elementType_BDown.SetParent(platformCrane);
                scriptTPL.elementParentLift = scriptLift.liftCrane;
                scriptTPL.scriptSBV.towerCrane.GetComponent<TowerCamera>().maxDownCamera -= 10;
            }// Element Rack TypeOf Lift
            if (numberElement == 5)
            {
                if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 1 && scriptTPL.previousElement.transform.parent == platformCrane)
                {
                    Action();
                    scriptTPL.blockConnectionArrow_Back = true;
                    scriptTPL.object_0 = elementCrane.GetComponent<TowerMarkerElement>();
                    detali[2].SetActive(true);
                    elementCrane.SetParent(platformCrane);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                    elementCrane.localPosition = new Vector3(pointInstanceElement.localPosition.x, scriptTPL.distanceInstallElementHit + 2.62938f, pointInstanceElement.localPosition.z);
                }
                if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 2)
                {
                    if (scriptTPL.previousElement.transform.parent == scriptLift.liftCrane)
                    {
                        Action();
                        scriptTPL.blockConnectionArrow_Back = true;
                        scriptTPL.object_0 = elementCrane.GetComponent<TowerMarkerElement>();
                        detali[2].SetActive(true);
                        elementCrane.SetParent(scriptTPL.elementParentLift);
                        elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                        elementCrane.localPosition = new Vector3(-0.025f, scriptTPL.previousElement.transform.localPosition.y + 2.610005f, 0);
                    }
                    if (scriptTPL.previousElement.transform.parent == platformCrane)
                    {
                        Action();
                        scriptTPL.blockConnectionArrow_Back = true;
                        scriptTPL.object_0 = elementCrane.GetComponent<TowerMarkerElement>();
                        detali[2].SetActive(true);
                        elementCrane.SetParent(platformCrane);
                        elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                        elementCrane.localPosition = new Vector3(pointInstanceElement.localPosition.x, scriptTPL.distanceInstallElementHit + 2.617293f, pointInstanceElement.localPosition.z);
                    }
                }
                if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 4 && scriptTPL.previousElement.transform.parent == platformCrane)
                {
                    Action();
                    scriptTPL.blockConnectionArrow_Back = true;
                    scriptTPL.object_0 = elementCrane.GetComponent<TowerMarkerElement>();
                    detali[2].SetActive(true);
                    elementCrane.SetParent(scriptTPL.elementParentLift);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                    elementCrane.localPosition = new Vector3(-0.025f, scriptTPL.elementParentLift.localPosition.y + 2.629f, 0);
                }
            }// Element Swivel Platform_Down
            if (numberElement == 6 && scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 5)
            {
                if (scriptTPL.previousElement.transform.parent == platformCrane || scriptLift.liftCrane)
                {
                    Action();
                    elementCrane.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                    elementCrane.localPosition = new Vector3(0.082f, scriptTPL.scriptTCC.rotationElementCrane.localPosition.y + 8.145315f, 0);
                    detali[0].SetActive(true);
                    detali[2].SetActive(true);
                    scriptTPL.checkSwivelPlatform_Block += 1;
                    scriptTPL.scriptTCC.blockController_Int += 1;
                    scriptTPL.blockCable_Bool = true;
                    scriptTPL.object_1 = elementCrane.GetComponent<TowerMarkerElement>();
                    AddCableArrowForward();
                    if (scriptTPL.numberArrowCenter_Int == 1)
                    {
                        AddCableArrowCenter();
                    }
                    if (scriptTPL.object_2 != null && scriptTPL.object_2.transform.parent == scriptTPL.scriptTCC.rotationElementCrane)
                    {
                        detali[1].SetActive(true);
                    }
                    if (scriptTPL.blockConnectionArrow_Center == true)
                    {
                        scriptTPL.object_2.detali[0].SetActive(true);
                    }
                    if (scriptTPL.blockArrowIfBoomCartActive == false)
                    {
                        scriptTPL.object_4.detali[0].SetActive(true);
                        scriptTPL.object_4.detali[2].SetActive(true);
                    }
                    ActivationUI_SwitchCrane();
                }
            }// Element Counterweight Crane_Rack
            if (numberElement == 7 && scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 5)
            {
                if (scriptTPL.previousElement.transform.parent == platformCrane || scriptLift.liftCrane)
                {
                    Action();
                    detali[0].SetActive(true);
                    detali[1].SetActive(true);
                    detali[2].SetActive(true);
                    detali[3].GetComponent<BoxCollider>().enabled = true;
                    detali[4].SetActive(true);
                    elementCrane.GetComponent<BoxCollider>().size = new Vector3(7.84165f, 0.5192124f, 1.805371f);
                    elementCrane.GetComponent<BoxCollider>().center = new Vector3(-1.41716f, -0.0648053f, 0);
                    elementCrane.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                    elementCrane.localPosition = new Vector3(6.71f, 1.095f, -0.006f);
                    scriptTPL.checkSwivelPlatform_Block += 1;
                    if (scriptTPL.autoInstallCounterweightUpPlatform == true)
                    {
                        for (int i = 0; i < scriptTPL.counterweightUp.Length; i++)
                        {
                            scriptTPL.counterweightUp[i].gameObject.SetActive(true);
                        }
                    }
                }
            }// Element Counterweight Crane
            if (numberElement == 8 && scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 5)
            {
                if (scriptTPL.previousElement.transform.parent == platformCrane || scriptLift.liftCrane)
                {
                    Action();
                    scriptTPL.object_0.detali[0].SetActive(true);
                    elementCrane.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                    elementCrane.localPosition = new Vector3(0.021f, 1.591f, 2.233f);
                    detali[0].transform.localRotation = Quaternion.Euler(90, 0, 0);
                    detali[1].transform.localRotation = Quaternion.Euler(-144.934f, 0, 0);
                    scriptTPL.checkSwivelPlatform_Block += 1;
                    scriptTPL.scriptTCC.blockController_Int += 1;
                    ActivationUI_SwitchCrane();
                }
            } // Element Tower Cabin
            if (numberElement == 9 && scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 7 && scriptTPL.previousElement.transform.parent == scriptTPL.scriptTCC.rotationElementCrane)
            {
                Action();
                scriptTPL.object_2 = elementCrane.GetComponent<TowerMarkerElement>();
                detali[1].SetActive(true);
                elementCrane.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
                elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                elementCrane.localPosition = new Vector3(7.617f, 2.161f, 0.023f);
                scriptTPL.checkCounterweightCrane_Block += 1;
                scriptTPL.scriptTCC.blockController_Int += 1;
                scriptTPL.blockLiftingWinch_Bool = true;
                if (scriptTPL.object_1 != null && scriptTPL.object_1.transform.parent == scriptTPL.scriptTCC.rotationElementCrane)
                {
                    scriptTPL.object_1.detali[1].SetActive(true);
                }
                if (scriptTPL.blockConnectionArrow_Center == true)
                {
                    detali[0].SetActive(true);
                }
                if (scriptTPL.blockArrowIfBoomCartActive == false)
                {
                    scriptTPL.object_4.detali[0].SetActive(true);
                    scriptTPL.object_4.detali[2].SetActive(true);
                }
                ActivationUI_SwitchCrane();
            }// Element Lifting Winch
            if (numberElement == 10 && scriptTPL.blockInstallCounterweight == true && scriptTPL.numberCounterweight_Int < 5 && scriptTPL.autoInstallCounterweightUpPlatform == false)
            {
                Action();
                elementCrane.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
                elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                scriptTPL.numberCounterweight_Int += 1;
                if (scriptTPL.scriptTCC.counterweightUI_Int < 100)
                {
                    scriptTPL.scriptTCC.counterweightUI_Int += 20;
                    scriptTPL.scriptTCC.counterweightUI.text = scriptTPL.scriptTCC.counterweightUI_Int.ToString() + "%";
                }
                if (scriptTPL.numberCounterweight_Int == 1)
                {
                    elementCrane.localPosition = pointCounterweight_0;
                }
                else if (scriptTPL.numberCounterweight_Int == 2)
                {
                    elementCrane.localPosition = pointCounterweight_1;
                }
                else if (scriptTPL.numberCounterweight_Int == 3)
                {
                    elementCrane.localPosition = pointCounterweight_2;
                }
                else if (scriptTPL.numberCounterweight_Int == 4)
                {
                    elementCrane.localPosition = pointCounterweight_3;
                }
                else if (scriptTPL.numberCounterweight_Int == 5)
                {
                    elementCrane.localPosition = pointCounterweight_4;
                }
            } // Counterweight
            if (numberElement == 11 && scriptTPL.blockHandrail_bool == true && scriptTPL.numberHandrail_Int < 6 && scriptTPL.autoInstallHandrail == false)
            {
                Action();
                scriptTPL.numberHandrail_Int += 1;
                elementCrane.SetParent(scriptLift.liftCrane);
                if (descriptionElementHandrail == "Up_L")
                {
                    elementCrane.localPosition = new Vector3(-0.399f, -0.994f, -2.034f);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                }
                if (descriptionElementHandrail == "Up_R")
                {
                    elementCrane.localPosition = new Vector3(-0.401f, -0.994f, 2.039f);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                }
                if (descriptionElementHandrail == "Up_C")
                {
                    elementCrane.localPosition = new Vector3(1.821f, -0.994f, -0.002f);
                    elementCrane.localRotation = Quaternion.Euler(0, 90, 0);
                }
                if (descriptionElementHandrail == "Down_L")
                {
                    elementCrane.localPosition = new Vector3(0.128f, -4.775f, -2.037f);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                }
                if (descriptionElementHandrail == "Down_R")
                {
                    elementCrane.localPosition = new Vector3(0.116f, -4.775f, 2.023f);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                }
                if (descriptionElementHandrail == "Down_C")
                {
                    elementCrane.localPosition = new Vector3(1.812f, -4.775f, -0.004f);
                    elementCrane.localRotation = Quaternion.Euler(0, 90, 0);
                }
            } // Element Handrail
            if (numberElement == 12)
            {

                if (descriptionElementArrow == "Back" && scriptTPL.blockConnectionArrow_Back == true && scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 5)
                {
                    Action();
                    elementCrane.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
                    elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                    elementCrane.localPosition = new Vector3(-5.493f, 1.889f, 0.002f);
                    scriptTPL.blockSwivelPlatform_IfArrowBack = false;
                    scriptTPL.blockConnectionArrow_Center = true;
                    scriptTPL.object_0.detali[1].SetActive(true);
                    detali[1].SetActive(false);
                    detali[2].GetComponent<BoxCollider>().enabled = true;
                    scriptTPL.scriptTCC.pointMovingBack = detali[3].transform;
                    scriptTPL.arrowBoxCollider_Back = elementCrane.GetComponent<BoxCollider>();
                    if (scriptTPL.blockLiftingWinch_Bool == true)
                    {
                        scriptTPL.object_2.detali[0].SetActive(true);
                    }
                    scriptTPL.scriptTCC.pointLineArrow_BoomCart = detali[4].transform;
                }
                if (descriptionElementArrow == "Center" && scriptTPL.blockConnectionArrow_Center == true && scriptTPL.previousElement.GetComponent<TowerMarkerElement>().numberElement == 12 && scriptTPL.numberArrowCenter_Int < 4 && scriptTPL.previousElement.transform.parent == scriptTPL.scriptTCC.rotationElementCrane)
                {
                    if (scriptTPL.numberArrowCenter_Int == 1 && scriptTPL.blockCable_Bool == false)
                    {
                        scriptTPL.blockConnectionArrow_CenterNext = false;
                    } else
                        scriptTPL.blockConnectionArrow_CenterNext = true;
                    if (scriptTPL.blockConnectionArrow_CenterNext == true)
                    {
                        Action();
                        elementCrane.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
                        elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                        elementCrane.localPosition = new Vector3(scriptTPL.previousElement.transform.localPosition.x + -7.9859f, scriptTPL.previousElement.transform.localPosition.y, 0);
                        scriptTPL.numberArrowCenter_Int += 1;
                        detali[0].SetActive(true);
                        detali[1].SetActive(false);
                        detali[2].GetComponent<BoxCollider>().enabled = true;
                    }
                    if (scriptTPL.numberArrowCenter_Int == 1)
                    {
                        scriptTPL.object_3[0] = this.gameObject.GetComponent<TowerMarkerElement>();
                        AddCableArrowCenter();
                        scriptTPL.arrowBoxCollider_Center1 = elementCrane.GetComponent<BoxCollider>();
                    }
                    else if (scriptTPL.numberArrowCenter_Int == 2)
                    {
                        scriptTPL.object_3[1] = this.gameObject.GetComponent<TowerMarkerElement>();
                        scriptTPL.object_3[0].blockDisconnectArrow_Center = false;
                        scriptTPL.arrowBoxCollider_Center2 = elementCrane.GetComponent<BoxCollider>();
                    }
                    else if (scriptTPL.numberArrowCenter_Int == 3)
                    {
                        scriptTPL.object_3[2] = this.gameObject.GetComponent<TowerMarkerElement>();
                        scriptTPL.object_3[1].blockDisconnectArrow_Center = false;
                        AddCableArrowCenter();
                        scriptTPL.arrowBoxCollider_Center3 = elementCrane.GetComponent<BoxCollider>();
                    }
                    else if (scriptTPL.numberArrowCenter_Int == 4)
                    {
                        scriptTPL.object_3[3] = this.gameObject.GetComponent<TowerMarkerElement>();
                        scriptTPL.object_3[2].blockDisconnectArrow_Center = false;
                        scriptTPL.arrowBoxCollider_Center4 = elementCrane.GetComponent<BoxCollider>();
                    }
                }
                if (descriptionElementArrow == "Forward" && scriptTPL.previousElement.transform.parent == scriptTPL.scriptTCC.rotationElementCrane)
                {
                    if (scriptTPL.numberArrowCenter_Int == 1 && scriptTPL.blockCable_Bool == false)
                    {
                        scriptTPL.blockConnectionArrow_ForwardNext = false;
                    }
                    else scriptTPL.blockConnectionArrow_ForwardNext = true;
                    if (scriptTPL.blockConnectionArrow_ForwardNext == true)
                    {
                        Action();
                        detali[0].SetActive(true);
                        detali[1].SetActive(false);
                        detali[2].GetComponent<BoxCollider>().enabled = true;
                        scriptTPL.scriptTCC.pointMovingForward = detali[3].transform;
                        elementCrane.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
                        elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                        scriptTPL.blockConnectionArrow_Forward = true;
                        AddCableArrowForward();
                        scriptTPL.arrowBoxCollider_Forward = elementCrane.GetComponent<BoxCollider>();
                        if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().descriptionElementArrow == "Back")
                        {
                            elementCrane.localPosition = new Vector3(scriptTPL.previousElement.transform.localPosition.x - 7.927001f, 1.8873f, 0);
                        }
                        if (scriptTPL.previousElement.GetComponent<TowerMarkerElement>().descriptionElementArrow == "Center")
                        {
                            elementCrane.localPosition = new Vector3(scriptTPL.previousElement.transform.localPosition.x - 7.902101f, 1.8873f, 0);
                            scriptTPL.previousElement.GetComponent<TowerMarkerElement>().blockDisconnectArrow_Center = false;
                        }
                    }
                }
            }
            if (numberElement == 13 && scriptTPL.blockBoomCartTrigger_bool == true && scriptTPL.blockConnectionArrow_Forward == true)
            {
                Action();
                elementCrane.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
                elementCrane.localRotation = Quaternion.Euler(0, 0, 0);
                elementCrane.localPosition = new Vector3(scriptTPL.elementArrowCheck.GetComponentInParent<TowerMarkerElement>().transform.localPosition.x, 0.667f, 0);
                if (scriptTPL.blockLiftingWinch_Bool == true && scriptTPL.blockCable_Bool == true)
                {
                    detali[0].SetActive(true);
                    detali[2].SetActive(true);
                }
                detali[1].SetActive(true);
                scriptTPL.blockArrowIfBoomCartActive = false;
                elementCrane.GetComponent<BoxCollider>().center = new Vector3(-0.01000006f, -0.7924314f, -0.06966439f);
                elementCrane.GetComponent<BoxCollider>().size = new Vector3(1.991657f, 0.4929007f, 4.229908f);
                scriptTPL.arrowBoxCollider_Forward.enabled = false;
                scriptTPL.arrowBoxCollider_Back.enabled = false;
                AddCableArrowBoomCart();
                if (scriptTPL.arrowBoxCollider_Center1 != null)
                {
                    scriptTPL.arrowBoxCollider_Center1.enabled = false;
                }
                if (scriptTPL.arrowBoxCollider_Center2 != null)
                {
                    scriptTPL.arrowBoxCollider_Center2.enabled = false;
                }
                if (scriptTPL.arrowBoxCollider_Center3 != null)
                {
                    scriptTPL.arrowBoxCollider_Center3.enabled = false;
                }
                if (scriptTPL.arrowBoxCollider_Center4 != null)
                {
                    scriptTPL.arrowBoxCollider_Center4.enabled = false;
                }
                scriptTPL.object_4 = elementCrane.GetComponent<TowerMarkerElement>();
                scriptTPL.scriptTCC.boomCart_Detal0 = detali[3].transform;
                scriptTPL.scriptTCC.pointLineHook0 = detali[8].transform;
                scriptTPL.scriptTCC.pointLineHook1 = detali[4].transform;
                scriptTPL.scriptTCC.pointLineHook2 = detali[5].transform;
                scriptTPL.scriptTCC.pointLineHook3 = detali[6].transform;
                scriptTPL.scriptTCC.pointLineHook4 = detali[7].transform;
                scriptTPL.scriptTCC.CreateCableHook();
                ActivationUI_SwitchCrane();
            }
        }
    }
    public void AddCableArrowForward()
    {
        if (scriptTPL.blockConnectionArrow_Forward == true && scriptTPL.blockCable_Bool == true && scriptTPL.numberArrowCenter_Int == 0)
        {
            GameObject cable = Instantiate(Resources.Load("Prefab/Cable Arrow Forward _0 Element") as GameObject);
            scriptTPL.cableArrow_Foward = cable.transform;
            scriptTPL.cableArrow_Foward.SetParent(scriptTPL.object_1.transform);
            scriptTPL.cableArrow_Foward.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.cableArrow_Foward.localPosition = pointCableArrow_Forward_0;
        }
        if (scriptTPL.blockConnectionArrow_Forward == true && scriptTPL.blockCable_Bool == true && scriptTPL.numberArrowCenter_Int == 1)
        {
            GameObject cable = Instantiate(Resources.Load("Prefab/Cable Arrow Center(Forward 1)_3 Element") as GameObject);
            scriptTPL.cableArrow_Foward = cable.transform;
            scriptTPL.cableArrow_Foward.SetParent(scriptTPL.object_1.transform);
            scriptTPL.cableArrow_Foward.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.cableArrow_Foward.localPosition = pointCableArrow_Forward_1;
        }
        if (scriptTPL.blockConnectionArrow_Forward == true && scriptTPL.blockCable_Bool == true && scriptTPL.numberArrowCenter_Int == 2)
        {
            GameObject cable = Instantiate(Resources.Load("Prefab/Cable Arrow Forward_2  Element") as GameObject);
            scriptTPL.cableArrow_Foward = cable.transform;
            scriptTPL.cableArrow_Foward.SetParent(scriptTPL.object_1.transform);
            scriptTPL.cableArrow_Foward.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.cableArrow_Foward.localPosition = pointCableArrow_Forward_2_3_4;
        }
        if (scriptTPL.blockConnectionArrow_Forward == true && scriptTPL.blockCable_Bool == true && scriptTPL.numberArrowCenter_Int == 3)
        {
            GameObject cable = Instantiate(Resources.Load("Prefab/Cable Arrow Forward_3 Element") as GameObject);
            scriptTPL.cableArrow_Foward = cable.transform;
            scriptTPL.cableArrow_Foward.SetParent(scriptTPL.object_1.transform);
            scriptTPL.cableArrow_Foward.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.cableArrow_Foward.localPosition = pointCableArrow_Forward_2_3_4;
        }
        if (scriptTPL.blockConnectionArrow_Forward == true && scriptTPL.blockCable_Bool == true && scriptTPL.numberArrowCenter_Int == 4)
        {
            GameObject cable = Instantiate(Resources.Load("Prefab/Cable Arrow Forward_4 Element") as GameObject);
            scriptTPL.cableArrow_Foward = cable.transform;
            scriptTPL.cableArrow_Foward.SetParent(scriptTPL.object_1.transform);
            scriptTPL.cableArrow_Foward.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.cableArrow_Foward.localPosition = pointCableArrow_Forward_2_3_4;
        }
    }
    public void AddCableArrowCenter()
    {
        if (scriptTPL.numberArrowCenter_Int == 1 && scriptTPL.blockCable_Bool == true && scriptTPL.cableArrowCenter == null)
        {
            GameObject cable = Instantiate(Resources.Load("Prefab/Cable Arrow Center _0 Element") as GameObject);
            scriptTPL.cableArrowCenter = cable.transform;
            scriptTPL.cableArrowCenter.SetParent(scriptTPL.object_1.transform);
            scriptTPL.cableArrowCenter.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.cableArrowCenter.localPosition = pointCableArrow_Forward_0;
        }
        if (scriptTPL.numberArrowCenter_Int == 3)
        {
            Destroy(scriptTPL.cableArrowCenter.gameObject);
            GameObject cable = Instantiate(Resources.Load("Prefab/Cable Arrow Center(Forward 1)_3 Element") as GameObject);
            scriptTPL.cableArrowCenter = cable.transform;
            scriptTPL.cableArrowCenter.SetParent(scriptTPL.object_1.transform);
            scriptTPL.cableArrowCenter.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.cableArrowCenter.localPosition = pointCableArrow_Forward_1;
        }
    }
    private void AddCableArrowBoomCart()
    {
        if (scriptTPL.numberArrowCenter_Int == 0)
        {
            GameObject cableBoomCart = Instantiate(Resources.Load("Prefab/Cable Arrow BoomCart 0")) as GameObject;
            scriptTPL.pointCableBoomCart = cableBoomCart.transform;
            scriptTPL.pointCableBoomCart.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
            scriptTPL.pointCableBoomCart.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.pointCableBoomCart.localPosition = pointLineBoomCart;
        }
        if (scriptTPL.numberArrowCenter_Int == 1)
        {
            GameObject cableBoomCart = Instantiate(Resources.Load("Prefab/Cable Arrow BoomCart 1")) as GameObject;
            scriptTPL.pointCableBoomCart = cableBoomCart.transform;
            scriptTPL.pointCableBoomCart.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
            scriptTPL.pointCableBoomCart.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.pointCableBoomCart.localPosition = pointLineBoomCart;
        }
        if (scriptTPL.numberArrowCenter_Int == 2)
        {
            GameObject cableBoomCart = Instantiate(Resources.Load("Prefab/Cable Arrow BoomCart 2")) as GameObject;
            scriptTPL.pointCableBoomCart = cableBoomCart.transform;
            scriptTPL.pointCableBoomCart.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
            scriptTPL.pointCableBoomCart.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.pointCableBoomCart.localPosition = pointLineBoomCart;
        }
        if (scriptTPL.numberArrowCenter_Int == 3)
        {
            GameObject cableBoomCart = Instantiate(Resources.Load("Prefab/Cable Arrow BoomCart 3")) as GameObject;
            scriptTPL.pointCableBoomCart = cableBoomCart.transform;
            scriptTPL.pointCableBoomCart.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
            scriptTPL.pointCableBoomCart.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.pointCableBoomCart.localPosition = pointLineBoomCart;
        }
        if (scriptTPL.numberArrowCenter_Int == 4)
        {
            GameObject cableBoomCart = Instantiate(Resources.Load("Prefab/Cable Arrow BoomCart 4")) as GameObject;
            scriptTPL.pointCableBoomCart = cableBoomCart.transform;
            scriptTPL.pointCableBoomCart.SetParent(scriptTPL.scriptTCC.rotationElementCrane);
            scriptTPL.pointCableBoomCart.localRotation = Quaternion.Euler(0, 0, 0);
            scriptTPL.pointCableBoomCart.localPosition = pointLineBoomCart;
        }
    }
    public void DestroyCableArrow()
    {
        if (scriptTPL.blockConnectionArrow_Forward == false || scriptTPL.blockCable_Bool == false)
        {
            if (scriptTPL.cableArrow_Foward != null)
            {
                Destroy(scriptTPL.cableArrow_Foward.gameObject);
                scriptTPL.cableArrow_Foward = null;
            }
        }
    }
    private void ActivationUI_SwitchCrane()
    {
        if (scriptTPL.scriptTCC.blockController_Int == 3 && scriptTPL.blockArrowIfBoomCartActive == false)
        {
            scriptTPL.scriptSBV.window.enabled = true;
            scriptTPL.scriptSBV.description.enabled = true;
            scriptTPL.scriptSBV._key.enabled = true;
        }
        else
        {
            scriptTPL.scriptSBV.window.enabled = false;
            scriptTPL.scriptSBV.description.enabled = false;
            scriptTPL.scriptSBV._key.enabled = false;
        }
    }
    public void Action()
    {
        if (scriptBigH.connected_Bool == false)
        {
            scriptBigH.ConnectedCargo();
            scriptBigH.connected_Bool = true;
        }
        if (scriptSmallH.connected_Bool == false)
        {
            scriptSmallH.ConnectedCargo();
            scriptSmallH.connected_Bool = true;
        }
        if (scriptTPL.scriptTCC.connectedCargo_Bool == false)
        {
            scriptTPL.scriptTCC.ConnectedCargo();
            DestroyImmediate(scriptLift.elementCrane_Capture.GetComponent<ConstantForce>());
            DestroyImmediate(scriptLift.elementCrane_Capture.GetComponent<Rigidbody>());
            scriptTPL.scriptTCC.connectedCargo_Bool = true;
        }
        if (scriptTPL.blockMarker_Bool == true)
        {
            DestroyImmediate(elementCrane.GetComponent<ConstantForce>());
            DestroyImmediate(elementCrane.GetComponent<Rigidbody>());
        }
        else
        {
            DestroyImmediate(scriptLift.elementCrane_Capture.GetComponent<ConstantForce>());
            DestroyImmediate(scriptLift.elementCrane_Capture.GetComponent<Rigidbody>());
        }
    }
    private void CheckBlockElement_Rack_TypeA()
    {
        if(scriptTPL.leftCounterweightDown > 0 || scriptTPL.rightCounterweightDown > 0 || scriptTPL.forwardCounterweightDown > 0 || scriptTPL.backCounterweightDown > 0)
        {
            scriptTPL.blockElement_Rack_TypeA = false;
        }
        if (scriptTPL.leftCounterweightDown == 0 && scriptTPL.rightCounterweightDown == 0 && scriptTPL.forwardCounterweightDown == 0 && scriptTPL.backCounterweightDown == 0)
        {
            scriptTPL.blockElement_Rack_TypeA = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CaptureTowerCrane" && scriptTPL.numberElementAll < scriptLift.maxElementType_B)
        {
            scriptTPL.windowKey.enabled = true;
            scriptTPL.textKey.text = scriptLift.loadElement.ToString();
            scriptTPL.description.text = scriptLift.descriptionInstal;
            scriptLift.elementCrane_Capture = transform;
            scriptLift.blockLoadElement_Bool = true;
        }
        if (other.tag == "CounterweightUp")
        {
            scriptTPL.windowKey.enabled = true;
            scriptTPL.textKey.text = scriptTPL.installCrane.ToString();
            scriptTPL.description.text = scriptTPL.actionDescription[1];
            scriptTPL.blockInstallCounterweight = true;
        }
        if (other.tag == "HandrailLiftCrane")
        {
            scriptTPL.windowKey.enabled = true;
            scriptTPL.textKey.text = scriptTPL.installCrane.ToString();
            scriptTPL.description.text = scriptTPL.actionDescription[1];
            scriptTPL.blockHandrail_bool = true;
        }
        if (other.tag == "BoomCartArrow" && numberElement == 13 && scriptTPL.blockArrowIfBoomCartActive == true)
        {
            scriptTPL.windowKey.enabled = true;
            scriptTPL.textKey.text = scriptTPL.installCrane.ToString();
            scriptTPL.description.text = scriptTPL.actionDescription[1];
            scriptTPL.blockBoomCartTrigger_bool = true;
            scriptTPL.elementArrowCheck = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CaptureTowerCrane")
        {
            scriptTPL.windowKey.enabled = false;
            scriptTPL.textKey.text = "";
            scriptTPL.description.text = "";
            scriptLift.elementCrane_Capture = null;
            scriptLift.blockLoadElement_Bool = false;
        }
        if (other.tag == "CounterweightUp")
        {
            scriptTPL.windowKey.enabled = false;
            scriptTPL.textKey.text = "";
            scriptTPL.description.text = "";
            scriptTPL.blockInstallCounterweight = false;
        }
        if (other.tag == "HandrailLiftCrane")
        {
            scriptTPL.windowKey.enabled = false;
            scriptTPL.textKey.text = "";
            scriptTPL.description.text = "";
            scriptTPL.blockHandrail_bool = false;
        }
        if (other.tag == "BoomCartArrow" && numberElement == 13)
        {
            scriptTPL.windowKey.enabled = false;
            scriptTPL.textKey.text = "";
            scriptTPL.description.text = "";
            scriptTPL.blockBoomCartTrigger_bool = false;
            scriptTPL.EnambleBoxColliderArrow();
        }
    }

}

