using Fusion;
using UnityEngine;

namespace VRC2.Events
{
    public class PlayerHelper : MonoBehaviour
    {
        #region Player

        private NetworkRunner _networkRunner;
        private GameObject _localPlayer;
        private GameObject _remotePlayer;

        [HideInInspector]
        public NetworkRunner networkRunner
        {
            get
            {
                if (_networkRunner == null)
                {
                    _networkRunner = GameObject.FindFirstObjectByType<NetworkRunner>();
                }

                return _networkRunner;
            }
        }

        [HideInInspector]
        public GameObject localPlayer // HighFidelityFirstPerson
        {
            get
            {
                if (_localPlayer == null)
                {
                    var players = GameObject.FindObjectsOfType<OVRCustomSkeleton>(includeInactive: true);

                    if (networkRunner == null || !networkRunner.IsRunning)
                    {
                        if (players != null)
                        {
                            _localPlayer = players[0].gameObject;
                            Debug.LogWarning($"Find local player: {_localPlayer.name}");
                        }
                    }
                    else
                    {
                        // find the object having input authority
                        foreach (var player in players)
                        {
                            var no = player.gameObject.GetComponent<NetworkObject>();
                            if (no.HasInputAuthority)
                            {
                                _localPlayer = no.gameObject;

                                Debug.LogWarning($"Find local player: {_localPlayer.name}");
                            }
                            else
                            {
                                _remotePlayer = no.gameObject;
                                Debug.LogWarning($"Find remote player: {_localPlayer.name}");
                            }
                        }
                    }

                }

                return _localPlayer;
            }
        }

        [HideInInspector]
        public GameObject remotePlayer // HighFidelityFirstPerson without 
        {
            get
            {
                // get local player first, and the remote player will be set
                var lp = localPlayer;
                return _remotePlayer;
            }
        }



        #endregion

        public void ResetLocalPlayer()
        {
            _localPlayer = null;
        }
    }
}