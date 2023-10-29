using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

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

    [SerializeField] private LocalVolumetricFog orb;

    [Space, SerializeField] private float neededDistance;

    private int currentStage = 0;
    private int currentCrystals;

    private float[] orbSizeMap = new float[]
    {
        0f, 4f, 5.5f, 7f
    };

    private void Update()
    {
        orb.parameters.size = Vector3.one * orbSizeMap[currentCrystals];

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