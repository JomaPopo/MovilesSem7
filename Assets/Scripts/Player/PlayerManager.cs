using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public PlayerData Data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Cargar datos desde Cloud Save
    public async Task LoadPlayerData(string defaultName)
    {
        var keys = new HashSet<string> { "playerData" };
        var savedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if (savedData != null && savedData.ContainsKey("playerData"))
        {
            string json = savedData["playerData"].Value.GetAs<string>();
            Data = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            // ?? Obtener el nombre desde Authentication
            string authName = await AuthenticationService.Instance.GetPlayerNameAsync();
            if (string.IsNullOrEmpty(authName)) authName = defaultName;

            Data = new PlayerData(authName);
            await SavePlayerData();
        }
    }

    // Guardar datos en Cloud Save
    public async Task SavePlayerData()
    {
        string json = JsonUtility.ToJson(Data);
        var playerData = new Dictionary<string, object> { { "playerData", json } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
    }

    // Experiencia necesaria para el próximo nivel
    public int GetExpToNextLevel()
    {
        return 100 + (Data.level - 1) * 50;
    }

    // Ganar experiencia
    public async Task AddExperience(int amount)
    {
        Data.experience += amount;

        int expNeeded = GetExpToNextLevel();

        if (Data.experience >= expNeeded)
        {
            Data.experience -= expNeeded;
            Data.level++;
            Data.skillPoints += 3;
        }

        await SavePlayerData();
    }

    // Mejorar estadística
    public async Task UpgradeStat(string stat)
    {
        if (Data.skillPoints <= 0) return;

        switch (stat)
        {
            case "strength": Data.strength++; break;
            case "defense": Data.defense++; break;
            case "agility": Data.agility++; break;
        }

        Data.skillPoints--;
        await SavePlayerData();
    }

    // Actualizar nombre
    public async Task UpdatePlayerName(string newName)
    {
        Data.playerName = newName;

        // ?? Guardar en Authentication
        await AuthenticationService.Instance.UpdatePlayerNameAsync(newName);

        // ?? Guardar en Cloud Save
        await SavePlayerData();
    }
}
