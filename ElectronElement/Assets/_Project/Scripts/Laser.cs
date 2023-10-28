using UnityEngine;

public class Laser : MonoBehaviour
{
    [System.Serializable]
    public struct Stage
    {
        public Transform transform;
        public int neededMats;
    }

    [SerializeField] private Animator anim;
    [SerializeField] private string animatorIntName;

    [Space, SerializeField] private Stage[] stages;
    [SerializeField] private Transform crystal;
    [SerializeField] private int neededCrystals;
    private int currentCrystals;

    [Space, SerializeField] private float neededDistance;

    private int currentStage = 0;

    private void Update()
    {
        //To prevent IndexOutOfRangeException
        if (currentStage < stages.Length)
        {
            PlayerData nearestPlayer = GameManager.Instance.GetClosestPlayerToPoint(stages[currentStage].transform.position, out float distance);

            if (distance <= neededDistance &&
                nearestPlayer.shelfLooter.lootedShelves >= stages[currentStage].neededMats &&
                Input.GetKeyDown(KeyCode.E))
            {
                nearestPlayer.shelfLooter.lootedShelves -= stages[currentStage].neededMats;
                currentStage++;
                anim.SetInteger(animatorIntName, currentStage);
            }
        }
        else if (currentStage == stages.Length)
        {
            PlayerData nearestPlayer = GameManager.Instance.GetClosestPlayerToPoint(crystal.position, out float distance);

            if (distance <= neededDistance &&
                nearestPlayer.shelfLooter.lootedCrystals >= 1 &&
                Input.GetKeyDown(KeyCode.E))
            {
                nearestPlayer.shelfLooter.lootedCrystals--;
                currentCrystals++;

                if (currentCrystals >= neededCrystals)
                {
                    currentStage++;
                    anim.SetInteger(animatorIntName, currentStage);
                }
            }
        }
    }

    public void Shoot()
    {
        Debug.Log("Everyone is ded");
    }
}