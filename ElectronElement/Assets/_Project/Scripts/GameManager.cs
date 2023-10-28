using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<PlayerData> allPlayers = new();

    [SerializeField] private Popup popupPrefab;
    [SerializeField] private Transform popupArea;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerData data = player.GetComponent<PlayerData>();
            allPlayers.Add(data);

            player.GetComponent<Health>().onDied += () =>
            {
                Popup popup = Instantiate(popupPrefab, popupArea);
                popup.killedPlayerName = data.Name;
                popup.killedPlayerImage = data.Image;
                popup.UpdateUI();
            };
        }
    }

    public PlayerData GetClosestPlayerToPoint(Vector3 point, out float distance)
    {
        distance = float.MaxValue;
        PlayerData closest = null;
        foreach (PlayerData player in allPlayers)
        {
            float dist = Vector3.Distance(player.transform.position, point);
            if (dist < distance)
            {
                distance = dist;
                closest = player.GetComponent<PlayerData>();
            }
        }
        return closest;
    }
}