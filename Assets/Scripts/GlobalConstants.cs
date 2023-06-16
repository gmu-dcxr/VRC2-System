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
            return (GlobalConstants.networkRunner != null && !localPlayer.IsNone);
        }

        // P2, participant 2, check size, color, water level, etc.
        public static bool Checker
        {
            get { return true; }
        }

        // P1, participant 1, install pipe, move, rotate, etc
        public static bool Checkee
        {
            get { return !Checker; }
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
