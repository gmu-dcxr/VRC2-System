using System;
using System.Collections.Concurrent;
using System.Linq;
using NodeCanvas.Tasks.Actions;
using Oculus.Interaction.DistanceReticles;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VRC2.Pipe;
using Object = UnityEngine.Object;

namespace VRC2.Events
{
    public class PipeCollisionDetector : MonoBehaviour
    {
        private Object pipeParent;

        private bool connected = false;

        private GameObject parentObject;

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

        void HandlePipeCollision(GameObject otherpipe)
        {
            if (connected) return;

            if (!hintManager.glued)
            {
                Debug.LogWarning("Please glue it first");
                return;
            }

            Debug.Log($"HandlePipeCollision: {otherpipe.name}");

            // disable glue hint first
            hintManager.HideHint();

            var cip = gameObject.transform.parent;
            var cipRoot = gameObject.transform.parent;

            // get root cip
            while (true)
            {
                if (cipRoot.parent == null) break;
                cipRoot = cipRoot.parent;
            }

            var oip = otherpipe.transform.parent.gameObject; // other interactable pipe

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
            DisableInteraction(cipRoot.gameObject);
            DisableInteraction(oip.gameObject);

            // get extends
            var cid = PipeHelper.GetExtendsX(gameObject);
            var oid = PipeHelper.GetExtendsX(otherpipe);

            print(cid);
            print(oid);

            var offset = Vector3.zero;
            offset.x = cid + oid;

            // // make a dummy object to calculate the target position
            // var dummy = new GameObject();
            // dummy.transform.position = cip.transform.position;
            // dummy.transform.rotation = cip.transform.rotation;
            // dummy.transform.Translate(offset, Space.Self);
            // var targetPos = dummy.transform.position;

            // initialize parent at the current pipe position and left controller's height
            var pos = cipRoot.transform.position;
            pos.y = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch).y;

            var rot = cipRoot.transform.rotation;
            parentObject = Instantiate(pipeParent, pos, rot) as GameObject;
            // set parent
            cipRoot.transform.parent = parentObject.transform;
            // oip.transform.position = targetPos;
            // oip.transform.rotation = cip.transform.rotation;
            oip.transform.parent = parentObject.transform;

            // update local position
            var localCip = cipRoot.transform.localPosition;
            var localOip = oip.transform.localPosition;

            localCip.x = -(cid + oid); // move it to left 
            localCip.y = 0;
            localCip.z = 0;

            localOip.x = 0;
            localOip.y = 0;
            localOip.z = 0;

            cipRoot.transform.localPosition = localCip;
            oip.transform.localPosition = localOip;


            // fix local rotation
            rot = Quaternion.Euler(0, 0, 0);
            cipRoot.transform.localRotation = rot;
            oip.transform.localRotation = rot;

            // set parent to attach the the left-hand controller
            parentObject.GetComponent<PipesContainerManager>()
                .AttachToController(GlobalConstants.LeftOVRControllerVisual);

            connected = true;
        }

        void DisableRigidBody(GameObject interactable)
        {
            Rigidbody rb = null;
            if (interactable.TryGetComponent<Rigidbody>(out rb))
            {
                // delete its rigid body
                GameObject.Destroy(rb);
            }
        }

        void DisableInteraction(GameObject interactable)
        {
            // disable GrabInteractable who own ReticleDataIcon
            var go = interactable.GetComponentInChildren<ReticleDataIcon>();
            if (go != null)
            {
                go.gameObject.SetActive(false);
                DisableRigidBody(interactable);
            }
        }
    }
}