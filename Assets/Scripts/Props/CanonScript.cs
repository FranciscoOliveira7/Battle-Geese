using System.Collections;
using UnityEngine;

public class CanonScript : MonoBehaviour
{
    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right
    }

    public GameObject projectile;
    public float cooldown;
    public float projectileSpeed;
    public LayerMask layerMask;
    public Direction shootDirection;

    private void OnEnable()
    {
        StartCoroutine(ShootProjectile());
    }

    private void OnDisable()
    {
        StopCoroutine(ShootProjectile());
    }

    IEnumerator ShootProjectile()
    {
        while (true)
        {
            // Instantiate the projectile
            GameObject bala = Instantiate(projectile, transform.position, Quaternion.identity);

            // Ensure the projectile has the IProjectile component
            IProjectile projectileComponent = bala.GetComponent<IProjectile>();

            // Determine the shooting direction based on the selected direction
            Vector3 direction = Vector3.zero;
            switch (shootDirection)
            {
                case Direction.Forward:
                    direction = -transform.forward;
                    break;
                case Direction.Backward:
                    direction = transform.forward;
                    break;
                case Direction.Left:
                    direction = transform.right;
                    break;
                case Direction.Right:
                    direction = -transform.right;
                    break;
            }

            // Spawn the projectile with the specified parameters
            projectileComponent.Spawn(
                transform.position,
                transform.position + direction,
                projectileSpeed,
                30,
                layerMask
            );

            // Wait for the cooldown duration before shooting again
            yield return new WaitForSeconds(cooldown);
        }
    }
}
