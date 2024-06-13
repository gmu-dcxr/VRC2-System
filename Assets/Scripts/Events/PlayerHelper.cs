using UnityEngine;

using NetworkRunner = Fusion.NetworkRunner;
using NetworkObject = FishNet.Object.NetworkObject;

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
                    var players = GameObject.FindObjectsOfType<OVRCustomSkeleton>(includeInactive: false);

                    // Fusion
                    if (networkRunner == null || !networkRunner.IsRunning)
                    {
                        if (players != null && players.Length > 0)
                        {
                            _localPlayer = players[0].gameObject;
                            Debug.LogWarning($"Find local player: {_localPlayer.name}");
                        }
                    }
                    else
                    {
                        // FishNet
                        // find the object that is the owner
                        foreach (var player in players)
                        {
                            var no = player.gameObject.GetComponent<NetworkObject>();
                            if (no.IsOwner)
                            {
                                _localPlayer = no.gameObject;

                                Debug.LogWarning($"Find local player: {_localPlayer.name}");
                            }
                            else
                            {
                                _remotePlayer = no.gameObject;
                                Debug.LogWarning($"Find remote player: {_remotePlayer.name}");
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

        private GameObject _localParent;

        // merge camera rig and avatar and move to top
        public void MergeLocalPlayer()
        {
            // generate parent
            if (_localParent == null)
            {
                _localParent = new GameObject();
                _localParent.name = "Local Player";
            }
            
            var rig = GameObject.FindObjectOfType<OVRCameraRig>().gameObject;
            _localParent.transform.position = Vector3.zero;
            _localParent.transform.rotation = Quaternion.identity;

            rig.transform.parent = _localParent.transform;
            
            if (localPlayer != null)
            {
                localPlayer.transform.parent = _localParent.transform;   
            }

            // move to top
            _localParent.transform.SetAsFirstSibling();
        }
    }
}