namespace OneEngineGame
{
    public static class HumanBone
    {
        public const int Body = 0,
        Head = 1,
        LeftArm = 2,
        LeftForearm = 3,
        RightArm = 4,
        RightForearm = 5,
        LeftLeg = 6,
        LeftKnee = 7,
        RightLeg = 8,
        RightKnee = 9,
        Count = 10;

        public static string[] Names { get; } = 
        {
            "Body", "Head", "LeftArm", "LeftForearm", "RightArm", "RightForearm", "LeftLeg", "LeftKnee", "RightLeg", "RightKnee"
        };
    }
}
