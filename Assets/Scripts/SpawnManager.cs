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
    int waveNumber = 0;
    public int buzzsawCount = 0;
    public enum GameState
    {
        Menu, // Press X to Start
        Controls, // Display controls to player.
        Playing, // Buzzsaw spawns, let player know to avoid them.
        Wave1, // Moving saw
        Wave2, // Reset original saw, spawn a new saw
        Wave3, // Reset both saws, spawn a new saw, position all in other three corners.
        Wave4, // Get rid of three saws, Spawn a big moving one in center
        Wave5, // Reset big saw position, spawn 4 in a cross extending out from big saw.
        Wave6, // Get rid of big saw, spawn 9 saws and reset position to top, have them go vertically.
        Wave7, // Get rid of 9 saws, spawn 14 big saws, 2 of which are side by side and go towards middle. 1 small one on the far right that goes vertically faster.
        Boss, // Spawn red cat in center. Once you reach activation distance have the cat start following the player. Buzzsaws spawn from random distances from player
              // and move in the direction of player. Goal is replaced with a projectile to damage cat, cat takes 10 hits before death.
    }

    public GameState currentState { get; private set; }

    private void OnEnable()
    {
        Actions.ResetPlayer += ResetPlayer;
        Actions.NextWave += NextWave;
    }

    private void Start()
    {
        ChangeState(GameState.Menu);
    }

    public void ChangeState(GameState newState)
    {
        ExitState(currentState);
        currentState = newState;
        EnterState(currentState);
    }

    private void Update()
    {
        
    }

    public void ExitState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                break;
            case GameState.Controls:
                break;
            case GameState.Playing:
                break;
            case GameState.Wave1:
                break;
            case GameState.Wave2:
                break;
            case GameState.Wave3:
                break;
            case GameState.Wave4:
                break;
            case GameState.Wave5:
                break;
            case GameState.Wave6:
                break;
            case GameState.Wave7:
                break;
            case GameState.Boss:
                break;
        }
    }
    public void EnterState(GameState state)
    {
        switch (state) 
        {
            case GameState.Menu:
                break;
            case GameState.Controls:
                break;
            case GameState.Playing:
                break;
            case GameState.Wave1:
                break;
            case GameState.Wave2:
                break;
            case GameState.Wave3:
                break;
            case GameState.Wave4:
                break;
            case GameState.Wave5:
                break;
            case GameState.Wave6:
                break;
            case GameState.Wave7:
                break;
            case GameState.Boss:
                break;
        }
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
