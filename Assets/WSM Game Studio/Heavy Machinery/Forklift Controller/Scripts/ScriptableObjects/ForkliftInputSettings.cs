using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [CreateAssetMenu(fileName = "NewForkliftInputSettings", menuName = "WSM Game Studio/Heavy Machinery/Forklift Input Settings", order = 1)]
    public class ForkliftInputSettings : ScriptableObject
    {
        public KeyCode toggleEngine = KeyCode.T;
        public KeyCode forksUp = KeyCode.Alpha1;
        public KeyCode forksDown = KeyCode.Alpha2;
        public KeyCode forksRight = KeyCode.Alpha3;
        public KeyCode forksLeft = KeyCode.Alpha4;
        public KeyCode mastTiltBackwards = KeyCode.Alpha5;
        public KeyCode mastTiltForwards = KeyCode.Alpha6;

        public KeyCode[] customEventTriggers;
    }
}
