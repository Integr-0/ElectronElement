using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static GunData;

[RequireComponent(typeof(AudioSource))]
public class Gun : NetworkBehaviour
{
    [SerializeField] private PauseMenu pause;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private GunData data;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform shotPoint;

    private float currentAmmo;
    private float nextTimeToFire;

    [System.Obsolete]
    private void Awake()
    {
        muzzleFlash.startSize = data.muzzleFlashSize;
        currentAmmo = data.bulletsInMag;
    }

    private void Update()
    {
        if (!IsOwner || pause.IsPaused) return;

        ammoText.text = $"{currentAmmo}/{data.bulletsInMag}";

        if ((!data.canHold && Input.GetMouseButtonDown(0)) || (data.canHold && Input.GetMouseButton(0)))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }

    private void Shoot()
    {
        if (Time.time < nextTimeToFire) return;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        nextTimeToFire = Time.time + (1 / data.fireRate);
        currentAmmo--;

        muzzleFlash.Play();
        GetComponent<AudioSource>().PlayOneShot(data.shotSound);

        if (Physics.Raycast(shotPoint.position,
                            shotPoint.forward, 
                            out RaycastHit hit,
                            data.maxRange, 
                            data.shootableLayers))
        {
            if (hit.transform.TryGetComponent(out ShootableCollider s))
            {
                s.health.ChangeHealth(CalculateDamage(hit, s.bodyPart));
            }
        }

        if (currentAmmo <= 0) Reload();
    }

    private async void Reload()
    {
        Debug.Log($"Reloading {gameObject.name} for {data.reloadTimeSeconds} seconds");
        await Task.Delay((int)(data.reloadTimeSeconds * 1000));
        currentAmmo = data.bulletsInMag;
    }

    private int CalculateDamage(RaycastHit hit, ShotType shotType)
    {
        RangeType range;

        if (hit.distance <= data.lowToMidRangeDistance) range = RangeType.Low;
        else if (hit.distance < data.midToHighRangeDistance) range = RangeType.Mid;
        else range = RangeType.High;

        return -data.damageMap[(shotType, range)];
    }
}
