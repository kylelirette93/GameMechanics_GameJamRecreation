using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameStateManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public SpawnManager SpawnManager;

    // UI Panels for various states.
    public GameObject menuPanel;
    public GameObject controlsPanel;
    public GameObject tutorialPanel;
    public GameObject gameplayPanel;
    public GameObject winPanel;
    public GameObject gameOverPanel;
    GameObject goal;
    GameObject player;
    public enum GameState
    {
        MainMenu,
        Controls,
        Tutorial,
        Gameplay,
        Boss,
        GameWin,
        GameOver
    }

    public GameState currentState { get; private set; }
    public GameState previousState { get; private set; }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
        goal = GameObject.Find("Goal");
        player = GameObject.Find("Player");
    }

    public void ChangeState(GameState newState)
    {
        ExitState(currentState);
        previousState = currentState;
        currentState = newState;
        EnterState(currentState);
    }

    private void Update()
    {
        if (currentState == GameState.MainMenu && Input.GetKeyDown(KeyCode.X))
        {
            // Reset waves and lives when starting a new game.
            player.SetActive(true);
            SpawnManager.Reset();
            PlayerController.Lives = 9;
            ChangeState(GameState.Controls);
        }
        if (currentState == GameState.Controls && SpawnManager.WaveNumber > 0)
        {
            ChangeState(GameState.Tutorial);
        }
        else if (currentState == GameState.Tutorial && SpawnManager.WaveNumber > 1)
        {
            ChangeState(GameState.Gameplay);
        }
        else if (currentState == GameState.Gameplay && SpawnManager.WaveNumber > 8)
        {
            ChangeState(GameState.Boss);
        }
        else if (currentState == GameState.GameWin && Input.GetKeyDown(KeyCode.M))
        {
            SpawnManager.ResetPlayer();
            ChangeState(GameState.MainMenu);

        }
        else if (currentState == GameState.GameWin && Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        else if (currentState == GameState.GameOver && Input.GetKeyDown(KeyCode.M))
        {
            SpawnManager.ResetPlayer();
            ChangeState(GameState.MainMenu);

        }
        else if (currentState == GameState.GameOver && Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    private void ExitState(GameState state)
    {
        // Deactivate UI when exiting that state.
        previousState = state;
        switch (state)
        {
            case GameState.MainMenu:
                menuPanel.SetActive(false);
                // Exit MainMenu
                break;
            case GameState.Controls:
                controlsPanel.SetActive(false);
                // Exit Controls
                break;
            case GameState.Tutorial:
                tutorialPanel.SetActive(false);
                break;
            case GameState.Gameplay:
                // Exit Gameplay
                break;
            case GameState.GameWin:
                winPanel.SetActive(false);
                break;
            case GameState.Boss:
                break;
            case GameState.GameOver:               
                // Exit GameOver
                gameOverPanel.SetActive(false);
                break;
        }
    }

    private void EnterState(GameState state)
    {
        // Active UI when entering that state.
        switch (state)
        {
            case GameState.MainMenu:
                menuPanel.SetActive(true);
                Time.timeScale = 0;
                // Enter MainMenu
                break;
            case GameState.Controls:
                controlsPanel.SetActive(true);
                Time.timeScale = 1;
                goal.SetActive(true);
                // Enter Controls
                break;
            case GameState.Tutorial:
                tutorialPanel.SetActive(true);
                GameManager.instance.audioManager.PlayAudio(GameManager.instance.audioManager.gameplayMusic);
                // Start timer when entering tutorial level.
                Timer.instance.StartTimer();
                break;
            case GameState.Gameplay:
                // Enter Gameplay
                gameplayPanel.SetActive(true);
                break;
            case GameState.Boss:
                // Enter Boss state
                goal.SetActive(false);
                GameManager.instance.audioManager.PlayAudio(GameManager.instance.audioManager.bossMusic);
                break;
            case GameState.GameWin:
                // Enter Game win state.
                // Stop timer before displaying win panel.
                Timer.instance.StopTimer();
                gameplayPanel.SetActive(false);
                winPanel.SetActive(true);
                GameManager.instance.audioManager.StopAudio();
                Time.timeScale = 1;
                break;
            case GameState.GameOver:
                // Enter GameOver
                // Reset timer if game over.
                Timer.instance.ResetTime();
                gameplayPanel.SetActive(false);
                gameOverPanel.SetActive(true);
                GameManager.instance.audioManager.StopAudio();
                player.SetActive(false);
                Time.timeScale = 0;
                break;
        }
    }
}
