using UnityEngine;

public class GrenadeThrower : Unity.Netcode.NetworkBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private float throwForce = 40f;
    [SerializeField] private Rigidbody grenadePrefab;

    private float nextTimeToFire;

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            Throw();
        }
    }

    private void Throw()
    {
        if (Time.time < nextTimeToFire) return;

        nextTimeToFire = Time.time + (1 / fireRate);

        Rigidbody grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        grenade.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}