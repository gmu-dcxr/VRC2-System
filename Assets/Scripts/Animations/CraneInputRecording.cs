using System;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace VRC2.Animations
{
    public class CraneInputRecording : MonoBehaviour
    {
        [Header("Crane Settings")] public float startRotation;
        public float endRotation;

        [Space(30)] public SwitchingBetweenVehicles scriptSwitch;
        public TowerPlatformInstallation scriptTPL;
        public Material lineMaterial;

        [HideInInspector] public int blockController_Int = 0;

        //__________Boom Cart_____________________
        public Transform boomCart;

        // public KeyCode forwardBoomCart = KeyCode.Q;
        // public KeyCode backBoomCart = KeyCode.E;
        public float speedBoomCart = 0.75f;

        [Space(30)]
        //__________Rotation Crane________________
        public Transform rotationElementCrane;

        // public KeyCode leftCrane = KeyCode.A;
        // public KeyCode rightCrane = KeyCode.D;
        public float speedRotationCrane = 4.5f;
        public float smoothRotationCrane = 1.6f;
        private float floatRotCabin = 0;
        [Space(30)] private AudioSource soundCrane;
        public AudioClip motorCrane;
        public AudioClip startMotorCrane;
        public AudioClip stopMotorCrane;
        public AudioClip detaliClip;
        private bool blockPlayOneShot_RotationCrane = true;
        private bool blockPlayOneShot_BoomCart = true;
        private bool blockPlayOneShot_Hook = true;
        [HideInInspector] public Transform pointMovingForward;
        [HideInInspector] public Transform pointMovingBack;
        public Transform pointLineArrow_BoomCart;
        public Transform boomCart_Detal0;
        public Transform pointLineHook0;
        public Transform pointLineHook1;
        public Transform pointLineHook2;
        public Transform pointLineHook3;
        public Transform pointLineHook4;
        private LineRenderer lineRenHook_1;
        private LineRenderer lineRenHook_2;
        private LineRenderer lineRenHook_3;
        private bool onLineCableHook = false;
        private Vector3 minMovingBoomCart;
        private Vector3 maxMovingBoomCart;

        [HideInInspector] public bool blockControllerCrane = false;

        // [Space(30)] [Header("SettingsHook")] 
        // public KeyCode upMovingHook = KeyCode.W;
        // public KeyCode downMovingHook = KeyCode.S;
        // public KeyCode seizeTheCargo = KeyCode.C;
        [Space(20)] [Header("Rotation Cargo")] public KeyCode rotationCargo_Press = KeyCode.LeftShift;

        // public KeyCode leftRotationCargo = KeyCode.Mouse0;
        // public KeyCode rightRotation = KeyCode.Mouse1;
        private float offsetRotationHook = 0;
        public float speedRotationHook = 1;
        private float targetVelosityPlatform = 30f;
        private bool rotPlatformBool = false;
        public Transform hook;
        public float speedMovingHook = 1;
        public float distanceCargoToHook = 3.2f;
        public Transform decayHookPoint;
        public Transform decayHookOn;
        public Transform rotationCargo_A;
        public Transform rotationCargo_B;
        public float speedDecay_RotationCargo = 265;
        private float offsetRotDecay = 0;
        public float massHook = 50;
        private Transform pointHook;
        private Transform pointRayDecay;

        [Tooltip(
            "Limits hook lift. If the hook position is equal to this value, then the hook will stop at the top point.")]
        public float minUpHook = 0.3f;

        [Tooltip(
            "Limits the descent of the hook. If the hook position is equal to this value, then the hook will stop at the lowest point.")]
        public float minDownHook = 0.05f;

        private float maxOffset = 0;
        private float offsetAnchor = 0;
        private bool addPhysicsHook_Bool = true;
        private ConfigurableJoint jointHook;
        [HideInInspector] public GameObject _cargo;
        [HideInInspector] public bool connectedCargo_Bool = true;
        private float distanceCargo = 0f;
        private Transform pointLineCargo_1;
        private Transform pointLineCargo_2;
        private Transform pointLineCargo_3;
        private Transform pointLineCargo_4;
        public Transform pointCheckDistanceUI;

        // [Space(30)] [Header("Settings UI Panel")]
        // public Text distanceCartUI;
        //
        // public Text hookCargoMassUI;
        // public Text distanceHookUI;
        // public Text rotationCraneUI;
        // public Text counterweightUI;
        // public Image rotationCrane_DUI;
        private RectTransform rotationCrane_D;
        [HideInInspector] public int counterweightUI_Int = 0;
        private bool blockSound_A = true;
        private bool blockSound_B = true;

        private bool blockSound_C = true;

        // Stabilization Cargo If Connection Hook
        private bool blockStabilizationCargo = true;
        private float checkRotationCrane;
        private float moveHook;
        private Vector3 checkMoveCart;
        private RaycastHit hit;

        #region Input Actions

        private CraneInputActions craneIA;

        private InputAction cartInput;
        private InputAction craneInput;
        private InputAction hookInput;
        private InputAction cargoInput;

        private InputAction seizeInput;
        private InputAction releaseInput;



        #endregion

        [Header("Hack")] [HideInInspector]
        public GameObject backupCargo; // sometime automatic detecting cargo doesn't work.

        #region Crane status check

        [HideInInspector] public bool Ready { get; set; }

        [HideInInspector] public bool StopRotating { get; set; }

        public System.Action OnReady;

        #endregion

        public void Start()
        {
            Ready = false;
            // scriptTPL = this.gameObject.GetComponent<TowerPlatformInstallation>();
            // scriptSwitch = this.gameObject.GetComponent<SwitchingBetweenVehicles>();
            GameObject _pointHooh = new GameObject("PointHook");
            _pointHooh.transform.SetParent(boomCart);
            pointHook = _pointHooh.transform;
            pointHook.localPosition = new Vector3(0, -0.832f, 0);
            GameObject _pointRD = new GameObject("Point Raycast Decay");
            pointRayDecay = _pointRD.transform;
            pointRayDecay.SetParent(hook);
            pointRayDecay.localPosition = new Vector3(0, -1.275f, 0);
            decayHookPoint.localScale = new Vector3(0.56f, 0.56f, 0.56f);
            decayHookOn.localScale = new Vector3(2.8f, 2.8f, 2.8f);
            decayHookOn.localPosition = new Vector3(0, -0.98f, -1.31f);
            soundCrane = rotationElementCrane.GetComponent<AudioSource>();
            rotationCargo_A.localRotation = Quaternion.Euler(90, 0, 0);
            rotationCargo_A.localScale = new Vector3(1.8f, 1.8f, 1.25f);

            // hack here
            CreateCableHook();

            // initialize crane rotation
            floatRotCabin = startRotation;
            rotationElementCrane.localRotation = Quaternion.Euler(0, startRotation, 0);
            Ready = true;

            StopRotating = false;
        }

        private void Awake()
        {
            InitInputActions();
        }

        public void InitInputActions()
        {
            craneIA = new CraneInputActions();
            craneIA.Enable();

            cartInput = craneIA.Crane.Cart;
            craneInput = craneIA.Crane.Crane;
            hookInput = craneIA.Crane.Hook;
            cargoInput = craneIA.Crane.Cargo;

            seizeInput = craneIA.Crane.Seize;
        }

        public void Update()
        {
            // if (blockController_Int == 3 && scriptTPL.blockArrowIfBoomCartActive == false &&
            // blockControllerCrane == true)
            // UIPanelCrane();
            
            if (true)
            {
                AnimationBoomCart();
                RotateCrane();
                MovingHook();
                CheckCargo();
            }

            if (connectedCargo_Bool == false)
            {
                LineCargo();
            }
        }

        public void FixedUpdate()
        {
            if (connectedCargo_Bool == false)
            {
                if (rotPlatformBool == true)
                {
                    HingeJoint hin = _cargo.GetComponent<HingeJoint>();
                    JointMotor mot = new JointMotor();
                    mot.targetVelocity = offsetRotationHook;
                    mot.force = speedRotationHook;
                    hin.motor = mot;
                    hin.useMotor = true;
                }
                else if (rotPlatformBool == false)
                {
                    _cargo.GetComponent<HingeJoint>().useMotor = false;
                }
            }

            StabilizationCargoConnection();
        }

        public void RotationDecayCargo()
        {
            var decayRot = Quaternion.AngleAxis(offsetRotDecay, Vector3.forward);
            rotationCargo_B.localRotation = Quaternion.Lerp(rotationCargo_B.localRotation, decayRot,
                Time.deltaTime * speedDecay_RotationCargo / 1f);
        }

        public void StabilizationCargoConnection()
        {
            if (connectedCargo_Bool == false)
            {
                float rotX = hook.localRotation.x;
                float rotZ = hook.localRotation.z;
                _cargo.GetComponent<HingeJoint>().axis = new Vector3(rotX, 1, rotZ);
                if (blockStabilizationCargo == true)
                {
                    if (checkRotationCrane != floatRotCabin || checkMoveCart != boomCart.localPosition ||
                        moveHook != offsetAnchor)
                    {
                        _cargo.GetComponent<Rigidbody>().isKinematic = false;
                        blockStabilizationCargo = false;
                    }
                }

                if (rotPlatformBool == true)
                {
                    rotationCargo_A.gameObject.SetActive(true);
                    rotationCargo_A.transform.rotation = Quaternion.LookRotation(-hit.normal);
                    RotationDecayCargo();
                }
            }
        }

        public void LateUpdate()
        {
            if (onLineCableHook == true)
            {
                // Line Renderer 1___________________________________________________
                Vector3[] line1 = new Vector3[2];
                line1[0] = pointLineArrow_BoomCart.position;
                line1[1] = pointLineHook0.position;
                lineRenHook_1.positionCount = line1.Length;
                lineRenHook_1.SetPositions(line1);
                // Line Renderer 2___________________________________________________
                Vector3[] line2 = new Vector3[2];
                line2[0] = pointLineHook1.position;
                line2[1] = pointLineHook2.position;
                lineRenHook_2.positionCount = line2.Length;
                lineRenHook_2.SetPositions(line2);
                // Line Renderer 3___________________________________________________
                Vector3[] line3 = new Vector3[2];
                line3[0] = pointLineHook3.position;
                line3[1] = pointLineHook4.position;
                lineRenHook_3.positionCount = line3.Length;
                lineRenHook_3.SetPositions(line3);
            }
        }

        private void AnimationBoomCart()
        {
            // if (Input.GetKey(forwardBoomCart))
            if (cartInput.ReadValue<Vector2>().x > 0)
            {
                ForwardBoomCart();
            }

            else if (cartInput.ReadValue<Vector2>().x < 0)
            {
                BackwardBoomCart();

            }
            // else if (cartInput.ReadValue<Vector2>().x == 0)
            else
            {
                StopBoomCart();
            }
        }

        public void MovingHook()
        {
            // if (Input.GetKey(upMovingHook))
            if (hookInput.ReadValue<Vector2>().y > 0)
            {
                UpHook();
            }
            else if (hookInput.ReadValue<Vector2>().y < 0)
            {
                DownHook();
            }
            else
            {              
                StopHook();
            }

            // jointHook.anchor = new Vector3(0, offsetAnchor, 0);

            // if ((Input.GetKey(rotationCargo_Press) && Input.GetKey(leftRotationCargo)))
            if (cargoInput.ReadValue<Vector2>().x < 0)
            {
                rotPlatformBool = true;
                offsetRotationHook = +targetVelosityPlatform;
                offsetRotDecay -= Time.deltaTime * speedDecay_RotationCargo;
            }
            // else if ((Input.GetKey(rotationCargo_Press) && Input.GetKey(rightRotation)))
            else if (cargoInput.ReadValue<Vector2>().x > 0)
            {
                rotPlatformBool = true;
                offsetRotationHook = -targetVelosityPlatform;
                offsetRotDecay += Time.deltaTime * speedDecay_RotationCargo;
            }

            // if (Input.GetKeyUp(rotationCargo_Press) || Input.GetKeyUp(leftRotationCargo) ||
            // Input.GetKeyUp(rightRotation))
            if (cargoInput.ReadValue<Vector2>().x == 0)
            {
                rotPlatformBool = false;
                rotationCargo_A.gameObject.SetActive(false);
            }

            if (soundCrane.pitch > 1 && blockSound_C == true)
            {
                soundCrane.pitch -= Time.deltaTime * 0.85f;
            }
        }

        private void RotateCrane()
        {
            // if (Input.GetKey(leftCrane))
            if (craneInput.ReadValue<Vector2>().x < 0)
            {
                TurnLeft();
            }
//______________________________________________________________________________________________________________________________________

            // if (Input.GetKey(rightCrane))
            else if (craneInput.ReadValue<Vector2>().x > 0)
            {
                TurnRight();
            }
//______________________________________________________________________________________________________________________________________
            // else if (Input.GetKeyUp(rightCrane))
            else if (craneInput.ReadValue<Vector2>().x == 0)
            {
                TurnStop();
            }

            // if (!StopRotating)
            // {
            //     var crane = Quaternion.AngleAxis(floatRotCabin, Vector3.up);
            //     rotationElementCrane.localRotation = Quaternion.Lerp(rotationElementCrane.localRotation, crane,
            //         Time.deltaTime * speedRotationCrane / smoothRotationCrane);   
            // }
            // var cranePanel = Quaternion.AngleAxis(floatRotCabin, Vector3.forward);
            // rotationCrane_D.rotation = Quaternion.Lerp(rotationCrane_D.rotation, cranePanel,
            //     Time.deltaTime * speedRotationCrane / smoothRotationCrane);
        }

        public void ForceUpdateRotation(float y)
        {
            floatRotCabin = y;
            rotationElementCrane.localRotation = Quaternion.Euler(0, y, 0);
        }

        public float GetCraneRotation()
        {
            return rotationElementCrane.localRotation.eulerAngles.y;
        }

        public void ForceUpdateBoomCart(float x)
        {
            var vec = boomCart.localPosition;
            vec.x = x;
            boomCart.localPosition = vec;
        }

        public void CreateCableHook()
        {
            // Line Renderer 1___________________________________________________
            lineRenHook_1 = pointLineArrow_BoomCart.GetComponent<LineRenderer>();
            lineRenHook_1.startWidth = 0.032f;
            lineRenHook_1.endWidth = 0.032f;
            lineRenHook_1.material = lineMaterial;
            // Line Renderer 2___________________________________________________
            lineRenHook_2 = pointLineHook1.GetComponent<LineRenderer>();
            lineRenHook_2.startWidth = 0.032f;
            lineRenHook_2.endWidth = 0.032f;
            lineRenHook_2.material = lineMaterial;
            // Line Renderer 3___________________________________________________
            lineRenHook_3 = pointLineHook3.GetComponent<LineRenderer>();
            lineRenHook_3.startWidth = 0.032f;
            lineRenHook_3.endWidth = 0.032f;
            lineRenHook_3.material = lineMaterial;
            if (onLineCableHook == false)
            {
                onLineCableHook = true;
                lineRenHook_1.enabled = true;
                lineRenHook_2.enabled = true;
                lineRenHook_3.enabled = true;
                boomCart_Detal0.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else if (onLineCableHook == true)
            {
                onLineCableHook = false;
                lineRenHook_1.enabled = false;
                lineRenHook_2.enabled = false;
                lineRenHook_3.enabled = false;
                boomCart_Detal0.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (scriptTPL.numberArrowCenter_Int == 0)
            {
                maxMovingBoomCart = new Vector3(-15.761f, boomCart.localPosition.y, boomCart.localPosition.z);
            }

            if (scriptTPL.numberArrowCenter_Int == 1)
            {
                maxMovingBoomCart = new Vector3(-23.779f, boomCart.localPosition.y, boomCart.localPosition.z);
            }

            if (scriptTPL.numberArrowCenter_Int == 2)
            {
                maxMovingBoomCart = new Vector3(-31.755f, boomCart.localPosition.y, boomCart.localPosition.z);
            }

            if (scriptTPL.numberArrowCenter_Int == 3)
            {
                maxMovingBoomCart = new Vector3(-39.576f, boomCart.localPosition.y, boomCart.localPosition.z);
            }

            if (scriptTPL.numberArrowCenter_Int == 4)
            {
                maxMovingBoomCart = new Vector3(-47.497f, boomCart.localPosition.y, boomCart.localPosition.z);
            }

            minMovingBoomCart = new Vector3(-2.783f, boomCart.localPosition.y, boomCart.localPosition.z);

            AddPhysicsHook();
        }

        public void AddPhysicsHook()
        {
            if (addPhysicsHook_Bool == true)
            {
                // hook.SetParent(null);
                hook.gameObject.AddComponent<Rigidbody>();
                hook.gameObject.GetComponent<Rigidbody>().mass = massHook;
                hook.gameObject.GetComponent<Rigidbody>().drag = 1.1f;
                hook.gameObject.AddComponent<ConstantForce>();
                hook.gameObject.GetComponent<ConstantForce>().force = new Vector3(0, -0.01f, 0);
                jointHook = hook.gameObject.AddComponent<ConfigurableJoint>();
                jointHook.xMotion = ConfigurableJointMotion.Locked;
                jointHook.yMotion = ConfigurableJointMotion.Limited;
                jointHook.zMotion = ConfigurableJointMotion.Locked;
                jointHook.angularYMotion = ConfigurableJointMotion.Locked;
                pointHook.gameObject.AddComponent<Rigidbody>();
                pointHook.GetComponent<Rigidbody>().isKinematic = true;
                jointHook.connectedBody = pointHook.GetComponent<Rigidbody>();
                jointHook.autoConfigureConnectedAnchor = false;
                jointHook.anchor = new Vector3(0, 0.5f, 0);
                offsetAnchor = 0.5f;
                decayHookPoint.gameObject.SetActive(true);
                addPhysicsHook_Bool = false;
            }
            else if (addPhysicsHook_Bool == false)
            {
                DestroyImmediate(hook.GetComponent<ConfigurableJoint>());
                DestroyImmediate(hook.GetComponent<ConstantForce>());
                DestroyImmediate(hook.GetComponent<Rigidbody>());
                DestroyImmediate(pointHook.GetComponent<Rigidbody>());
                hook.SetParent(boomCart);
                hook.localRotation = Quaternion.Euler(0, 0, 0);
                hook.localPosition = new Vector3(-0.00399971f, -1.041f, -0.001f);
                jointHook = null;
                decayHookPoint.gameObject.SetActive(false);
                addPhysicsHook_Bool = true;
            }
        }

        public void CheckCargo()
        {
            if (addPhysicsHook_Bool == false)
            {
                Ray ray = new Ray(pointRayDecay.position, -Vector3.up);
                int layerCargo = (1 << 10);
                int layerIgnore = ~(1 << 2);
                if (Physics.Raycast(ray, out hit, 1000, layerIgnore))
                {
                    decayHookPoint.position = hit.point + hit.normal * 0.01f;
                    decayHookPoint.rotation = Quaternion.LookRotation(-hit.normal);
                }

                if (Physics.Raycast(ray, out hit, 100))
                {
                    maxOffset = hit.distance;
                }

                RaycastHit hitCargo;
                if (Physics.Raycast(ray, out hitCargo, distanceCargoToHook, layerCargo) && maxOffset > 1.7f &&
                    connectedCargo_Bool == true)
                {
                    decayHookOn.gameObject.SetActive(true);
                    _cargo = hitCargo.collider.gameObject;
                    distanceCargo = hitCargo.distance;
                    Vector3 decayOn = scriptSwitch.towerCrane.transform.position - transform.position;
                    decayHookOn.rotation = Quaternion.LookRotation(-decayOn, Vector3.up);
                }
                else
                {
                    decayHookOn.gameObject.SetActive(false);
                    distanceCargo = 0;
                }

                // if (Input.GetKeyDown(seizeTheCargo)) // Connected Cargo
                if (seizeInput.triggered) // Connected Cargo
                {
                    SeizeCargo();
                }
            }
        }

        public void ConnectedCargo()
        {
            if (connectedCargo_Bool == true)
            {
                // fix just in case
                if (_cargo == null)
                {
                    _cargo = backupCargo;
                }

                if (_cargo.GetComponent<ConstantForce>() != null)
                {
                    DestroyImmediate(_cargo.GetComponent<ConstantForce>());
                }

                if (_cargo.GetComponent<Rigidbody>() == null)
                {
                    _cargo.AddComponent<Rigidbody>();
                }

                _cargo.GetComponent<Rigidbody>().drag = 1.18f;
                _cargo.GetComponent<Rigidbody>().mass = _cargo.GetComponent<CargoCrane_L>().mass;
                _cargo.AddComponent<HingeJoint>();
                hook.position = new Vector3(_cargo.transform.position.x, hook.position.y, _cargo.transform.position.z);
                _cargo.GetComponent<HingeJoint>().connectedBody = hook.GetComponent<Rigidbody>();
                _cargo.GetComponent<HingeJoint>().anchor = new Vector3(0, distanceCargo, 0);
                _cargo.GetComponent<HingeJoint>().autoConfigureConnectedAnchor = false;
                _cargo.GetComponent<HingeJoint>().connectedAnchor = new Vector3(0, -2f, 0);
                _cargo.gameObject.layer = 2;
                decayHookOn.gameObject.SetActive(false);
                pointRayDecay.gameObject.AddComponent<LineRenderer>();
                pointRayDecay.GetComponent<LineRenderer>().startWidth = 0.032f;
                pointRayDecay.GetComponent<LineRenderer>().endWidth = 0.032f;
                pointRayDecay.GetComponent<LineRenderer>().material = lineMaterial;
                pointLineCargo_1 = _cargo.GetComponent<CargoCrane_L>().pointLineCargo1;
                pointLineCargo_2 = _cargo.GetComponent<CargoCrane_L>().pointLineCargo2;
                pointLineCargo_3 = _cargo.GetComponent<CargoCrane_L>().pointLineCargo3;
                pointLineCargo_4 = _cargo.GetComponent<CargoCrane_L>().pointLineCargo4;
                checkRotationCrane = floatRotCabin;
                checkMoveCart = boomCart.localPosition;
                moveHook = offsetAnchor;
                blockStabilizationCargo = true;
            }
            else if (connectedCargo_Bool == false)
            {
                DestroyImmediate(_cargo.GetComponent<HingeJoint>());
                _cargo.AddComponent<ConstantForce>();
                _cargo.GetComponent<ConstantForce>().force = new Vector3(0, -0.2f, 0);
                _cargo.gameObject.layer = 10;
                DestroyImmediate(pointRayDecay.GetComponent<LineRenderer>());
                pointLineCargo_1 = null;
                pointLineCargo_2 = null;
                pointLineCargo_3 = null;
                pointLineCargo_4 = null;
                _cargo.GetComponent<CargoCrane_L>().StartCoroutine("DestroyComponent");
                _cargo = null;
            }
        }

        private void LineCargo()
        {
            Vector3[] lineCargo = new Vector3[8];
            lineCargo[0] = pointRayDecay.position;
            lineCargo[1] = pointLineCargo_1.position;
            lineCargo[2] = pointRayDecay.position;
            lineCargo[3] = pointLineCargo_2.position;
            lineCargo[4] = pointRayDecay.position;
            lineCargo[5] = pointLineCargo_3.position;
            lineCargo[6] = pointRayDecay.position;
            lineCargo[7] = pointLineCargo_4.position;
            pointRayDecay.GetComponent<LineRenderer>().positionCount = lineCargo.Length;
            pointRayDecay.GetComponent<LineRenderer>().SetPositions(lineCargo);
        }

        #region Connect Cargo Hardcode

        public void ManuallyConnectCargo()
        {
            if (addPhysicsHook_Bool == false)
            {
                Ray ray = new Ray(pointRayDecay.position, -Vector3.up);
                int layerCargo = (1 << 10);
                int layerIgnore = ~(1 << 2);
                if (Physics.Raycast(ray, out hit, 1000, layerIgnore))
                {
                    decayHookPoint.position = hit.point + hit.normal * 0.01f;
                    decayHookPoint.rotation = Quaternion.LookRotation(-hit.normal);
                }

                if (Physics.Raycast(ray, out hit, 100))
                {
                    maxOffset = hit.distance;
                }

                RaycastHit hitCargo;
                if (Physics.Raycast(ray, out hitCargo, distanceCargoToHook, layerCargo) && maxOffset > 1.7f &&
                    connectedCargo_Bool == true)
                {
                    decayHookOn.gameObject.SetActive(true);
                    _cargo = hitCargo.collider.gameObject;
                    distanceCargo = hitCargo.distance;
                    Vector3 decayOn = scriptSwitch.towerCrane.transform.position - transform.position;
                    decayHookOn.rotation = Quaternion.LookRotation(-decayOn, Vector3.up);
                }
                else
                {
                    decayHookOn.gameObject.SetActive(false);
                    distanceCargo = 0;
                }

                if (connectedCargo_Bool == true)
                {
                    ConnectedCargo();
                    connectedCargo_Bool = false;
                }
                else if (connectedCargo_Bool == false)
                {
                    ConnectedCargo();
                    connectedCargo_Bool = true;
                }
            }
        }

        #endregion

        #region API

        public void ForwardBoomCart()
        {
            blockSound_A = false;
            if (!soundCrane.isPlaying)
            {
                soundCrane.PlayOneShot(detaliClip);
                soundCrane.PlayOneShot(startMotorCrane);
                soundCrane.Play();
            }
            else
            {
                boomCart.localPosition = Vector3.MoveTowards(boomCart.localPosition, maxMovingBoomCart,
                    speedBoomCart * Time.deltaTime);
            }

            if (boomCart.localPosition == maxMovingBoomCart)
            {
                if (blockSound_C == true && blockSound_B == true)
                {
                    soundCrane.Stop();
                }

                if (blockPlayOneShot_BoomCart == true)
                {
                    soundCrane.PlayOneShot(detaliClip, 0.45f);
                    blockPlayOneShot_BoomCart = false;
                }
            }
        }

        public void BackwardBoomCart()
        {
            blockSound_A = false;
            if (!soundCrane.isPlaying)
            {
                soundCrane.PlayOneShot(detaliClip);
                soundCrane.PlayOneShot(startMotorCrane);
                soundCrane.Play();
            }
            else
            {
                boomCart.localPosition = Vector3.MoveTowards(boomCart.localPosition, minMovingBoomCart,
                    speedBoomCart * Time.deltaTime);
            }

            if (boomCart.localPosition == minMovingBoomCart)
            {
                if (blockSound_C == true && blockSound_B == true)
                {
                    soundCrane.Stop();
                }

                if (blockPlayOneShot_BoomCart == true)
                {
                    soundCrane.PlayOneShot(detaliClip, 0.45f);
                    blockPlayOneShot_BoomCart = false;
                }
            }
        }

        public void StopBoomCart()
        {
            if (blockSound_B == true && blockSound_C == true && blockSound_A == false && blockPlayOneShot_BoomCart == false)
            {
                 soundCrane.Stop();
                 soundCrane.PlayOneShot(detaliClip);
                 soundCrane.PlayOneShot(stopMotorCrane);
            }

            blockPlayOneShot_BoomCart = true;
            blockSound_A = true;
        }

        public void UpHook()
        {
            if (offsetAnchor > minUpHook)
            {
                blockSound_C = false;
                if (!soundCrane.isPlaying)
                {
                    soundCrane.PlayOneShot(detaliClip);
                    soundCrane.PlayOneShot(startMotorCrane);
                    soundCrane.Play();
                }
                else
                {
                    offsetAnchor -= speedMovingHook * Time.deltaTime;
                }

                if (soundCrane.isPlaying && blockSound_A == false && soundCrane.pitch < 1.3f)
                {
                    soundCrane.pitch += Time.deltaTime * 0.35f;
                }
            }
            if (offsetAnchor < 1) 
            {
                //print("REACHED TOP");
                blockPlayOneShot_Hook = false;
            }
            //print("OffsetAnchor: " + offsetAnchor);
            jointHook.anchor = new Vector3(0, offsetAnchor, 0);
        }

        public void DownHook()
        {
            if (minDownHook < maxOffset)
            {
                blockSound_C = false;
                if (!soundCrane.isPlaying)
                {
                    soundCrane.PlayOneShot(detaliClip);
                    soundCrane.PlayOneShot(startMotorCrane);
                    soundCrane.Play();
                }
                else
                {
                    offsetAnchor += speedMovingHook * Time.deltaTime;
                }

                if (soundCrane.isPlaying && blockSound_A == false && soundCrane.pitch < 1.3f)
                {
                    soundCrane.pitch += Time.deltaTime * 0.35f;   
                }
                
            }
            if (maxOffset < 2)
            {
                //print("REACHED BOTTOM\n");
                blockPlayOneShot_Hook = false;
            }
           // print("MinDownHook: " + minDownHook + "\nMaxOffset: " + maxOffset);
            jointHook.anchor = new Vector3(0, offsetAnchor, 0);
        }

        public void StopHook()
        {
            if (blockSound_A == true && blockSound_B == true && blockPlayOneShot_Hook == false)
            {
                soundCrane.Stop();
                soundCrane.PlayOneShot(detaliClip);
                soundCrane.PlayOneShot(stopMotorCrane);
            }
            blockPlayOneShot_Hook = true;
            blockSound_C = true;
        }

        public void TurnLeft()
        {
            blockSound_B = false;           
            if (!soundCrane.isPlaying)
            {
                soundCrane.PlayOneShot(detaliClip);
                soundCrane.PlayOneShot(startMotorCrane);
                soundCrane.Play();
            }
            else
            {
                floatRotCabin -= Time.deltaTime * speedRotationCrane;
            }

            if (soundCrane.isPlaying && blockSound_A == false || blockSound_C == false)
            {
                if (blockPlayOneShot_RotationCrane == true)
                {
                    soundCrane.PlayOneShot(detaliClip, 0.45f);
                    blockPlayOneShot_RotationCrane = false;
                }
            }

            if (!StopRotating)
            {
                var crane = Quaternion.AngleAxis(floatRotCabin, Vector3.up);
                rotationElementCrane.localRotation = Quaternion.Lerp(rotationElementCrane.localRotation, crane,
                    Time.deltaTime * speedRotationCrane / smoothRotationCrane);
            }
            
        }

        public void TurnRight()
        {
            blockSound_B = false;
            if (!soundCrane.isPlaying)
            {
                soundCrane.PlayOneShot(detaliClip);
                soundCrane.PlayOneShot(startMotorCrane);
                soundCrane.Play();
            }
            else
            {
                floatRotCabin += Time.deltaTime * speedRotationCrane;
            }

            if (soundCrane.isPlaying && blockSound_A == false || blockSound_C == false)
            {
                if (blockPlayOneShot_RotationCrane == true)
                {
                    soundCrane.PlayOneShot(detaliClip, 0.45f);
                    blockPlayOneShot_RotationCrane = false;
                }
            }

            if (!StopRotating)
            {
                var crane = Quaternion.AngleAxis(floatRotCabin, Vector3.up);
                rotationElementCrane.localRotation = Quaternion.Lerp(rotationElementCrane.localRotation, crane,
                    Time.deltaTime * speedRotationCrane / smoothRotationCrane);
            }
            if (floatRotCabin >= startRotation) 
            {
                StopRotating = true;
            }
        }

        public void TurnStop()
        {
            //print("Cabing rot: "+ floatRotCabin);
            if (blockSound_A == true && blockSound_C == true && StopRotating == true)
            {
               // print("STOP ROTATING SOUND");
                soundCrane.Stop();
                soundCrane.PlayOneShot(detaliClip);
                soundCrane.PlayOneShot(stopMotorCrane);
                StopRotating = false;
            }

            blockPlayOneShot_RotationCrane = true;
            blockSound_B = true;

            if (!StopRotating)
            {
                var crane = Quaternion.AngleAxis(floatRotCabin, Vector3.up);
                rotationElementCrane.localRotation = Quaternion.Lerp(rotationElementCrane.localRotation, crane,
                    Time.deltaTime * speedRotationCrane / smoothRotationCrane);
            }            
        }

        public void SeizeCargo()
        {            
            if (connectedCargo_Bool == true)
            {
                ConnectedCargo();
                connectedCargo_Bool = false;
            }
            else if (connectedCargo_Bool == false)
            {
                ConnectedCargo();
                connectedCargo_Bool = true;
            }
        }


        #endregion

        #region UI Panel

        public void UIPanelCrane()
        {
            // Boom Cart
            var distanceCart = Vector3.Distance(pointCheckDistanceUI.position, boomCart.position).ToString("0.0") + "m";
            // Hook Distance
            // var distanceHook = Vector3.Distance(pointRayDecay.position, decayHookPoint.position).ToString("0.0") + "m";
            var distanceHook = Math.Abs(pointCheckDistanceUI.position.y - pointRayDecay.position.y).ToString("0.0") + "m";
            // Rotation Crane
            var rotationCrane = Mathf.RoundToInt(rotationElementCrane.localEulerAngles.y).ToString();
            
            print($"{distanceCart} - {distanceHook} - {rotationCrane}");
        }

        [HideInInspector]
        public float DistanceCart
        {
            get => Vector3.Distance(pointCheckDistanceUI.position, boomCart.position);
        }

        [HideInInspector]
        public float DistanceHook
        {
            get => Math.Abs(pointCheckDistanceUI.position.y - pointRayDecay.position.y);
        }

        [HideInInspector]
        public int RotationCrane
        {
            get => Mathf.RoundToInt(rotationElementCrane.localEulerAngles.y);
        }
        

        #endregion
    }
}