using System.Collections;
using UnityEngine;

public class ProjectileMovement2D : MonoBehaviour
{
    [SerializeField, Min(0f)] private float speed = 5f;
    [SerializeField, Min(0.1f)] private float activeTime = 2f;

    private bool isMoving;
    private Vector2 movementDirection;
    private Coroutine disableCoroutine;

    private void OnEnable()
    {
        isMoving = false;
        movementDirection = Vector2.zero;
        disableCoroutine = null;
    }

    private void Update()
    {
        if (!isMoving) return;

        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
    }

    public void StartMoving()
    {
        movementDirection = transform.right.normalized;
        isMoving = true;

        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }

        disableCoroutine = StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(activeTime);
        isMoving = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}