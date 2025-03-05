using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Static singleton to allow global access to this class and referenced managers.
    public static GameManager instance;
    public AudioManager audioManager;
    public GameStateManager gameStateManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    
    }
}
