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
            GameObject.Destroy(rb);

            // disable its interactable ability
            var reticle = interactablePipe.GetComponentInChildren<ReticleDataIcon>();
            reticle.gameObject.SetActive(false);
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

        public static NetworkObject GetPipePrefabRef(PipeParameters para)
        {
            var table = NetworkProjectConfig.Global.PrefabTable;

            var name = GetPipePrefabName(para);

            var path = $"{GlobalConstants.PipePrefabsPath}{name}.prefab";

            GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

            var no = go.GetComponent<NetworkObject>();
            var nid = no.NetworkGuid;

            NetworkPrefabId npid;
            NetworkObject networkObject = null;
            if (table.TryGetId(nid, out npid))
            {
                table.TryGetPrefab(npid, out networkObject);
            }

            return networkObject;
        }

        public static NetworkObject GetStraightPipePrefabRef(PipeDiameter diameter)
        {
            var para = new PipeParameters();
            para.diameter = diameter;
            para.angle = PipeBendAngles.Angle_0;

            return GetPipePrefabRef(para);
        }
    }
}