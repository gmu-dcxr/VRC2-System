using UnityEngine;
using Fusion;
using Oculus.Interaction;
using Oculus.Interaction.DistanceReticles;
using Unity.VisualScripting;
using UnityEditor;
using PipeParameters = VRC2.Pipe.PipeConstants.PipeParameters;
using PipeDiameter = VRC2.Pipe.PipeConstants.PipeDiameter;
using PipeBendAngles = VRC2.Pipe.PipeConstants.PipeBendAngles;

namespace VRC2.Pipe
{
    public static class PipeHelper
    {
        public static void BeforeMove(ref GameObject interactablePipe)
        {
            // remove rigid body, interactable, and collision detector

            // remove its rigid body
            var rb = interactablePipe.GetComponent<Rigidbody>();
            if (rb != null)
            {
                GameObject.Destroy(rb);   
            }

            // disable its interactable ability
            var reticle = interactablePipe.GetComponentInChildren<ReticleDataIcon>();
            if (reticle != null)
            {
                reticle.gameObject.SetActive(false);   
            }
        }

        public static void EnsureRigidBody(ref GameObject obj)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                // add new one
                rb = obj.AddComponent<Rigidbody>();
            }

            // update detection method
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.useGravity = true;
        }

        public static void EnsureNoRigidBody(ref GameObject obj)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                GameObject.Destroy(rb);
            }
        }

        public static void AfterMove(ref GameObject interactablePipe)
        {
            // restore rigid body if need
            var rb = interactablePipe.GetComponent<Rigidbody>();
            if (rb == null)
            {
                // add new one
                rb = interactablePipe.AddComponent<Rigidbody>();
            }

            // update detection method
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.useGravity = true;

            // enable interactable ability
            var reticle = interactablePipe.GetComponentInChildren<ReticleDataIcon>(true);
            reticle.gameObject.SetActive(true);

            // update rigid body
            var si = interactablePipe.GetComponent<SnapInteractor>();
            si.InjectRigidbody(rb);

            var dgi = interactablePipe.GetComponentInChildren<DistanceGrabInteractable>();
            dgi.InjectRigidbody(rb);
        }

        public static float GetExtendsX(GameObject pipe)
        {
            var mesh = pipe.GetComponent<MeshFilter>().mesh;

            var vertices = mesh.vertices;

            var minx = vertices[0].x;
            var maxx = minx;

            foreach (var v in vertices)
            {
                if (v.x > maxx) maxx = v.x;
                if (v.x < minx) minx = v.x;
            }

            var p1 = Vector3.zero;
            p1.x = minx;

            var p2 = Vector3.zero;
            p2.x = maxx;

            var t = pipe.transform;

            p1 = t.TransformPoint(p1);
            p2 = t.TransformPoint(p2);

            return Vector3.Distance(p1, p2);
        }

        public static float GetExtendsZ(GameObject pipe)
        {
            // real diameter
            var mesh = pipe.GetComponent<MeshFilter>().mesh;

            var vertices = mesh.vertices;

            var minz = vertices[0].z;
            var maxz = minz;

            foreach (var v in vertices)
            {
                if (v.x > maxz) maxz = v.z;
                if (v.x < minz) minz = v.z;
            }

            var p1 = Vector3.zero;
            p1.x = minz;

            var p2 = Vector3.zero;
            p2.x = maxz;

            var t = pipe.transform;

            p1 = t.TransformPoint(p1);
            p2 = t.TransformPoint(p2);

            return Vector3.Distance(p1, p2);
        }

        public static float GetExtendsY(GameObject pipe)
        {
            // real diameter
            var mesh = pipe.GetComponent<MeshFilter>().mesh;

            var vertices = mesh.vertices;

            var miny = vertices[0].y;
            var maxy = miny;

            foreach (var v in vertices)
            {
                if (v.x > maxy) maxy = v.y;
                if (v.x < miny) miny = v.y;
            }

            var p1 = Vector3.zero;
            p1.x = miny;

            var p2 = Vector3.zero;
            p2.x = maxy;

            var t = pipe.transform;

            p1 = t.TransformPoint(p1);
            p2 = t.TransformPoint(p2);

            return Vector3.Distance(p1, p2);
        }

        public static Vector3 GetExtendsXYZ(GameObject pipe)
        {
            var x = GetExtendsX(pipe);
            var y = GetExtendsY(pipe);
            var z = GetExtendsZ(pipe);

            return new Vector3(x, y, z);
        }


        public static (Vector3, Vector3) GetRightMostCenter(GameObject pipe)
        {
            var mesh = pipe.GetComponent<MeshFilter>().mesh;

            var vertices = mesh.vertices;

            var minx = vertices[0].x;
            var maxx = minx;

            var miny = vertices[0].y;
            var maxy = miny;

            var minz = vertices[0].z;
            var maxz = minz;

            foreach (var v in vertices)
            {
                if (v.x > maxx) maxx = v.x;
                if (v.x < minx) minx = v.x;
                if (v.y > maxy) maxy = v.y;
                if (v.y < miny) miny = v.y;
                if (v.z > maxz) maxz = v.z;
                if (v.z < minz) minz = v.z;
            }

            var p1 = Vector3.zero;
            p1.x = maxx;
            p1.y = (miny + maxy) / 2.0f;
            p1.z = (minz + maxz) / 2.0f;

            var t = pipe.transform;

            var p2 = p1;
            p2.x = minx;

            p1 = t.TransformPoint(p1);
            p2 = t.TransformPoint(p2);

            var center = t.TransformPoint(Vector3.zero);

            var d1 = Vector3.Distance(p1, center);
            var d2 = Vector3.Distance(p2, center);

            if (d1 > d2)
            {
                return (p1, (p1 - p2).normalized);
            }
            else
            {
                return (p2, (p2 - p1).normalized);
            }
        }

        public static string GetPipePrefabName(PipeParameters para)
        {
            // format: {diameter} {angle} pipe
            var name = "";
            switch (para.diameter)
            {
                case PipeDiameter.Diameter_1:
                    name += "1";
                    break;
                case PipeDiameter.Diameter_2:
                    name += "2";
                    break;
                case PipeDiameter.Diameter_3:
                    name += "3";
                    break;
                case PipeDiameter.Diameter_4:
                    name += "4";
                    break;
                default:
                    break;
            }

            name += " inch ";

            switch (para.angle)
            {
                case PipeBendAngles.Angle_0:
                    name += "straight";
                    break;
                case PipeBendAngles.Angle_45:
                    name += "45 deg";
                    break;
                case PipeBendAngles.Angle_90:
                    name += "90 deg";
                    break;
                case PipeBendAngles.Angle_135:
                    name += "135 deg";
                    break;
                default:
                    break;
            }

            name += " pipe";
            return name;
        }

        public static GameObject GetPipePrefab(PipeParameters para)
        {
            var name = GetPipePrefabName(para);

            var path = $"{GlobalConstants.PipePrefabsPath}{name}.prefab";

            GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

            return go;
        }

        public static NetworkObject GetPipePrefabRef(PipeParameters para)
        {
            // TODO: PrefabTable is not working, why?
            // var table = NetworkProjectConfig.Global.PrefabTable;

            GameObject go = GetPipePrefab(para);

            var no = go.GetComponent<NetworkObject>();
            // var nid = no.NetworkGuid;

            // NetworkPrefabId npid;
            // NetworkObject networkObject = null;
            // if (table.TryGetId(nid, out npid))
            // {
            //     table.TryGetPrefab(npid, out networkObject);
            // }

            return no;
        }

        public static GameObject GetStraightPipePrefab(PipeDiameter diameter)
        {
            var para = new PipeParameters();
            para.diameter = diameter;
            para.angle = PipeBendAngles.Angle_0;

            return GetPipePrefab(para);
        }

        public static NetworkObject GetStraightPipePrefabRef(PipeDiameter diameter)
        {
            var para = new PipeParameters();
            para.diameter = diameter;
            para.angle = PipeBendAngles.Angle_0;

            return GetPipePrefabRef(para);
        }

        public static NetworkObject GetPipeContainerPrefab()
        {
            // TODO: PrefabTable is not working, why?
            // var table = NetworkProjectConfig.Global.PrefabTable;
            var container =
                AssetDatabase.LoadAssetAtPath(GlobalConstants.pipePipeConnectorPrefabPath, typeof(GameObject));
            var no = container.GetComponent<NetworkObject>();
            var nid = no.NetworkGuid;

            // NetworkPrefabId npid;
            // NetworkObject networkObject = null;
            // if (table.TryGetId(nid, out npid))
            // {
            //     table.TryGetPrefab(npid, out networkObject);
            // }

            return no;
        }

        public static void DisableRigidBody(GameObject interactable)
        {
            Rigidbody rb = null;
            if (interactable.TryGetComponent<Rigidbody>(out rb))
            {
                // delete its rigid body
                GameObject.Destroy(rb);
            }
        }

        public static void DisableInteraction(GameObject interactable)
        {
            // disable GrabInteractable who own ReticleDataIcon
            var go = interactable.GetComponentInChildren<ReticleDataIcon>();
            if (go != null)
            {
                go.gameObject.SetActive(false);
                DisableRigidBody(interactable);
            }
        }

        public static GameObject GetClampPrefab(int size)
        {
            var name = $"InteractableClampS{size}";

            var path = $"{GlobalConstants.ClampPrefabsPath}{name}.prefab";

            GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

            return go;
        }

        public static GameObject GetRoot(GameObject go)
        {
            var root = go.transform;
            while (true)
            {
                if (root.parent == null) break;

                root = root.parent;
            }

            return root.gameObject;
        }

        public static bool IsSimpleStraightNotcutPipe(GameObject go)
        {
            var root = GetRoot(go);
            var pm = root.GetComponent<PipeManipulation>();
            if (pm == null) return false;
            if (pm.angle != PipeBendAngles.Angle_0) return false;
            return pm.IsNotCut();
        }

        public static bool IsSimplePipe(GameObject go)
        {
            var root = GetRoot(go);
            var pm = root.GetComponent<PipeManipulation>();
            return pm != null;
        }

        public static void UpdateBoxColliders(GameObject pipe, bool enable)
        {
            var colliders = Utils.GetChildren<BoxCollider>(pipe);

            foreach (var go in colliders)
            {
                if (go.GetComponent<BoxCollider>() != null)
                {
                    go.GetComponent<BoxCollider>().enabled = enable;
                }
            }
        }

        #region Material

        public static string GetPipeMaterialPath(PipeParameters para)
        {

            var path = "";
            switch (para.diameter)
            {
                case PipeDiameter.Diameter_1:
                    path += "1 inch/1 ";
                    break;
                case PipeDiameter.Diameter_2:
                    path += "2 inch/2 ";
                    break;
                case PipeDiameter.Diameter_3:
                    path += "3 inch/3 ";
                    break;
                case PipeDiameter.Diameter_4:
                    path += "4 inch/4 ";
                    break;
                default:
                    break;
            }

            switch (para.type)
            {
                case PipeConstants.PipeType.Electrical:
                    path += "electrical ";
                    break;
                case PipeConstants.PipeType.Gas:
                    path += "gas ";
                    break;
                case PipeConstants.PipeType.Water:
                    path += "water ";
                    break;
                case PipeConstants.PipeType.Sewage:
                    path += "sewage ";
                    break;
                default:
                    break;
            }

            switch (para.color)
            {
                case PipeConstants.PipeColor.Blue:
                    path += "b";
                    break;
                case PipeConstants.PipeColor.Green:
                    path += "g";
                    break;
                case PipeConstants.PipeColor.Magenta:
                    path += "m";
                    break;
                case PipeConstants.PipeColor.Yellow:
                    path += "y";
                    break;

                default:
                    break;
            }

            var fullpath = $"{GlobalConstants.pipeMaterialPath}{path}.mat";

            return fullpath;
        }

        public static Material LoadPipeMaterial(PipeParameters para)
        {
            var path = GetPipeMaterialPath(para);

            Material mat = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;

            return mat;
        }

        #endregion

        #region Pipe Fall Logic

        // return true only if all clamps are visible
        public static bool ShouldPipeFall(GameObject root)
        {
            // return false if there is at least one ClampHintManager.Clamped == true
            var children = Utils.GetChildren<ClampHintManager>(root);

            foreach (var child in children)
            {
                var chm = child.GetComponent<ClampHintManager>();
                if (chm.Clamped) return false;
            }

            return true;
        }

        #endregion
    }
}