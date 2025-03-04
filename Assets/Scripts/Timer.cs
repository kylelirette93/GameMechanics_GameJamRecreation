using UnityEngine;

public class Timer : MonoBehaviour 
{
    public static Timer instance;

    public int elapsedTime = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartTimer()
    {
       InvokeRepeating("CountTime", 1, 1);
    }

    public void StopTimer()
    {
        // Stop the timer.
        CancelInvoke("CountTime");
    }
    public void CountTime()
    {
        elapsedTime += 1;
    }

    public void ResetTime()
    {
        elapsedTime = 0;
    }
}

