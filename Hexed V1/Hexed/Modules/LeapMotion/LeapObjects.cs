namespace Hexed.Modules.LeapMotion
{
    internal class LeapObjects
    {
        public class HandObject
        {
            public bool IsLeft { get; set; }
            public bool IsRight { get; set; }
            public Position Position { get; set; }
            public Rotation Rotation { get; set; }
            public FingerObject[] Fingers { get; set; }
        }

        public class FingerObject
        {
            public FingerType Type { get; set; }
            public BoneObject[] bones { get; set; }
        }

        public class BoneObject
        {
            public BoneType Type { get; set; }
            public Rotation Rotation { get; set; }
        }

        public class Position
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }

        public class Rotation
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float W { get; set; }
        }

        public enum FingerType
        {
            TYPE_THUMB = 0,
            TYPE_INDEX = 1,
            TYPE_MIDDLE = 2,
            TYPE_RING = 3,
            TYPE_PINKY = 4,
            TYPE_UNKNOWN = -1
        }

        public enum BoneType
        {
            TYPE_INVALID = -1,
            TYPE_METACARPAL = 0,
            TYPE_PROXIMAL = 1,
            TYPE_INTERMEDIATE = 2,
            TYPE_DISTAL = 3
        }
    }
}
