using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class UILogin : MonoBehaviour
{
    [SerializeField] private Transform loginPanel;
    [SerializeField] private Transform userPanel;

    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text playerIDTxt;
    [SerializeField] private TMP_Text playerNameTxt;

    [SerializeField] private UnityPlayerAuth unityPlayerAuth;

    private void Start()
    {
        loginPanel.gameObject.SetActive(true);
        userPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        loginButton?.onClick.AddListener(LoginButton);
        unityPlayerAuth.OnSignedIn += UnityPlayerOnSignedIn;
    }

    private async void UnityPlayerOnSignedIn(PlayerInfo playerInfo, string playerName)
    {
        loginPanel.gameObject.SetActive(false);
        userPanel.gameObject.SetActive(true);

        playerIDTxt.text = "ID: " + playerInfo.Id;
        playerNameTxt.text = playerName;

        // ?? Cargar datos de Cloud Save para este jugador
        await PlayerManager.Instance.LoadPlayerData(playerName);

        // Pasar a la escena de menú
        SceneManager.LoadScene("MenuScene");
    }

    private async void LoginButton()
    {
        await unityPlayerAuth.InitSignIn();
    }

    private void OnDisable()
    {
        loginButton?.onClick.RemoveListener(LoginButton);
        unityPlayerAuth.OnSignedIn -= UnityPlayerOnSignedIn;
    }
}
