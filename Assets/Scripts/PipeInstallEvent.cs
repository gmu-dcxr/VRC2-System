using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2
{
    public class PipeInstallEvent
    {
        public static string EmptyEvent = ""; // empty event
        public static string P1CheckStorage = "P1CheckStorage";
        public static string P1PickUpPipe = "P1PickUpPipe";
        public static string P1DemandAIDroneToMovePipe = "P1DemandAIDroneToMovePipe";
        public static string P2GiveInstruction = "P2GiveInstruction";
        public static string P2CheckSizeAndColor = "P2CheckSizeAndColor";
        public static string P2MeasureDistance = "P2MeasureDistance";
        public static string P2DemandRobotBendOrCut = "P2DemandRobotBendOrCut";
        public static string P2CheckLengthAndAngle = "P2CheckLengthAndAngle";
        public static string P2CheckLevel = "P2CheckLevel"; // Horizontal and vertical level
        public static string P1Glue = "P1Glue";
        public static string P1Place = "P1Place";
        public static string P1Adjust = "P1Adjust";
        public static string P1Clamp = "P1Clamp";
    }
}