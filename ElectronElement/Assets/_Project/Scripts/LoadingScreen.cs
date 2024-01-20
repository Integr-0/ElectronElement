using UnityEngine;
using TMPro;
using System.Linq;

public class LoadingScreen : MonoBehaviourSingleton<LoadingScreen>
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text taskQueueText;
    [SerializeField] private GameObject panel;

    private System.Collections.Generic.List<string> currentTasks;

    public void Activate(string header, params string[] allTasks)
    {
        currentTasks = allTasks.ToList();

        panel.SetActive(true);

        headerText.text = header;
        DisplayQueue();
    }
    public void Deactivate() => panel.SetActive(false);

    public void MarkTaskCompleted()
    {
        if (currentTasks.Count > 0) currentTasks.RemoveAt(0);
        if (currentTasks.Count == 0)
        {
            panel.SetActive(false);
        }
        DisplayQueue();
    }

    private void DisplayQueue()
    {
        taskQueueText.text = "Queue:\n";
        foreach (var task in currentTasks)
        {
            taskQueueText.text += task + "\n";
        }
    }
}