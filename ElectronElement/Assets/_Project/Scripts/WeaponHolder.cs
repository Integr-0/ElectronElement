using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    private Transform[] weapons;

    private void Awake()
    {
        weapons = transform.GetComponentsInChildren<Transform>(true);
    }

    private void Update()
    {
        for (int i = 0; i < weapons.Length; i++)
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
            weapon.gameObject.SetActive(false);
        }
        weapons[i].gameObject.SetActive(true);
    }
}