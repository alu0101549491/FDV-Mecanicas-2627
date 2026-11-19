using System.Collections.Generic;
using UnityEngine;
using GameplayPhysics;

public class InvisiblePlatformReveal2D : MonoBehaviour
{
    private readonly HashSet<GameObject> revealedPlatforms = new HashSet<GameObject>();

    private void Start()
    {
        GameObject[] invisiblePlatforms = GameObject.FindGameObjectsWithTag("Invisible");
        foreach (GameObject platform in invisiblePlatforms)
        {
            SetPlatformVisibility(platform, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryRevealPlatform(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryRevealPlatform(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject platform = FindInvisiblePlatform(collision.gameObject);
        if (platform == null || !revealedPlatforms.Remove(platform)) return;

        SetPlatformVisibility(platform, false);
    }

    private void TryRevealPlatform(Collision2D collision)
    {
        if (!Contact2D.IsContactFromAbove(collision)) return;

        GameObject platform = FindInvisiblePlatform(collision.gameObject);
        if (platform == null || !revealedPlatforms.Add(platform)) return;

        SetPlatformVisibility(platform, true);
    }

    private GameObject FindInvisiblePlatform(GameObject collisionObject)
    {
        Transform current = collisionObject.transform;
        while (current != null)
        {
            if (current.CompareTag("Invisible")) return current.gameObject;
            current = current.parent;
        }

        return null;
    }

    private void SetPlatformVisibility(GameObject platform, bool isVisible)
    {
        Renderer[] renderers = platform.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer renderer in renderers) renderer.enabled = isVisible;
    }
}