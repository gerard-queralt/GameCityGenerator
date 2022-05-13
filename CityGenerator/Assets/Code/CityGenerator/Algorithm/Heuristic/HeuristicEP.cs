using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeuristicEP<T> : HeuristicCalculator where T : CityElement
{
    public HeuristicEP(Dictionary<CityGeneratorParameters.CityElementPair, float> i_affinities) : base(i_affinities) { }

    public sealed override float Calculate(CityElement i_element, Vector3 i_position, ElementPlacer.CityElementInstance i_placedElement)
    {
        T element = (T)i_element;
        T placedElement = (T)i_placedElement.m_element;
        return Calculate(element, i_position, placedElement, i_placedElement);
    }

    public abstract float Calculate(T i_element, Vector3 i_position, T i_placedElement, ElementPlacer.CityElementInstance i_placedElementGameObject);
}
