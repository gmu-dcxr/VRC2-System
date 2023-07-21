using System;
using System.Collections.Concurrent;
using System.Linq;
using Fusion;
using NodeCanvas.Tasks.Actions;
using Oculus.Interaction.DistanceReticles;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VRC2.Pipe;
using Object = UnityEngine.Object;

namespace VRC2.Events
{
    public class PipeCollisionDetector : NetworkBehaviour
    {
        private Object pipeParent;

        private bool connected = false;

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

        private Vector3 _cLocalPos;
        private Quaternion _cLocalRot;
        private Vector3 _oLocalPos;
        private Quaternion _oLocalRot;
        private NetworkId _cid;
        private NetworkId _oid;
        private NetworkId _pid;

        #endregion




        private void Start()
        {
            InitializeOVRControllerVisual();
            InitializePokeLocation();
            // pre-load object
            pipeParent = AssetDatabase.LoadAssetAtPath(GlobalConstants.pipePipeConnectorPrefabPath, typeof(GameObject));
        }

        private void Update()
        {
        }

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
            if (go.CompareTag(GlobalConstants.clampObjectTag))
            {
                HandleClampCollision(go);
            }

            // collision with glue
            if (go.CompareTag(GlobalConstants.glueObjectTag))
            {
                HandleGlueCollision(go);
            }
        }

        void HandleGlueCollision(GameObject glue)
        {
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

            // remove pipe's rigid boby
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
            if (Runner != null && Runner.IsRunning && Runner.IsClient) return true;

            return hintManager.glued;
        }

        void HandlePipeCollision(GameObject otherpipe)
        {
            if (connected) return;

            if (!IsGlued())
            {
                Debug.LogWarning("Please glue it first");
                return;
            }

            if (!RightHandHoldRightPipe(otherpipe)) return;

            var cip = gameObject.transform.parent;
            var oip = otherpipe.transform.parent.gameObject; // other interactable pipe

            var d1 = cip.GetComponent<PipeManipulation>().diameter;
            var d2 = oip.GetComponent<PipeManipulation>().diameter;

            if (d1 != d2)
            {
                Debug.LogWarning("Different diameters of pipes can not connect");
                return;
            }



            Debug.Log($"HandlePipeCollision: {otherpipe.name}");

            // disable glue hint first
            hintManager.HideHint();

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

            InitializeParent(cipRoot.gameObject, oip, pos, rot);

            // // initialize a parent object
            // var parentObject = Instantiate(pipeParent, pos, rot) as GameObject;
            //
            // // update parent
            // oip.transform.parent = parentObject.transform;
            // cipRoot.transform.parent = parentObject.transform;
            //
            // // set parent to attach the the left-hand controller
            // parentObject.GetComponent<PipesContainerManager>()
            //     .AttachToController(GlobalConstants.LeftOVRControllerVisual);

            connected = true;
        }

        #region Pipe Connecting

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

            GameObject.Destroy(obj);

            otherpipe.transform.parent.parent = null;

            return rot;
        }

        Quaternion GetParentRotation(GameObject oip)
        {
            // get angle
            var angle = oip.GetComponent<PipeManipulation>().angle;

            var rot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

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
            Vector3 clocalpos, Quaternion clocalrot, Vector3 olocalpos, Quaternion olocalrot,
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


        void InitializeParent(GameObject cip, GameObject oip, Vector3 pos, Quaternion rot)
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

                    // set parent to attach the the left-hand controller
                    parentObject.GetComponent<PipesContainerManager>()
                        .AttachToController(GlobalConstants.LeftOVRControllerVisual);

                    // disable networktransform
                    DisableNetworkTransform(ref cip);
                    DisableNetworkTransform(ref oip);

                    // send message
                    var pid = spo.Id;
                    var oid = oip.GetComponent<NetworkObject>().Id;
                    var cid = cip.GetComponent<NetworkObject>().Id;

                    var cipt = cip.transform;
                    var oipt = oip.transform;

                    RPC_SendMessage(cid, oid, pid,
                        cipt.localPosition, cipt.localRotation,
                        oipt.localPosition, oipt.localRotation);
                }
                else
                {
                    // update local side (client side)
                    var runner = GlobalConstants.networkRunner;

                    var parentObj = runner.FindObject(_pid).gameObject;

                    // disable network transform
                    DisableNetworkTransform(ref cip);
                    DisableNetworkTransform(ref oip);

                    // // disable interaction
                    // PipeHelper.DisableInteraction(cip);
                    // PipeHelper.DisableInteraction(oip);

                    while (cip.transform.parent != parentObj.transform)
                    {
                        print("Update cip parent");
                        cip.transform.parent = parentObj.transform;
                    }

                    while (oip.transform.parent != parentObj.transform)
                    {
                        print("Update oip parent");
                        oip.transform.parent = parentObj.transform;
                    }

                    cip.transform.localPosition = _cLocalPos;
                    cip.transform.localRotation = _cLocalRot;

                    oip.transform.localPosition = _oLocalPos;
                    oip.transform.localRotation = _oLocalRot;


                    print("Update parent");
                }
            }
            else
            {
                // initialize locally
                var parentObject = Instantiate(pipeParent, pos, rot) as GameObject;

                // update parent
                oip.transform.parent = parentObject.transform;
                cip.transform.parent = parentObject.transform;

                // set parent to attach the the left-hand controller
                parentObject.GetComponent<PipesContainerManager>()
                    .AttachToController(GlobalConstants.LeftOVRControllerVisual);
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