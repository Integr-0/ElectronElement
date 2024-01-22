using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShootableCollider : MonoBehaviour
{
    public GunData.ShotType bodyPart;
    public Health health;
}