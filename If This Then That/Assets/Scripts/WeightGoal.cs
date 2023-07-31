using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightGoal : MonoBehaviour
{
    [SerializeField] private Arduino arduino;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private TextMeshProUGUI weightTextMesh;
    [Range(500, 1000)] [SerializeField] private float minWeight = 1000;
    [Range(1000, 10000)] [SerializeField] private float maxWeight = 10000;
    [HideInInspector] public float targetWeight;
    [HideInInspector] public bool isOverweight;

    private void Start()
    {
        targetWeight = Mathf.CeilToInt(Random.Range(minWeight, maxWeight));
        weightTextMesh.text = "Target weight: " + targetWeight + " Grams";
    }

    private void Update()
    {
        if (arduino.weightState > (targetWeight * 1.33f) && !countdownTimer.gameEnded)
        {
            isOverweight = true;
            countdownTimer.gameEnded = true;
            Debug.Log("Exceeded weight limit");
        }
    }
}
