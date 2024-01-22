using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunData : ScriptableObject
{
    public enum ShotType
    {
        Head, Body, LegsAndHands
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
        public int damage;
    }

    public ShotData[] shotData;
    public Dictionary<(ShotType, RangeType), int> damageMap
    {
        get
        {
            Dictionary<(ShotType, RangeType), int> output = new();
            foreach (var shot in shotData)
            {
                output[(shot.shotType, shot.rangeType)] = shot.damage;
            }
            return output;
        }
    }

    [Space]

    public int bulletsInMag;
    public float reloadTimeSeconds;

    [Space]

    public float lowToMidRangeDistance;
    public float midToHighRangeDistance;

    [Space, Space]

    [Tooltip("Number of shots per second")] public float fireRate;
    public bool canHold;
    public float maxRange;
    public float muzzleFlashSize;

    [Space]

    [Tooltip("All the layers the gun can shoot at. Layers not in the list will be ignored")] 
    public LayerMask shootableLayers;
    public AudioClip shotSound;

    public bool canGiveToCamera;
}