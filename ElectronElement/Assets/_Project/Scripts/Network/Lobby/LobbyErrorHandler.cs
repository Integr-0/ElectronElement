using System.Threading.Tasks;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyErrorHandler : MonoBehaviour
{
    [SerializeField] private LobbyMaster master;

    public void HandleException(LobbyServiceException e)
    {
        if (e.Message == "Rate limit has been exceeded") //Too many requests (couldn't find a better condition)
        {
            master.Variables.tooManyReqestsPopup.SetActive(true);
        }
        else
        {
            Debug.LogWarning(e);
        }
    }

    public async void ToggleObjectForPopupTime(GameObject obj)
    {
        obj.SetActive(true);
        await Task.Delay(LobbyVariables.POPUP_ACTIVE_TIME_MILLISECONDS);
        obj.SetActive(false);
    }
}