using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class District
{
    [SerializeField] uint m_targetInhabitants;
    [SerializeField] Bounds m_area;
    [SerializeField] CityElement[] m_elementsArray;
    private HashSet<CityElement> m_elements;
    [SerializeField] Texture m_roadTexture;
    [SerializeField] float m_roadWidthMin;
    [SerializeField] float m_roadWidthMax;
    [SerializeField] uint m_crossroads;

    public uint targetInhabitants
    {
        get
        {
            return m_targetInhabitants;
        }
    }

    public Bounds area
    {
        get
        {
            return m_area;
        }
    }

    public HashSet<CityElement> cityElements
    {
        get
        {
            return m_elements;
        }
    }

    public Texture roadTexture
    {
        get
        {
            return m_roadTexture;
        }
    }

    public float roadWidthMin
    {
        get
        {
            return m_roadWidthMin;
        }
    }

    public float roadWidthMax
    {
        get
        {
            return m_roadWidthMax;
        }
    }

    public uint nCrossroads
    {
        get
        {
            if (m_crossroads == 0)
            {
                uint randomNumber = (uint)Mathf.RoundToInt(Random.Range(0, /*tmp*/ 10));
                return randomNumber;
            }
            return m_crossroads;
        }
    }

    public void CreateElementsSet()
    {
        m_elements = new HashSet<CityElement>(m_elementsArray);
    }
}
