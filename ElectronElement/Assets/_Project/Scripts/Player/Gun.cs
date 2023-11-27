using Unity.Netcode;
using UnityEngine;
using static WeaponData;

[RequireComponent(typeof(AudioSource))]
public class Gun : NetworkBehaviour
{
    [SerializeField] private WeaponData data;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform shotPoint;
    private float nextTimeToFire;

    [System.Obsolete]
    private void Awake()
    {
        muzzleFlash.startSize = data.muzzleFlashSize;
    }

    private void Update()
    {
        if (!IsOwner) return;

        if ((!data.canHold && Input.GetMouseButtonDown(0)) || (data.canHold && Input.GetMouseButton(0)))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time < nextTimeToFire) return;

        nextTimeToFire = Time.time + (1 / data.fireRate);

        muzzleFlash.Play();

        if (Physics.Raycast(shotPoint.position,
                            shotPoint.forward, 
                            out RaycastHit hit,
                            data.maxRange, 
                            data.shootableLayers))
        {
            GetComponent<AudioSource>().PlayOneShot(data.shotSound);
            if (hit.transform.TryGetComponent(out ShootableCollider s))
            {
                s.health.ChangeHealth(CalculateDamage(hit, s.bodyPart));
            }
        }
    }

    private float CalculateDamage(RaycastHit hit, ShotType shotType)
    {
        RangeType range;

        if (hit.distance <= data.lowToMidRangeDistance) range = RangeType.Low;
        else if (hit.distance < data.midToHighRangeDistance) range = RangeType.Mid;
        else range = RangeType.High;

        return -data.damageMap[(shotType, range)];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(shotPoint.position, shotPoint.forward * data.maxRange);
    }
}
