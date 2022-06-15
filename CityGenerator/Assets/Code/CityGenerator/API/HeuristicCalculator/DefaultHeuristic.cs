using System.Collections.Generic;
using UnityEngine;

public class DefaultHeuristic : HeuristicCalculator
{
    public override float Calculate(CityElement i_element,
                                    Vector3 i_position,
                                    CityElementInstance i_placedElement,
                                    Dictionary<City.CityElementPair, float> i_affinities)
    {
        float affinity = AffinityBetween(i_element, i_placedElement.m_element, i_affinities);
        float distance = Vector3.Distance(i_position, i_placedElement.transform.position);
        return affinity * 1 / distance;
    }
}
