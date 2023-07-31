using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultScore : MonoBehaviour
{
    [SerializeField] private Arduino arduino;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private WeightGoal weightGoal;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;
    [SerializeField] private int score;
    [SerializeField] private int perfectBonus = 100000;

    private void Update()
    {
        if (countdownTimer.gameEnded)
        {
            if (!weightGoal.isOverweight)
            {
                int weightBonus = Mathf.CeilToInt((Mathf.Abs(weightGoal.targetWeight - arduino.resultWeight) / weightGoal.targetWeight * -1 + 1) * perfectBonus);
                int timeBonus = Mathf.CeilToInt(countdownTimer.timer * 1000);
                if (countdownTimer.timer <= 0) timeBonus = 0;
                scoreTextMesh.text = "Time bonus: " + timeBonus + "\n" + "Weight bonus: " + weightBonus + "\n" + "Score: " + (weightBonus + timeBonus).ToString();
            }
            else
            {
                scoreTextMesh.text = "Exceeded weight limit!" + "\n" + "Score: " + 0;
            }
        }

    }
}
