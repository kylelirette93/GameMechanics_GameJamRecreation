using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovePattern : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    float leftSideOfScreen = -8.5f;
    float rightSideOfScreen = 8.5f;

    void Update()
    {
        // Move the buzzsaw vertically.
        transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);

        // Check if the buzzsaw has reached the top or bottom of the screen and reverse direction.
        if (transform.position.x <= leftSideOfScreen || transform.position.x >= rightSideOfScreen)
        {
            moveSpeed = -moveSpeed;
        }

        // Clamp the position to ensure it stays within bounds.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftSideOfScreen, rightSideOfScreen), transform.position.y, transform.position.z);
    }
}