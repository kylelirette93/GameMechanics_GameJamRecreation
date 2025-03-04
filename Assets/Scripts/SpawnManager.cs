using UnityEngine;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] waves;
    public GameObject player;
    private GameObject waveInstance;
    public Transform spawnPoint;
    public bool IsResetting { get { return isResetting; } }
    bool isResetting = false;
    public int WaveNumber { get { return waveNumber; } }
    int waveNumber = 0;
    public int buzzsawCount = 0;
    private float tweenFactor = 1f;
    Quaternion originalRotation;

    private void OnEnable()
    {
        originalRotation = transform.rotation;
        Actions.ResetPlayer += ResetPlayer;
        Actions.NextWave += NextWave;
    }

    void NextWave()
    {
        // Start the next wave.
        StartCoroutine(HandleNextWave());
    }

    private void Update()
    {
        if (GameManager.instance.gameStateManager.currentState == GameStateManager.GameState.GameWin ||
            GameManager.instance.gameStateManager.currentState == GameStateManager.GameState.GameOver)
        {
            Destroy(waveInstance);
        }
    }

    IEnumerator HandleNextWave()
    {
        // Clear the current wave.
        if (waveNumber > 0)
        {
            Destroy(waveInstance);
        }

        // Reset the player.
        yield return ResetPlayerCoroutine();

        // Start the next wave.
        if (waveNumber < waves.Length)
        {
            waveInstance = Instantiate(waves[waveNumber], new Vector2(0, 0), Quaternion.identity);
            waveNumber++;
            Debug.Log("Starting wave: " + waveNumber);
        }
        else
        {
            Debug.Log("No more waves to start.");
        }
    }

    public void Reset()
    {
        // Start back at first wave.
        waveNumber = 0;
        DestroyLeftovers();
    }

    private void DestroyLeftovers()
    {
        // Destroy leftover projectiles.
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }
    }

    IEnumerator ResetPlayerCoroutine()
    {
        isResetting = true;
        // Reset player if they hit a buzzsaw.
        player.transform.DOMove(spawnPoint.position, tweenFactor);
        yield return player.transform.DOShakeRotation(tweenFactor).WaitForCompletion();
        player.transform.rotation = originalRotation;
        isResetting = false;
    }

    public void ResetPlayer()
    {
        // Reset player if they hit a buzzsaw.
        StartCoroutine(ResetPlayerCoroutine());
    }
}