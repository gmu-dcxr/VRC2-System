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
using VRC2.Events;
using FishNetNetworkObject = FishNet.Object.NetworkObject;

namespace VRC2
{

    [RequireComponent(typeof(NetworkRunner))]
    public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        [Header("Character Prefab")] public NetworkPrefabRef _playerPrefab;

        public string prefabName;

        private NetworkRunner _runner;

        // where to spawn
        [Header("Spawn Transform")] public List<Transform> spawnTransforms;
        public Transform cameraRig; // move camera rig along with spawn transform

        [Header("Setting")] public bool hideSelf = false;

        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        private bool isServer = false;

        // private bool gameStarted = false;

        // private GenderSyncer _genderSyncer;

        public System.Action OnGameStarted;

        #region Player helper

        private PlayerHelper _playerHelper;

        private PlayerHelper playerHelper
        {
            get
            {
                if (_playerHelper == null)
                {
                    _playerHelper = FindFirstObjectByType<PlayerHelper>();
                }

                return _playerHelper;
            }
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            _runner = gameObject.GetComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            // _genderSyncer = FindObjectOfType<GenderSyncer>();
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

        public bool ReadyToSyncGender(bool client)
        {
            // return false if _runner is null or not running
            if (_runner == null || !_runner.IsRunning) return false;

            // runner is running and active players count > 1
            return _runner.IsRunning && (_runner.IsClient == client) && _runner.ActivePlayers.Count() > 1;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private IEnumerable<GameObject> GetAllClones()
        {
            var name = prefabName + "(Clone)";
            // hide client self in client view
            var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == name);
            return objects;
        }

        public GameObject GetSelfObject()
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

        #region FishNet

        public int FindLocalFishNetObjectId()
        {
            var gos = GameObject.FindObjectsOfType<OVRCustomSkeleton>();

            foreach (var go in gos)
            {
                var obj = go.gameObject.GetComponent<FishNetNetworkObject>();
                if (obj.Owner.IsLocalClient)
                {
                    return obj.ObjectId;
                }
            }

            return -1;
        }

        public GameObject GetFishNetObjectByID(int id)
        {
            var gos = GameObject.FindObjectsOfType<OVRCustomSkeleton>();

            foreach (var go in gos)
            {
                var obj = go.gameObject.GetComponent<FishNetNetworkObject>();
                if (obj.ObjectId.Equals(id))
                {
                    return go.gameObject;
                }
            }

            return null;
        }

        #endregion

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

                var position = Vector3.zero;
                var rotation = Quaternion.identity;

                if (spawnTransforms != null && spawnTransforms.Count > _spawnedCharacters.Count)
                {
                    print($"Update spawn transform for {_spawnedCharacters.Count}");

                    var t = spawnTransforms[_spawnedCharacters.Count];
                    position = t.position;
                    rotation = t.rotation;
                }

                NetworkObject networkPlayerObject =
                    runner.Spawn(_playerPrefab, position, rotation, player);
                // Keep track of the player avatars so we can remove it when they disconnect
                _spawnedCharacters.Add(player, networkPlayerObject);

                // set player object for updating character
                runner.SetPlayerObject(player, networkPlayerObject);

                // The first will be host (P1), all spawning actions will be done host
                // The second will be client (P2)

                if (GlobalConstants.localPlayer == PlayerRef.None)
                {
                    // update camera rig
                    cameraRig.transform.position = position;
                    cameraRig.transform.rotation = rotation;
                    // host
                    GlobalConstants.localPlayer = player;
                    // fishnet
                    GlobalConstants.localFishNetPlayer = FindLocalFishNetObjectId();
                    print($"Set localFishNetPlayer = {GlobalConstants.localFishNetPlayer}");

                    // update transform
                    var fp = GetFishNetObjectByID(GlobalConstants.localFishNetPlayer);
                    fp.transform.position = position;
                    fp.transform.rotation = rotation;
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

                // fishnet
                GlobalConstants.localFishNetPlayer = FindLocalFishNetObjectId();
                print($"Set localFishNetPlayer = {GlobalConstants.localFishNetPlayer}");

                if (spawnTransforms != null && spawnTransforms.Count > 1)
                {
                    var t = spawnTransforms[1];
                    // update camera rig
                    cameraRig.transform.position = t.position;
                    cameraRig.transform.rotation = t.rotation;

                    // update transform
                    var fp = GetFishNetObjectByID(GlobalConstants.localFishNetPlayer);
                    fp.transform.position = t.position;
                    fp.transform.rotation = t.rotation;
                }

                // print("Sync in client from client to host");
                // // sync client to host of local player
                // _genderSyncer.Synchronize(GlobalConstants.localPlayer.PlayerId,
                //     GlobalConstants.playerGender == PlayerGender.Male);
            }

            // _genderSyncer.RequestSync(GlobalConstants.localPlayer.PlayerId,
            // GlobalConstants.playerGender == PlayerGender.Male);

            if (hideSelf)
            {
                HideSelfNetworkObject();
            }

            // reset player helper
            playerHelper.ResetLocalPlayer();
            // move up
            playerHelper.ArrangeLocalPlayer();
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