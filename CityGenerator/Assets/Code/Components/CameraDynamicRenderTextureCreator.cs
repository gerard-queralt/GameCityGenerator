using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraDynamicRenderTextureCreator : MonoBehaviour
{
    RenderTexture m_renderTexture;

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        m_renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        camera.targetTexture = m_renderTexture;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
