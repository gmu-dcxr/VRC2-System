using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fusion;

namespace VRC2.Events
{

    public class P2MeasureDistanceEvent : BaseEvent
    {

        private float distance;

        private bool executing = false;

        public void Initialize()
        {
            distance = 0.0f;
        }

        public override void Execute()
        {
            executing = true;
        }

        public void Stop()
        {
            executing = false;
            // TODO: add other cleaning event
        }

        private void Update()
        {
            // update dialog content
            if (!executing) return;
            var msg = $"The distance is {distance}.\nSend the instruction to robot?";
            dialogManager.content = msg;
        }
    }
}