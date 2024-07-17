using System;
using UnityEngine;
using VRC2.Pipe.Clamp;

namespace VRC2.Events
{
    [RequireComponent(typeof(ClampStatusMonitor))]
    public class ClampScaleInitializer: MonoBehaviour
    {
        [Tooltip("Size of the clamp, 1,2,3, or 4 inches")]
        public int clampSize = 1;

        private void Start()
        {
            // Bug: it's better not to change scale here. Instead, change it in the prefabs.
            // // get scale
            // var scale = GlobalConstants.GetClampScaleBySize(clampSize);
            // gameObject.transform.localScale = scale;
        }
    }
}