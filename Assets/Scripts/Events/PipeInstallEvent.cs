using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2.Events
{
    public enum PipeInstallEvent
    {
        EmptyEvent = 0, // empty event
        VoiceControl = 2001, // voice control
        P2GiveP1Instruction = 1000,
        P1GetInstruction = 1001,
        P1CheckStorage = 1,
        P1PickUpPipe = 2,
        P1Deprecate = 21,
        P2CheckSizeAndColor = 3,
        P1GetSizeAndColorResult = 31,
        P1CommandAIDrone = 4,
        P2GiveInstruction = 5,
        P2MeasureDistance = 7,
        P2MeasureDistanceResult = 71,
        P2CommandRobotBendOrCut = 8,
        P2CheckLengthAndAngle = 9,
        P1CheckGlue = 91,
        P1CheckClamp = 92,
        P2CheckLevel = 10, // Horizontal and vertical level
        P1GetLevelResult = 101,
        P1Glue = 11,
        P1Place = 12,
        P1Adjust = 13,
        P1Clamp = 14,
        DirectMessage = 9999,
    }
}