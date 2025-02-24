using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] MovementState currentState;  // Enum to select movement state

    float topOfScreen = 4.5f;
    float bottomOfScreen = -4.5f;
    float leftSideOfScreen = -8.5f;
    float rightSideOfScreen = 8.5f;

    Vector3 centerPosition;
    Vector3 originalPosition;

    public AnimationCurve curve;

    // Enum to define different movement states
    public enum MovementState
    {
        Diagonal,
        UpDown,
        LeftRight
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
                Vector3 upPosition = new Vector3(0, topOfScreen, 0);  // Adjust top position for UpDown movement
                transform.position = Vector3.Lerp(new Vector3(0, bottomOfScreen, 0), upPosition, curve.Evaluate(timeFactor));
                break;

            case MovementState.LeftRight:
                // Left-right movement from left to right
                Vector3 rightPosition = new Vector3(rightSideOfScreen, 0, 0);  // Adjust right position for LeftRight movement
                transform.position = Vector3.Lerp(new Vector3(leftSideOfScreen, 0, 0), rightPosition, curve.Evaluate(timeFactor));
                break;
        }
    }
}
