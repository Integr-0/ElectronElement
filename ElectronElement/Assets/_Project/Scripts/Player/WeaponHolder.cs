using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponHolder : NetworkBehaviour
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
        if (!IsOwner) return;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                ActivateWeapon(i);
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