using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGeneratorParameters : MonoBehaviour
{
    [System.Serializable]
    public struct CityElementAffinity
    {
        public CityElement elementA;
        public CityElement elementB;
        [Range(0f, 1f)]public float affinity;
    }

    public struct CityElementPair
    {
        public CityElement first;
        public CityElement second;
    }

    [SerializeField] uint m_targetInhabitants;
    [SerializeField] Vector3 m_areaCenter;
    [SerializeField] Vector3 m_areaSize;
    private Bounds m_area;
    [SerializeField] CityElement[] m_cityElements;
    private HashSet<CityElement> m_elementsSet;
    [SerializeField] CityElementAffinity[] m_affinities;
    private Dictionary<CityElementPair, float> m_affinitiesDict;
    [SerializeField] Texture m_roadTexture;
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
            return m_elementsSet;
        }
    }

    public Dictionary<CityElementPair, float> affinities
    {
        get
        {
            return m_affinitiesDict;
        }
    }

    public Texture roadTexture
    {
        get
        {
            return m_roadTexture;
        }
    }

    public uint nCrossroads
    {
        get
        {
            if(m_crossroads == 0)
            {
                uint randomNumber = (uint) Mathf.RoundToInt(UnityEngine.Random.Range(0, /*tmp*/ 10));
                return randomNumber;
            }
            return m_crossroads;
        }
    }

    private void Awake()
    {
        Debug.Assert(m_cityElements.Length > 0, "No CityElements set");
        m_area = new Bounds(m_areaCenter, m_areaSize);
        m_elementsSet = new HashSet<CityElement>(m_cityElements);
        PopulateDictionary();
        Debug.Assert(m_roadTexture != null, "Road texture not set");
    }

    private void PopulateDictionary()
    {
        m_affinitiesDict = new Dictionary<CityElementPair, float>();
        int index = 0; //Used to give better warnings
        foreach (CityElementAffinity affinity in m_affinities)
        {
            Debug.Assert(affinity.elementA != null, "Element A not set in affinity " + index);
            Debug.Assert(affinity.elementB != null, "Element B not set in affinity " + index);
            if (affinity.elementA && affinity.elementB)
            {
                Debug.Assert(m_elementsSet.Contains(affinity.elementA),
                                "Element A [" + affinity.elementA.name + "] is not an element of the City, so this affinity will have no effect");
                Debug.Assert(m_elementsSet.Contains(affinity.elementB),
                                "Element B [" + affinity.elementB.name + "] is not an element of the City, so this affinity will have no effect");
                
                CityElementPair pair = new CityElementPair { first = affinity.elementA, second = affinity.elementB };
                
                bool affinityNotSet = !m_affinitiesDict.ContainsKey(pair) 
                                && !m_affinitiesDict.ContainsKey(new CityElementPair { first = affinity.elementB, second = affinity.elementA });
                Debug.Assert(affinityNotSet,
                                "Affinity between Elements " + affinity.elementA.name + " and " + affinity.elementB.name + " already defined");
                if (affinityNotSet)
                {
                    m_affinitiesDict.Add(pair, affinity.affinity);
                }
            }
            index++;
        }
    }
}
