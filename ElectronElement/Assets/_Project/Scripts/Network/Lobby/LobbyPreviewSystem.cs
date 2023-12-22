using UnityEngine;

public class LobbyPreviewSystem : MonoBehaviour
{
    public struct JoinData
    {
        public string Name;
        public int CharacterIndex;
    }

    [SerializeField] private LobbyMaster master;

    public void DisplayPreviews()
    {
        master.Variables.playerPreviewParent.DestroyChildren();
        foreach (var player in master.Variables.joinedLobby.Players)
        {
            var data = player.Data;

            //guard clause if there aren't enough elements in the dictionary
            //if the dataCount is smaller than 2 it will throw an error
            if (data == null || data.Count < 2)
                return;

            JoinData joinData = new()
            {
                CharacterIndex = int.Parse(data[LobbyVariables.KEY_PLAYER_CHAR_INDEX].Value),
                Name = data[LobbyVariables.KEY_PLAYER_NAME].Value,
            };

            InstantiatePlayerPreview(joinData);
        }
    }
    public void InstantiatePlayerPreview(JoinData data)
    {
        var prev = Instantiate(master.Variables.playerPreviewPrefab, master.Variables.playerPreviewParent);

        prev.CharacterImg.sprite = master.Variables.characterImages[data.CharacterIndex];
        prev.GamertagText.text = data.Name;
    }
}