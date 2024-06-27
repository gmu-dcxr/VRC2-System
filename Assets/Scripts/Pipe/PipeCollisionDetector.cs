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

                // make sure it is enabled
                if (_glueHintManager != null && !_glueHintManager.enabled)
                {
                    _glueHintManager.enabled = true;
                }

                return _glueHintManager;
            }
        }

        private GameObject _wall;

        private GameObject wall
        {
            get
            {
                if (_wall == null)
                {
                    print("_wall is set");
                    _wall = GameObject.FindGameObjectWithTag(GlobalConstants.wallTag);
                }

                return _wall;
            }
        }

        private WallCollisionDetector _wallCollisionDetector;

        private WallCollisionDetector wallCollisionDetector
        {
            get
            {
                if (_wallCollisionDetector == null)
                {
                    print("_wallCollisionDetector is set");
                    _wallCollisionDetector = wall.GetComponent<WallCollisionDetector>();
                }

                return _wallCollisionDetector;
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

        private GameObject leftViusal => vrHelper.leftVisual;
        private GameObject rightViusal => vrHelper.leftVisual;
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
            // BUG: sometimes it doesn't work
            // var leftHandPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            // var rightHandPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            var leftHandPos = leftViusal.transform.position;
            var rightHandPos = rightViusal.transform.position;

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

            // var ppos = GetRightPipeNewPivot(gameObject, otherpipe);
            // get new oip transform and parent rotation
            var (oipp, oipr, ppos, prot) = GetRightPipeRootTransform(gameObject, otherpipe);

            // get the relative position and rotation after connecting
            var (clp, clr, olp, olr) = GetRelativeTransform(ppos, prot, cipRoot.gameObject, oip, oipp, oipr);

            // precess the label
            // PostprocessOIPLabel(cipRoot.gameObject, oip.gameObject);

            // disable all components
            DisableAllComponents(cipRoot.gameObject);

            // if cip is on th wall and distance is small enough
            var cipOnWall = OnTheWall(cipRoot.gameObject) && wallCollisionDetector.ShouldCompensate(cipRoot.position);

            InitializeParent(cipRoot.gameObject, oip, ppos, prot, clp, clr, olp, olr, otherpipe, cipOnWall);

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

        /// <summary>
        /// Get the new pivot position of right pipe segment
        ///
        /// current pipe is the segment not the whole pipe, the same as other pipe
        /// </summary>
        /// <param name="currentpipe"></param>
        /// <param name="otherpipe"></param>
        /// <returns></returns>
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

            // destroy the new object
            GameObject.Destroy(obj);
            return pos;
        }

        // Deprecated because of a simpler implementation, i.e., changing the zoffset
        /// <summary>
        /// Post process the calculated oip transform for the simple connector
        /// </summary>
        /// <param name="oip"></param> the right hand whole pipe (should be a connector) transform
        /// <param name="refer"></param>
        /// <param name="pp"></param>
        /// <param name="pr"></param>
        /// <returns></returns>
        (Vector3, Quaternion) PostProcessConnector(Transform oip, GameObject refer, Vector3 pp, Quaternion pr)
        {
            var pft = oip.GetComponent<PipeGrabFreeTransformer>();
            var pcm = oip.GetComponent<PipeConnectorManipulation>();

            if (pft == null || pcm == null || !pft.IsSimpleConnector || !pcm.Flipped)
            {
                // return if not a simple connector or not flipped
                return (pp, pr);
            }

            // rotate the parent
            var ot = refer.transform;
            var p = ot.InverseTransformPoint(oip.position);
            var f = ot.InverseTransformVector(oip.forward);
            var u = ot.InverseTransformVector(oip.up);
            // rotate ot
            refer.transform.Rotate(Vector3.right, 180f, Space.Self);
            // change it backup to the word coordinate
            ot = refer.transform;
            pp = ot.TransformPoint(p);
            f = ot.TransformVector(f);
            u = ot.TransformVector(u);
            // update cip
            pr = Quaternion.LookRotation(f, u);

            return (pp, pr);
        }


        /// <summary>
        /// Get the right hand whole pipe transform
        ///
        /// current pipe and other pipe are both segments
        /// </summary>
        /// <param name="currentpipe"></param>
        /// <param name="otherpipe"></param>
        /// <returns>oip position, oip rotation, parent position and parent rotation</returns>
        (Vector3, Quaternion, Vector3, Quaternion) GetRightPipeRootTransform(GameObject currentpipe,
            GameObject otherpipe)
        {
            // pipe length in the x direction
            var ox = PipeHelper.GetExtendsX(otherpipe);
            var (cc, cr) = PipeHelper.GetRightMostCenter(currentpipe);

            // whole pipe 
            var oip = otherpipe.transform.parent;

            // initialize a new object
            var obj1 = new GameObject();
            var obj2 = new GameObject();
            var pup = currentpipe.transform.up;
            var pforward = Vector3.Cross(cr, pup);
            obj1.transform.position = cc;
            obj1.transform.rotation = Quaternion.LookRotation(pforward, pup);
            obj1.transform.Translate(ox, 0, 0);

            // this is the position of the parent
            var newPos = obj1.transform.position;

            // update with the other pipe transform
            obj2.transform.position = otherpipe.transform.position;
            obj2.transform.rotation = otherpipe.transform.rotation;

            // set to parent
            oip.parent = obj2.transform;

            var forward = currentpipe.transform.forward;
            // get up vector from forward and left (-cr) of the left-hand pipe
            var up = Vector3.Cross(-cr, forward);
            // generate a rotation use the left-hand pipe forward and up
            var newRot = Quaternion.LookRotation(forward, up);

            // set new position
            obj2.transform.position = newPos;
            // set new rotation
            obj2.transform.rotation = newRot;

            // get parent position and rotation
            var pp = oip.position;
            var pr = oip.rotation;

            oip.parent = null;

            // (pp, pr) = PostProcessConnector(oip, obj, pp, pr);

            // destroy
            GameObject.Destroy(obj1);
            GameObject.Destroy(obj2);

            // get parent position
            return (pp, pr, newPos, newRot);
        }

        /// <summary>
        /// Sometimes world transform doesn't work well for connecting, calculate the relative transform before connecting
        /// </summary>
        /// <param name="p">parent position</param>
        /// <param name="r">parent rotation</param>
        /// <param name="cipRoot">cip root</param>
        /// <param name="oipRoot">oip root</param>
        /// <param name="oipp">calculated oip world position</param>
        /// <param name="oipr">calculated oip world rotation</param>
        /// <returns></returns>
        (Vector3, Quaternion, Vector3, Quaternion) GetRelativeTransform(Vector3 p, Quaternion r, GameObject cipRoot,
            GameObject oipRoot, Vector3 oipp, Quaternion oipr)
        {
            // initialize a new object using the other pipe transform
            var obj = new GameObject();
            obj.transform.position = p;
            obj.transform.rotation = r;

            // update oip root position
            oipRoot.transform.position = oipp;
            oipRoot.transform.rotation = oipr;

            cipRoot.transform.parent = obj.transform;
            oipRoot.transform.parent = obj.transform;

            var clp = cipRoot.transform.localPosition;
            var clr = cipRoot.transform.localRotation;

            var olp = oipRoot.transform.localPosition;
            var olr = oipRoot.transform.localRotation;

            // de parent
            cipRoot.transform.parent = null;
            oipRoot.transform.parent = null;

            // destroy
            GameObject.Destroy(obj);

            return (clp, clr, olp, olr);
        }

        // Deprecated because of a simpler implementation, i.e., changing the zoffset
        /// <summary>
        /// Flip the label because of the flipped connector
        /// </summary>
        /// <param name="ciproot"></param>
        /// <param name="oip"></param>
        void PostprocessOIPLabel(GameObject ciproot, GameObject oip)
        {
            var pcm = ciproot.GetComponent<PipesContainerManager>();
            var pconnm = oip.GetComponent<PipeConnectorManipulation>();

            if (pcm != null && pconnm == null)
            {
                // left is a container and right is not a container
                var oippcm = pcm.oip.GetComponent<PipeConnectorManipulation>();
                if (oippcm != null && oippcm.Flipped)
                {
                    // oip in container is a connector and flipped
                    // flip the text label to make it look nicer
                    var pm = oip.GetComponent<PipeManipulation>();
                    var scale = pm.pipeLabel.transform.localScale;
                    // just flip the x
                    scale.x *= -1;
                    pm.pipeLabel.transform.localScale = scale;
                }
            }
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

        /// <summary>
        /// Initialize parent and update transforms
        /// </summary>
        /// <param name="cip">current pipe root</param>
        /// <param name="oip">other pipe root</param>
        /// <param name="pos">parent position</param>
        /// <param name="rot">parent rotation</param>
        /// <param name="ciplp">cip local position</param>
        /// <param name="ciplr">cip local rotation</param>
        /// <param name="oiplp">oip local position</param>
        /// <param name="oiplr">oip local rotation</param>
        /// <param name="contact">contact part of oip</param>
        /// <param name="ciponwall">if cip is on the wall</param>
        void InitializeParent(GameObject cip, GameObject oip, Vector3 pos, Quaternion rot,
            Vector3 ciplp, Quaternion ciplr,
            Vector3 oiplp, Quaternion oiplr, GameObject contact,
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

                    oip.transform.localPosition = oiplp;
                    oip.transform.localRotation = oiplr;
                    cip.transform.localPosition = ciplp;
                    cip.transform.localRotation = ciplr;

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

                    RPC_SendMessage(cid, oid, pid, ciplp, ciplr, oiplp, oiplr, contact.name);
                }
            }
            else
            {
                // initialize locally
                var parentObject = Instantiate(pipeParent, pos, rot) as GameObject;

                // update parent
                oip.transform.parent = parentObject.transform;
                cip.transform.parent = parentObject.transform;

                oip.transform.localPosition = oiplp;
                oip.transform.localRotation = oiplr;

                cip.transform.localPosition = ciplp;
                cip.transform.localRotation = ciplr;

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