using System.Collections;
using UnityEngine;
using GameplayPhysics;

public class BreakablePlatform2D : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField, Min(0f)] private float breakDelay = 0.5f;

    private Rigidbody2D platformRigidbody;
    private bool isBreaking;

    private void Awake()
    {
        platformRigidbody = GetComponent<Rigidbody2D>();
        if (platformRigidbody == null)
        {
            Debug.LogError("BreakablePlatform2D necesita un Rigidbody2D.", this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBreaking || platformRigidbody == null) return;
        if (!collision.gameObject.CompareTag(playerTag)) return;
        if (!Contact2D.IsContactFromAbove(collision, Vector2.down)) return;

        isBreaking = true;
        StartCoroutine(BreakPlatform());
    }

    private IEnumerator BreakPlatform()
    {
        yield return new WaitForSeconds(breakDelay);
        platformRigidbody.bodyType = RigidbodyType2D.Dynamic;
    }
}
