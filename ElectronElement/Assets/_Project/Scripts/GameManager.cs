using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    private readonly List<PlayerData> allPlayers = new();

    [SerializeField] private LobbyMaster lobbyMaster;

    [Space, SerializeField] private Popup popupPrefab;
    [SerializeField] private Transform popupArea;

    [Space, SerializeField] private GameObject lobbyParent;
    [SerializeField] private GameObject inLobbyScreen;

    private void Start()
    {
        lobbyMaster.OneTimeInit();

        // The weirdest workaround I've ever seen
#if UNITY_EDITOR
        NetworkManager.Singleton.SceneManager.OnLoadComplete += (clientID, scene, loadMode) =>
        {
            foreach (var netObj in FindObjectsOfType<NetworkObject>())
            {
                netObj.AlwaysReplicateAsRoot  = !netObj.AlwaysReplicateAsRoot;
                netObj.AlwaysReplicateAsRoot  = !netObj.AlwaysReplicateAsRoot;
            }
        };
#endif
    }

    private void GetAllPlayers()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerData data = player.GetComponent<PlayerData>();
            allPlayers.Add(data);

            player.GetComponent<Health>().onDied += () => DisplayPopup(data);
        }

        async void DisplayPopup(PlayerData data)
        {
            Popup popup = Instantiate(popupPrefab, popupArea);
            popup.killedPlayerName = data.Name;
            popup.killedPlayerImage = data.Image;
            popup.UpdateUI();
            await Task.Delay(1000);
            Destroy(popup.gameObject);
        }
    }

    public PlayerData GetClosestPlayerToPoint(Vector3 point, out float distance)
    {
        distance = float.MaxValue;
        PlayerData closest = null;
        foreach (PlayerData player in allPlayers)
        {
            if (player == null) continue; // Because of weird stuff when unloading scenes (should not be null here)

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
        lobbyMaster.Variables.load.MarkTaskCompleted();

        GetAllPlayers();

        Debug.Log("Game Started (Client)");
    }

    public void ResetLobby()
    {
        lobbyParent.SetActive(true);
        inLobbyScreen.SetActive(false);
    }
}