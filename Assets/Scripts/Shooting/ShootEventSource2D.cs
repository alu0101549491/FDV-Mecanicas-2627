using UnityEngine;
using UnityEngine.Events;

public class ShootEventSource2D : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    public event UnityAction ShootRequested;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RequestShot(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RequestShot(other.gameObject);
    }

    private void RequestShot(GameObject other)
    {
        if (other.CompareTag(playerTag))
        {
            ShootRequested?.Invoke();
        }
    }
}
