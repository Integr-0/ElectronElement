using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text)), ExecuteInEditMode]
public class ParentNameGetter : MonoBehaviour
{
    private void Update()
    {
        GetComponent<TMP_Text>().text = transform.parent.name;
    }
}
