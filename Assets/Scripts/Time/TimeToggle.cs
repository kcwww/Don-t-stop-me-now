
using UnityEngine;
using System.Collections;

public class TimeToggle : MonoBehaviour
{
    [SerializeField] private float onTime = 1f;      // 켜져 있는 시간
    [SerializeField] private float offTime = 2f;     // 꺼져 있는 시간
    [SerializeField] private float startDelay = 0f;  // 시작 지연 (박스마다 다르게 설정)
    [SerializeField] private float hiddenAlpha = 0.1f; // 투명해질 때 알파 값 (0=완전투명, 1=불투명)

    TimeManager timeManager;


    private Renderer[] renderers;
    private Collider col;
    private Material[] materials;

    void Awake()
    {
        
        renderers = GetComponentsInChildren<Renderer>();
        col = GetComponent<Collider>();

        // 머티리얼 배열 저장
        materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
    }

    void Start()
    {
        timeManager = TimeManager.Instance;
        StartCoroutine(ToggleRoutine());
    }


    IEnumerator ToggleRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // 켜기
            SetVisible(true);
            float timer = 0f;
            while (timer < onTime)
            {
                if (!timeManager.isPaused) timer += Time.deltaTime; // !timeManager.isPaused 추가
                yield return null;
            }

            // 끄기
            SetVisible(false);
            timer = 0f;
            while (timer < offTime)
            {
                if (!timeManager.isPaused) timer += Time.deltaTime;
                yield return null;
            }
        }
    }



    void SetVisible(bool visible)
    {
        if (visible)
        {
            foreach (var mat in materials)
            {
                Color c = mat.color;
                c.a = 1f;         // 완전 보이게
                mat.color = c;
            }
            if (col != null) col.enabled = true;
        }
        else
        {
            foreach (var mat in materials)
            {
                Color c = mat.color;
                c.a = hiddenAlpha; // 반투명
                mat.color = c;
            }
            if (col != null) col.enabled = false;
        }
    }
}
