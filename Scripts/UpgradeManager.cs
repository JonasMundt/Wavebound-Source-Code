using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UpgradeManager : MonoBehaviour
{
    [System.Serializable]
    public class Upgrade
    {
        public int waveNumber;
        public string description;
        public UpgradeType type;
    }

    public enum UpgradeType
    {
        FasterShooting,
        DamageAura,
        MoveSpeed,
        MaxHP,
        SpawnBuddy
    }

    public Upgrade[] upgrades;
    public GameObject upgradePanel;
    public TMP_Text upgradeDescriptionText;
    public GameObject buddyPrefab;
    private PlayerShooting playerShooting;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerShooting = player.GetComponent<PlayerShooting>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
        upgradePanel.SetActive(false);
    }

    public void TryOfferUpgrade(int currentWave)
    {
        foreach (Upgrade upgrade in upgrades)
        {
            if (upgrade.waveNumber == currentWave)
            {
                upgradeDescriptionText.text = "Upgrade verfügbar:\n" + upgrade.description;
                upgradePanel.SetActive(true);
                Time.timeScale = 0f; // Pause das Spiel
                _currentUpgrade = upgrade;
                break;
            }
        }
    }

    private Upgrade _currentUpgrade;

    public void AcceptUpgrade()
    {
        ApplyUpgrade(_currentUpgrade.type);
        ClosePanel();
    }

    public void SkipUpgrade()
    {
        ClosePanel();
    }

    void ClosePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void ApplyUpgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.FasterShooting:
                playerShooting.shootInterval /= 2f;
                break;
            case UpgradeType.DamageAura:
                Transform aura = player.transform.Find("Aura");
                if (aura != null)
                {
                    aura.gameObject.SetActive(true);
                }
                break;
            case UpgradeType.MoveSpeed:
                playerMovement.moveSpeed += 2f;
                break;
            case UpgradeType.MaxHP:
                playerHealth.SetMaxHP(200);
                break;
            case UpgradeType.SpawnBuddy:
            Vector3 spawnPos = player.transform.position + new Vector3(1.5f, 0, 0);
            GameObject buddy = Instantiate(buddyPrefab, spawnPos, Quaternion.identity);
            BuddyAI buddyAI = buddy.GetComponent<BuddyAI>();
            if (buddyAI != null)
            {
                buddyAI.player = player.transform;
                buddyAI.projectilePrefab = playerShooting.projectilePrefab;
            }
                break;
            
        }
    }
}
