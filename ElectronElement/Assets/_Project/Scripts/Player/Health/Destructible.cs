using UnityEngine;

[RequireComponent(typeof(Health))]
public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyedVersion;
    [SerializeField] private ParticleSystem destructionParticles;
    private void Awake()
    {
        GetComponent<Health>().onDied += () =>
        {
            destructionParticles.Play();

            Instantiate(destroyedVersion, transform.position, transform.rotation, transform.parent);
            Destroy(gameObject);
        };
    }
}