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
        P2CheckPipe = 3,
        P1DemandAIDroneToMovePipe = 4,
        P2GiveInstruction = 5,
        P2CheckSizeAndColor = 6,
        P2MeasureDistance = 7,
        P2DemandRobotBendOrCut = 8,
        P2CheckLengthAndAngle = 9,
        P2CheckLevel = 10, // Horizontal and vertical level
        P1Glue = 11,
        P1Place = 12,
        P1Adjust = 13,
        P1Clamp = 14,
    }
}