using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShelfLooter : MonoBehaviour
{
    [SerializeField] private GameObject emptyShelfPrefab;
    [SerializeField] private float lootDist = 1f;

    private int lootedShelves = 0;
    private List<GameObject> lootableShelves;

    private void Awake()
    {
        lootableShelves = GameObject.FindGameObjectsWithTag("Shelf").ToList();
    }

    private void Update()
    {
        var shelf = closestShelf(out float distance);

        if(distance < lootDist)
        {
            lootedShelves++;
            Instantiate(emptyShelfPrefab,
                        shelf.transform.position,
                        shelf.transform.rotation,
                        shelf.transform.parent);

            lootableShelves.Remove(shelf);
            Destroy(shelf);
        }

        float a = 5;
        while (a > 0)
        {

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
}