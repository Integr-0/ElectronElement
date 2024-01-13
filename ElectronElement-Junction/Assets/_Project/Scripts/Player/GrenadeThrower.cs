using UnityEngine;

public class GrenadeThrower : Unity.Netcode.NetworkBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private float throwForce = 40f;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform cam;

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

        GameObject grenade = Instantiate(grenadePrefab, cam.position, cam.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(cam.forward * throwForce, ForceMode.VelocityChange);
    }
}