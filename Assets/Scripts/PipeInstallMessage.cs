using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRC2
{
    public class PipeInstallMessage
    {
        public static string BaseMessage = "(empty message)";
        public static string P1WrongInstructionP2 = "P1 gave wrong instructions to P2";
        public static string P2WrongInstructionAIDrone = "P2 gave wrong instructions to AI drone";
        public static string AIDroneWrongPipes = "AI drone carried wrong pipes";
        public static string P1GetWrongPipe = "P1 got a wrong pipe";
        public static string P2WrongInstructionRobot = "P2 gave wrong instructions to robot";
        public static string RobotWrongBendOrCut = "Robot made a wrong bend or cut of the pipe";
        public static string P1ForgetToGlue = "P1 forgot to glue the pipe";
        public static string P1IncorrectPosition = "P1 place the glue in a wrong position/rotation";
        public static string P1ForgetToClamp = "P1 forgot to clamp the pipe";
    }
}