using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ToggleTime()
    {
        isPaused = !isPaused;
    }
}
