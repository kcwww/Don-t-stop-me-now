using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    bool isStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isStarted)
        {
            isStarted = true;
            StartCoroutine(LoadStageOne());
        }
    }
    
    IEnumerator LoadStageOne()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Stage1");
    }
}
