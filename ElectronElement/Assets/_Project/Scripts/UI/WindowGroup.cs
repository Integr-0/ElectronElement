using UnityEngine;

public class WindowGroup : MonoBehaviour
{
    public void ToggleObject(GameObject obj)
    {
        bool active = !obj.activeSelf;

        transform.SetChildrenActive(false);

        obj.SetActive(active);
    }
}