using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Grenade : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float explosionForce;

    [SerializeField, Tooltip("This is the damage if you're standing directly next to the grenade, will be changed based on distance")] 
    private int damage;

    [SerializeField] private float bounceTimeSeconds = 3f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private int effectDurationSeconds;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private LayerMask colliderLayers;

    private bool exploded = false;
    private float explosionCountdown;

    private void Awake()
    {
        explosionCountdown = bounceTimeSeconds;
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

    public async void Explode()
    {
        GetComponent<AudioSource>().PlayOneShot(explosionSound);

        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, radius, colliderLayers, QueryTriggerInteraction.Ignore);

        foreach (var collider in nearbyColliders)
        {
            if (collider.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }
            if (collider.TryGetComponent(out Health h))
            {
                float distanceFromPoint = Vector3.Distance(collider.transform.position, transform.position);
                int distanceBasedDamage = (int)(damage * Mathf.Lerp(1, 0, distanceFromPoint / radius));
                h.ChangeHealth(-distanceBasedDamage);
            }
        }

        Destroy(gameObject);

        await Task.Delay(effectDurationSeconds * 1000);

        if (Application.isPlaying) Destroy(effect);
    }
}