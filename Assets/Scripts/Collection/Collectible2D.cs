using UnityEngine;

public class Collectible2D : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField, Min(1)] private int scoreValue = 1;

    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.CompareTag(playerTag)) return;

        PlayerCollection2D collection = other.GetComponent<PlayerCollection2D>();
        if (collection == null)
        {
            Debug.LogError("El jugador necesita un componente PlayerCollection2D.", other);
            return;
        }

        collected = true;
        collection.AddScore(scoreValue);
        Destroy(gameObject);
    }
}
