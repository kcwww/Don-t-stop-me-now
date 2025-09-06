using System.Collections;
using UnityEngine;

enum Direction
{
    Up,
    Down,
    negativeX,
    positiveX,
    negativeZ,
    positiveZ
}

public class UpTrigger : MonoBehaviour
{
    [SerializeField] float upAmount = 16.0f;
    [SerializeField] float moveDuration = 4.0f;
    [SerializeField] Transform Platform;
    [SerializeField] Direction direction = Direction.Up;

    bool isMoving = false;


    private void OnTriggerEnter(Collider other)
    {
        if (isMoving) return;
        if (other.CompareTag("Player"))
        {

            // 러프하게 이동
            if (direction == Direction.Up)
                StartCoroutine(MoveUp(other.transform));
            else if (direction == Direction.negativeX)
                StartCoroutine(MoveX(other.transform));
            else if (direction == Direction.negativeZ)
                StartCoroutine(MoveZ(other.transform));
            isMoving = true;
        }
    }

    IEnumerator MoveUp(Transform player)
    {

        Vector3 startPosition = Platform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, upAmount, 0);
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            Platform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Platform.position = targetPosition;

        gameObject.SetActive(false);
    }



    IEnumerator MoveX(Transform player)
    {

        Vector3 startPosition = Platform.position;
        Vector3 targetPosition = startPosition + new Vector3(upAmount, 0, 0);
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            Platform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Platform.position = targetPosition;

        gameObject.SetActive(false);
    }

    IEnumerator MoveZ(Transform player)
    {

        Vector3 startPosition = Platform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, 0, upAmount);
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            Platform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Platform.position = targetPosition;

        gameObject.SetActive(false);
    }
}
