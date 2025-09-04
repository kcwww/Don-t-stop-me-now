using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Stage1"); // Stage1 씬으로 이동
    }
}
