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
            } else if (player == remotePlayer)
            {
                GlobalConstants.remotePlayer = PlayerRef.None;
            }
        }
    }
}
