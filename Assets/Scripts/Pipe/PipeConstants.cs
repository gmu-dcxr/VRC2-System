using System;
using Fusion;

namespace VRC2.Pipe
{
    public static class PipeConstants
    {
        public enum PipeColor
        {
            Default = -1,
            Magenta = 0,
            Yellow = 1,
            Green = 2,
            Blue = 3
        }

        public enum PipeType
        {
            Default = -1,
            Sewage = 0, // pipe is the normal one (cylinder)
            Gas = 1, //  pipe with intense wrinkle
            Water = 2, // Water pipe: cylinder with metal material
            Electrical = 3 // pipe with loose wrinkle
        }

        public enum PipeBendAngles
        {
            Default = -1,
            Angle_0 = 0,
            Angle_45 = 1,
            Angle_90 = 2,
            Angle_135 = 3
        };

        public enum PipeDiameter
        {
            Default = -1,
            Diameter_1 = 0,
            Diameter_2 = 1,
            Diameter_3 = 2,
            Diameter_4 = 3,
        }

        public struct PipeParameters : INetworkStruct
        {
            public PipeType type;
            public PipeColor color;
            public PipeDiameter diameter;
            public PipeBendAngles angle;
            public float a;

            public float b;

            // add amount, 0 is invalid
            public int amount;

            // add connector
            public PipeDiameter connectorDiamter;
            // 0 is invalid
            public int connectorAmount;

            public string ToString()
            {
                return
                    $"{Enum.GetName(typeof(PipeType), type)} - " +
                    $"{Enum.GetName(typeof(PipeColor), color)} - " +
                    $"{Enum.GetName(typeof(PipeDiameter), diameter)} - " +
                    $"{Enum.GetName(typeof(PipeBendAngles), angle)} - " +
                    $"{a.ToString("f2")} - {b.ToString("f2")} - " +
                    $"{amount}\n" +
                    $"Connector {Enum.GetName(typeof(PipeDiameter), connectorDiamter)} - " +
                    $"{connectorAmount}";
            }
        }
    }
}