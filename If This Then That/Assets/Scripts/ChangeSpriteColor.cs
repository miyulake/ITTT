using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteColor : MonoBehaviour
{
    [SerializeField] private float Speed = 1;

    [SerializeField] private Image sprite;

    private void Update()
    {
        sprite.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * Speed, 1), 1, 0.5f));
    }
}
