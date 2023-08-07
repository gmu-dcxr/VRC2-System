using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerPlatformInstallation : MonoBehaviour
{
    [HideInInspector]
    public SwitchingBetweenVehicles scriptSBV;
    [Tooltip("Script  'CraneBigHook'")]
    public CraneBigHook_L scriptBigHook;
    [Tooltip("Script  'CraneSmallHook'")]
    public CraneSmallHook_L scriptSmallHook;
    public TowerAnimationLift scriptAnimLift;
    [HideInInspector]
    public TowerControllerCrane scriptTCC;
    public Material redMaterial;
    public Material greenMaterial;
    private bool blockMaterilRay = true;
    private bool blockMaterilTrigger = true;
    private int _materialSwitching = 0;
    private MeshRenderer rendererDecayBig;
    private MeshRenderer rendererDecaySmall;
    public Transform decayPlatformBig;
    private Vector3 startPositionDecayBig;
    private Vector3 startPositionDecaySmall;
    public Transform decayPlatformSmall;
    public Transform platformCraneBig;
    public Transform platformCraneSmall;
    public KeyCode includingDecal_Key = KeyCode.P;
    private bool includingDecal_Bool = true;
    public KeyCode installCrane = KeyCode.O;
    private bool installCrane_Bool = false;
    [Tooltip("The distance of the object from the ground. If it is more than this number, the object will be highlighted in red.")]
    public float checkDistance = 0.48f;
    public string[] actionDescription;
    public Image windowKey;
    public Text textKey;
    public Text description;
    public GameObject towerPaneController;
    [HideInInspector]
    public float distanceInstallElementHit = 0;
    [HideInInspector]
    public GameObject previousElement;
    [HideInInspector]
    public Transform elementParentLift;
    [HideInInspector]
    public int numberElement_RackType_B = 0;
    [HideInInspector]
    public int numberElement_Rack_Type_B_UpElementLift = 0;
    [HideInInspector]
    public int numberElementAll = 0;
    [HideInInspector]
    public int leftCounterweightDown = 0;
    [HideInInspector]
    public int rightCounterweightDown = 0;
    [HideInInspector]
    public int forwardCounterweightDown = 0;
    [HideInInspector]
    public int backCounterweightDown = 0;
    [HideInInspector]
    public bool blockElement_Rack_TypeA = true;
    [Tooltip("Automatically install crane counterweight down platform")]
    [Space(10)]
    public bool autoInstallCounterweightDownPlatform = false;
    [Tooltip("Automatically install crane counterweight up platform")]
    [Space(10)]
    public bool autoInstallCounterweightUpPlatform = false;
    [Tooltip("Automatically install crane lift Handrail")]
    [Space(10)]
    public bool autoInstallHandrail = false;
    [HideInInspector]
    public Transform[] counterweightDown;
    [HideInInspector]
    public Transform[] counterweightUp;
    [HideInInspector]
    public Transform[] handrailLiftCrane;
    [HideInInspector]
    public bool blockMarker_Bool = false;
    [HideInInspector]
    public int checkSwivelPlatform_Block = 0;
    [HideInInspector]
    public int checkCounterweightCrane_Block = 0;
    [HideInInspector]
    public bool blockConnectionElement = true;
    [HideInInspector]
    public int numberCounterweight_Int = 0;
    [HideInInspector]
    public int numberHandrail_Int = 0;
    [HideInInspector]
    public int numberArrowCenter_Int = 0;
    [HideInInspector]
    public bool blockConnectionArrow_Forward = false;
    [HideInInspector]
    public bool blockConnectionArrow_Back = false;
    [HideInInspector]
    public bool blockCable_Bool = false;
    [HideInInspector]
    public bool blockSwivelPlatform_IfArrowBack = true;
    [HideInInspector]
    public bool blockConnectionArrow_Center = false;
    [HideInInspector]
    public bool blockConnectionArrow_CenterNext = true;
    [HideInInspector]
    public bool blockConnectionArrow_ForwardNext = true;
    [HideInInspector]
    public Transform elementArrowCheck;
    // Disables the collider on the boom if a dolly is installed
    [HideInInspector]
    public BoxCollider arrowBoxCollider_Back;
    [HideInInspector]
    public BoxCollider arrowBoxCollider_Forward;
    [HideInInspector]
    public BoxCollider arrowBoxCollider_Center1;
    [HideInInspector]
    public BoxCollider arrowBoxCollider_Center2;
    [HideInInspector]
    public BoxCollider arrowBoxCollider_Center3;
    [HideInInspector]
    public BoxCollider arrowBoxCollider_Center4;
    //__________________________________________________________
    [HideInInspector]
    public bool blockHandrail_bool = false;
    [HideInInspector]
    public bool blockInstallCounterweight = false;
    // Cable Point__________________________________________________________
    [HideInInspector]
    public Transform cableArrow_Foward;
    [HideInInspector]
    public Transform cableArrowCenter;
    [HideInInspector]
    public bool blockBoomCartTrigger_bool = false;
    [HideInInspector]
    public Transform pointCableBoomCart;
    [HideInInspector]
    public bool blockArrowIfBoomCartActive = true;
    [HideInInspector]
    public bool blockLiftingWinch_Bool = false;
    [HideInInspector]
    public TowerMarkerElement object_0; // = Swivel Platform_Down
    [HideInInspector]
    public TowerMarkerElement object_1; // = Counterweight Crane_Rack
    [HideInInspector]
    public TowerMarkerElement object_2; // = Lifting Winch
    [HideInInspector]
    public TowerMarkerElement[] object_3; // = Arrow
    [HideInInspector]
    public TowerMarkerElement object_4; // = Boom Cart

     void Awake()
    {
        scriptTCC = gameObject.GetComponent<TowerControllerCrane>();
    }
    void Start()
    {
        scriptBigHook.mSctiptTower = this.gameObject.GetComponent<TowerPlatformInstallation>();
        scriptSmallHook.mSctiptTower = this.gameObject.GetComponent<TowerPlatformInstallation>();
        scriptAnimLift.scriptTPL = gameObject.GetComponent<TowerPlatformInstallation>();
        scriptSBV = gameObject.GetComponent<SwitchingBetweenVehicles>();
        rendererDecayBig = decayPlatformBig.GetComponent<MeshRenderer>();
        rendererDecaySmall = decayPlatformSmall.GetComponent<MeshRenderer>();
        startPositionDecayBig = decayPlatformBig.localPosition;
        startPositionDecaySmall = decayPlatformSmall.localPosition;
        AutoInstallCounterweight();
        AutoInstallCounterweightUp();
        AutoInstallHandrail();
        object_3 = new TowerMarkerElement[4];
    }
    void Update()
    {
        if(gameObject.TryGetComponent(typeof(HingeJoint),out Component com))
        {
            IncludingDecal();
            if (!includingDecal_Bool)
            {
                decayPlatformBig.rotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.rotation.z);
                RaycastHit hit;
                if (Physics.Raycast(decayPlatformBig.position, Vector3.down * 100, out hit))
                {
                    if (checkDistance > hit.distance)
                    {
                        if (blockMaterilRay == true)
                        {
                            rendererDecayBig.material = greenMaterial;
                            rendererDecaySmall.material = greenMaterial;
                            _materialSwitching = 1;
                            blockMaterilRay = false;
                        }
                    }
                    else
                    {
                        rendererDecayBig.material = redMaterial;
                        rendererDecaySmall.material = redMaterial;
                        _materialSwitching = 0;
                        blockMaterilRay = true;
                    }
                }
                if (_materialSwitching == 1)
                {
                    windowKey.enabled = true;
                    textKey.text = installCrane.ToString();
                    description.text = actionDescription[1];
                }
                else
                {
                    windowKey.enabled = false;
                    textKey.text = "";
                    description.text = "";
                }
            }
            else if (includingDecal_Bool)
            {
                windowKey.enabled = true;
                textKey.text = includingDecal_Key.ToString();
                description.text = actionDescription[0].ToString();
            }
            if (installCrane_Bool == true)
            {
                platformCraneBig.gameObject.SetActive(false);
                platformCraneSmall.gameObject.SetActive(false);
                platformCraneBig.SetParent(transform);
                platformCraneSmall.SetParent(transform);
                decayPlatformBig.SetParent(transform);
                decayPlatformBig.localPosition = startPositionDecayBig;
                decayPlatformBig.localRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                decayPlatformSmall.SetParent(decayPlatformBig);
                decayPlatformSmall.localPosition = startPositionDecaySmall;
                decayPlatformSmall.localRotation = Quaternion.Euler(0,0,0);
                installCrane_Bool = false;
            }
            InstallCrane();
        }
    }
    private void IncludingDecal()
    {
        if (Input.GetKeyDown(includingDecal_Key) && includingDecal_Bool == true)
        {
            decayPlatformBig.gameObject.SetActive(true);
            decayPlatformSmall.gameObject.SetActive(true);
            windowKey.enabled = false;
            textKey.text = "";
            description.text = "";
            includingDecal_Bool = false;
        }
        else if(Input.GetKeyDown(includingDecal_Key) && includingDecal_Bool == false)
        {
            decayPlatformBig.gameObject.SetActive(false);
            decayPlatformSmall.gameObject.SetActive(false);
            includingDecal_Bool = true;
        }
    }
    public void InstallCrane()
    {
        if(Input.GetKeyDown(installCrane) && _materialSwitching == 1)
        {
            if (scriptBigHook.connected_Bool == false)
            {
                scriptBigHook.ConnectedCargo();
                scriptBigHook.connected_Bool = true;
            }
            if (scriptSmallHook.connected_Bool == false)
            {
                scriptSmallHook.ConnectedCargo();
                scriptSmallHook.connected_Bool = true;
            }
            DestroyImmediate(gameObject.GetComponent<ConstantForce>());
            DestroyImmediate(gameObject.GetComponent<Rigidbody>());
            decayPlatformBig.gameObject.SetActive(false);
            decayPlatformSmall.gameObject.SetActive(false);
            decayPlatformBig.SetParent(null);
            decayPlatformSmall.SetParent(null);
            platformCraneBig.SetParent(null);
            platformCraneBig.gameObject.SetActive(true);
            platformCraneSmall.gameObject.SetActive(true);
            platformCraneBig.position = new Vector3(decayPlatformBig.position.x, decayPlatformBig.position.y - 0.23f, decayPlatformBig.position.z);
            platformCraneBig.rotation = Quaternion.Euler(0, decayPlatformBig.eulerAngles.y, 0);
            transform.position = new Vector3(decayPlatformSmall.position.x, decayPlatformSmall.position.y + 0.2f, decayPlatformSmall.position.z);
            transform.rotation = Quaternion.Euler(0, decayPlatformSmall.eulerAngles.y, 0);
            platformCraneBig.SetParent(transform);
            windowKey.enabled = false;
            textKey.text = "";
            description.text = "";
            installCrane_Bool = true;
        }
    }
    public void UnhookCargo() // Performs actions if you unhook the load from the hook
    {
        platformCraneBig.gameObject.layer = 0;
        if (!includingDecal_Bool)
        {
            decayPlatformBig.gameObject.SetActive(false);
            decayPlatformSmall.gameObject.SetActive(false);
            includingDecal_Bool = true;
        }
        windowKey.enabled = false;
        textKey.text = "";
        description.text = "";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (blockMaterilTrigger == true)
        {
            _materialSwitching = 0;
            rendererDecayBig.material = redMaterial;
            rendererDecaySmall.material = redMaterial;
            blockMaterilTrigger = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        rendererDecayBig.material = greenMaterial;
        rendererDecaySmall.material = greenMaterial;
        _materialSwitching = 1;
        blockMaterilTrigger = true;
    }
    public void AutoInstallCounterweight()
    {
        if (autoInstallCounterweightDownPlatform == true)
        {
            counterweightDown = new Transform[28];
            GameObject L_counter0 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[0] = L_counter0.transform;
            GameObject L_counter1 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[1] = L_counter1.transform;
            GameObject L_counter2 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[2] = L_counter2.transform;
            GameObject L_counter3 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[3] = L_counter3.transform;
            GameObject L_counter4 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[4] = L_counter4.transform;
            GameObject L_counter5 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[5] = L_counter5.transform;
            GameObject L_counter6 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[6] = L_counter6.transform;
            GameObject R_counter0 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[7] = R_counter0.transform;
            GameObject R_counter1 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[8] = R_counter1.transform;
            GameObject R_counter2 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[9] = R_counter2.transform;
            GameObject R_counter3 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[10] = R_counter3.transform;
            GameObject R_counter4 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[11] = R_counter4.transform;
            GameObject R_counter5 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[12] = R_counter5.transform;
            GameObject R_counter6 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[13] = R_counter6.transform;
            GameObject F_counter0 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[14] = F_counter0.transform;
            GameObject F_counter1 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[15] = F_counter1.transform;
            GameObject F_counter2 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[16] = F_counter2.transform;
            GameObject F_counter3 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[17] = F_counter3.transform;
            GameObject F_counter4 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[18] = F_counter4.transform;
            GameObject F_counter5 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[19] = F_counter5.transform;
            GameObject F_counter6 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[20] = F_counter6.transform;
            GameObject B_counter0 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[21] = B_counter0.transform;
            GameObject B_counter1 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[22] = B_counter1.transform;
            GameObject B_counter2 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[23] = B_counter2.transform;
            GameObject B_counter3 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[24] = B_counter3.transform;
            GameObject B_counter4 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[25] = B_counter4.transform;
            GameObject B_counter5 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[26] = B_counter5.transform;
            GameObject B_counter6 = Instantiate(Resources.Load("Prefab/Tower Counterweight Platform_Auto") as GameObject);
            counterweightDown[27] = B_counter6.transform;
            for(int i = 0; i < counterweightDown.Length; i++)
            {
                counterweightDown[i].SetParent(transform);
                counterweightDown[i].gameObject.SetActive(false);
            }
            //Left
            counterweightDown[0].localPosition = new Vector3(-0.106f, -1.456618f, -6.621f);
            counterweightDown[1].localPosition = new Vector3(-0.106f, -1.142354f, -6.621f);
            counterweightDown[2].localPosition = new Vector3(-0.106f, -0.8280907f, -6.621f);
            counterweightDown[3].localPosition = new Vector3(-0.106f, -0.5138271f, -6.621f);
            counterweightDown[4].localPosition = new Vector3(-0.106f, -0.1995635f, -6.621f);
            counterweightDown[5].localPosition = new Vector3(-0.106f, 0.1147001f, -6.621f);
            counterweightDown[6].localPosition = new Vector3(-0.106f, 0.4289638f, -6.621f);
            counterweightDown[0].localRotation = Quaternion.Euler(0,180,0);
            counterweightDown[1].localRotation = Quaternion.Euler(0, 180, 0);
            counterweightDown[2].localRotation = Quaternion.Euler(0, 180, 0);
            counterweightDown[3].localRotation = Quaternion.Euler(0, 180, 0);
            counterweightDown[4].localRotation = Quaternion.Euler(0, 180, 0);
            counterweightDown[5].localRotation = Quaternion.Euler(0, 180, 0);
            counterweightDown[6].localRotation = Quaternion.Euler(0, 180, 0);
            //Right
            counterweightDown[7].localPosition = new Vector3(3.608f, -1.456618f, -6.637f);
            counterweightDown[8].localPosition = new Vector3(3.608f, -1.142354f, -6.637f);
            counterweightDown[9].localPosition = new Vector3(3.608f, -0.8280907f, -6.637f);
            counterweightDown[10].localPosition = new Vector3(3.608f, -0.5138271f, -6.637f);
            counterweightDown[11].localPosition = new Vector3(3.608f, -0.1995635f, -6.637f);
            counterweightDown[12].localPosition = new Vector3(3.608f, 0.1147001f, -6.637f);
            counterweightDown[13].localPosition = new Vector3(3.608f, 0.4289638f, -6.637f);
            counterweightDown[7].localRotation = Quaternion.Euler(0, 0, 0);
            counterweightDown[8].localRotation = Quaternion.Euler(0, 0, 0);
            counterweightDown[9].localRotation = Quaternion.Euler(0, 0, 0);
            counterweightDown[10].localRotation = Quaternion.Euler(0, 0, 0);
            counterweightDown[11].localRotation = Quaternion.Euler(0, 0, 0);
            counterweightDown[12].localRotation = Quaternion.Euler(0, 0, 0);
            counterweightDown[13].localRotation = Quaternion.Euler(0, 0, 0);
            //Forward
            counterweightDown[14].localPosition = new Vector3(1.746f, -1.456618f, -4.767f);
            counterweightDown[15].localPosition = new Vector3(1.746f, -1.142354f, -4.767f);
            counterweightDown[16].localPosition = new Vector3(1.746f, -0.8280907f, -4.767f);
            counterweightDown[17].localPosition = new Vector3(1.746f, -0.5138271f, -4.767f);
            counterweightDown[18].localPosition = new Vector3(1.746f, -0.1995635f, -4.767f);
            counterweightDown[19].localPosition = new Vector3(1.746f, 0.1147001f, -4.767f);
            counterweightDown[20].localPosition = new Vector3(1.746f, 0.4289638f, -4.767f);
            counterweightDown[14].localRotation = Quaternion.Euler(0, -90, 0);
            counterweightDown[15].localRotation = Quaternion.Euler(0, -90, 0);
            counterweightDown[16].localRotation = Quaternion.Euler(0, -90, 0);
            counterweightDown[17].localRotation = Quaternion.Euler(0, -90, 0);
            counterweightDown[18].localRotation = Quaternion.Euler(0, -90, 0);
            counterweightDown[19].localRotation = Quaternion.Euler(0, -90, 0);
            counterweightDown[20].localRotation = Quaternion.Euler(0, -90, 0);
            //Back
            counterweightDown[21].localPosition = new Vector3(1.756f, -1.456618f, -8.483f);
            counterweightDown[22].localPosition = new Vector3(1.756f, -1.142354f, -8.483f);
            counterweightDown[23].localPosition = new Vector3(1.756f, -0.8280907f, -8.483f);
            counterweightDown[24].localPosition = new Vector3(1.756f, -0.5138271f, -8.483f);
            counterweightDown[25].localPosition = new Vector3(1.756f, -0.1995635f, -8.483f);
            counterweightDown[26].localPosition = new Vector3(1.756f, 0.1147001f, -8.483f);
            counterweightDown[27].localPosition = new Vector3(1.756f, 0.4289638f, -8.483f);
            counterweightDown[21].localRotation = Quaternion.Euler(0, 90, 0);
            counterweightDown[22].localRotation = Quaternion.Euler(0, 90, 0);
            counterweightDown[23].localRotation = Quaternion.Euler(0, 90, 0);
            counterweightDown[24].localRotation = Quaternion.Euler(0, 90, 0);
            counterweightDown[25].localRotation = Quaternion.Euler(0, 90, 0);
            counterweightDown[26].localRotation = Quaternion.Euler(0, 90, 0);
            counterweightDown[27].localRotation = Quaternion.Euler(0, 90, 0);
            counterweightDown[0].gameObject.AddComponent<BoxCollider>();
            counterweightDown[0].GetComponent<BoxCollider>().center = new Vector3(-1.849527f, 0.7871721f, 0.003969672f);
            counterweightDown[0].GetComponent<BoxCollider>().size = new Vector3(5.299809f, 2.209512f, 5.25888f);
        }
    }
    public void AutoInstallCounterweightUp()
    {
        if(autoInstallCounterweightUpPlatform == true)
        {
            counterweightUp = new Transform[5];
            GameObject counter0Up = Instantiate(Resources.Load("Prefab/Counterweight Auto") as GameObject);
            counterweightUp[0] = counter0Up.transform;
            GameObject counter1Up = Instantiate(Resources.Load("Prefab/Counterweight Auto") as GameObject);
            counterweightUp[1] = counter1Up.transform;
            GameObject counter2Up = Instantiate(Resources.Load("Prefab/Counterweight Auto") as GameObject);
            counterweightUp[2] = counter2Up.transform;
            GameObject counter3Up = Instantiate(Resources.Load("Prefab/Counterweight Auto") as GameObject);
            counterweightUp[3] = counter3Up.transform;
            GameObject counter4Up = Instantiate(Resources.Load("Prefab/Counterweight Auto") as GameObject);
            counterweightUp[4] = counter4Up.transform;
            for (int i = 0; i < counterweightUp.Length; i++)
            {
                counterweightUp[i].SetParent(scriptTCC.rotationElementCrane);
                counterweightUp[i].localRotation = Quaternion.Euler(0, 0, 0);
                counterweightUp[i].gameObject.SetActive(false);
            }
            counterweightUp[0].localPosition = new Vector3(9.760001f, 1.818f, -0.01100159f);
            counterweightUp[1].localPosition = new Vector3(10.23f, 1.818f, -0.01100159f);
            counterweightUp[2].localPosition = new Vector3(10.711f, 1.818f, -0.01100159f);
            counterweightUp[3].localPosition = new Vector3(11.178f, 1.818f, -0.01100159f);
            counterweightUp[4].localPosition = new Vector3(11.654f, 1.818f, -0.01100159f);
            counterweightUp[0].gameObject.AddComponent<BoxCollider>();
            counterweightUp[0].GetComponent<BoxCollider>().center = new Vector3(0.9450623f, -1.495624f, 0.00377375f);
            counterweightUp[0].GetComponent<BoxCollider>().size = new Vector3(2.342577f, 2.988751f, 1.307086f);
        }
    }
    public void AutoInstallHandrail()
    {
        if(autoInstallHandrail == true)
        {
            handrailLiftCrane = new Transform[6];
            GameObject hanUp_L = Instantiate(Resources.Load("Prefab/Handrail_Up Big_Left_Auto") as GameObject);
            handrailLiftCrane[0] = hanUp_L.transform;
            GameObject hanUp_R = Instantiate(Resources.Load("Prefab/Handrail_Up Big_Right_Auto") as GameObject);
            handrailLiftCrane[1] = hanUp_R.transform;
            GameObject hanUp_C = Instantiate(Resources.Load("Prefab/Handrail_Up Big_Center_Auto") as GameObject);
            handrailLiftCrane[2] = hanUp_C.transform;
            GameObject hanDown_L = Instantiate(Resources.Load("Prefab/Handrail_Down_Small_Left_Auto") as GameObject);
            handrailLiftCrane[3] = hanDown_L.transform;
            GameObject hanDown_R = Instantiate(Resources.Load("Prefab/Handrail_Down_Small_Right_Auto") as GameObject);
            handrailLiftCrane[4] = hanDown_R.transform;
            GameObject hanDown_C = Instantiate(Resources.Load("Prefab/Handrail_Down_Small_Center_Auto") as GameObject);
            handrailLiftCrane[5] = hanDown_C.transform;
            for (int i = 0; i < handrailLiftCrane.Length; i++)
            {
                handrailLiftCrane[i].SetParent(scriptAnimLift.liftCrane);
                handrailLiftCrane[i].gameObject.SetActive(false);
            }
            handrailLiftCrane[0].localRotation = Quaternion.Euler(0, 0, 0);
            handrailLiftCrane[1].localRotation = Quaternion.Euler(0, 0, 0);
            handrailLiftCrane[2].localRotation = Quaternion.Euler(0, 90, 0);
            handrailLiftCrane[3].localRotation = Quaternion.Euler(0, 0, 0);
            handrailLiftCrane[4].localRotation = Quaternion.Euler(0, 0, 0);
            handrailLiftCrane[5].localRotation = Quaternion.Euler(0, 90, 0);
            handrailLiftCrane[0].localPosition = new Vector3(-0.399f, -0.994f, -2.034f);
            handrailLiftCrane[1].localPosition = new Vector3(-0.401f, -0.994f, 2.039f);
            handrailLiftCrane[2].localPosition = new Vector3(1.821f, -0.994f, -0.002f);
            handrailLiftCrane[3].localPosition = new Vector3(0.128f, - 4.775f, -2.037f);
            handrailLiftCrane[4].localPosition = new Vector3(0.116f, -4.775f, 2.023f);
            handrailLiftCrane[5].localPosition = new Vector3(1.812f, -4.775f, -0.004f);
        }
    }
    public void EnambleBoxColliderArrow()
    {
        if (arrowBoxCollider_Back.enabled == false)
        {
            arrowBoxCollider_Forward.enabled = true;
            arrowBoxCollider_Back.enabled = true;
            if (arrowBoxCollider_Center1 != null)
            {
                arrowBoxCollider_Center1.enabled = true;
            }
            if (arrowBoxCollider_Center2 != null)
            {
                arrowBoxCollider_Center2.enabled = true;
            }
            if (arrowBoxCollider_Center3 != null)
            {
                arrowBoxCollider_Center3.enabled = true;
            }
            if (arrowBoxCollider_Center4 != null)
            {
                arrowBoxCollider_Center4.enabled = true;
            }
        }
    }
}
