using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [CreateAssetMenu(fileName = "DumpTruckInputSettings", menuName = "WSM Game Studio/Heavy Machinery/Dump Truck Input Settings", order = 1)]
    public class DumpTruckInputSettings : ScriptableObject
    {
        public KeyCode toggleEngine = KeyCode.T;
        public KeyCode dumpBedUp = KeyCode.Keypad4;
        public KeyCode dumpBedDown = KeyCode.Keypad1;

        public KeyCode[] customEventTriggers;
    } 
}
