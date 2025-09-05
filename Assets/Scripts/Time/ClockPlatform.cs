using System.Collections;
using UnityEngine;

public enum ClockType { Odd, Even }

public class ClockPlatform : MonoBehaviour
{
    [SerializeField] private ClockType clockType;
    public bool isClockActive = true;

    private Renderer rend;
    private Collider col;
    private Coroutine clockCoroutine;
    private bool isVisible = true;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    void Start()
    {
        float delay = (clockType == ClockType.Even) ? 1f : 0f;
        clockCoroutine = StartCoroutine(ClockRoutine(delay));
    }

    IEnumerator ClockRoutine(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            if (isClockActive)
            {
                isVisible = !isVisible;
                rend.enabled = isVisible;
                col.enabled = isVisible;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void OnDisable()
    {
        if (clockCoroutine != null)
        {
            StopCoroutine(clockCoroutine);
            clockCoroutine = null;
        }
    }

    void OnEnable()
    {
        if (clockCoroutine == null)
        {
            float delay = (clockType == ClockType.Even) ? 1f : 0f;
            clockCoroutine = StartCoroutine(ClockRoutine(delay));
        }
    }
}
