using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiagonalMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    float topOfScreen = 4.5f;
    float bottomOfScreen = -4.5f;
    float leftSideOfScreen = -8.5f;
    float rightSideOfScreen = 8.5f;
    public bool moveTowardsCenter = false;
    float switchDistance;
    Vector3 movementDirection;

    Vector3 centerPosition;  // Store the center position of the screen
    Vector3 originalPosition;

    public AnimationCurve curve;
    void Start()
    {
        // Initialize the center position of the screen.
        centerPosition = new Vector3(0, 0, 0);
        originalPosition = transform.position;
    }

    void Update()
    {
        if (moveTowardsCenter)
        {
            transform.position = Vector3.Lerp(originalPosition, centerPosition, curve.Evaluate(Mathf.Abs(Mathf.Sin(Time.time))));
        }
        else
        {
            transform.position = Vector3.Lerp(centerPosition, originalPosition, curve.Evaluate(Mathf.Abs(Mathf.Sin(Time.time))));
        }
    }
}
