using UnityEngine;

[RequireComponent(typeof(Health))]
public class Destructible : MonoBehaviour
{
    [SerializeField, Tooltip("Can be null if no destroyed version exists")] private GameObject destroyedVersion;
    [SerializeField] private ParticleSystem destructionParticles;
    private void Awake()
    {
        GetComponent<Health>().onDied += () =>
        {
            destructionParticles.Play();

            if (destroyedVersion != null) Instantiate(destroyedVersion, transform.position, transform.rotation, transform.parent);
            Destroy(gameObject);
        };
    }
}