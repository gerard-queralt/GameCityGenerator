using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CityGeneratorParameters;

public abstract class HeuristicCalculator : ScriptableObject
{
    public abstract float Calculate(CityElement i_element,
                                    Vector3 i_position,
                                    CityElementInstance i_placedElement,
                                    Dictionary<CityElementPair, float> i_affinities);

    protected float AffinityBetween(CityElement i_element, CityElement i_neighbour, Dictionary<CityElementPair, float> i_affinities)
    {
        CityElementPair pair = new CityElementPair
        {
            first = i_element,
            second = i_neighbour
        };
        if (i_affinities.ContainsKey(pair))
        {
            return i_affinities[pair];
        }
        pair = new CityElementPair //reversed
        {
            first = i_neighbour,
            second = i_element
        };
        if (i_affinities.ContainsKey(pair))
        {
            return i_affinities[pair];
        }
        return i_element.defaultAffinity + i_neighbour.defaultAffinity;
    }
}
