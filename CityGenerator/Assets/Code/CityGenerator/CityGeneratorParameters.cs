using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGeneratorParameters : MonoBehaviour
{
    [SerializeField] CityElement[] m_cityElements;
    private HashSet<CityElement> m_elementsSet;
    [SerializeField] Texture m_pathTexture;

    private void Awake()
    {
        Debug.Assert(m_cityElements.Length > 0, "No CityElements set");
        m_elementsSet = new HashSet<CityElement>(m_cityElements);
        Debug.Assert(m_pathTexture != null, "Path texture not set");
    }
}
