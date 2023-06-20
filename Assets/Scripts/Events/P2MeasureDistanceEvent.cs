using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fusion;

namespace VRC2.Events
{

    public class P2MeasureDistanceEvent : BaseEvent
    {

        private float distance = 1.0f;

        public void Initialize()
        {
            distance = 0.0f;
        }

        public override void Execute()
        {
            dialogManager.UpdateDialog("Instruction", $"The distance is {distance}.\nSend the instruction to robot?"
                , "OK", "Cancel",
                PipeInstallEvent.P2MeasureDistanceResult);
            dialogManager.Show(true);
        }
    }
}