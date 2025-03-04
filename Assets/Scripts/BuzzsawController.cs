using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuzzsawController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] MovementState currentState;  // Enum to select movement state

    float topOfScreen = 4.75f;
    float bottomOfScreen = -4.75f;
    float leftSideOfScreen = -8.75f;
    float rightSideOfScreen = 8.75f;

    Vector3 centerPosition;
    Vector3 originalPosition;

    public AnimationCurve curve;
    private Vector3 moveDirection = Vector3.zero;

    // Enum to define different movement states
    public enum MovementState
    {
        Diagonal,
        UpDown,
        LeftRight,
        Reflect
    }

    void Start()
    {
        centerPosition = new Vector3(0, 0, 0);
        originalPosition = transform.position;
    }

    void Update()
    {
        float timeFactor = Mathf.Abs(Mathf.Sin(Time.time * moveSpeed));

        // Handle different movement types based on the selected enum state
        switch (currentState)
        {
            case MovementState.Diagonal:
                // Diagonal movement from bottom-left to top-right
                transform.position = Vector3.Lerp(originalPosition, centerPosition, curve.Evaluate(timeFactor));
                break;

            case MovementState.UpDown:
                // Up-down movement from bottom to top
                Vector3 upPosition = new Vector3(transform.position.x, topOfScreen, 0);  // Adjust top position for UpDown movement
                transform.position = Vector3.Lerp(new Vector3(transform.position.x, bottomOfScreen, 0), upPosition, curve.Evaluate(timeFactor));
                break;

            case MovementState.LeftRight:
                // Left-right movement from left to right
                Vector3 rightPosition = new Vector3(rightSideOfScreen, 0, 0);  // Adjust right position for LeftRight movement
                transform.position = Vector3.Lerp(new Vector3(leftSideOfScreen, 0, 0), rightPosition, curve.Evaluate(timeFactor));
                break;
            case MovementState.Reflect:
                // Reflect movement each time it clamps to any of the screen edges
                transform.position = moveDirection;
                if (transform.position.y >= topOfScreen || transform.position.y <= bottomOfScreen)
                {
                    moveDirection.y = -moveDirection.y;
                }
                break;
        }
    }
}