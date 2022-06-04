using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeuristicEP<T> : HeuristicCalculator where T : CityElement
{
    public sealed override float Calculate(CityElement i_element,
                                           Vector3 i_position,
                                           ElementPlacer.CityElementInstance i_placedElement,
                                           Dictionary<CityGeneratorParameters.CityElementPair, float> i_affinities)
    {
        T element = (T)i_element;
        T placedElement = (T)i_placedElement.m_element;
        return Calculate(element, i_position, placedElement, i_placedElement, i_affinities);
    }

    public abstract float Calculate(T i_element,
                                    Vector3 i_position,
                                    T i_placedElement,
                                    ElementPlacer.CityElementInstance i_placedElementGameObject,
                                    Dictionary<CityGeneratorParameters.CityElementPair, float> i_affinities);
}
