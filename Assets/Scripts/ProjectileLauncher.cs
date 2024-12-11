using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;

    public void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);

        Vector3 origScale = projectile.transform.localScale;

        // flip the position of the projectilebased on the player fliposition
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 0.6f: -0.6f,
            origScale.y,
            origScale.z
            );
    }
}
