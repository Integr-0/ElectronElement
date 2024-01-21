using TMPro;
using UnityEngine;

public class TitleValues : MonoBehaviour
{
    [SerializeField] private TMP_Text gameNameText;
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private TMP_Text companyText;

    private void Awake()
    {
        Debug.Log("Initializing Titles...");
        gameNameText.text = Application.productName;
        versionText.text = Application.version;
        companyText.text = "by " + Application.companyName;
    }
}