using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Laser : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private string animatorIntName;

    [Space, SerializeField] private int[] stages;
    [SerializeField] private Transform terminal;
    [SerializeField] private int neededCrystals;

    [SerializeField] private LocalVolumetricFog orb;

    [Space, SerializeField] private float neededDistance;

    private int currentStage = 0;
    private int currentCrystals;

    private float[] orbSizeMap = new float[]
    {
        0f, 5.5f, 7f, 0f
    };

    private void Update()
    {
        orb.parameters.size = Vector3.one * orbSizeMap[currentCrystals];

        if (currentStage < stages.Length)
        {
            PlayerData nearestPlayer = GameManager.Instance.GetClosestPlayerToPoint(terminal.position, out float distance);

            if (distance > neededDistance) return;

            infoText.text = nearestPlayer.shelfLooter.lootedShelves >= stages[currentStage] ?
                $"You have enough materials for Stage {currentStage + 1}\n(e)" :
                $"Stage {currentStage + 1} requires {stages[currentStage]} materials\n" +
                $"You have {nearestPlayer.shelfLooter.lootedShelves}";

            if (nearestPlayer.shelfLooter.lootedShelves >= stages[currentStage] &&
                Input.GetKeyDown(KeyCode.E))
            {
                nearestPlayer.shelfLooter.lootedShelves -= stages[currentStage];
                currentStage++;

                anim.SetInteger(animatorIntName, currentStage);
            }    
        }
        else if (currentStage == stages.Length)
        {
            PlayerData nearestPlayer = GameManager.Instance.GetClosestPlayerToPoint(terminal.position, out float distance);

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

            infoText.text = $"Energizing laser...\n{neededCrystals - currentCrystals} energy crystals remaining";
        }
    }

    public void Shoot()
    {
        Debug.Log("Everyone is ded");
        infoText.text = "Laser has been fired";
    }
    public void FinishEnergize()
    {
        infoText.text = "Laser energized.\nCalibrating aim...";
    }
    public void FinishCalibration()
    {
        infoText.text = "Aim calibrated.\nHeating up...";
    }
}