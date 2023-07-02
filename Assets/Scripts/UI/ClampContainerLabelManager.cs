using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRC2.Events;

namespace VRC2
{
    public class ClampContainerLabelManager : HoveringLabelManager
    {
        // Start is called before the first frame update
        void Start()
        {
            // must call it
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            // update the label
            base.label = $"Clamp Count: {GlobalConstants.currentClampCount}";
        }
    }
}