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
    [Space(30)] [Header("Gender")] public TextMeshProUGUI maleTMP;
    public TextMeshProUGUI femaleTMP;

    public Color selectedColor = Color.red;

    private Color normalColor;

    private PlayerSpawner _playerSpawner;

    [Space(30)] [Header("Gameobjects")] public GameObject canvas;
    public GameObject lobby;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        normalColor = maleTMP.color;

        _playerSpawner = FindObjectOfType<PlayerSpawner>();

        _playerSpawner.OnGameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
        // hide canvas and lobby
        canvas.SetActive(false);
        lobby.SetActive(false);
    }

    void Update()
    {

    }

    public void HostButton()
    {
        if (GlobalConstants.playerGender == PlayerGender.Undefined)
        {
            Debug.LogError("Please select gender first");
            return;
        }

        _playerSpawner.StartHost();

    }

    public void JoinButton()
    {
        if (GlobalConstants.playerGender == PlayerGender.Undefined)
        {
            Debug.LogError("Please select gender first");
            return;
        }

        _playerSpawner.StartClient();
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
}