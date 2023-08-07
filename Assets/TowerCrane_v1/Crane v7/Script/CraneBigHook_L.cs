using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneBigHook_L : MonoBehaviour
{
    private CraneCarController_L _mScriptCar;
    private CraneController_L _mScriptC;
    private CraneDopArrow_L _mScriptDop;
    private UIPanelCrane_L _mScriptUI;
    public Transform bigHook;
    [Header("Connected Cargo")]
    public KeyCode connected_Key;
    [HideInInspector]
    public bool connected_Bool = true;
    [Header("Controller Hook")]
    public KeyCode upHook_Key;
    public KeyCode downHook_Key;
    public KeyCode ab_Key;
    private bool liftHook_Bool = true;
    private float offsetHook = 0;
    public float speedHook = 0f;
    public float massHook = 20;
    public float maxMassHook = 0f;
    [HideInInspector]
    public bool addPhysicsHookBig_Bool = true;
    private ConfigurableJoint joinHook;
    [HideInInspector]
    public bool hookOnCollision_Bool = true;
    private bool blocedUpHook_Bool = true;
    private float checkDistanceBloked = 0;
    [Header("Distance between Hook and Arrow")]
    public float minDistanceHook = 0.74f;
    private float offsetAnchor = 0;
    private bool s = true;
    [Tooltip("Limits the descent of the hook. If the hook position is equal to this value, then the hook will stop at the lowest point.")]
    public float minDownHook = 9f;
    private float maxOffset = 0;
    //Wheel Arrow 3 Dop Arrow
    private Transform _dopArrowWheel_3;
    [Header("Decay Point Hook")]
    public Transform decayHookPoint;
    public Transform decayHookOn;
    public Transform rotationCargo_A;
    public Transform rotationCargo_B;
    public float speedDecay_RotationCargo = 265;
    private float offsetRotDecay = 0;
    public float _distanceCargoToHook = 3.6f;
    [HideInInspector]
    public GameObject _cargo;
    private RaycastHit hitCargo;
    private RaycastHit hit;
    private float _distanceCargo = 0f;
    public GameObject pointLineRenderer;
    public float cableStart = 0.037f;
    public float cableEnd = 0.037f;
    public Material matLine;
    private Transform pointLineCargo_1;
    private Transform pointLineCargo_2;
    private Transform pointLineCargo_3;
    private Transform pointLineCargo_4;
    [Header("Rotation Cargo")]
    public KeyCode rotLeft;
    public KeyCode rotRight;
    public KeyCode abKey;
    private float motorRot = 0.0f;
    public float speedRot = 0f;
    private float targetVelosityPlatform = 30f;
    private bool rotPlatformBool = false;
    // Stabilization Cargo If Connection Hook
    private bool blockStabilizationCargo = true;
    private float checkRotationCrane;
    private float checkUpArrow;
    private float moveHook;
    private Vector3 checkMoveArrow;
    [HideInInspector]
    public TowerPlatformInstallation mSctiptTower; // Tower Crane
    private TowerMarkerElement scriptMarcer; // Tower Crane
    private Transform pointRaycartElement; // Tower Crane
    [HideInInspector]
    public Vector3 elementRaycastVector; // Tower Crane
    private Vector3 rayLeft; // Tower Crane
    private Vector3 rayRight; // Tower Crane
    private Vector3 rayDown; // Tower Crane
    private Vector3 rayForward; // Tower Crane
    private Vector3 rayBack; // Tower Crane
   
    void Start()
    {
        _mScriptCar = transform.GetComponentInParent<CraneCarController_L>();
        _mScriptC = transform.GetComponentInParent<CraneController_L>();
        _mScriptDop = transform.GetComponentInParent<CraneDopArrow_L>();
        _mScriptUI = transform.GetComponentInParent<UIPanelCrane_L>();
        _dopArrowWheel_3 = _mScriptDop.wheelArrow_3;
        decayHookPoint.localScale = new Vector3(0.56f, 0.56f, 0.56f);
        decayHookOn.localScale = new Vector3(2.8f, 2.8f, 2.8f);
        decayHookOn.localPosition = new Vector3(0, -0.98f, -1.31f);
        elementRaycastVector = new Vector3(0, -1, 0);
        rayLeft = new Vector3(-1, 0, 0);
        rayRight = new Vector3(1, 0, 0);
        rayDown = new Vector3(0, -1, 0);
        rayForward = new Vector3(0, 0, 1);
        rayBack = new Vector3(0, 0, -1);
        rotationCargo_A.localRotation = Quaternion.Euler(90, 0, 0);
        rotationCargo_A.localScale = new Vector3(1.8f, 1.8f, 1.25f);
    }
    void Update()
    {
        if (_mScriptCar.pover_Bool == false && addPhysicsHookBig_Bool == false)
        {
            if (Input.GetKey(upHook_Key) && Input.GetKey(ab_Key) && hookOnCollision_Bool == true)
            {
                LiftHook();
                liftHook_Bool = true;
            }
            else if (Input.GetKey(downHook_Key) && Input.GetKey(ab_Key) && blocedUpHook_Bool == true)
            {
                LiftHook();
                liftHook_Bool = false;
            }
            if (_mScriptC.numberHook == 1)
            {
                //Raycast Decay
                Ray ray = new Ray(bigHook.position, -Vector3.up);
                int layerCargo = (1 << 10);
                int layerIgnor = ~(1 << 2);
                if (Physics.Raycast(ray, out hit, 1000, layerIgnor))
                {
                    decayHookPoint.transform.position = hit.point + hit.normal * 0.01f;
                    decayHookPoint.rotation = Quaternion.LookRotation(-hit.normal);
                }
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    maxOffset = hit.distance;
                }
                //Raycast Cargo
                if (Physics.Raycast(ray, out hitCargo, _distanceCargoToHook, layerCargo) && connected_Bool == true)
                {
                    _mScriptUI.InfoKey();
                    _mScriptUI.InfoKey_Int = 1;
                    _mScriptUI.infoKeyCode = connected_Key.ToString();
                    _mScriptUI.infoDescription = "Connected Cargo";
                    decayHookOn.gameObject.SetActive(true);
                    _cargo = hitCargo.collider.gameObject;
                    _distanceCargo = hitCargo.distance;
                }
                else
                {
                    _mScriptUI.InfoKey();
                    _mScriptUI.InfoKey_Int = 2;
                    decayHookOn.gameObject.SetActive(false);
                    _distanceCargo = 0;
                }
                if (maxOffset < minDownHook)
                {
                    hookOnCollision_Bool = false;
                }
                else
                    hookOnCollision_Bool = true;
                // Tower Crane ________________________________________________________________________________________________
                if (connected_Bool == false && pointRaycartElement != null)
                {
                    RaycastHit hitElement;
                 //   Debug.DrawRay(pointRaycartElement.position, pointRaycartElement.TransformDirection(elementRaycastVector) * 1.1f,Color.red);
                    Ray rayElement = new Ray(pointRaycartElement.position, pointRaycartElement.TransformDirection(elementRaycastVector) * 1.1f);
                    if(Physics.Raycast(rayElement, out hitElement, 1.1f) && hitElement.collider.gameObject.GetComponent<TowerMarkerElement>() && hitElement.collider.GetComponentInChildren<TowerMarkerElement>())
                    {
                        mSctiptTower.windowKey.enabled = true;
                        mSctiptTower.textKey.text = mSctiptTower.installCrane.ToString();
                        mSctiptTower.description.text = mSctiptTower.actionDescription[1];
                        mSctiptTower.distanceInstallElementHit = hitElement.transform.localPosition.y;
                        mSctiptTower.previousElement = hitElement.transform.gameObject;
                        mSctiptTower.blockMarker_Bool = true;
                    }
                    else if (mSctiptTower.scriptAnimLift.blockLoadElement_Bool == false)
                    {
                        mSctiptTower.windowKey.enabled = false;
                        mSctiptTower.textKey.text = "";
                        mSctiptTower.description.text = "";
                        mSctiptTower.blockMarker_Bool = false;
                    }
                }
                if (Input.GetKeyDown(connected_Key) && connected_Bool == true && decayHookOn.gameObject.activeInHierarchy == true)
                {
                    CheckElementCraneDisconnect();
                    if (mSctiptTower.blockConnectionElement == true)
                    {
                        ConnectedCargo();
                        connected_Bool = false;
                    }
                }
                else if (Input.GetKeyDown(connected_Key) && connected_Bool == false)
                {
                    ConnectedCargo();
                    connected_Bool = true;
                }
                if (connected_Bool == false && mSctiptTower.blockConnectionElement == true)
                {
                    LineCargo();
                }
                //Rotation Cargo
                if (Input.GetKey(rotLeft) && Input.GetKey(abKey))
                {
                    rotPlatformBool = true;
                    motorRot = +targetVelosityPlatform;
                    offsetRotDecay -= Time.deltaTime * speedDecay_RotationCargo;
                }
                else if (Input.GetKey(rotRight) && Input.GetKey(abKey))
                {
                    rotPlatformBool = true;
                    motorRot = -targetVelosityPlatform;
                    offsetRotDecay += Time.deltaTime * speedDecay_RotationCargo;
                }
                else if (Input.GetKeyUp(rotLeft) || Input.GetKeyUp(rotRight) || Input.GetKeyUp(abKey))
                {
                    rotPlatformBool = false;
                    rotationCargo_A.gameObject.SetActive(false);
                }
            }
        }
    }
     void LateUpdate()
    {
        if (_mScriptCar.pover_Bool == false && addPhysicsHookBig_Bool == false)
        {
            Vector3 decayOn = _mScriptCar.mScriptCamera.transform.position - transform.position;
            decayHookOn.rotation = Quaternion.LookRotation(-decayOn, Vector3.up);
        }
        if (rotPlatformBool == true)
        {
            rotationCargo_A.gameObject.SetActive(true);
            rotationCargo_A.transform.rotation = Quaternion.LookRotation(-hit.normal);
            RotationDecayCargo();
        }
    }
    void FixedUpdate()
    {
        if (_mScriptCar.pover_Bool == false && addPhysicsHookBig_Bool == false)
        {
            SoftJointLimit SL = new SoftJointLimit();
            SL.limit = offsetHook;
            joinHook.linearLimit = SL;
            if (_cargo != null && _cargo.GetComponent<HingeJoint>() != null)
            {
                if (rotPlatformBool == true)
                {
                    HingeJoint hin = _cargo.GetComponent<HingeJoint>();
                    JointMotor joi = new JointMotor();
                    joi.targetVelocity = motorRot;
                    joi.force = speedRot;
                    hin.motor = joi;
                    hin.useMotor = true;
                }
                else if (rotPlatformBool == false)
                {
                    _cargo.GetComponent<HingeJoint>().useMotor = false;
                }
            }
        }
        StabilizationCargoConnection();
    }
    public void AddPhysicsHookBig()
    {
        if(addPhysicsHookBig_Bool == true)
        {
            Rigidbody rigHook = gameObject.AddComponent<Rigidbody>();
            rigHook.mass = massHook;
            rigHook.drag = 1.5f;
            ConfigurableJoint j = gameObject.AddComponent<ConfigurableJoint>();
            joinHook = j;
            joinHook.xMotion = ConfigurableJointMotion.Locked;
            joinHook.yMotion = ConfigurableJointMotion.Limited;
            joinHook.zMotion = ConfigurableJointMotion.Locked;
            joinHook.angularYMotion = ConfigurableJointMotion.Locked;
            if (_mScriptDop.spreadTheArrow_Bool == true)
            {
                joinHook.connectedBody = _mScriptC.arrow_6.GetComponent<Rigidbody>();
            }
             if (_mScriptDop.spreadTheArrow_Bool == false)
            {
                joinHook.connectedBody = _mScriptDop.wheelArrow_3.GetComponent<Rigidbody>();
            }
            joinHook.autoConfigureConnectedAnchor = false;
            _mScriptC.CheckDistanceHook();
            if (_mScriptDop.spreadTheArrow_Bool == true)
            {
                joinHook.anchor = new Vector3(0, _mScriptC.distanceAnchorBigHook, 0);
                joinHook.connectedAnchor = new Vector3(0.12f, -0.38f, 4.39f);
            }
            if(_mScriptDop.spreadTheArrow_Bool == false)
            {
                joinHook.anchor = new Vector3(0,0.96f, 0);
                joinHook.connectedAnchor = new Vector3(0,0,0);
            }
           
            offsetAnchor = joinHook.anchor.y;
            s = true;
            addPhysicsHookBig_Bool = false;
        }
        else if(addPhysicsHookBig_Bool == false)
        {
            Destroy(gameObject.GetComponent<ConfigurableJoint>());
            Destroy(gameObject.GetComponent<ConstantForce>());
            Destroy(gameObject.GetComponent<Rigidbody>());
            addPhysicsHookBig_Bool = true;
        }
    }
    public void StabilizationCargoConnection()
    {
        if (connected_Bool == false)
        {
            float rotX = bigHook.localRotation.x;
            float rotZ = bigHook.localRotation.z;
            _cargo.GetComponent<HingeJoint>().axis = new Vector3(rotX, 1, rotZ);
        if (blockStabilizationCargo == true)
          {
             if (checkRotationCrane != _mScriptC.floatRotCabin || checkUpArrow != _mScriptC.floatArrow || checkMoveArrow != _mScriptC.arrow_6.localPosition || moveHook != offsetHook)
             {
              _cargo.GetComponent<Rigidbody>().isKinematic = false;
              blockStabilizationCargo = false;
             }
          }
        }
    }
    public void RotationDecayCargo()
    {
        var decayRot = Quaternion.AngleAxis(offsetRotDecay, Vector3.forward);
        rotationCargo_B.localRotation = Quaternion.Lerp(rotationCargo_B.localRotation, decayRot, Time.deltaTime * speedDecay_RotationCargo / 1f);
    }
    private void LiftHook()
    {
        if (_mScriptC.numberHook == 1)
        {
            if (liftHook_Bool == true)
            {
                offsetHook += speedHook * Time.deltaTime;
                _mScriptC.ropeReel_B.Rotate(Vector3.left * -_mScriptC.speedWheelCable * Time.deltaTime);
            }
            else
            {
                //Reduces the distance between hook and arrow to fit
                if (joinHook.anchor.y > minDistanceHook && s == true)
                {
                    offsetAnchor -= speedHook * Time.deltaTime;
                    joinHook.anchor = new Vector3(0, offsetAnchor, 0);
                }
                offsetHook -= speedHook * Time.deltaTime;
                _mScriptC.ropeReel_B.Rotate(Vector3.left * _mScriptC.speedWheelCable * Time.deltaTime);
            }
            if(offsetHook < 0 && s == true)
            {
                offsetHook = 0;
            }
            _mScriptC.SoundPitchCraneCabin();
        }
        //Blocks the lifting of the hook up if the hook reaches the minimum point
        if (_mScriptDop.spreadTheArrow_Bool == true)
        {
            checkDistanceBloked = Vector3.Distance(bigHook.position, _mScriptC.wheelArrowBig_3.position);
        }
        if (_mScriptDop.spreadTheArrow_Bool == false)
        {
            checkDistanceBloked = Vector3.Distance(bigHook.position, _mScriptDop.wheelArrow_3.position);
        }
        if (checkDistanceBloked < minDistanceHook)
        {
            s = false;
            blocedUpHook_Bool = false;
        }
        else blocedUpHook_Bool = true;
    }
    public void ConnectedCargo()
    {
        if (connected_Bool == true)
        {
            if (_cargo.GetComponent<ConstantForce>() != null)
            {
                DestroyImmediate(_cargo.GetComponent<ConstantForce>());
            }
            if (_cargo.GetComponent<Rigidbody>() == null)
            {
                _cargo.AddComponent<Rigidbody>();
                _cargo.GetComponent<Rigidbody>().isKinematic = true;
            }
            _cargo.GetComponent<Rigidbody>().drag = 0.18f;
            _cargo.GetComponent<Rigidbody>().mass = _cargo.GetComponent<CargoCrane_L>().mass;
            _cargo.AddComponent<HingeJoint>();
            transform.position = new Vector3(_cargo.transform.position.x, transform.position.y, _cargo.transform.position.z);
            _cargo.GetComponent<HingeJoint>().connectedBody = gameObject.GetComponent<Rigidbody>();
            _cargo.GetComponent<HingeJoint>().anchor = new Vector3(0, _distanceCargo, 0);
            _cargo.GetComponent<HingeJoint>().autoConfigureConnectedAnchor = false;
            _cargo.GetComponent<HingeJoint>().connectedAnchor = new Vector3(0, 0, 0);
            if (_mScriptDop.spreadTheArrow_Bool == true)
            {
                _mScriptUI.textBigCargoKg.text = _cargo.GetComponent<Rigidbody>().mass.ToString();
            }
            if (_mScriptDop.spreadTheArrow_Bool == false)
            {
                _mScriptUI.textKgCargoDopArrow.text = _cargo.GetComponent<Rigidbody>().mass.ToString();
            }

            _cargo.gameObject.layer = 2;
            if (_cargo.transform.childCount > 0)
            {
                foreach (Transform cargoChild in _cargo.GetComponentInChildren<Transform>())
                {
                    cargoChild.gameObject.layer = 2;
                }
            }
            decayHookOn.gameObject.SetActive(false);
            pointLineCargo_1 = _cargo.GetComponent<CargoCrane_L>().pointLineCargo1;
            pointLineCargo_2 = _cargo.GetComponent<CargoCrane_L>().pointLineCargo2;
            pointLineCargo_3 = _cargo.GetComponent<CargoCrane_L>().pointLineCargo3;
            pointLineCargo_4 = _cargo.GetComponent<CargoCrane_L>().pointLineCargo4;
            pointLineRenderer.AddComponent<LineRenderer>();
            // Tower Crane_________________________________________________________________________________
            if (_cargo.GetComponent<TowerMarkerElement>())
            {
                _cargo.GetComponent<TowerMarkerElement>().elementCrane = _cargo.transform;
                if (_cargo.GetComponent<TowerMarkerElement>().numberElement > 0)
                {
                    pointRaycartElement = _cargo.GetComponent<TowerMarkerElement>().pointRaycast;
                }
            }
            RaycastVectorHook();
            checkRotationCrane = _mScriptC.floatRotCabin;
            checkUpArrow = _mScriptC.floatArrow;
            checkMoveArrow = _mScriptC.arrow_6.localPosition;
            moveHook = offsetHook;
            blockStabilizationCargo = true;
        }
        else if (connected_Bool == false)
        {
            DestroyImmediate(_cargo.GetComponent<HingeJoint>());
            _cargo.AddComponent<ConstantForce>();
            _cargo.GetComponent<ConstantForce>().force = new Vector3(0, -0.2f, 0);
            _cargo.gameObject.layer = 10;
            if (_mScriptDop.spreadTheArrow_Bool == true)
            {
                _mScriptUI.textBigCargoKg.text = massHook.ToString();
            }
            if (_mScriptDop.spreadTheArrow_Bool == false)
            {
                _mScriptUI.textKgCargoDopArrow.text = massHook.ToString();
            }
            DestroyImmediate(pointLineRenderer.GetComponent<LineRenderer>());
            pointLineCargo_1 = null;
            pointLineCargo_2 = null;
            pointLineCargo_3 = null;
            pointLineCargo_4 = null;
            _cargo.GetComponent<CargoCrane_L>().StartCoroutine("DestroyComponent");
            _cargo = null;
            if (mSctiptTower) // Tower Crane
            {
                mSctiptTower.UnhookCargo();
            }
            pointRaycartElement = null; // Tower Crane
        }
    }
    public void LineCargo()
    {
        pointLineRenderer.GetComponent<LineRenderer>().startWidth = cableStart;
        pointLineRenderer.GetComponent<LineRenderer>().endWidth = cableEnd;
        Vector3[] lineCargo = new Vector3[8];
        lineCargo[0] = new Vector3(pointLineRenderer.transform.position.x, pointLineRenderer.transform.position.y, pointLineRenderer.transform.position.z);
        lineCargo[1] = new Vector3(pointLineCargo_1.position.x, pointLineCargo_1.position.y, pointLineCargo_1.position.z);
        lineCargo[2] = new Vector3(pointLineRenderer.transform.position.x, pointLineRenderer.transform.position.y, pointLineRenderer.transform.position.z);
        lineCargo[3] = new Vector3(pointLineCargo_2.position.x, pointLineCargo_2.position.y, pointLineCargo_2.position.z);
        lineCargo[4] = new Vector3(pointLineRenderer.transform.position.x, pointLineRenderer.transform.position.y, pointLineRenderer.transform.position.z);
        lineCargo[5] = new Vector3(pointLineCargo_3.position.x, pointLineCargo_3.position.y, pointLineCargo_3.position.z);
        lineCargo[6] = new Vector3(pointLineRenderer.transform.position.x, pointLineRenderer.transform.position.y, pointLineRenderer.transform.position.z);
        lineCargo[7] = new Vector3(pointLineCargo_4.position.x, pointLineCargo_4.position.y, pointLineCargo_4.position.z);
        pointLineRenderer.GetComponent<LineRenderer>().positionCount = lineCargo.Length;
        pointLineRenderer.GetComponent<LineRenderer>().SetPositions(lineCargo);
        pointLineRenderer.GetComponent<LineRenderer>().material = matLine;
    }
    public void RaycastVectorHook() // Tower Crane
    {
            if (_cargo.GetComponent<TowerMarkerElement>()!= null && _cargo.GetComponent<TowerMarkerElement>().numberElement == 7)
            {
                    elementRaycastVector = rayLeft;
            }
            else if(_cargo.GetComponent<TowerMarkerElement>() != null && _cargo.GetComponent<TowerMarkerElement>().numberElement == 12)
            {
                    elementRaycastVector = rayRight;
            }
            else if (_cargo.GetComponent<TowerMarkerElement>() != null && _cargo.GetComponent<TowerMarkerElement>().numberElement == 8)
            {
                    elementRaycastVector = rayBack;
            }
            else
            {
                    elementRaycastVector = rayDown;
            }
        }
    private void CheckElementCraneDisconnect() // Tower Crane
    {
        if (hitCargo.collider.GetComponent<TowerMarkerElement>())
        {
            scriptMarcer = hitCargo.collider.GetComponent<TowerMarkerElement>();
        }
        if (scriptMarcer != null)
        {
            if (scriptMarcer.GetComponent<TowerMarkerElement>().numberElement == 5 && mSctiptTower.checkSwivelPlatform_Block > 0)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if (scriptMarcer.numberElement == 5 && mSctiptTower.blockSwivelPlatform_IfArrowBack == false)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if(scriptMarcer.numberElement == 6 && mSctiptTower.blockConnectionArrow_Forward == true && mSctiptTower.numberArrowCenter_Int > 0)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if(scriptMarcer.numberElement == 6 && mSctiptTower.numberArrowCenter_Int > 1)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if (scriptMarcer.numberElement == 7 && mSctiptTower.checkCounterweightCrane_Block > 0)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if (scriptMarcer.numberElement == 7 && mSctiptTower.numberCounterweight_Int > 0)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if (scriptMarcer.numberElement == 12 && scriptMarcer.descriptionElementArrow == "Back" && mSctiptTower.numberArrowCenter_Int > 0)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if(scriptMarcer.numberElement == 12 && scriptMarcer.descriptionElementArrow == "Center" && scriptMarcer.blockDisconnectArrow_Center == false)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if(scriptMarcer.numberElement == 12 && scriptMarcer.descriptionElementArrow == "Back" && mSctiptTower.blockConnectionArrow_Forward == true)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if(scriptMarcer.numberElement == 13 && mSctiptTower.scriptTCC.connectedCargo_Bool == false)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else if(scriptMarcer.numberElement == 1 && mSctiptTower.blockElement_Rack_TypeA == false)
            {
                mSctiptTower.blockConnectionElement = false;
            }
            else
                mSctiptTower.blockConnectionElement = true;
        }
    }
}
