using UnityEngine;
public class TeleportButton : MonoBehaviour
{
    
    public Transform targetPosition; 
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = targetPosition.position; 
        }
    }
}