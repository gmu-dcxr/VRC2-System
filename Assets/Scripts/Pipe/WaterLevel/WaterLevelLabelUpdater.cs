using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;

namespace VRC2.Events
{
    struct WaterLevelInput: INetworkInput
    {
        public Vector3 position;
        public Quaternion rotation;
        public int angle;
    }
    
    public class WaterLevelLabelUpdater : NetworkBehaviour, INetworkRunnerCallbacks
    {
        [Header("Label")] public TextMeshPro textMeshPro;

        public NetworkPrefabRef waterLevel;

        private bool assigned = false;

        private NetworkRunner _runner
        {
            get => GlobalConstants.networkRunner;
        }

        public PlayerRef _player
        {
            get => GlobalConstants.remotePlayer;
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (!assigned)
            {
                UpdateInputAuthority();
            }
            else
            {
                // do nothing
            }
        }

        int GetDegree()
        {
            var t = gameObject.transform.rotation.eulerAngles;
            return (int)(Math.Abs(t.x % 90));
        }

        void UpdateInputAuthority()
        {
            // update authority on the host side
            if (_runner != null && _runner.IsRunning && _runner.IsServer && _player != PlayerRef.None)
            {
                print("assign water level");
                //
                var no = gameObject.GetComponent<NetworkObject>();
                no.AssignInputAuthority(_player);

                if (no.HasInputAuthority)
                {
                    assigned = true;
                }
            }
        }
        
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var no = gameObject.GetComponent<NetworkObject>();
            if (no.HasInputAuthority)
            {
                var t = gameObject.transform;

                var wli = new WaterLevelInput();
                wli.position = t.position;
                wli.rotation = t.rotation;
                wli.angle = GetDegree();

                input.Set(wli);
            }
            else
            {
                if (GetInput<WaterLevelInput>(out var wli) == false) return;

                var pos = wli.position;
                var rot = wli.rotation;
                var angle = wli.angle;

                gameObject.transform.position = pos;
                gameObject.transform.rotation = rot;
                
                textMeshPro.text = $"{angle}";
            }
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            throw new NotImplementedException();
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            throw new NotImplementedException();
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            throw new NotImplementedException();
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            throw new NotImplementedException();
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            throw new NotImplementedException();
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            throw new NotImplementedException();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            throw new NotImplementedException();
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            throw new NotImplementedException();
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            throw new NotImplementedException();
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            throw new NotImplementedException();
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            throw new NotImplementedException();
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            throw new NotImplementedException();
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            throw new NotImplementedException();
        }
    }
}