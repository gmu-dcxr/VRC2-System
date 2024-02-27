using UnityEngine;

namespace VRC2.Utility
{
    public class VRHelper: MonoBehaviour
    {
        [Header("OVRControllerVisual")] public GameObject leftVisual;
        public GameObject rightVisual;

        [Space(30)]
        [Header("PokeLocation")] public GameObject leftPoke;
        public GameObject rightPoke;
    }
}