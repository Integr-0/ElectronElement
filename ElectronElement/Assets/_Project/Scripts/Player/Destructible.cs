using UnityEngine;

public class Destructible : Health
{
    [SerializeField] private GameObject destroyedVersion;
    [SerializeField] private ParticleSystem destructionParticles;
    private void Awake()
    {
        onDied += () =>
        {
            destructionParticles.Play();

            Instantiate(destroyedVersion, transform.position, transform.rotation, transform.parent);
            Destroy(gameObject);
        };
    }
}
