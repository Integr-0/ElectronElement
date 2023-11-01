using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    private readonly List<GameObject> weapons = new();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            weapons.Add(child.gameObject);
        }
    }

    private void Update()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (Input.GetKeyDown((2).ToString()))
            {
                ActivateWeapon(1);
            }
        }
    }

    private void ActivateWeapon(int i)
    {
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapons[i].SetActive(true);
    }
}