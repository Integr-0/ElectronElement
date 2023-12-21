using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyPlayerData : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public void WriteCurrentPlayerDataToJoinedLobby()
    {
        PlayerDataObject gamertagData = new(PlayerDataObject.VisibilityOptions.Member, master.Variables.gamertag);
        PlayerDataObject charIndexData = new(PlayerDataObject.VisibilityOptions.Member, master.Variables.characterIndex.ToString());

        //if is host just take the first dictionary
        if (master.Variables.hostedLobby != null)
        {
            SetValues(0);

            return;
        }

        int index = 0;
        while (master.Variables.joinedLobby.Players[index].Data != null)
            index++;

        SetValues(index);

        void SetValues(int index)
        {
            master.Variables.joinedLobby.Players[index].Data = new() { [LobbyVariables.KEY_PLAYER_NAME] = gamertagData }; //Initialize Dictionary with the gamertagData (so it isn't null)
            master.Variables.joinedLobby.Players[index].Data.Add(LobbyVariables.KEY_PLAYER_CHAR_INDEX, charIndexData); //Now we can add stuff normally
        }
    }
}