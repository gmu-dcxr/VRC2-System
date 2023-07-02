using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2
{
    public class GlueContainerLabelManager : HoveringLabelManager
    {
        // Start is called before the first frame update
        void Start()
        {
            // must call it
            base.Start();
            // update the label
            base.label = $"Glue Capacity: {GlobalConstants.currentGlueCapacitiy * 100}%";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}