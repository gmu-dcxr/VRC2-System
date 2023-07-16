namespace VRC2.Conditions
{
    public class Condition
    {
        protected Existence _existence;
        protected Frequency _frequency;
        protected Format _format;
        protected Quality _quality;
        protected TimeLimits _timeLimits;
        protected Context _context;
        protected Amount _amount;

        public string name = "";
        public string shortName = "";

        public Condition()
        {
        }

        public string ToString()
        {
            return $"Condition: {name}({shortName})\n" +
                   $"Existence: {Utils.GetDisplayName<Existence>(_existence)}\n" +
                   $"Frequency: {Utils.GetDisplayName<Frequency>(_frequency)}\n" +
                   $"Format: {Utils.GetDisplayName<Format>(_format)}\n" +
                   $"Quality: {Utils.GetDisplayName<Quality>(_quality)}\n" +
                   $"Time limits: {Utils.GetDisplayName<TimeLimits>(_timeLimits)}\n" +
                   $"Context: {Utils.GetDisplayName<Context>(_context)}\n" +
                   $"Amount: {Utils.GetDisplayName<Amount>(_amount)}";
        }
    }

    public class Baseline : Condition
    {
        public Baseline() : base()
        {
            name = "Baseline";
            shortName = "Baseline";

            _existence = Existence.HasWarning;
            _frequency = Frequency.OneTime;
            _format = Format.Audio;
            _quality = Quality.Good;
            _timeLimits = TimeLimits.Duration10Sec;
            _context = Context.Relevant;
            _amount = Amount.NotOverload;
        }
    }

    public class Condition1 : Baseline
    {
        public Condition1() : base()
        {
            name = "Condition 1";
            shortName = "C1";

            _existence = Existence.NoWarning;
        }
    }

    public class Condition2 : Baseline
    {
        public Condition2() : base()
        {
            name = "Condition 2";
            shortName = "C2";

            _frequency = Frequency.Repeat;
        }
    }

    public class Condition3 : Baseline
    {
        public Condition3() : base()
        {
            name = "Condition 3";
            shortName = "C3";

            _format = Format.Visual;
            _timeLimits = TimeLimits.Duration10Sec;
        }
    }

    public class Condition4 : Baseline
    {
        public Condition4() : base()
        {
            name = "Condition 4";
            shortName = "C4";

            _quality = Quality.Bad;
        }
    }

    public class Condition5 : Baseline
    {
        public Condition5() : base()
        {
            name = "Condition 5";
            shortName = "C5";

            _format = Format.Visual;
            _timeLimits = TimeLimits.Duration5Sec;
        }
    }

    public class Condition6 : Baseline
    {
        public Condition6() : base()
        {
            name = "Condition 6";
            shortName = "C6";

            _context = Context.Irrelevant;
        }
    }

    public class Condition7 : Baseline
    {
        public Condition7() : base()
        {
            name = "Condition 7";
            shortName = "C7";

            _amount = Amount.Overload;
        }
    }
}