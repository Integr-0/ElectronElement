using UnityEngine;
using TMPro;
using System.Linq;

public class LoadingScreen : MonoBehaviourSingleton<LoadingScreen>
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text taskQueueText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private UnityEngine.UI.Slider progressBar;

    private System.Collections.Generic.List<string> currentTasks;
    public void Activate(string header = "", params string[] allTasks)
    {
        currentTasks = allTasks.ToList();

        panel.SetActive(true);

        progressBar.minValue = 0f;
        progressBar.maxValue = 1f;
        progressBar.value = 0f;
        progressText.text = $"{currentTasks[0]}: 0%";

        headerText.text = header;

        taskQueueText.text = "Queue:\n";

        foreach (var task in currentTasks)
            taskQueueText.text += $"{task}\n";
    }
    public void Deactivate()
    {
        panel.SetActive(false);
    }

    public async void TaskCompleted(int artificialDelayMilliseconds = 0)
    {
        await System.Threading.Tasks.Task.Delay(artificialDelayMilliseconds);

        currentTasks.RemoveAt(0);
        if(currentTasks.Count == 0)
        {
            Deactivate();
            return;
        }
        taskQueueText.text = "Queue:\n";

        foreach (var task in currentTasks) 
            taskQueueText.text += $"{task}\n";

        SetProgress(0);
    }
    public void SetProgress(float progress)
    {
        progressBar.value = progress;
        progressText.text = $"{currentTasks[0]}: {progress * 100}%";
    }
}