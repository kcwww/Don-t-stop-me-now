using System.Collections;
using UnityEngine;

public class ChaseMe : MonoBehaviour
{
    [SerializeField] GameObject[] chasingEnemyies;
    [SerializeField] Transform playerTransform;
    [SerializeField] float chasingSpeed = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ChasePlayer());
        }
    }

    IEnumerator ChasePlayer()
    {
        while (true)
        {
            foreach (var enemy in chasingEnemyies)
            {
                if (enemy != null)
                {
                    Vector3 direction = (playerTransform.position - enemy.transform.position).normalized;
                    enemy.transform.position += direction * chasingSpeed * Time.deltaTime;
                }
            }
            yield return null;
        }
    }


}
