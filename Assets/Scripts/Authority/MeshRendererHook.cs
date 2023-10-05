using UnityEngine;

namespace VRC2.Authority
{
    public class MeshRendererHook : AuthorityHook
    {
        [Space(30)] [Header("Default")] public bool defaultValue = false;
        private MeshRenderer renderer
        {
            get
            {
                var mr = gameObject.GetComponent<MeshRenderer>();
                if (mr != null) return mr;
                return null;
            }
        }

        public override void DisableP1()
        {
            if (renderer == null) return;

            renderer.enabled = false;
        }

        public override void DisableP2()
        {
            if (renderer == null) return;

            renderer.enabled = false;
        }

        public override void Default()
        {
            if (renderer == null) return;

            renderer.enabled = defaultValue;
        }
    }
}