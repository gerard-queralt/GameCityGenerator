using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CityGeneratorParameters;
using static ElementPlacer;

public abstract class HeuristicCalculator
{
    private Dictionary<CityElementPair, float> m_affinities;

    public HeuristicCalculator(Dictionary<CityElementPair, float> i_affinities)
    {
        m_affinities = i_affinities;
    }

    public abstract float Calculate(CityElement i_element, Vector3 i_position, CityElementInstance i_placedElement);

    protected float AffinityBetween(CityElement i_element, CityElement i_neighbour)
    {
        CityElementPair pair = new CityElementPair
        {
            first = i_element,
            second = i_neighbour
        };
        if (m_affinities.ContainsKey(pair))
        {
            return m_affinities[pair];
        }
        pair = new CityElementPair //reversed
        {
            first = i_neighbour,
            second = i_element
        };
        if (m_affinities.ContainsKey(pair))
        {
            return m_affinities[pair];
        }
        return i_element.defaultAffinity + i_neighbour.defaultAffinity;
    }
}
