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
            Debug.Log($"NetworkRunner is null: {networkRunner == null}");
            Debug.Log($"localPlayer is null: {GlobalConstants.localPlayer.IsNone}");
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
    }
}
