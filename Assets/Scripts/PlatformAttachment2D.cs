using UnityEngine;

public class PlatformAttachment2D : MonoBehaviour
{
    [SerializeField] private string platformTag = "Platform";
    [SerializeField] private bool requireTopContact = true;
    [SerializeField, Min(0f)] private float minimumUpwardNormal = 0.5f;

    private Transform currentPlatform;
    private Transform originalParent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryAttachToPlatform(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (currentPlatform == null) TryAttachToPlatform(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == currentPlatform) DetachFromPlatform();
    }

    private void OnDisable()
    {
        DetachFromPlatform();
    }

    private void TryAttachToPlatform(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(platformTag))       return;
        if (requireTopContact && !IsContactFromAbove(collision)) return;
        if (currentPlatform == null) originalParent = transform.parent;

        Transform platform = collision.transform;

        if (currentPlatform != platform)
        {
            transform.SetParent(platform, true);
            currentPlatform = platform;
        }
    }

    private bool IsContactFromAbove(Collision2D collision)
    {
        ContactPoint2D[] contacts = collision.contacts;
        foreach (ContactPoint2D contact in contacts)
        {
            if (Vector2.Dot(contact.normal, Vector2.up) >= minimumUpwardNormal)
            {
                return true;
            }
        }

        return false;
    }

    private void DetachFromPlatform()
    {
        if (currentPlatform == null) return;

        transform.SetParent(originalParent, true);
        currentPlatform = null;
        originalParent = null;
    }
}
