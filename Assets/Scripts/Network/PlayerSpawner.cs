using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC2;
using VRC2.Character;

namespace VRC2
{

    [RequireComponent(typeof(NetworkRunner))]
    public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        [Header("Character Prefab")] public NetworkPrefabRef _playerPrefab;

        public string prefabName;

        private NetworkRunner _runner;

        [Header("Setting")] public bool hideSelf = false;

        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        private bool isServer = false;

        // private bool gameStarted = false;

        private GenderSyncer _genderSyncer;

        public System.Action OnGameStarted;

        private bool genderSynced = false;

        // Start is called before the first frame update
        void Start()
        {
            _runner = gameObject.GetComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            _genderSyncer = FindObjectOfType<GenderSyncer>();
            genderSynced = false;
        }

        private void OnRequestStartGame(string obj)
        {
            if (obj == "Host")
            {
                StartGame(GameMode.Host);
            }
            else if (obj == "Client")
            {
                StartGame(GameMode.Client);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!GlobalConstants.GameStarted) return;

            if (genderSynced) return;

            var clones = GetAllClones();
            if (clones.ToList<GameObject>().Count < 2) return;

            var so = GetSelfObject();

            if (so != null)
            {
                // sync gender
                _genderSyncer.Synchronize(GlobalConstants.localPlayer.PlayerId,
                    GlobalConstants.playerGender == PlayerGender.Male);
                
                genderSynced = true;
            }

        }

        private IEnumerable<GameObject> GetAllClones()
        {
            var name = prefabName + "(Clone)";
            // hide client self in client view
            var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == name);
            return objects;
        }

        private GameObject GetSelfObject()
        {
            var objects = GetAllClones();
            foreach (var go in objects)
            {
                var nwo = go.GetComponent<NetworkObject>();
                if (nwo.HasInputAuthority)
                {
                    return go;
                }
            }

            return null;
        }

        private void HideSelfNetworkObject()
        {
            var objects = GetAllClones();
            foreach (var go in objects)
            {
                var nwo = go.GetComponent<NetworkObject>();
                if (nwo.HasInputAuthority)
                {
                    go.SetActive(false);
                }
            }
        }

        private NetworkObject FindServerNetworkObjectClone()
        {
            var objects = GetAllClones();
            foreach (var go in objects)
            {
                var nwo = go.GetComponent<NetworkObject>();
                if (!nwo.HasInputAuthority)
                {
                    return nwo;
                }
            }

            return null;
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.LogError("OnPlayerJoined");
            print($"{player.PlayerId} {runner.IsServer} {runner.IsClient} {runner.IsSharedModeMasterClient}");
            if (runner.IsServer)
            {
                isServer = true;
                // Create a unique position for the player
                // Vector3 spawnPosition =
                // new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
                NetworkObject networkPlayerObject =
                    runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
                // Keep track of the player avatars so we can remove it when they disconnect
                _spawnedCharacters.Add(player, networkPlayerObject);

                // set player object for updating character
                runner.SetPlayerObject(player, networkPlayerObject);

                // The first will be host (P1), all spawning actions will be done host
                // The second will be client (P2)

                if (GlobalConstants.localPlayer == PlayerRef.None)
                {
                    // host
                    GlobalConstants.localPlayer = player;
                }
                else
                {
                    // client
                    GlobalConstants.remotePlayer = player;

                    // print("Sync in server from host to client");
                    // sync host to client of local player
                    // _genderSyncer.Synchronize(GlobalConstants.localPlayer.PlayerId,
                    //     GlobalConstants.playerGender == PlayerGender.Male);
                }
            }
            else
            {
                isServer = false;
                // p2 side
                GlobalConstants.remotePlayer = PlayerRef.None;
                GlobalConstants.localPlayer = player;

                // print("Sync in client from client to host");
                // // sync client to host of local player
                // _genderSyncer.Synchronize(GlobalConstants.localPlayer.PlayerId,
                //     GlobalConstants.playerGender == PlayerGender.Male);
            }

            // _genderSyncer.Synchronize(GlobalConstants.localPlayer.PlayerId,
                // GlobalConstants.playerGender == PlayerGender.Male);

            if (hideSelf)
            {
                HideSelfNetworkObject();
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            // Find and remove the players avatar
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
                GlobalConstants.RemovePlayer(player);
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            // var data = new NetworkInputData();
            //
            // if (Input.GetKey(KeyCode.W))
            //     data.direction += Vector3.forward;
            //
            // if (Input.GetKey(KeyCode.S))
            //     data.direction += Vector3.back;
            //
            // if (Input.GetKey(KeyCode.A))
            //     data.direction += Vector3.left;
            //
            // if (Input.GetKey(KeyCode.D))
            //     data.direction += Vector3.right;
            //
            // input.Set(data);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        public async void StartGame(GameMode mode)
        {
            var result = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "VRC2",
                CustomLobbyName = "VRC2",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
                // PlayerCount = 2,
                DisableClientSessionCreation = true
            });

            if (result.Ok)
            {
                // all good
                GlobalConstants.networkRunner = _runner;
                GlobalConstants.GameStarted = true;

                // callback
                OnGameStarted?.Invoke();
            }
            else
            {
                Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            }
        }

        public void StartHost()
        {
            GlobalConstants.Checker = false; // P1
            StartGame(GameMode.Host);
        }

        public void StartClient()
        {
            GlobalConstants.Checker = true; // P2
            StartGame(GameMode.Client);
        }

        private void OnGUI()
        {
            // if (!gameStarted)
            // {
            //     if (GUI.Button(new Rect(250, 10, 100, 40), "Host"))
            //     {
            //         StartHost();
            //     }
            //
            //     if (GUI.Button(new Rect(350, 10, 100, 40), "Join"))
            //     {
            //         StartClient();
            //     }
            // }
        }

        private void OnApplicationQuit()
        {
            if (_runner != null && _runner.IsRunning)
            {
                _runner.Shutdown();
            }
        }
    }
}