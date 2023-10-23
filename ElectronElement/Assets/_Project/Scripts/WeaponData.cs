using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public float damage_LowRange_Head;
    public float damage_LowRange_Body;
    public float damage_LowRange_Legs;

    [Space]

    public float damage_MidRange_Head;
    public float damage_MidRange_Body;
    public float damage_MidRange_Legs;

    [Space]

    public float damage_HighRange_Head;
    public float damage_HighRange_Body;
    public float damage_HighRange_Legs;

    [Space, Space]

    public float lowToMidRangeDistance;
    public float midToHighRangeDistance;

    [Space, Space]

    public float fireRate;
    public float maxRange;
    public float[] zoomLevels;

    [Space]

    public LayerMask shootableLayers;

    public bool canGiveToCamera;
}