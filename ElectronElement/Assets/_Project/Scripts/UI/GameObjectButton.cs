using UnityEngine;
using UnityEngine.Events;

public class GameObjectButton : MonoBehaviour
{
    [SerializeField, Tooltip("Leave null if you want no info")]
    private GameObject infoText;

    [SerializeField] private GameObject canvas;

    [SerializeField] private UnityEvent OnClick;

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
        if (infoText != null && enabled) OnClick.Invoke();
    }


    private void OnDisable()
    {
        canvas.SetActive(false);
    }
    private void OnEnable()
    {
        canvas.SetActive(true);
    }
}