using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalClearTrigger : MonoBehaviour
{
    [SerializeField] private string clearLayerName = "Clear";
    [SerializeField] private string targetSceneName = "Stage3";
    [SerializeField] private string playerTag = "Player";

    private int clearLayer;

    private void Awake()
    {
        clearLayer = LayerMask.NameToLayer(clearLayerName);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 들어오고, 이 트리거 오브젝트의 레이어가 Clear이며, 현재 씬이 Stage3일 때
        if (other.CompareTag(playerTag)
            && gameObject.layer == clearLayer
            && SceneManager.GetActiveScene().name == targetSceneName)
        {
            GameManager.Instance?.FinalClear();
        }
    }
}
