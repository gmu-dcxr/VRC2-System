using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fusion;

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

        #region Pipe Template

        private static IDictionary<int, NetworkPrefabRef> diameterPipePrefabRefDict;

        public static void AddPipeNetworkPrefabRef(int diameter, NetworkPrefabRef prefabRef)
        {
            if (diameterPipePrefabRefDict == null)
            {
                diameterPipePrefabRefDict = new Dictionary<int, NetworkPrefabRef>();
            }
            diameterPipePrefabRefDict.Add(diameter, prefabRef);
        }

        public static NetworkPrefabRef GetPipeNetworkPrefabRef(int diameter)
        {
            NetworkPrefabRef result = NetworkPrefabRef.Empty;

            if (diameterPipePrefabRefDict != null)
            {
                if (!diameterPipePrefabRefDict.TryGetValue(diameter, out result))
                {
                    Debug.LogWarning($"Failed to get pipe prefer for diameter {diameter}");
                    result = NetworkPrefabRef.Empty;
                }
            }

            return result;
        }

        // This is to spawn networked pipe object
        public static GameObject pipeSpawnTemplate;
        // This is to call spawn event
        public static string menuObjectTag = "Menu";
        // This is to despawn object
        public static NetworkId lastSpawned = new NetworkId();

        #endregion

        #region Pipe Collision with the Wall
        // It's the tag of the pipe, not the InteractablePipe
        public static string pipeObjectTag = "Pipe";

        #endregion

        #region Clamp Collision with the Wall

        public static string clampObjectTag = "Clamp";

        #endregion

        #region Voice Communication

        public static string voiceRecorderTag = "Voice";
        

        #endregion
    }
}
