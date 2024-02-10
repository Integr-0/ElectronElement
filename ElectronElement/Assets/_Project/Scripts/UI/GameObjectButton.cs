using UnityEngine;
using UnityEngine.Events;

public class GameObjectButton : MonoBehaviour
{
    [SerializeField, Tooltip("Leave null if you want no info")]
    private GameObject infoText;

    [SerializeField, Tooltip("Leave null if you want no info")]
    private GameObject canvas;

    private bool active = true;

    public void SetActiveState(bool active)
    {
        this.active = active;
    }

    public UnityEvent OnClick;

    private void OnMouseEnter()
    {
        if (infoText != null && active) infoText.SetActive(true);
    }
    private void OnMouseExit()
    {
        if (infoText != null && active) infoText.SetActive(false);
    }
    private void OnMouseUpAsButton()
    {
        if (active) OnClick?.Invoke();
    }


    private void OnDisable()
    {
        if (canvas != null) canvas.SetActive(false);
    }
    private void OnEnable()
    {
        if (canvas != null) canvas.SetActive(true);
    }
}