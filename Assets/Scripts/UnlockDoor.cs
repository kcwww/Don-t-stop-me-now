using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 50f;
    [SerializeField] GameObject DoorAndKey;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerEffect>().TriggerParticle(EffectType.Implosion);
            Destroy(DoorAndKey);
        }
    }
}
