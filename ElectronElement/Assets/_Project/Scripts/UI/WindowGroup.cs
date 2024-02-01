using UnityEngine;

public class WindowGroup : MonoBehaviour
{
    [SerializeField] private GameObject[] windows;
    public void ToggleObject(GameObject obj)
    {
        bool active = !obj.activeSelf;

        DeactivateAll();

        obj.SetActive(active);
    }
    public void DeactivateAll()
    {
        foreach (GameObject window in windows) window.SetActive(false);
    }
}