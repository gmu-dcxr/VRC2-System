using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC2;

public class StartScene : MonoBehaviour
{
    public string sceneToGoTo = "WIP";
    private NetworkRunner _runner;
    private bool gameStarted = false;
    private bool host = false;
    private bool join = false;

    [Space(30)] [Header("Gender")] public TextMeshProUGUI maleTMP;
    public TextMeshProUGUI femaleTMP;

    public Color selectedColor = Color.red;

    private Color normalColor;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        normalColor = maleTMP.color;
    }

    void Update()
    {
        if (host)
        {
            GlobalConstants.GameStarted = true;
            GlobalConstants.Checker = false; // P1
            StartGame(GameMode.Host);
            host = false;
        }
        else if (join)
        {
            GlobalConstants.GameStarted = true;
            GlobalConstants.Checker = true; // P2
            StartGame(GameMode.Client);
            join = false;
        }
    }

    public void HostButton()
    {
        if (GlobalConstants.playerGender == PlayerGender.Undefined)
        {
            Debug.LogError("Please select gender first");
            return;
        }

        if (!gameStarted)
        {
            host = true;
            SceneManager.LoadScene(sceneToGoTo);
        }
    }

    public void JoinButton()
    {
        if (GlobalConstants.playerGender == PlayerGender.Undefined)
        {
            Debug.LogError("Please select gender first");
            return;
        }

        if (!gameStarted)
        {
            join = true;
            SceneManager.LoadScene(sceneToGoTo);
        }
    }

    void UpdateTextColor(bool male)
    {
        // back to normal color
        maleTMP.color = normalColor;
        femaleTMP.color = normalColor;
        if (male)
        {
            maleTMP.color = selectedColor;
            GlobalConstants.playerGender = PlayerGender.Male;
        }
        else
        {
            femaleTMP.color = selectedColor;
            GlobalConstants.playerGender = PlayerGender.Female;
        }
    }

    public void MaleButton()
    {
        // update color
        UpdateTextColor(true);
    }

    public void FemaleButton()
    {
        UpdateTextColor(false);
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