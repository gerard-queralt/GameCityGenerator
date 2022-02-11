using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdvancedCityElement : CityElement
{
    public enum Affinity
    {
        VeryClose,
        Close,
        Indifferent,
        Far,
        VeryFar
    }

    [System.Serializable]
    public struct CityElementAffinity
    {
        public CityElement element;
        public Affinity affinity;
    }

    public abstract CityElementAffinity[] cityElementAffinities
    {
        get;
    }
}
