using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FishNet.Managing;
using FishNet.Transporting.Tugboat;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRC2;

public class StartScene : MonoBehaviour
{
    // [Space(30)] [Header("Gender")] public TextMeshProUGUI maleTMP;
    // public TextMeshProUGUI femaleTMP;
    //
    // public Color selectedColor = Color.red;
    //
    // private Color normalColor;

    [Space(30)] [Header("PlayerSpawner")] public PlayerSpawner _playerSpawner;

    [Space(30)] [Header("Gameobjects")] public GameObject startMenu;
    public GameObject mainEventSystem;
    public Button hostButton;
    public Button joinButton;

    [Space(30)] [Header("FishNet")] public Tugboat boat;

    [Space(30)] [Header("Debug")] public bool enableDebug = false;

    void Start()
    {
        mainEventSystem.SetActive(false);

        // normalColor = maleTMP.color;

        // _playerSpawner = FindObjectOfType<PlayerSpawner>();

        _playerSpawner.OnGameStarted += OnGameStarted;

        hostButton.onClick.AddListener(HostButton);
        joinButton.onClick.AddListener(JoinButton);
    }

    private void OnGameStarted()
    {
        mainEventSystem.SetActive(true);
        startMenu.SetActive(false);
    }

    void Update()
    {
        if (!enableDebug) return;

        if (Input.GetKeyUp(KeyCode.H))
        {
            HostButton();
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            JoinButton();
        }

        else if (Input.GetKeyUp(KeyCode.M))
        {
            // MaleButton();
            GlobalConstants.playerGender = PlayerGender.Male;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            // FemaleButton();
            GlobalConstants.playerGender = PlayerGender.Female;
        }
    }

    public void HostButton()
    {
        // start Fish-Net
        boat.StartConnection(true);
        boat.StartConnection(false);
        _playerSpawner.StartHost();

    }

    public void JoinButton()
    {
        boat.StartConnection(false);
        _playerSpawner.StartClient();
    }

    // void UpdateTextColor(bool male)
    // {
    //     // back to normal color
    //     maleTMP.color = normalColor;
    //     femaleTMP.color = normalColor;
    //     if (male)
    //     {
    //         maleTMP.color = selectedColor;
    //         GlobalConstants.playerGender = PlayerGender.Male;
    //     }
    //     else
    //     {
    //         femaleTMP.color = selectedColor;
    //         GlobalConstants.playerGender = PlayerGender.Female;
    //     }
    // }
    //
    // public void MaleButton()
    // {
    //     // update color
    //     UpdateTextColor(true);
    // }
    //
    // public void FemaleButton()
    // {
    //     UpdateTextColor(false);
    // }
}