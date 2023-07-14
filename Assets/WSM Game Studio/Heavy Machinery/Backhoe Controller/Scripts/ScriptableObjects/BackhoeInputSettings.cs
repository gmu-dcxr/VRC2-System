using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [CreateAssetMenu(fileName = "NewBackhoeInputSettings", menuName = "WSM Game Studio/Heavy Machinery/Backhoe Input Settings", order = 1)]
    public class BackhoeInputSettings : ScriptableObject
    {
        public KeyCode toggleEngine = KeyCode.T;
        public KeyCode loaderFrameUp = KeyCode.Alpha4;
        public KeyCode loaderFrameDown = KeyCode.Alpha3;
        public KeyCode frontBucketUp = KeyCode.Alpha2;
        public KeyCode frontBucketDown = KeyCode.Alpha1;
        public KeyCode stabilizerLegsUp = KeyCode.Alpha5;
        public KeyCode stabilizerLegsDown = KeyCode.Alpha6;

        public KeyCode swingFrameLeft = KeyCode.Keypad7;
        public KeyCode swingFrameRight = KeyCode.Keypad8;
        public KeyCode boomForwards = KeyCode.Keypad6;
        public KeyCode boomBackwards = KeyCode.Keypad3;
        public KeyCode armForwards = KeyCode.Keypad5;
        public KeyCode armBackwards = KeyCode.Keypad2;
        public KeyCode rearBucketUp = KeyCode.Keypad4;
        public KeyCode rearBucketDown = KeyCode.Keypad1;

        public KeyCode[] customEventTriggers;
    } 
}
