using System;
using System.Timers;
using UnityEngine;


namespace VRC2.Scenarios
{
    public class Incident : MonoBehaviour
    {
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

        public System.Action OnStart;
        public System.Action OnFinish;

        private bool started = false;

        private bool ready = false;

        private Timer _timer;

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


        public void InitIncident(string desc, string warning, string time)
        {
            _desc = desc;
            _warning = warning;
            _rawTime = time;
            Helper.ParseTime(time, ref startInSec, ref endInSec);

            _timer = new Timer();
        }

        public void Execute(int timestamp)
        {
            startTimestamp = timestamp;
            ready = true;
        }

        private void Update()
        {
            if (!ready) return;

            var sec = Helper.SecondNow();

            if (sec - startTimestamp >= startInSec)
            {
                if (!started)
                {
                    // start it
                    started = true;
                    OnStart();
                }
                else
                {
                    // check whether it needs to stop
                    if (sec - startTimestamp >= endInSec)
                    {
                        // time to stop it
                        started = false;
                        OnFinish();
                    }
                }
            }
        }

        public void ForceQuit()
        {
            started = false;
        }
    }
}