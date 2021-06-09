using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    public float smoothness = 0.02f;
    public Color currentColor;
    public Color finalColor;
    public SpriteRenderer renderer;


    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        currentColor = renderer.color;
        StartCoroutine("LerpColor");
    }

    private IEnumerator LerpColor()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / ColorDuration.duration; //The amount of change to apply.
        while (progress < 1)
        {
            currentColor = Color.Lerp(Color.white, finalColor, progress);
            renderer.color = currentColor;
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
}
