using UnityEngine;
public class TeleportButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform targetPosition; // �̵���ų ��ġ (�� ������Ʈ�� ����� �Ҵ�)
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Player �±׸� ���� ������Ʈ��
        {
            other.transform.position = targetPosition.position; // Ư�� ��ġ�� �̵�
        }
    }
}