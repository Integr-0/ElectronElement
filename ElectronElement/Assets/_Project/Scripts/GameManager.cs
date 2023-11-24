using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public List<PlayerData> allPlayers = new();

    [SerializeField] private Popup popupPrefab;
    [SerializeField] private Transform popupArea;

    [Space, SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject popupCanvas;
    protected override void Awake()
    {
        base.Awake();
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

    [ServerRpc]
    private void DespawnInstanceServerRpc()
    {
        GetComponent<NetworkObject>().Despawn(destroy: true);
    }


    public void HostStartGame()
    {
        lobbyCanvas.SetActive(false);
        popupCanvas.SetActive(true);

        Debug.Log("Game Started (Host)");
    }
    public void ClientStartGame()
    {
        lobbyCanvas.SetActive(false);
        popupCanvas.SetActive(true);

        Debug.Log("Game Started (Client)");
    }
}