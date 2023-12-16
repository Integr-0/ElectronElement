using UnityEngine;
using UnityEngine.Events;

public class ActionOnKeyPress : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [Space,SerializeField] private UnityEvent action;
    [Space, SerializeField] private bool onlyOnNewPress = true;

    private void Update()
    {
        if ((onlyOnNewPress && Input.GetKeyDown(key)) ||
            (!onlyOnNewPress && Input.GetKey(key)))
        {
            action.Invoke();
        }
    }
}
