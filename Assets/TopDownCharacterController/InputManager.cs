using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour, GameInput.IPlayerActions
{
    GameInput gameInput;
    void Start()
    {
        // Initialize the instance of game input.
        gameInput = new GameInput();

        // Enable the player action map.
        gameInput.Player.Enable();

        // Set the callbacks for the player action map.
        gameInput.Player.SetCallbacks(this);
    }



    public void OnMove(InputAction.CallbackContext context)
    {
        // Invoke the move event with value of the context.
        Actions.MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Invoke the start interact event, based on started context.
            Actions.StartInteractEvent?.Invoke();
        }
        if (context.canceled)
        {
            // Invoke the canceled interact event, based on canceled context.
            Actions.CancelInteractEvent?.Invoke();
        }
    }
}

public static class Actions
{
    // Define events for each action.
    public static Action<Vector2> MoveEvent;
    public static Action StartInteractEvent;
    public static Action CancelInteractEvent;
}