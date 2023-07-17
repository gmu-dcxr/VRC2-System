using System;
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

        public System.Action<int> OnStart;
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

        public int StartInSec
        {
            get => startInSec;
        }

        public int EndInSec
        {
            get => endInSec;
        }


        public void InitIncident(string scenario, int idx, string time, string desc, string warning)
        {
            _scenario = scenario;
            _id = idx;
            _desc = desc;
            _warning = warning;
            _rawTime = time;
            Helper.ParseTime(time, ref startInSec, ref endInSec);
        }

        public void Execute(int timestamp)
        {
            print($"{Scenario} - Incident #{_id} - Execute()");
            startTimestamp = timestamp;
            ready = true;
            started = false;
            finished = false;
        }

        private void Update()
        {
            if (!ready) return;

            if (finished) return;

            var sec = Helper.SecondNow();

            var localts = sec - startTimestamp;

            if (localts >= startInSec)
            {
                if (!started)
                {
                    print($"{Scenario} - Incident #{_id} - Start @ {localts}");
                    // start it
                    started = true;
                    if (OnStart != null)
                    {
                        OnStart(_id);
                    }
                }
                else if(endInSec != -1)
                {
                    // check whether it needs to stop
                    if (localts >= endInSec)
                    {
                        print($"{Scenario} - Incident #{_id} - Finish @ {localts}");
                        // time to stop it
                        finished = true;
                        if (OnFinish != null)
                        {
                            OnFinish(_id);
                        }
                    }
                }
            }
        }

        public void ForceQuit()
        {
            finished = true;
        }
    }
}