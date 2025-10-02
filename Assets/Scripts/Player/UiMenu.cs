using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text playerNameTxt;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private TMP_Text expTxt;
    [SerializeField] private TMP_Text skillPointsTxt;

    [Header("Stat Texts")]
    [SerializeField] private TMP_Text strengthTxt;
    [SerializeField] private TMP_Text defenseTxt;
    [SerializeField] private TMP_Text agilityTxt;

    [Header("Name UI")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button updateNameBtn;

    [Header("Buttons")]
    [SerializeField] private Button strengthBtn;
    [SerializeField] private Button defenseBtn;
    [SerializeField] private Button agilityBtn;

    private void OnEnable()
    {
        strengthBtn.onClick.AddListener(() => { _ = PlayerManager.Instance.UpgradeStat("strength"); RefreshUI(); });
        defenseBtn.onClick.AddListener(() => { _ = PlayerManager.Instance.UpgradeStat("defense"); RefreshUI(); });
        agilityBtn.onClick.AddListener(() => { _ = PlayerManager.Instance.UpgradeStat("agility"); RefreshUI(); });

        updateNameBtn.onClick.AddListener(UpdateName);
    }
    private void Start()
    {
      
        InitUI();
    }

    public void InitUI()
    {
        
        nameInput.text = PlayerManager.Instance.Data.playerName;
        RefreshUI();
    }

    private async void UpdateName()
    {
        await PlayerManager.Instance.UpdatePlayerName(nameInput.text);
        RefreshUI();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ = PlayerManager.Instance.AddExperience(20);
            RefreshUI();
        }
       
    }

    public void RefreshUI()
    {
        var data = PlayerManager.Instance.Data;

        playerNameTxt.text = "Nombre: " + data.playerName;
        levelTxt.text = "Nivel: " + data.level;
        expTxt.text = $"{data.experience}/{PlayerManager.Instance.GetExpToNextLevel()}";
        skillPointsTxt.text = "Puntos: " + data.skillPoints;

        strengthTxt.text = "Fuerza: " + data.strength;
        defenseTxt.text = "Defensa: " + data.defense;
        agilityTxt.text = "Agilidad: " + data.agility;
    }
}
