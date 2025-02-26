using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] waves;
    public GameObject player;
    private GameObject waveInstance;
    public Transform spawnPoint;
    float speed = 0;
    public int WaveNumber { get {  return waveNumber; } }
    int waveNumber = 0;
    public int buzzsawCount = 0;
    public GameObject buzzsawPrefab;
   

    private void OnEnable()
    {
        Actions.ResetPlayer += ResetPlayer;
        Actions.NextWave += NextWave;
    }
    
    void NextWave()
    {
        // Start the next wave.
        ResetPlayer();
        if (waveNumber < waves.Length)
        {
            if (waveNumber > 0)
            {
                Destroy(waveInstance);
            }
            waveInstance = Instantiate(waves[waveNumber], new Vector2(0, 0), Quaternion.identity);
            waveNumber++;
            Debug.Log("Starting wave: " + waveNumber);

        }
        else
        {
            Debug.Log("No more waves to start.");
        }
    }


    public void ResetPlayer()
    {
        // Reset player if they hit a buzzsaw.
        player.transform.position = spawnPoint.position;
    }
}
