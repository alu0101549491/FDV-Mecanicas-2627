using UnityEngine;

public class EnemyCollision2D : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask ignoredLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldHandleCollision(collision))
        {
            Debug.Log("Colisión con un enemigo: " + collision.gameObject.name);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (ShouldHandleCollision(collision))
        {
            Debug.Log("Fin de la colisión con un enemigo: " + collision.gameObject.name);
        }
    }

    private bool ShouldHandleCollision(Collision2D collision)
    {
        if (ignoredLayer >= 0 && collision.gameObject.layer == ignoredLayer)
        {
            return false;
        }

        int collisionLayer = 1 << collision.gameObject.layer;
        return (enemyLayer.value & collisionLayer) != 0;
    }
}