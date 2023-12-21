using UnityEngine;
using DG.Tweening;

public class WindowGroup : MonoBehaviour
{
    public void ToggleObject(GameObject obj)
    {
        bool active = !obj.activeSelf;

        DeactivateAll();

        obj.SetActive(active);
    }
    public void DeactivateAll()
    {
        transform.SetChildrenActive(false);
    }
}