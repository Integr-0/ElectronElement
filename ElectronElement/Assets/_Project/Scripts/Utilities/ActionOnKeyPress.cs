using UnityEngine;
using UnityEngine.Events;

public class ActionOnKeyPress : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [Space,SerializeField] private UnityEvent action;
    [Space, SerializeField] private bool onlyOnDownFrame = true;

    private void Update()
    {
        if (onlyOnDownFrame && Input.GetKeyDown(key) ||
            !onlyOnDownFrame && Input.GetKey(key))
        {
            action.Invoke();
        }
    }
}
