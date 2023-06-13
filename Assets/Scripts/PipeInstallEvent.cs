using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2
{
    public enum PipeInstallEvent
    {
        EmptyEvent = 0, // empty event
        P1CheckStorage = 1,
        P1PickUpPipe = 2,
        P1DemandAIDroneToMovePipe = 3,
        P2GiveInstruction = 4,
        P2CheckSizeAndColor = 5,
        P2MeasureDistance = 6,
        P2DemandRobotBendOrCut = 7,
        P2CheckLengthAndAngle = 8,
        P2CheckLevel = 9, // Horizontal and vertical level
        P1Glue = 10,
        P1Place = 11,
        P1Adjust = 12,
        P1Clamp = 13,
    }
}