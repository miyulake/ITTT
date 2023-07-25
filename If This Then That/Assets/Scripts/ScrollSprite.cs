using UnityEngine;
using UnityEngine.UI;


public class ScrollImage : MonoBehaviour
{
    public float scrollSpeedX = 1f;
    public float scrollSpeedY = 1f;

    private Image sprite;
    private Material material;

    void Start()
    {
        sprite = GetComponent<Image>();
        material = sprite.material;
    }

    void Update()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;
        material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}