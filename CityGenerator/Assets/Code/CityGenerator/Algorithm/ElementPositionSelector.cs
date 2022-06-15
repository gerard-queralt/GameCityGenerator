using System.Collections.Generic;
using UnityEngine;

using static City;

public class ElementPositionSelector
{
    private Dictionary<CityElementPair, float> m_affinities;
    private HeuristicCalculator m_heuristic;

    public ElementPositionSelector(Dictionary<CityElementPair, float> i_affinities, HeuristicCalculator i_heuristic)
    {
        m_affinities = i_affinities;
        m_heuristic = i_heuristic;
    }

    public float ComputeHeuristic(CityElement i_element, Vector3 i_position, HashSet<CityElementInstance> i_placedElements)
    {
        float heuristic = 0;
        foreach (CityElementInstance neighbour in i_placedElements)
        {
            heuristic += m_heuristic.Calculate(i_element, i_position, neighbour, m_affinities);
        }
        return heuristic;
    }
}
