using UnityEngine;

/// <summary>
/// This component sets the listeners of the GameObjectButton component to the required functions for the interactive main menu
/// </summary>
[RequireComponent(typeof(GameObjectButton))]
public class UIGOButton : MonoBehaviour
{
    [SerializeField] private MainMenuCamera cam;
    [SerializeField] private GameObject window;
    [SerializeField] private WindowGroup group;

    [Space, SerializeField] private Transform lookAtTarget;
    [SerializeField] private Transform moveTarget;
    private void Start()
    {
        var b = GetComponent<GameObjectButton>();
        b.OnClick.AddListener(async () =>
        {
            await cam.ZoomToObject(lookAtTarget.position, moveTarget.position);

            b.SetActiveState(false);

            group.DeactivateAll();
            window.SetActive(true);
        });
    }
}