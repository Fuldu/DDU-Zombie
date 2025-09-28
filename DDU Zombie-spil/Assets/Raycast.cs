using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionArea : MonoBehaviour
{
    [Header("Cone Settings")]
    public float viewRadius = 5f;
    public float viewAngle = 60f;
    public Transform lightDirection;

    [Header("Circle Settings")]
    public float circleRadius = 3f; // radius rundt om spilleren

    [Header("Layers")]
    public LayerMask enemyMask;
    public LayerMask wallMask;

    void Update()
    {
        // Find alle enemies indenfor den større af de to radius
        float maxRadius = Mathf.Max(viewRadius, circleRadius);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, maxRadius, enemyMask);

        foreach (Collider2D enemy in enemies)
        {
            Vector2 dirToEnemy = (enemy.transform.position - transform.position).normalized;
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            bool inCone = Vector2.Angle(lightDirection.right, dirToEnemy) < viewAngle / 2f && distanceToEnemy <= viewRadius;
            bool inCircle = distanceToEnemy <= circleRadius;

            if (inCone || inCircle)
            {
                // Raycast kun mod vægge
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToEnemy, distanceToEnemy, wallMask);

                if (hit.collider == null)
                {
                    // Zombie synlig
                    enemy.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    // Zombie bag en væg
                    enemy.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else
            {
                // Zombie udenfor både kegle og cirkel
                enemy.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    // Debug: tegner kegle og cirkel i Scene View
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, circleRadius); // cirklen

        Gizmos.color = Color.green;
        Vector3 left = Quaternion.Euler(0, 0, viewAngle / 2) * lightDirection.right * viewRadius;
        Vector3 right = Quaternion.Euler(0, 0, -viewAngle / 2) * lightDirection.right * viewRadius;
        Gizmos.DrawLine(transform.position, transform.position + left);
        Gizmos.DrawLine(transform.position, transform.position + right);
        Gizmos.DrawWireSphere(transform.position, viewRadius); // kegle radius
    }
}
