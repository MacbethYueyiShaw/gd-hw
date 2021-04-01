using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public Material[] Skyboxes;
    public float RotationPerSecond = 1;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = Skyboxes[0];
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotationPerSecond);
    }
    public void ChangeSkybox()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }
}
