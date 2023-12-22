using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponHolder : NetworkBehaviour
{
    private readonly List<GameObject> weapons = new();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        foreach (Transform child in transform)
        {
            weapons.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
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