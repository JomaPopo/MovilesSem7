using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int level;
    public int experience;
    public int skillPoints;
    public int strength;
    public int defense;
    public int agility;

    public PlayerData(string name)
    {
        playerName = name;
        level = 1;
        experience = 0;
        skillPoints = 0;
        strength = 5;
        defense = 5;
        agility = 5;
    }
}
