using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameStateManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public SpawnManager SpawnManager;

    public GameObject menuPanel;
    public GameObject controlsPanel;
    public GameObject tutorialPanel;
    public enum GameState
    {
        MainMenu,
        Controls,
        Tutorial,
        Gameplay,
        GameOver
    }

    public GameState currentState { get; private set; }
    public GameState previousState { get; private set; }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
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
    }

    private void ExitState(GameState state)
    {
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
                case GameState.GameOver:
                // Exit GameOver
                break;
        }
    }

    private void EnterState(GameState state)
    {
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
                // Enter Controls
                break;
            case GameState.Tutorial:
                tutorialPanel.SetActive(true);
                GameManager.instance.audioManager.PlayAudio(GameManager.instance.audioManager.gameplayMusic);
                break;
            case GameState.Gameplay:
                // Enter Gameplay
                break;
            case GameState.GameOver:
                // Enter GameOver
                break;
        }
    }
}
