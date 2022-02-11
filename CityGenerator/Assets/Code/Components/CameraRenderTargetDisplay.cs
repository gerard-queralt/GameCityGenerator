using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CameraRenderTargetDisplay : MonoBehaviour
{
    [SerializeField] Camera m_camera;
    RawImage m_image;

    private void Awake()
    {
        m_image = GetComponent<RawImage>();
        Debug.Assert(m_camera != null, "Camera of GameObject[" + gameObject.name + "] is not set");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_image.texture = m_camera.targetTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
