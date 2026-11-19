using UnityEngine;

public class ProjectileActivation2D : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private string shootTrigger = "Dispara";

    private ProjectileMovement2D projectileMovement;

    private void Start()
    {
        if (projectile == null)
        {
            Debug.LogError("ProjectileActivation2D necesita un proyectil asignado.", this);
            return;
        }

        projectileMovement = projectile.GetComponent<ProjectileMovement2D>();
        if (projectileMovement == null)
        {
            Debug.LogError("El proyectil necesita un componente ProjectileMovement2D.", projectile);
        }

        projectile.SetActive(false);
    }

    public void ActivateProjectile()
    {
        if (projectile == null) return;

        projectile.SetActive(true);
        if (projectileMovement != null)
        {
            projectileMovement.StartMoving();
        }

        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger(shootTrigger);
        }
    }
}
