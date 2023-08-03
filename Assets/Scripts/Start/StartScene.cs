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

public class StartScene : MonoBehaviour
{
    public string sceneToGoTo = "WIP";
    private NetworkRunner _runner;
    private bool gameStarted = false;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HostButton()
    {
        if (!gameStarted)
        {
            SceneManager.LoadScene(sceneToGoTo);
            GlobalConstants.GameStarted = true;
            GlobalConstants.Checker = false; // P1
            StartGame(GameMode.Host);
        }
    }
    public void JoinButton()
    {
        if (!gameStarted)
        {
            SceneManager.LoadScene(sceneToGoTo);
            GlobalConstants.GameStarted = true;
            GlobalConstants.Checker = true; // P2
            StartGame(GameMode.Client);
        }
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
            gameStarted = true;
        }
        else
        {
            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
        }
    }
}
