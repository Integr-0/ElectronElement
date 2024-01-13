using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float explosionForce;
    [SerializeField] private float damage;
    [SerializeField] private float secondsBeforeExploding = 3f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private LayerMask colliderLayers;

    private bool exploded = false;
    private float explosionCountdown;

    private void Awake()
    {
        explosionCountdown = secondsBeforeExploding;
    }

    private void Update()
    {
        explosionCountdown -= Time.deltaTime;
        if (explosionCountdown <= 0f && !exploded)
        {
            Explode();
            exploded = true;
        }
    }

    public void Explode()
    {
       Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, radius, colliderLayers, QueryTriggerInteraction.Ignore);

        foreach (var collider in nearbyColliders)
        {
            if (collider.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }
            if (collider.TryGetComponent(out Health h))
            {
                h.ChangeHealth(-damage);
            }
        }

        Destroy(gameObject);
    }
}