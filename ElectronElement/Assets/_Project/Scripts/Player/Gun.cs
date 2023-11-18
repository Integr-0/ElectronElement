using UnityEngine;
using static WeaponData;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [SerializeField] private WeaponData data;
    [SerializeField] private ParticleSystem muzzleFlash;
    private float nextTimeToFire;

    [System.Obsolete]
    private void Awake()
    {
        muzzleFlash.startSize = data.muzzleFlashSize;
    }

    private void Update()
    {
        if ((!data.canHold && Input.GetMouseButtonDown(0)) || (data.canHold && Input.GetMouseButton(0)))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time < nextTimeToFire) return;

        nextTimeToFire = Time.time + (1 / data.fireRate);

        if (Physics.Raycast(transform.position, 
                            transform.forward, 
                            out RaycastHit hit, 
                            data.maxRange, 
                            data.shootableLayers))
        {
            muzzleFlash.Play();
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
}
