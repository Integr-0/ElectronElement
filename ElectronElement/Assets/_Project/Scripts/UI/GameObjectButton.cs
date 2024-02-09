using UnityEngine;
using UnityEngine.Events;

public class GameObjectButton : MonoBehaviour
{
    [SerializeField, Tooltip("Leave null if you want no info")]
    private GameObject infoText;

    [SerializeField, Tooltip("Leave null if you want no info")]
    private GameObject canvas;

    public UnityEvent OnClick;

    private void OnMouseEnter()
    {
        if (infoText != null) infoText.SetActive(true);
    }
    private void OnMouseExit()
    {
        if (infoText != null) infoText.SetActive(false);
    }
    private void OnMouseUpAsButton()
    {
        if (enabled) OnClick?.Invoke();
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