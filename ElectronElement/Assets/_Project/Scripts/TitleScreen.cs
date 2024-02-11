using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private float targetSpacing;
    [SerializeField] private float spaceIncrement;
    [SerializeField] private float colorIncrement;

    private bool listening = false;

    private void Update()
    {
        if (!listening) return;

        if (Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // go to next scene in the build settings
        }
    }

    private void Awake()
    {
        infoText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(true);

        AnimateMainText();
    }

    public async void AnimateMainText()
    {
        while (mainText.characterSpacing < targetSpacing)
        {
            mainText.characterSpacing += spaceIncrement * Time.deltaTime;
            await Task.Yield();
        }

        infoText.gameObject.SetActive(true);
        listening = true;

        AnimateInfoText();
    }
    public async void AnimateInfoText()
    {
        while (true)
        {
            while (infoText.color.a > 0)
            {
                infoText.color = new(infoText.color.r, infoText.color.g, infoText.color.b, infoText.color.a - (colorIncrement * Time.deltaTime));
                await Task.Yield();
            }

            while (infoText.color.a < 1)
            {
                infoText.color = new(infoText.color.r, infoText.color.g, infoText.color.b, infoText.color.a + (colorIncrement * Time.deltaTime));
                await Task.Yield();
            }
        }   
    }
}