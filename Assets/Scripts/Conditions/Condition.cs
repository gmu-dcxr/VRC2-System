using System;
using UnityEngine;
using VRC2.Scenarios;

namespace VRC2.Conditions
{
    public enum ConditionNumber
    {
        Baseline = 0,
        Condition_1 = 1,
        Condition_2 = 2,
        Condition_3 = 3,
        Condition_4 = 4,
        Condition_5 = 5,
        Condition_6 = 6,
        Condition_7 = 7,
        Condition_8 = 8,
    }

    public class Condition
    {
        protected Existence _existence;
        protected Frequency _frequency;
        protected Format _format;
        protected Quality _quality;
        protected TimeLimits _timeLimits;
        protected Context _context;
        protected Amount _amount;

        public Existence Existence
        {
            get => _existence;
        }

        public Frequency Frequency
        {
            get => _frequency;
        }

        public Format Format
        {
            get => _format;
            set => _format = value;
        }

        public Quality Quality
        {
            get => _quality;
        }

        public TimeLimits TimeLimits
        {
            get => _timeLimits;

        }

        public Context Context
        {
            get => _context;
        }

        public Amount Amount
        {
            get => _amount;
        }

        public string name = "";
        public string shortName = "";

        public Condition()
        {
            _existence = Existence.HasWarning;
            _frequency = Frequency.OneTime;
            _format = Format.Audio;
            _quality = Quality.Good;
            _timeLimits = TimeLimits.Duration10Sec;
            _context = Context.Relevant;
            _amount = Amount.NotOverload;
        }

        public virtual string ToString()
        {
            return $"{name}({shortName})\n" +
                   $"Existence: {Utils.GetDisplayName<Existence>(_existence)}\n" +
                   $"Frequency: {Utils.GetDisplayName<Frequency>(_frequency)}\n" +
                   $"Format: {Utils.GetDisplayName<Format>(_format)}\n" +
                   $"Quality: {Utils.GetDisplayName<Quality>(_quality)}\n" +
                   $"Time limits: {Utils.GetDisplayName<TimeLimits>(_timeLimits)}\n" +
                   $"Context: {Utils.GetDisplayName<Context>(_context)}\n" +
                   $"Amount: {Utils.GetDisplayName<Amount>(_amount)}";
        }

        public static Condition GetCondition(int idx)
        {
            Condition condition = null;
            switch (idx)
            {
                case 0:
                    return new Condition();
                case 1:
                    return new Condition1();
                case 2:
                    return new Condition2();
                case 3:
                    return new Condition3();
                case 4:
                    return new Condition4();
                case 5:
                    return new Condition5();
                case 6:
                    return new Condition6();
                case 7:
                    return new Condition7();
                case 8:
                    return new Condition8();
                default:
                    Debug.LogWarning($"Not found condition for {idx}");
                    break;
            }

            return condition;
        }

        public static Condition GetCondition(ConditionNumber cn)
        {
            var idx = (int)cn;
            Debug.LogWarning(idx);
            return GetCondition(idx);
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

    public class Condition8 : Baseline
    {
        public Condition8() : base()
        {
            name = "Condition 8";
            shortName = "C8";

            _format = Format.Audio;
        }
    }
}