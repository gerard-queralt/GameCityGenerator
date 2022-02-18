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
        public CityElement.Affinity affinity;
    }

    private struct CityElementPair
    {
        public CityElement first;
        public CityElement second;
    }

    [SerializeField] CityElement[] m_cityElements;
    private HashSet<CityElement> m_elementsSet;
    [SerializeField] CityElementAffinity[] m_affinities;
    private Dictionary<CityElementPair, CityElement.Affinity> m_afinitiesDict;
    [SerializeField] Texture m_pathTexture;

    private void Awake()
    {
        Debug.Assert(m_cityElements.Length > 0, "No CityElements set");
        m_elementsSet = new HashSet<CityElement>(m_cityElements);
        PopulateDictionary();
        Debug.Assert(m_pathTexture != null, "Path texture not set");
    }

    private void PopulateDictionary()
    {
        m_afinitiesDict = new Dictionary<CityElementPair, CityElement.Affinity>();
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
                bool affinityNotSet = !m_afinitiesDict.ContainsKey(pair) 
                                && !m_afinitiesDict.ContainsKey(new CityElementPair { first = affinity.elementB, second = affinity.elementA });
                Debug.Assert(affinityNotSet,
                                "Affinity between Elements " + affinity.elementA.name + " and " + affinity.elementB.name + " already defined");
                if (affinityNotSet)
                {
                    m_afinitiesDict.Add(pair, affinity.affinity);
                }
            }
            index++;
        }
    }
}
