using System;
using System.Collections;
using System.Timers;
using UnityEngine;


namespace VRC2.Scenarios
{
    public class Incident : MonoBehaviour
    {
        private string _scenario;
        private int _id;
        private string _desc;
        private string _warning;
        // warning delay
        private float? _wdelay;
        private string _rawTime;

        private int startInSec = 0;
        private int endInSec = -1; // till the end

        [HideInInspector] public int startTimestamp { set; get; }

        [HideInInspector]
        public int endTimestamp // end time stamp for the scenario
        {
            set;
            get;
        }

        public System.Action<int, float?> OnStart;
        public System.Action<int> OnFinish;

        private bool started = false;
        private bool finished = false;

        private bool ready = false;

        private Timer _timer;

        public string Scenario
        {
            get => _scenario;
        }

        public int ID
        {
            get => _id;
        }

        public string Desc
        {
            get => _desc;
        }

        public string Warning
        {
            get => _warning;
        }

        public float? WDelay
        {
            get => _wdelay;
        }
        
        public int StartInSec
        {
            get => startInSec;
        }

        public int EndInSec
        {
            get => endInSec;
        }


        public void InitIncident(string scenario, int idx, string time, string desc, string warning, float? wdelay)
        {
            _scenario = scenario;
            _id = idx;
            _desc = desc;
            _warning = warning;
            _wdelay = wdelay;
            _rawTime = time;
            Helper.ParseTime(time, ref startInSec, ref endInSec);
        }

        public void OverrideStartEnd(int offset)
        {
            startInSec = startInSec + offset;
            if (endInSec != -1)
            {
                endInSec = endInSec + offset;
            }
        }

        public void Execute(int timestamp)
        {
            startTimestamp = timestamp;
            ready = true;
            started = false;
            finished = false;
        }

        private void FixedUpdate()
        {
            if (!ready) return;

            if (finished) return;

            var sec = Helper.SecondNow();

            var localts = sec - startTimestamp;

            if (localts >= startInSec)
            {
                if (!started)
                {
                    //UNCOMMENT AFTER
                    //print($"{Scenario} - Incident #{_id} - Start @ {localts}");
                    if (OnStart != null)
                    {
                        OnStart(_id, _wdelay);
                    }

                    // start it
                    started = true;
                }
                else if (endInSec != -1)
                {
                    // check whether it needs to stop
                    if (localts >= endInSec)
                    {
                        print($"{Scenario} - Incident #{_id} - Finish @ {localts}");
                        // time to stop it
                        if (OnFinish != null)
                        {
                            OnFinish(_id);
                        }

                        finished = true;
                    }
                }
            }
        }

        public void ForceQuit()
        {
            finished = true;
        }

        public void Print()
        {
            print($"({Scenario}, {ID}, {Desc}, {Warning}, {StartInSec}, {EndInSec})");
        }
    }
}