using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Controls,
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
        
    }

    private void ExitState(GameState state)
    {
        previousState = state;
        switch (state)
        {
            case GameState.MainMenu:
                // Exit MainMenu
                break;
            case GameState.Controls:
                // Exit Controls
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
                // Enter MainMenu
                break;
            case GameState.Controls:
                // Enter Controls
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
