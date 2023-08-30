using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace VRC2.Animations
{
    public class BaseInputReplay : MonoBehaviour
    {
        private string folder = "InputRecordings";

        string GetFilePath(string filename) =>
            Path.Combine(Path.GetDirectoryName(Application.dataPath), folder, filename + ".inputtrace");

        public virtual void Awake()
        {
            InitTraces();
            InitControllers();
        }

        public virtual void InitTraces()
        {

        }

        public virtual void InitControllers()
        {

        }

        public void InitTrace(ref InputEventTrace trace, string filename)
        {
            if (trace == null)
            {
                trace = new InputEventTrace(Keyboard.current);
                trace.onEvent += OnEvent;
                trace.ReadFrom(GetFilePath(filename));
            }
        }

        void OnEvent(InputEventPtr ev)
        {
            Debug.Log(ev.ToString());
        }

        // public void Replay(InputEventTrace trace, Action finish)
        // {
        //     trace.Replay()
        //         .OnFinished(finish)
        //         .PlayAllEventsAccordingToTimestamps();
        // }

        public InputEventTrace.ReplayController InitController(ref InputEventTrace trace, Action onfinished,
            Action<InputEventPtr> action)
        {
            return trace.Replay()
                .OnFinished(onfinished)
                .OnEvent(action);
        }

        public bool IsFinished(ref InputEventTrace.ReplayController controller, bool rewind = false)
        {
            var res = controller.finished;
            if (res && rewind)
            {
                controller.Rewind();
            }

            return res;
        }

        public void ForceRewind(ref InputEventTrace.ReplayController controller)
        {
            controller.Rewind();
        }

        public void StopReplay(ref InputEventTrace.ReplayController controller, bool rewind)
        {
            controller.paused = true;

            if (rewind)
            {
                controller.Rewind();
            }
        }

        public void StartReplay(ref InputEventTrace.ReplayController controller, bool loop = false, bool stop = false)
        {
            if (stop)
            {
                controller.paused = true;
                return;
            }

            controller.paused = false;

            if (controller.position == 0)
            {
                controller.PlayAllEventsAccordingToTimestamps();
            }
            else
            {
                if (controller.finished)
                {
                    if (loop)
                    {
                        controller.Rewind();
                    }
                }
            }
        }
    }
}