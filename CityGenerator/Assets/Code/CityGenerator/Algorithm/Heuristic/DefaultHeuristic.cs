using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultHeuristic : HeuristicCalculator
{
    public DefaultHeuristic(Dictionary<CityGeneratorParameters.CityElementPair, float> i_affinities) : base(i_affinities){}

    public override float Calculate(CityElement i_element, Vector3 i_position, ElementPlacer.CityElementInstance i_placedElement)
    {
        float affinity = AffinityBetween(i_element, i_placedElement.m_element);
        float distance = Vector3.Distance(i_position, i_placedElement.transform.position);
        return affinity * 1 / distance;
    }
}
