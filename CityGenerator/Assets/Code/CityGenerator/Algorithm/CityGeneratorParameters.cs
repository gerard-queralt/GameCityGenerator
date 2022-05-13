using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGeneratorParameters
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

    private uint m_targetInhabitants;
    private Bounds m_area;
    private HashSet<CityElement> m_elements;
    private Dictionary<CityElementPair, float> m_affinities;
    private Texture m_roadTexture;
    private float m_roadWidthMin;
    private float m_roadWidthMax;
    private uint m_crossroads;
    private HeuristicCalculator m_heuristic;

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

    public CityGeneratorParameters(uint i_targetInhabitants,
                                   Bounds i_area,
                                   CityElement[] i_cityElements,
                                   CityElementAffinity[] i_affinities,
                                   Texture i_roadTexture,
                                   float i_roadWidthMin,
                                   float i_roadWidthMax,
                                   uint i_crossroads,
                                   System.Type i_heuristicType)
    {
        m_targetInhabitants = i_targetInhabitants;
        m_area = i_area;
        m_elements = new HashSet<CityElement>(i_cityElements);
        m_affinities = PopulateDictionary(i_affinities);
        m_roadTexture = i_roadTexture;
        m_roadWidthMin = i_roadWidthMin;
        m_roadWidthMax = i_roadWidthMax;
        m_crossroads = i_crossroads;
        if (i_heuristicType != null && typeof(HeuristicCalculator).IsAssignableFrom(i_heuristicType) && !i_heuristicType.IsAbstract)
        {
            System.Type[] contructorParameterTypes = new System.Type[1];
            contructorParameterTypes[0] = m_affinities.GetType();
            System.Reflection.ConstructorInfo constructor = i_heuristicType.GetConstructor(contructorParameterTypes);
            System.Object[] constructorParameters = new System.Object[1];
            constructorParameters[0] = m_affinities;
            m_heuristic = (HeuristicCalculator)constructor.Invoke(constructorParameters);
        }
        else
        {
            m_heuristic = new DefaultHeuristic(m_affinities);
        }
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
