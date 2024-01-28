using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ShelfLooter : NetworkBehaviour
{
    [SerializeField] private Transform emptyShelfPrefab;
    [SerializeField] private float lootDist = 1f;

    public int lootedShelves = 0;
    public int lootedCrystals = 0;
    private List<GameObject> lootableShelves;
    private List<GameObject> crystals;

    private void Awake()
    {
        lootableShelves = GameObject.FindGameObjectsWithTag("Shelf").ToList();
        crystals = GameObject.FindGameObjectsWithTag("Crystal").ToList();
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            var shelf = closestShelf(out float s_distance);
            var crystal = closestCrystal(out float c_distance);

            if (s_distance <= lootDist)
            {
                lootedShelves++;

                Instantiate(emptyShelfPrefab,
                            shelf.transform.position,
                            shelf.transform.rotation,
                            shelf.transform.parent).localScale = 
                            shelf.transform.localScale;

                lootableShelves.Remove(shelf);

                shelf.GetComponent<NetworkObject>().Despawn();
            }


            if(c_distance <= lootDist)
            {
                lootedCrystals++;

                crystals.Remove(crystal);
                crystal.GetComponent<NetworkObject>().Despawn();
            }
        }
    }
    private GameObject closestShelf(out float dist)
    {
        GameObject closest = default;
        dist = float.MaxValue;

        foreach (var shelf in lootableShelves)
        {
            float distance = Vector3.Distance(transform.position, shelf.transform.position);
            if (distance < dist)
            {
                closest = shelf;
                dist = distance;
            }
        }

        return closest;
    }
    private GameObject closestCrystal(out float dist)
    {
        GameObject closest = default;
        dist = float.MaxValue;

        foreach (var crystal in crystals)
        {
            float distance = Vector3.Distance(transform.position, crystal.transform.position);
            if (distance < dist)
            {
                closest = crystal;
                dist = distance;
            }
        }

        return closest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lootDist);
    }
}