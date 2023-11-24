using UnityEngine;
using TMPro;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class LoadingScreen : MonoBehaviourSingleton<LoadingScreen>
{
    private const string NAME_TOGGLE_LOAD_STATE_TRIGGER = "ChangeLoadState";

    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text taskQueueText;
    [SerializeField] private GameObject panel;
    private Animator anim;

    private System.Collections.Generic.List<string> currentTasks;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }
    public void Activate(string header = "", params string[] allTasks)
    {
        currentTasks = allTasks.ToList();

        panel.SetActive(true);

        anim.SetTrigger(NAME_TOGGLE_LOAD_STATE_TRIGGER);

        headerText.text = header;
    }
    private void Deactivate()
    {
        panel.SetActive(false);
        anim.SetTrigger(NAME_TOGGLE_LOAD_STATE_TRIGGER);
    }

    public void StartTask()
    {
        taskQueueText.text = "Queue:\n";
        foreach (var task in currentTasks)
        {
            taskQueueText.text += task + "\n";
        }

        currentTasks.RemoveAt(0);
        if (currentTasks.Count == 0)
        {
            Deactivate();
        }
    }
    public void MarkTaskCompleted()
    {
        
    }
}