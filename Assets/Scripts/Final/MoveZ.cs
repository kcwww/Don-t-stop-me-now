using System.Collections;
using UnityEngine;

public class MoveZ : MonoBehaviour
{
    [SerializeField] float moveAmount = 3.0f;
    [SerializeField] float direction = 1.0f; // 1 = forward, -1 = backward
    [SerializeField] float speed = 3.0f;     // 높을수록 빠름

    private void Start()
    {
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, 0, moveAmount * direction);

        // 이동 속도에 따라 소요 시간 계산
        float moveDuration = Mathf.Abs(moveAmount / speed);

        while (true)
        {
            // go to target position
            float elapsedTime = 0;
            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;

            // back to start position
            elapsedTime = 0;
            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = startPosition;
        }
    }
}
