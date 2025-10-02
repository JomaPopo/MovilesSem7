using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Authentication.PlayerAccounts;
using System;
using System.Threading.Tasks;

public class UnityPlayerAuth : MonoBehaviour
{
    public event Action<PlayerInfo, string> OnSignedIn;
    private PlayerInfo playerInfo;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        SetupEvents();

        PlayerAccountService.Instance.SignedIn += SignIn;
    }

    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Player ID " + AuthenticationService.Instance.PlayerId);
        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.Log("Sign In Failed: " + err);
        };
    }

    // Iniciar SignIn
    public async Task InitSignIn()
    {
        await PlayerAccountService.Instance.StartSignInAsync();
    }

    private async void SignIn()
    {
        try
        {
            string accessToken = PlayerAccountService.Instance.AccessToken;
            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
            Debug.Log("Login Success");

            playerInfo = AuthenticationService.Instance.PlayerInfo;
            string name = await AuthenticationService.Instance.GetPlayerNameAsync();

            OnSignedIn?.Invoke(playerInfo, name);
        }
        catch (Exception ex)
        {
            Debug.Log("Error signing in: " + ex);
        }
    }
}
