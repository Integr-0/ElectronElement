using System.Linq;
using UnityEngine;

public class GameObjectButtonGroup : MonoBehaviour
{
    public GameObjectButton[] buttons;
    public GameObject[] controlUI;

    private void Update()
    {
        bool active = !controlUI.Any(g => g.activeSelf);

        foreach (var button in buttons) button.enabled = active;
    }
}