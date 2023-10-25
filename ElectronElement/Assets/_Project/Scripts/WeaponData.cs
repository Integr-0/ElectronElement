using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public enum ShotType
    {
        Head, Body, Legs
    }
    public enum RangeType
    {
        Low, Mid, High
    }
    [System.Serializable]
    public struct ShotData
    {
        public ShotType shotType;
        public RangeType rangeType;
        public float damage;
    }

    public ShotData[] shotData;
    public Dictionary<(ShotType, RangeType), float> damageMap
    {
        get
        {
            foreach (var shot in shotData)
            {
                damageMap[(shot.shotType, shot.rangeType)] = shot.damage;
            }
            return damageMap;
        }
        set
        {
            damageMap = value;
        }
    }

    [Space]

    public float lowToMidRangeDistance;
    public float midToHighRangeDistance;

    [Space, Space]

    [Tooltip("Firing speed: 1 / fireRate")] public float fireRate;
    public float maxRange;
    public float[] zoomLevels;

    [Space]

    [Tooltip("All the layers the gun can shoot at. Layers not in the list will be ignored")] 
    public LayerMask shootableLayers;

    public bool canGiveToCamera;
}