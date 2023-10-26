using static WeaponData;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private WeaponData data;
    [Space, SerializeField] private Transform gunTip;
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

        if (Physics.Raycast(gunTip.position, 
                            transform.forward, 
                            out RaycastHit hit, 
                            data.maxRange, 
                            data.shootableLayers))
        {
            muzzleFlash.Play();
            GetComponent<AudioSource>().PlayOneShot(data.shotSound);
            if (hit.transform.TryGetComponent(out Health health))
            {
                health.TakeDamage(CalculateDamage(hit));
            }
        }
    }

    private float CalculateDamage(RaycastHit hit)
    {
        ShotType shot = hit.transform.tag switch
        {
            "Head" => ShotType.Head,
            "Body" => ShotType.Body,
            "Legs" => ShotType.Legs,
            _ => throw new UnityException("Wrong tag attached to collider. [Head, Body, Legs]")
        };
        RangeType range;
        if (hit.distance <= data.lowToMidRangeDistance) range = RangeType.Low;
        else if (hit.distance > data.lowToMidRangeDistance && hit.distance < data.midToHighRangeDistance) range = RangeType.Mid;
        else range = RangeType.High;

        return data.damageMap[(shot, range)];
    }
}
