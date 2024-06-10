using System;
using System.Collections.Concurrent;
using System.Linq;
using Fusion;
using Hack;
using NodeCanvas.Tasks.Actions;
using Oculus.Interaction;
using Oculus.Interaction.DistanceReticles;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VRC2.Hack;
using VRC2.Pipe;
using VRC2.Utility;
using Object = UnityEngine.Object;

namespace VRC2.Events
{
    public class PipeCollisionDetector : NetworkBehaviour
    {
        private Object pipeParent;

        [HideInInspector] public bool connected = false;

        private GlueHintManager _glueHintManager;

        private GlueHintManager hintManager
        {
            get
            {
                if (_glueHintManager == null)
                {
                    _glueHintManager = gameObject.transform.parent.GetComponent<GlueHintManager>();
                }

                return _glueHintManager;
            }
        }

        // minimum glue collision count
        private int minimumGlue = 10;
        private int glued = 0;

        #region RPC Messages

        private bool requiredUpdated = false;
        private bool requiredUpdatedDone = false;

        private Vector3 _cLocalPos;
        private Quaternion _cLocalRot;
        private Vector3 _oLocalPos;
        private Quaternion _oLocalRot;
        private NetworkId _cid;
        private NetworkId _oid;

        private NetworkId _pid;

        // right hand half pipe name
        private string _oconnected;

        #endregion

        #region Refactor controller visual and poke location

        private VRHelper _vrHelper;

        private VRHelper vrHelper
        {
            get
            {
                if (_vrHelper == null)
                {
                    _vrHelper = FindObjectOfType<VRHelper>();
                    if (_vrHelper == null)
                    {
                        Debug.LogError("Failed to find VRHelper.");
                    }
                }

                return _vrHelper;
            }
        }

        private GameObject _leftVisual;

        private GameObject leftViusal => vrHelper.leftVisual;
        private GameObject leftPoke => vrHelper.leftPoke;


        #endregion




        private void Start()
        {
            // deprecated
            // InitializeOVRControllerVisual();
            // InitializePokeLocation();

            // pre-load object
            pipeParent = AssetDatabase.LoadAssetAtPath(GlobalConstants.pipePipeConnectorPrefabPath, typeof(GameObject));
        }

        private void Update()
        {
            if (requiredUpdated && !requiredUpdatedDone)
            {
                UpdateOnClient();
            }
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        void InitializeOVRControllerVisual()
        {
            try
            {
                if (GlobalConstants.LeftOVRControllerVisual == null)
                {
                    var name = GlobalConstants.ControllerVisual;
                    var objects = VRC2.Utils.FindAll(name);
                    GameObject left = null, right = null;
                    foreach (var obj in objects)
                    {
                        var parent = obj.transform.parent.gameObject;
                        if (parent.name.StartsWith("Left"))
                        {
                            left = obj;
                            Debug.Log("Set LeftOVRControllerVisual");
                        }
                        else if (parent.name.StartsWith("Right"))
                        {
                            right = obj;
                            Debug.Log("Set RightOVRControllerVisual");
                        }
                    }

                    GlobalConstants.SetOVRControllerVisual(ref left, ref right);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Exception in InitializeOVRControllerVisual(): {e.ToString()}");
            }
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        void InitializePokeLocation()
        {
            try
            {
                if (GlobalConstants.LeftPokeObject == null)
                {
                    // make it only set once
                    var name = GlobalConstants.PokeLocation;
                    var objects = VRC2.Utils.FindAll(name);
                    foreach (var obj in objects)
                    {
                        var ppp = obj.transform.parent.parent.parent.gameObject;
                        // LeftController
                        if (ppp.name.StartsWith("Left"))
                        {
                            GlobalConstants.LeftPokeObject = obj;

                            Debug.Log("Set LeftPokeObject");
                        }
                        else if (ppp.name.StartsWith("Right")) // RightController
                        {
                            GlobalConstants.RightPokeObject = obj;
                            Debug.Log("Set RightPokeObject");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Exception in InitializePokeLocation(): {e.ToString()}");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
            // This is processed in ClampHintCollisionDetector
            // var go = other.gameObject;
            // if (go.CompareTag(GlobalConstants.clampObjectTag))
            // {
            //     // Enable ClampHintCollisionDetector
            //     var chm = gameObject.GetComponent<ClampHintManager>();
            //     chm.gameObject.SetActive(true);
            //     chm.SetClamped(false);
            //     print("pipe set clamped false");
            // }
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnterAndStay(other);
        }

        void OnTriggerEnterAndStay(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(GlobalConstants.pipeObjectTag))
            {
                HandlePipeCollision(go);
            }

            // collision with clamp
            else if (go.CompareTag(GlobalConstants.clampObjectTag))
            {
                // handle it in ClampHintCollisionDetector

                //HandleClampCollision(go);
            }

            // collision with glue
            else if (go.CompareTag(GlobalConstants.glueObjectTag))
            {
                HandleGlueCollision(go);
            }
        }

        void HandleGlueCollision(GameObject glue)
        {
            // only process if glue is being selected
            // glue interactable
            var gip = glue.transform.parent.gameObject;
            if (!gip.GetComponent<GlueGrabbingCallback>().beingSelected) return;

            // check glue capacity
            if (GlobalConstants.IsGlueUsedOut)
            {
                Debug.LogError("[PipeCollisionDetector] Glue is used out.");
                return;
            }

            glued += 1;
            if (glued >= minimumGlue)
            {
                // show glue hint
                hintManager.ShowHintFor(gameObject);
            }
        }

        void HandleClampCollision(GameObject clamp)
        {
            // get the Interactable clamp
            var iclamp = clamp.transform.parent.gameObject;

            // delete its rigid body
            Rigidbody rb = null;
            if (iclamp.TryGetComponent<Rigidbody>(out rb))
            {
                // clamp doesn't collide with the wall
                Debug.Log("Clamp doesn't collide with the wall.");
                return;
            }

            // current interactable pipe
            var cip = gameObject.transform.parent.parent;

            // find root parent (this is caused by the pipe merging)
            var root = cip;
            while (true)
            {
                if (root.parent == null) break;

                root = root.parent;
            }

            // remove the rigid body of the root object
            if (root.gameObject.TryGetComponent<Rigidbody>(out rb))
            {
                // remove its rigid body
                Debug.Log($"Remove rigid body for pipe {root.gameObject.name}");
                GameObject.Destroy(rb);
            }
        }

        bool RightHandHoldRightPipe(GameObject otherpipe)
        {
            // always return true for the client side
            if (Runner != null && Runner.IsRunning && Runner.IsClient) return true;

            // current interactable pipe
            // only move the pipe held by the right hand to right
            var leftHandPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            var rightHandPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            // other pipe distance should be left hand > right hand
            var otherIpipePos = otherpipe.transform.position;
            if (Vector3.Distance(otherIpipePos, leftHandPos) <
                Vector3.Distance(otherIpipePos, rightHandPos))
            {
                // Other pipe is held by the left hand
                return false;
            }

            // now, otherpipe is on the right-hand
            return true;
        }

        bool IsGlued()
        {
            // enable glue hint for both sides, so this is unnecessary.
            // if (Runner != null && Runner.IsRunning && Runner.IsClient) return true;

            // // fix for connector 
            // if (transform.parent.name.EndsWith("connector")) return true;

            return hintManager.glued;
        }

        bool CheckOtherPipe(GameObject otherpipe)
        {
            // other pipe must be a simple pipe
            var parent = otherpipe.transform.parent;

            return parent.parent == null;
        }

        bool OnTheWall(GameObject root)
        {
            PipeManipulation pm = root.GetComponent<PipeManipulation>();
            PipesContainerManager pcm = root.GetComponent<PipesContainerManager>();
            if (pm != null)
            {
                return pm.collidingWall;
            }
            else if (pcm != null)
            {
                return pcm.collidingWall;
            }

            return false;
        }

        void HandlePipeCollision(GameObject otherpipe)
        {
            if (connected) return;

            if (!IsGlued())
            {
                // disable this warning as it will print a lot
                // Debug.LogWarning("Please glue it first");
                return;
            }

            if (!RightHandHoldRightPipe(otherpipe)) return;

            if (!CheckOtherPipe(otherpipe)) return;

            var cip = gameObject.transform.parent;
            var oip = otherpipe.transform.parent.gameObject; // other interactable pipe

            var d1 = cip.GetComponent<PipeManipulation>().diameter;
            var d2 = oip.GetComponent<PipeManipulation>().diameter;

            if (d1 != d2)
            {
                Debug.LogWarning("Different diameters of pipes can not connect");
                return;
            }

            // can not connector 2 connectors
            if (cip.name.EndsWith("connector") && cip.name.Equals(oip.name))
            {
                Debug.LogWarning(" Can not connect 2 connectors");
                return;
            }

            Debug.Log($"HandlePipeCollision: {otherpipe.name}");

            // disable glue hint first
            hintManager.HideHint();

            // disable other pipe clamp hit
            otherpipe.GetComponent<ClampHintManager>().Hide();

            var cipRoot = gameObject.transform.parent;

            // get root cip
            while (true)
            {
                if (cipRoot.parent == null) break;
                cipRoot = cipRoot.parent;
            }



            // check whether cipRoot is pipe container
            PipesContainerManager pcm = null;
            if (cipRoot.TryGetComponent<PipesContainerManager>(out pcm))
            {
                // it's a pipe container
                if (pcm.AttachedToController())
                {
                    // attached to controller
                    // detach it
                    pcm.DetachController();
                }
            }

            // disable interactions
            PipeHelper.DisableInteraction(cipRoot.gameObject);
            PipeHelper.DisableInteraction(oip.gameObject);


            //// Fix the left part, and move the right part
            var newPivot = GetRightPipeNewPivot(gameObject, otherpipe);

            var parentPos = GetRightPipeRootPivot(gameObject, otherpipe, newPivot);

            var parentRot = GetRightPipeRootRotation(gameObject, otherpipe, newPivot);

            oip.transform.position = parentPos;
            oip.transform.rotation = parentRot;

            // initialize parent at the other pipe position
            var pos = oip.transform.position;

            // get the rotation based on the left controller's rotation
            var rot = GetParentRotation(oip);

            // disable all components
            DisableAllComponents(cipRoot.gameObject);

            // if cip is on th wall
            var cipOnWall = OnTheWall(cipRoot.gameObject);

            InitializeParent(cipRoot.gameObject, oip, pos, rot, otherpipe, cipOnWall);

            // // initialize a parent object
            // var parentObject = Instantiate(pipeParent, pos, rot) as GameObject;
            //
            // // update parent
            // oip.transform.parent = parentObject.transform;
            // cipRoot.transform.parent = parentObject.transform;
            //
            // // set parent to attach the the left-hand controller
            // parentObject.GetComponent<PipesContainerManager>()
            //     .AttachToController(leftVisual);

            // update glue hint flags
            // show left one
            gameObject.GetComponent<ClampHintManager>().SetCanShow(true);
            // hide right one
            otherpipe.GetComponent<ClampHintManager>().SetCanShow(false);

            // make the end of the other pipe not connectable
            otherpipe.GetComponent<PipeCollisionDetector>().connected = true;

            connected = true;
        }

        #region Pipe Connecting

        /// <summary>
        /// This is a hack way to disable all components
        /// </summary>
        /// <param name="obj"></param>
        void DisableAllComponents(GameObject obj)
        {
            // backup parent
            var p = obj.transform.parent;
            // disable cip components
            var t = new GameObject();
            t.transform.position = obj.transform.position;
            t.transform.rotation = obj.transform.rotation;

            obj.transform.parent = t.transform;
            GameObject.Destroy(t);

            // restore parent
            obj.transform.parent = p;
        }

        Vector3 GetRightPipeNewPivot(GameObject currentpipe, GameObject otherpipe)
        {
            // pipe length in the x direction
            var ox = PipeHelper.GetExtendsX(otherpipe);

            // right center, and right vector
            var (cc, cr) = PipeHelper.GetRightMostCenter(currentpipe);

            // initialize a new gameobject at the cc position, and rotation
            var obj = new GameObject();
            var up = currentpipe.transform.up;
            var forward = Vector3.Cross(cr, up);

            obj.transform.position = cc;
            obj.transform.rotation = Quaternion.LookRotation(forward, up);
            // translate to get the pivot position for the other pipe
            obj.transform.Translate(ox, 0, 0);

            var pos = obj.transform.position; // this is where the right pipe's pivot should be

            // destory the new object
            GameObject.Destroy(obj);
            return pos;
        }

        Vector3 GetRightPipeRootPivot(GameObject currentpipe, GameObject otherpipe, Vector3 newpivot)
        {
            var (cc, cr) = PipeHelper.GetRightMostCenter(currentpipe);

            var parent = otherpipe.transform.parent;

            var obj = new GameObject();
            obj.transform.position = otherpipe.transform.position;
            obj.transform.rotation = otherpipe.transform.rotation;

            var parentLocalPos = obj.transform.InverseTransformPoint(parent.position);

            // set new position
            obj.transform.position = newpivot;

            var forward = currentpipe.transform.forward;
            var up = Vector3.Cross(-cr, forward);
            var newRot = Quaternion.LookRotation(forward, up);

            // set new rotation
            obj.transform.rotation = newRot;

            // get the new position of the parent object
            var pos = obj.transform.TransformPoint(parentLocalPos);

            // destroy
            GameObject.Destroy(obj);

            return pos;
        }

        Quaternion GetRightPipeRootRotation(GameObject currentpipe, GameObject otherpipe, Vector3 newPivot)
        {
            var rot = currentpipe.transform.rotation;

            var (cc, cr) = PipeHelper.GetRightMostCenter(currentpipe);
            var forward = currentpipe.transform.forward;
            var up = Vector3.Cross(-cr, forward);
            var newRot = Quaternion.LookRotation(forward, up);

            var obj = new GameObject();
            obj.transform.position = otherpipe.transform.position;
            obj.transform.rotation = otherpipe.transform.rotation;

            otherpipe.transform.parent.parent = obj.transform;

            obj.transform.position = newPivot;
            obj.transform.rotation = newRot;

            rot = otherpipe.transform.parent.rotation;

            // this action will disable otherpipe.transform.parent's components
            GameObject.Destroy(obj);

            otherpipe.transform.parent.parent = null;

            return rot;
        }

        Quaternion GetParentRotation(GameObject oip)
        {
            // get angle
            var angle = oip.GetComponent<PipeManipulation>().angle;

            // var rot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

            var rot = oip.transform.rotation;

            var zoffset = 0;

            // rotate according to different pipe angles

            switch (angle)
            {
                case PipeConstants.PipeBendAngles.Angle_0:
                    break;
                case PipeConstants.PipeBendAngles.Angle_45:
                    zoffset = 135;
                    break;
                case PipeConstants.PipeBendAngles.Angle_90:
                    zoffset = 90;
                    break;
                case PipeConstants.PipeBendAngles.Angle_135:
                    zoffset = 45;
                    break;
                default:
                    break;
            }

            var vec = rot.eulerAngles;
            vec.z += zoffset;

            rot = Quaternion.Euler(vec);

            return rot;
        }

        #endregion

        #region Network Behavior

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_SendMessage(NetworkId cid, NetworkId oid, NetworkId parent,
            Vector3 clocalpos, Quaternion clocalrot, Vector3 olocalpos, Quaternion olocalrot, string oconnected,
            RpcInfo info = default)
        {
            var message = "";

            if (info.IsInvokeLocal)
                message = $"You sent container: {cid} {oid} {parent}\n";
            else
            {
                message = $"Some other said container: {cid} {oid} {parent}\n";

                _cid = cid;
                _oid = oid;
                _pid = parent;
                _cLocalPos = clocalpos;
                _cLocalRot = clocalrot;
                _oLocalPos = olocalpos;
                _oLocalRot = olocalrot;
                _oconnected = oconnected;

                requiredUpdated = true;
            }

            Debug.LogWarning(message);
        }

        internal GameObject GetChildByName(GameObject parent, string name)
        {
            var children = Utils.GetChildren(parent);
            foreach (var child in children)
            {
                if (child.name == name) return child;
            }

            return null;
        }

        void UpdateOnClient()
        {
            // update local side (client side)
            var runner = GlobalConstants.networkRunner;

            var cip = runner.FindObject(_cid).gameObject;
            var oip = runner.FindObject(_oid).gameObject;
            var parentObj = runner.FindObject(_pid).gameObject;

            print("UpdateOnClient");

            print(cip.name);
            print(oip.name);
            print(parentObj.name);
            print($"{_cid} - {_oid} - {_pid} - {_oconnected}");

            // update reference
            if (parentObj.TryGetComponent<PipesContainerManager>(out PipesContainerManager pcm1))
            {
                pcm1.SetReference(ref cip, ref oip);
            }

            // disable network transform
            DisableNetworkTransform(ref cip);
            DisableNetworkTransform(ref oip);

            // disable interaction
            PipeHelper.DisableInteraction(cip);
            PipeHelper.DisableInteraction(oip);
            PipeHelper.DisableInteraction(parentObj);

            // cip is a pipe
            if (cip.TryGetComponent<GlueHintManager>(out GlueHintManager m1))
            {
                // disable glue hint in cip
                m1.HideHint();
            }

            // cip is a container
            if (cip.TryGetComponent<PipesContainerManager>(out PipesContainerManager pcm2))
            {
                if (pcm2.oip.TryGetComponent<GlueHintManager>(out GlueHintManager m2))
                {
                    m2.HideHint();
                }
            }

            // hide right one glue hint
            var chms = oip.GetComponentsInChildren<ClampHintManager>();
            foreach (var chm in chms)
            {
                if (chm.gameObject.name.Equals(_oconnected))
                {
                    chm.SetCanShow(false);
                }
            }

            cip.transform.parent = parentObj.transform;
            oip.transform.parent = parentObj.transform;

            cip.transform.localPosition = _cLocalPos;
            cip.transform.localRotation = _cLocalRot;

            oip.transform.localPosition = _oLocalPos;
            oip.transform.localRotation = _oLocalRot;

            requiredUpdatedDone = true;
        }

        void ReleaseOip(GameObject oip)
        {
            var pgft = oip.GetComponent<PipeGrabFreeTransformer>();
            if (pgft != null)
            {
                pgft.SimulateRelease();
            }
        }


        void InitializeParent(GameObject cip, GameObject oip, Vector3 pos, Quaternion rot, GameObject contact,
            bool ciponwall)
        {
            if (Runner != null && Runner.IsRunning)
            {
                if (Runner.IsServer)
                {
                    var player = GlobalConstants.localPlayer;
                    var prefab = PipeHelper.GetPipeContainerPrefab();
                    var spo = Runner.Spawn(prefab, pos, rot, player);

                    var parentObject = spo.gameObject;

                    oip.transform.parent = parentObject.transform;
                    cip.transform.parent = parentObject.transform;

                    var pcm = parentObject.GetComponent<PipesContainerManager>();
                    if (ciponwall)
                    {
                        // cip is on the wall, disable interaction
                        pcm.SetInteraction(false);
                    }
                    else
                    {
                        // set parent to attach to the left-hand controller only if it's not on the wall
                        pcm.AttachToController(leftViusal);
                    }

                    // release oip
                    ReleaseOip(oip);

                    // update reference
                    pcm.SetReference(ref cip, ref oip, ref contact);

                    // disable networktransform
                    DisableNetworkTransform(ref cip);
                    DisableNetworkTransform(ref oip);

                    // send message
                    var pid = spo.Id;
                    var oid = oip.GetComponent<NetworkObject>().Id;
                    var cid = cip.GetComponent<NetworkObject>().Id;

                    var cipt = cip.transform;
                    var oipt = oip.transform;

                    print(
                        $"local cip: {cipt.localPosition.ToString("f5")} {cipt.localRotation.eulerAngles.ToString("f5")}");
                    print(
                        $"local oip: {oipt.localPosition.ToString("f5")} {oipt.localRotation.eulerAngles.ToString("f5")}");

                    RPC_SendMessage(cid, oid, pid,
                        cipt.localPosition, cipt.localRotation,
                        oipt.localPosition, oipt.localRotation,
                        contact.name);
                }
            }
            else
            {
                // initialize locally
                var parentObject = Instantiate(pipeParent, pos, rot) as GameObject;

                // update parent
                oip.transform.parent = parentObject.transform;
                cip.transform.parent = parentObject.transform;

                // update diameter
                var diameter = oip.GetComponent<PipeManipulation>().diameter;
                var pcm = parentObject.GetComponent<PipesContainerManager>();
                pcm.UpdateDiameter(diameter);

                if (ciponwall)
                {
                    // cip is on the wall, disable interaction
                    pcm.SetInteraction(false);
                }
                else
                {
                    // set parent to attach to the left-hand controller only if it's not on the wall
                    pcm.AttachToController(leftViusal);
                }

                // release oip
                ReleaseOip(oip);

                // update reference
                pcm.SetReference(ref cip, ref oip, ref contact);
            }
        }

        void DisableNetworkTransform(ref GameObject obj)
        {
            var nt = obj.GetComponent<NetworkTransform>();
            nt.enabled = false;
        }

        #endregion
    }
}