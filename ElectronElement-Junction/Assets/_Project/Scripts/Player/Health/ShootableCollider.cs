using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShootableCollider : MonoBehaviour
{
    public WeaponData.ShotType bodyPart;
    public Health health;
}