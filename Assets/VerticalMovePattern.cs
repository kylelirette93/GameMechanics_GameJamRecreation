using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovePattern : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    float topOfScreen = 4.5f;
    float bottomOfScreen = -4.5f;

    void Update()
    {
        // Move the buzzsaw vertically.
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Check if the buzzsaw has reached the top or bottom of the screen and reverse direction.
        if (transform.position.y >= topOfScreen || transform.position.y <= bottomOfScreen)
        {
            moveSpeed = -moveSpeed;
        }

        // Clamp the position to ensure it stays within bounds.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bottomOfScreen, topOfScreen), transform.position.z);
    }
}