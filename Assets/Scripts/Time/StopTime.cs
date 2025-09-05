using UnityEngine;
using System.Collections;

public class StopTime : MonoBehaviour
{
    TimeManager timeManager;

    void Start()
    {
        timeManager = TimeManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeManager.isPaused = true;
        }
    }
}










