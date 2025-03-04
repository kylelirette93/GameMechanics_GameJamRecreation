using TMPro;
using UnityEngine;

public class DisplayWinText : MonoBehaviour
{
    TextMeshProUGUI winText;

    void Start()
    {
        winText = GetComponent<TextMeshProUGUI>();
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (Timer.instance != null)
        {
            if (playerController != null)
            {
                if (playerController.Lives > 8)
                {
                    winText.text = "Purrfect run! No lives lost!" + "\nYou beat it in " + Timer.instance.elapsedTime + " seconds" + "\n" + "Press 'M' to return to Menu" + "\n" + "Press 'Q' to quit";
                }
                else
                {
                    winText.text = "You Win!" + "\nYou beat it in " + Timer.instance.elapsedTime + " seconds" + "\n" + "Press 'M' to return to Menu" + "\n" + "Press 'Q' to quit";
                }
            }
        }
    }
}
