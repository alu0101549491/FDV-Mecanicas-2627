using UnityEngine;

public class ProjectileMovement2D : MonoBehaviour
{
    [SerializeField, Min(0f)] private float speed = 5f;

    private bool isMoving;
    private Vector2 movementDirection;

    private void OnEnable()
    {
        isMoving = false;
        movementDirection = Vector2.zero;
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
    }
}