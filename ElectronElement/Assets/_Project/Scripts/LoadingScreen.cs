using UnityEngine;
using TMPro;
using System.Linq;

public class LoadingScreen : MonoBehaviourSingleton<LoadingScreen>
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text taskQueueText;
    [SerializeField] private GameObject panel;

    private System.Collections.Generic.List<string> currentTasks;

    public void Activate(string header = "", params string[] allTasks)
    {
        currentTasks = allTasks.ToList();

        panel.SetActive(true);

        headerText.text = header;
    }

    public void MarkTaskCompleted()
    {
        taskQueueText.text = "Queue:\n";
        foreach (var task in currentTasks)
        {
            taskQueueText.text += task + "\n";
        }

        currentTasks.RemoveAt(0);
        if (currentTasks.Count == 0)
        {
            panel.SetActive(false);
        }
    }
}