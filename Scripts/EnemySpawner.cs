using System.Collections;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject MaggotPrefab;
    public int enemiesPerWave = 20;
    public float spawnRadius = 5f;

    public TMP_Text waveText;

    private int currentWave = 1;
    private bool waveInProgress = false;
    public GameObject bossPrefab;
    private bool bossSpawned = false;
    private bool spawningStopped = false;
    private GameObject currentBoss;
    private GameOverManager gameOverManager;

    void Start()
    {
        StartCoroutine(WaveLoop());
        gameOverManager = FindObjectOfType<GameOverManager>();

        if (gameOverManager == null)
        {
            Debug.LogError("❌ GameOverManager wurde nicht gefunden!"); //Debuggen
        }
        else
        {
            Debug.Log("✅ GameOverManager wurde erfolgreich gefunden."); //Debuggen
        }
    }

    IEnumerator WaveLoop()
    {
        while (!spawningStopped)
        {
            if (!waveInProgress && NoEnemiesRemaining())
            {
                waveInProgress = true;

                Debug.Log("Spawning wave " + currentWave); //Debuggen
                UpdateWaveText();
                SpawnWave();

                currentWave++;
            }

            // Zeigt Endscreen, wenn Wave 57 erreicht wird
            if (currentWave == 57)
            {
                Debug.Log("🌟 EndScreen wird jetzt über Wave 57 angezeigt!"); //Debuggen
                StopSpawning();

                if (gameOverManager != null)
                {
                    gameOverManager.ShowEndScreen();
                    Debug.Log("✅ Endscreen wird angezeigt!"); //Debuggen
                }
                else
                {
                    Debug.LogError("❌ GameOverManager nicht gefunden!"); //Debuggen
                }

                yield break;
            }

            yield return null;
        }
    }

    void SpawnWave()
    {
        Debug.Log("Versuche Boss zu spawnen. Aktuelle Welle: " + currentWave + " BossSpawned: " + bossSpawned); //Debuggen

        if (currentWave == 55 && !bossSpawned)
        {
            currentBoss = Instantiate(bossPrefab, transform.position, Quaternion.identity);
            currentBoss.tag = "Enemy";
            bossSpawned = true;
            Debug.Log("Boss wurde erfolgreich gespawnt: " + currentBoss.name); //Debuggen
        }
        else if (currentWave < 55 && !bossSpawned)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
                GameObject enemy = Instantiate(MaggotPrefab, spawnPos, Quaternion.identity);
                enemy.tag = "Enemy";
            }
        }

        StartCoroutine(WaitForWaveToEnd());
    }

    IEnumerator WaitForWaveToEnd()
    {
        while (!NoEnemiesRemaining())
        {
            yield return new WaitForSeconds(0.5f);
        }

        waveInProgress = false;

        if (currentWave <= 50)
        {
            FindObjectOfType<UpgradeManager>()?.TryOfferUpgrade(currentWave);
        }
    }

    bool NoEnemiesRemaining()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Enemies remaining: " + enemyCount); //Debuggen

        return enemyCount == 0 && (currentBoss == null || !currentBoss.activeInHierarchy);
    }

    public void StopSpawning()
    {
        Debug.Log("Spawning gestoppt!"); //Debuggen
        StopAllCoroutines();
        spawningStopped = true;
        enabled = false;
    }

    void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = "Wave " + currentWave;
        }
    }

    public void BossDied() // Muss public sein!
    {
        Debug.Log("✅ BossDied() wurde aufgerufen."); //Debuggen
    }
}