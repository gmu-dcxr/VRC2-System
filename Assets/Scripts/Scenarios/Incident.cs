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
        private bool finished = false;

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
        }

        public void Execute(int timestamp)
        {
            print($"{this.GetType().Name}.Execute()");
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

            if (sec - startTimestamp >= startInSec)
            {
                if (!started)
                {
                    print($"{this.GetType().Name} - {_desc} start @ {sec}");
                    // start it
                    started = true;
                    if (OnStart != null)
                    {
                        OnStart();
                    }
                }
                else if(endInSec != -1)
                {
                    // check whether it needs to stop
                    if (sec - startTimestamp >= endInSec)
                    {
                        print($"{this.GetType().Name} - {_desc} finish @ {sec}");
                        // time to stop it
                        finished = true;
                        if (OnFinish != null)
                        {
                            OnFinish();
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