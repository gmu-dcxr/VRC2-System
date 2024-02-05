namespace VRC2.Conditions
{
    public enum Existence
    {
        HasWarning = 1,
        NoWarning = 2,
    }

    public enum Frequency
    {
        OneTime = 1,
        Repeat = 2,
    }

    public enum Format
    {
        Audio = 1,
        Visual = 2,
        Both = 3,
    }

    public enum Quality
    {
        Good = 1,
        Bad = 2,
    }

    public enum TimeLimits
    {
        Duration10Sec = 1,
        Duration5Sec = 2,
    }

    public enum Context
    {
        Relevant = 1,
        Irrelevant = 2,
    }

    public enum Amount
    {
        NotOverload = 1,
        Overload = 2,
    }
}