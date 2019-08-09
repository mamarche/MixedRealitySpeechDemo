using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    public MeshRenderer cubeRenderer;
    public Material redMaterial;
    public Material blueMaterial;

    public void ColorRedCommand()
    {
        cubeRenderer.material = redMaterial;
    }

    public void ColorBlueCommand()
    {
        cubeRenderer.material = blueMaterial;
    }
}
