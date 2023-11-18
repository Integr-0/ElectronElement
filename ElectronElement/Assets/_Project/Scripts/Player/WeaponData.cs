using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
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
        public float damage;
    }

    public ShotData[] shotData;
    public Dictionary<(ShotType, RangeType), float> damageMap
    {
        get
        {
            Dictionary<(ShotType, RangeType), float> output = new();
            foreach (var shot in shotData)
            {
                output[(shot.shotType, shot.rangeType)] = shot.damage;
            }
            return output;
        }
    }

    [Space]

    public float lowToMidRangeDistance;
    public float midToHighRangeDistance;

    [Space, Space]

    [Tooltip("Firing speed: 1 / fireRate")] public float fireRate;
    public bool canHold;
    public float maxRange;
    public float muzzleFlashSize;
    public float[] zoomLevels;

    [Space]

    [Tooltip("All the layers the gun can shoot at. Layers not in the list will be ignored")] 
    public LayerMask shootableLayers;
    public AudioClip shotSound;

    public bool canGiveToCamera;
}