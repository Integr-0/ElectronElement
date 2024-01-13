using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponHolder : NetworkBehaviour
{
    private readonly List<GameObject> weapons = new();
    private GameObject currentActiveWeapon;

    public override void OnNetworkSpawn()
    {
        foreach (Transform child in transform)
        {
            weapons.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        currentActiveWeapon = transform.GetChild(0).gameObject;
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
        currentActiveWeapon.SetActive(false);
        weapons[i].SetActive(true);
        currentActiveWeapon = weapons[i];
    }
}