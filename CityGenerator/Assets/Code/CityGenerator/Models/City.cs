using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [System.Serializable]
    public struct CityElementAffinity
    {
        public CityElement elementA;
        public CityElement elementB;
        [Range(-1f, 1f)]public float affinity;
    }

    public struct CityElementPair
    {
        public CityElement first;
        public CityElement second;
    }

    [SerializeField] uint m_targetInhabitants;
    [SerializeField] Bounds m_area;
    [SerializeField] CityElement[] m_elementsArray;
    private HashSet<CityElement> m_elements;
    [SerializeField] CityElementAffinity[] m_affinitiesArray;
    private Dictionary<CityElementPair, float> m_affinities;
    [SerializeField] Texture m_roadTexture;
    [SerializeField] float m_roadWidthMin;
    [SerializeField] float m_roadWidthMax;
    [SerializeField] uint m_crossroads;
    [SerializeField] HeuristicCalculator m_heuristic;

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

    public Dictionary<CityElementPair, float> affinities
    {
        get
        {
            return m_affinities;
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
            if(m_crossroads == 0)
            {
                uint randomNumber = (uint) Mathf.RoundToInt(Random.Range(0, /*tmp*/ 10));
                return randomNumber;
            }
            return m_crossroads;
        }
    }

    public HeuristicCalculator heuristic
    {
        get
        {
            return m_heuristic;
        }
    }

    public void Generate()
    {
        m_elements = new HashSet<CityElement>(m_elementsArray);
        m_affinities = PopulateDictionary(m_affinitiesArray);
        if (m_heuristic == null)
        {
            m_heuristic = ScriptableObject.CreateInstance<DefaultHeuristic>();
        }
        AlgorithmMain algorithmMain = new AlgorithmMain(this);
        algorithmMain.Run();
    }

    private Dictionary<CityElementPair, float> PopulateDictionary(CityElementAffinity[] i_affinities)
    {
        Dictionary<CityElementPair, float>  affinities = new Dictionary<CityElementPair, float>();
        int index = 0; //Used to give better warnings
        foreach (CityElementAffinity affinity in i_affinities)
        {
            Debug.Assert(affinity.elementA != null, "Element A not set in affinity " + index);
            Debug.Assert(affinity.elementB != null, "Element B not set in affinity " + index);
            if (affinity.elementA && affinity.elementB)
            {
                Debug.Assert(m_elements.Contains(affinity.elementA),
                                "Element A [" + affinity.elementA.name + "] is not an element of the City, so this affinity will have no effect");
                Debug.Assert(m_elements.Contains(affinity.elementB),
                                "Element B [" + affinity.elementB.name + "] is not an element of the City, so this affinity will have no effect");
                
                CityElementPair pair = new CityElementPair { first = affinity.elementA, second = affinity.elementB };
                
                bool affinityNotSet = !affinities.ContainsKey(pair) 
                                && !affinities.ContainsKey(new CityElementPair { first = affinity.elementB, second = affinity.elementA });
                Debug.Assert(affinityNotSet,
                                "Affinity between Elements " + affinity.elementA.name + " and " + affinity.elementB.name + " already defined");
                if (affinityNotSet)
                {
                    affinities.Add(pair, affinity.affinity);
                }
            }
            index++;
        }
        return affinities;
    }
}
