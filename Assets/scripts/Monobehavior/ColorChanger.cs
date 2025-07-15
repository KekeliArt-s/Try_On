using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    public Material[] colors;
    public Renderer targetRenderer;

    public void ChangeColor(int index)
    {
        if (index >= 0 && index < colors.Length)
        {
            targetRenderer.material = colors[index];
        }
    }
}

