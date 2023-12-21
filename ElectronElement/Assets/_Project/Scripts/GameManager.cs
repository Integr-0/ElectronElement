using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    private readonly List<PlayerData> allPlayers = new();

    [SerializeField] private Popup popupPrefab;
    [SerializeField] private Transform popupArea;

    [Space, SerializeField] private GameObject lobbyParent;
    [SerializeField] private GameObject inLobbyScreen;

    private void GetAllPlayers()
    {
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

    public void HostStartGame()
    {
        lobbyParent.SetActive(false);

        GetAllPlayers();

        Debug.Log("Game Started (Host)");
    }
    public void ClientStartGame()
    {
        lobbyParent.SetActive(false);

        Debug.Log("Game Started (Client)");
    }

    public void ResetLobby()
    {
        lobbyParent.SetActive(true);
        inLobbyScreen.SetActive(false);
    }
}