using System;

namespace VRC2.Pipe
{
    public static class PipeConstants
    {
        public enum PipeMaterialColor
        {
            Default = 0,
            Magenta = 1,
            Blue = 2,
            Yellow = 3,
            Green = 4
        }
        
        public enum PipeType
        {
            Default = 0,
            Sewage = 1, // pipe is the normal one (cylinder)
            Water = 2, // Water pipe: cylinder with metal material
            Gas = 3, //  pipe with intense wrinkle
            Electrical = 4 // pipe with loose wrinkle
        }
        
        public enum PipeBendAngles
        {
            Default = -1,
            Angle_0 = 0,
            Angle_45 = 1,
            Angle_90 = 2,
            Angle_135 = 3
        };
        
        public struct PipeBendCutParameters
        {
            public PipeType type;
            public PipeMaterialColor color;
            public int diameter;
            public PipeBendAngles angle;
            public float a;
            public float b;

            public string ToString()
            {
                return
                    $"{Enum.GetName(typeof(PipeType), type)} - " +
                    $"{Enum.GetName(typeof(PipeMaterialColor), color)} - " +
                    $"{diameter} - " +
                    $"{Enum.GetName(typeof(PipeBendAngles), angle)} - " +
                    $"{a.ToString("f2")} - {b.ToString("f2")}";
            }
        }
    }
}