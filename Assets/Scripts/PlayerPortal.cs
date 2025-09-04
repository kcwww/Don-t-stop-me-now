using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerPortal : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "StartPortal"; // 인스펙터에서 레이어 이름 지정 가능

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힌 오브젝트가 StartPortal 레이어라면
        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            StartCoroutine(LoadStage1AfterDelay(1f));
        }
    }

    private IEnumerator LoadStage1AfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 1초 대기
        SceneManager.LoadScene("Stage1");       // Stage1 씬으로 이동
    }
}
