#if UNITY_WSA
using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
    
}
#else

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

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Prefab")] public NetworkPrefabRef _playerPrefab;
    public string prefabName;

    public NetworkPrefabRef _pipePrefab;

    public GameObject src;

    [Header("Setting")] public bool hideSelf = false;

    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private bool isServer = false;

    private NetworkRunner _runner;
    
    // local player
    private PlayerRef _localPlayer;

    // Start is called before the first frame update
    void Start()
    {
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

    private void AttachSourceTransforms(NetworkObject nwo, GameObject src)
    {
        var go = nwo.gameObject;
        var childCount = go.transform.childCount;
        for (var i = 0; i < childCount; i++)
        {
            var g = go.transform.GetChild(i).gameObject;
            TransformAttachment ta;
            if (g.TryGetComponent<TransformAttachment>(out ta))
            {
                if (g.name == src.name)
                {
                    ta.source = src;
                    Debug.LogWarning($"[WARN]Attached transform for {g.name}");
                }
            }
        }
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
        if (runner.IsServer)
        {
            isServer = true;
            // Create a unique position for the player
            // Vector3 spawnPosition =
            // new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, networkPlayerObject);

            // p1 side
            if (networkPlayerObject.HasInputAuthority)
            {
                // attach transform
                // AttachSourceTransforms(networkPlayerObject, src);
                _localPlayer = player;
                GlobalConstants.localPlayer = player;
            }
        }
        else
        {
            isServer = false;
            // p2 side
            GlobalConstants.remotePlayer = player;
        }

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

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
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
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "VRC2",
            CustomLobbyName = "VRC2",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 2,
            DisableClientSessionCreation = true
        });

        if (result.Ok)
        {
            // all good
            GlobalConstants.networkRunner = _runner;
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }

    void CreateNewPipe()
    {
        _runner.Spawn(_pipePrefab, new Vector3(0f, 1.5f, 2f), Quaternion.identity, _localPlayer);
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 120, 100, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }

            if (GUI.Button(new Rect(100, 120, 100, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 200, 100, 40), "Pipe"))
            {
                CreateNewPipe();
            }
        }
    }
}
#endif