using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    [SerializeField] District[] m_districtsArray;
    private HashSet<District> m_districts;
    [SerializeField] CityElementAffinity[] m_affinitiesArray;
    private Dictionary<CityElementPair, float> m_affinities;
    [SerializeField] HeuristicCalculator m_heuristic;

    public HashSet<District> districts
    {
        get
        {
            return m_districts;
        }
    }

    public Dictionary<CityElementPair, float> affinities
    {
        get
        {
            return m_affinities;
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
        m_districts = new HashSet<District>(m_districtsArray);
        foreach (District district in m_districts)
        {
            district.CreateElementsSet();
        }
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
        HashSet<CityElement> cityElements = m_districts.Aggregate(new HashSet<CityElement>(),
                                                                  (elements, district) => new HashSet<CityElement>(elements.Concat(district.cityElements)));

        Dictionary<CityElementPair, float>  affinities = new Dictionary<CityElementPair, float>();
        int index = 0; //Used to give better warnings
        foreach (CityElementAffinity affinity in i_affinities)
        {
            Debug.Assert(affinity.elementA != null, "Element A not set in affinity " + index);
            Debug.Assert(affinity.elementB != null, "Element B not set in affinity " + index);
            if (affinity.elementA && affinity.elementB)
            {
                Debug.Assert(cityElements.Contains(affinity.elementA),
                                "Element A [" + affinity.elementA.name + "] is not an element of the City, so this affinity will have no effect");
                Debug.Assert(cityElements.Contains(affinity.elementB),
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
