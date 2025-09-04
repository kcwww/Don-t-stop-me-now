using System.Collections;
using UnityEngine;

public enum ItemType
{
    Default,
    DoubleJump,
    Dash,
    Flash,
    JumpDash,
    JumpFlash,
    DashFlash,
    Triple,
}

public class Item : MonoBehaviour
{
    public ItemType itemType;

    [SerializeField] float itemRespawnTime = 3.0f;
    [SerializeField] GameObject Model;
    [SerializeField] float rotateSpeed = 50f;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerAbility>().GiveAbility(itemType);
            other.GetComponent<PlayerEffect>().TriggerParticle(EffectType.Implosion);
            StartCoroutine(RespawnItem());
        }
    }

    IEnumerator RespawnItem()
    {
        Model.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(itemRespawnTime);
        Model.SetActive(true);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
