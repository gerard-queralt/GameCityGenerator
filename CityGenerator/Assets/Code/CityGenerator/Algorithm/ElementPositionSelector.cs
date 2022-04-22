using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CityGeneratorParameters;
using static ElementPlacer;

public class ElementPositionSelector
{
    private Dictionary<CityElementPair, float> m_affinities;

    public ElementPositionSelector(Dictionary<CityElementPair, float> i_affinities)
    {
        m_affinities = i_affinities;
    }

    public float ComputeHeuristic(CityElement i_element, Vector3 i_position, HashSet<CityElementInstance> i_placedElements)
    {
        float heuristic = 0;
        foreach (CityElementInstance neighbour in i_placedElements)
        {
            float affinity = AffinityBetween(i_element, neighbour.m_element);
            float distance = Vector3.Distance(i_position, neighbour.transform.position);
            heuristic += affinity * 1 / distance;
        }
        return heuristic;
    }

    private float AffinityBetween(CityElement i_element, CityElement i_neighbour)
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
