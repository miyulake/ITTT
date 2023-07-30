using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeTextMesh;
    public float timer = 60;
    [SerializeField] private GameObject scaleWeight;
    public bool gameEnded;

    private void Update()
    {
        timeTextMesh.text = "Time left: " + Mathf.CeilToInt(timer).ToString();

        if (timer <= 0 || gameEnded)
        {
            timeTextMesh.text = "Game ended";
            scaleWeight.SetActive(true);
            gameEnded = true;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
