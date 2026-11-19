using UnityEngine;

public class ProjectileActivation2D : MonoBehaviour
{
    [SerializeField] private ProjectileMovement2D projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private string shootTrigger = "Dispara";
    [SerializeField] private string fireButton = "Fire1";
    [SerializeField] private ShootEventSource2D shootEventSource;

    private void Awake()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("ProjectileActivation2D necesita un prefab de proyectil.", this);
        }

        if (firePoint == null)
        {
            Debug.LogError("ProjectileActivation2D necesita un punto de disparo.", this);
        }
    }

    private void OnEnable()
    {
        if (shootEventSource != null)
        {
            shootEventSource.ShootRequested += ActivateProjectile;
        }
    }

    private void OnDisable()
    {
        if (shootEventSource != null)
        {
            shootEventSource.ShootRequested -= ActivateProjectile;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown(fireButton))
        {
            ActivateProjectile();
        }
    }

    public void ActivateProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        ProjectileMovement2D projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );
        projectile.StartMoving();

        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger(shootTrigger);
        }
    }
}
