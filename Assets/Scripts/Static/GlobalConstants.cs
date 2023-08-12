using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Fusion;
using VRC2.Pipe;

namespace VRC2
{
    public static class GlobalConstants
    {
        public static NetworkRunner networkRunner = null;
        public static PlayerRef localPlayer = PlayerRef.None;
        public static PlayerRef remotePlayer = PlayerRef.None;

        public static bool GameStarted = false;

        public static void RemovePlayer(PlayerRef player)
        {
            if (player == localPlayer)
            {
                GlobalConstants.localPlayer = PlayerRef.None;
            }
            else if (player == remotePlayer)
            {
                GlobalConstants.remotePlayer = PlayerRef.None;
            }
        }

        public static bool IsNetworkReady()
        {
            return (GlobalConstants.networkRunner != null && !GlobalConstants.localPlayer.IsNone);
        }

        // P2, participant 2, check size, color, water level, etc.
        public static bool Checker = false;

        // P1, participant 1, install pipe, move, rotate, etc
        public static bool Checkee
        {
            get { return !Checker; }
        }

        public static bool IsP1
        {
            get { return Checkee; }
        }

        public static bool IsP2
        {
            get { return Checker; }
        }

        public static bool DialogFirstButton
        {
            get { return true; }
        }

        public static bool DialogSecondButton
        {
            get { return !DialogFirstButton; }
        }

        #region Pipe Spawn Template

        // This is to spawn networked pipe object
        public static GameObject pipeSpawnTemplate;

        // This is to call spawn event
        public static string menuObjectTag = "Menu";

        // This is to despawn object
        public static NetworkId lastSpawned = new NetworkId();

        #endregion

        public static string glueObjectTag = "Glue";

        #region Pipe Collision with the Wall

        // It's the tag of the pipe, not the InteractablePipe
        public static string pipeObjectTag = "Pipe";

        #endregion

        #region Clamp Collision with the Wall

        public static string clampObjectTag = "Clamp";
        public static string ClampPrefabsPath = "Assets/Prefabs/";

        #endregion

        #region Box Collision with the Wall

        public static string boxObjectTag = "Box";

        #endregion

        #region Voice Communication

        public static string voiceRecorderTag = "Voice";


        #endregion

        #region Start Settings

        // clamp count
        public static int clampInitialCount = 20;

        // box count
        public static int boxInitialCount = 10;

        // glue percentage
        public static float glueInitialCapacity = 1.0f;

        // clamp consumption per action
        public static int clampConsumption = 1;

        // box consumption per action
        public static int boxConsumption = 1;

        // glue consumption per action
        public static float glueConsumption = 0.1f;

        // current clamp,box, and glue
        public static int currentClampCount = clampInitialCount;
        public static int currentBoxCount = boxInitialCount;
        public static float currentGlueCapacitiy = glueInitialCapacity;

        public static bool IsClampUsedOut
        {
            get => currentBoxCount == 0;
        }

        public static bool IsBoxUsedOut
        {
            get => currentBoxCount == 0;
        }

        public static bool IsGlueUsedOut
        {
            get => currentGlueCapacitiy == 0;
        }

        public static bool UseClamp()
        {
            if (IsClampUsedOut) return false;

            currentClampCount -= clampConsumption;
            return true;
        }

        public static bool UseBox()
        {
            if (IsBoxUsedOut) return false;
            currentBoxCount -= boxConsumption;
            return true;
        }

        public static bool UseGlue()
        {
            if (IsGlueUsedOut) return false;
            currentGlueCapacitiy -= glueConsumption;
            return true;
        }



        #endregion

        #region Clamp Related


        #region Clamp Size-Scale Mapper

        private static IDictionary<int, Vector3> clampSizeScaleDict = null;

        public static Vector3 GetClampScaleBySize(int size)
        {
            if (clampSizeScaleDict == null)
            {
                clampSizeScaleDict = new Dictionary<int, Vector3>();
                // 1 inch, y doesn't matter much
                clampSizeScaleDict.Add(1, new Vector3(0.15f, 0.15f, 0.15f));
                // 2 inch
                clampSizeScaleDict.Add(2, new Vector3(0.3f, 0.15f, 0.5f));
                // 3 inch
                clampSizeScaleDict.Add(3, new Vector3(0.5f, 0.15f, 0.7f));
                // 4 inch
                clampSizeScaleDict.Add(4, new Vector3(0.6f, 0.15f, 0.9f));
            }

            Vector3 value = Vector3.one;
            clampSizeScaleDict.TryGetValue(size, out value);
            return value;
        }

        #endregion

        #endregion

        #region Pipe Connecting Pipe Parent Prefab

        // connect two pipes into one pipe, this would be the parent object of two connected pipes
        public static string pipePipeConnectorPrefabPath = "Assets/Prefabs/InteractablePipeContainer.prefab";

        // for collision detection
        public static string interactablePipeContainer = "InteractablePipeContainer";

        #endregion

        #region Pipe Bend/Cut Manipulation

        public static string PipePrefabsPath = "Assets/Prefabs/Pipe/";
        public static GameObject lastSpawnedPipe = null;
        public static string BendCutRobot = "BendCutRobot";

        #endregion

        #region AI Drone

        public static string AIDroneDeliver = "AIDroneDeliver";


        #endregion

        #region PokeLocation Objects

        public static string PokeLocation = "PokeLocation";
        public static GameObject LeftPokeObject = null;
        public static GameObject RightPokeObject = null;

        #endregion


        #region Controller Visuals

        public static string ControllerVisual = "OVRControllerVisual";

        public static GameObject LeftOVRControllerVisual = null;
        public static GameObject RightOVRControllerVisual = null;

        public static void SetOVRControllerVisual(ref GameObject left, ref GameObject right)
        {
            object _locker = new object();
            lock (_locker)
            {
                LeftOVRControllerVisual = left;
                RightOVRControllerVisual = right;
            }
        }

        #endregion

        #region SAGAT

        public static string SAGATTag = "SAGAT";

        #endregion

        #region Clamp Hint

        public static string clampHintTag = "ClampHint";

        #endregion

        #region Wall / Boxes

        public static string wallTag = "Wall";
        public static string wallBoxesTag = "Boxes";

        #endregion

        #region Dynamical Pipe Material

        public static string pipeMaterialPath = "Assets/Materials/Pipe/";

        #endregion

        #region EyeTracking

        public static string eyeTrackingLogger = "EyeTrackingLog";

        #endregion

        #region Warning Voice

        public static string warningAudioPath = "Assets/Audio/VRC2/";

        #endregion

        #region Instruction

        // private static string instructionImagesResource = "Assets/Resources/Task/";
        public static string instructionImagesResource = "Task";

        public static string instructionTag = "Instruction";

        public static Texture2D loadTaskInstruction(int start, int end)
        {
            var filename = $"T-{start}-{end}";
            return loadTaskInstruction(filename);
        }

        public static Texture2D loadTaskInstruction(string filename)
        {
            var name = $"{instructionImagesResource}/{filename}";
            Debug.Log(name);
            Texture2D texture = Resources.Load<Texture2D>(name);
            texture.alphaIsTransparency = true;
            return texture;
        }

        public static Texture2D loadTrainingInstruction()
        {
            var filename = "Training";
            return loadTaskInstruction(filename);
        }

        #endregion
    }
}
