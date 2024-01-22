using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GrenadeThrower : Unity.Netcode.NetworkBehaviour
{
    [SerializeField] private PauseMenu pause;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private float fireRate;
    [SerializeField] private float throwForce = 40f;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform cam;

    [Space, SerializeField] private int bulletsInMag = 5;
    [SerializeField] private float reloadTimeSeconds = 2f;

    private float nextTimeToFire;
    private int currentAmmo;

    private void Awake()
    {
        currentAmmo = bulletsInMag;
    }

    private void Update()
    {
        if (!IsOwner || pause.IsPaused) return;

        ammoText.text = $"{currentAmmo}/{bulletsInMag}";

        if (Input.GetMouseButtonDown(0)) Throw();
        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }

    private void Throw()
    {
        if (Time.time < nextTimeToFire) return;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        currentAmmo--;

        nextTimeToFire = Time.time + (1 / fireRate);

        GameObject grenade = Instantiate(grenadePrefab, cam.position, cam.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(cam.forward * throwForce, ForceMode.VelocityChange);

        if (currentAmmo <= 0) Reload();
    }

    private async void Reload()
    {
        Debug.Log($"Reloading {gameObject.name} for {reloadTimeSeconds} seconds");
        await Task.Delay((int)(reloadTimeSeconds * 1000));
        currentAmmo = bulletsInMag;
    }
}