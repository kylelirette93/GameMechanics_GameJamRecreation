using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    PlayerController playerController;
    TextMeshProUGUI livesText;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        livesText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateLives();
    }

    public void UpdateLives()
    {
        // Update text based on player lives.
        if (playerController != null)
        {
            livesText.text = "Lives: " + playerController.Lives;
        }
    }
}
