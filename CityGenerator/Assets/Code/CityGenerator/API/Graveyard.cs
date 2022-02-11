using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Graveyard Definition", menuName = "City Generator/Graveyard Definition", order = 1)]
public class Graveyard : AdvancedCityElementEP
{
    [SerializeField] CityElementAffinity[] m_cityElementAffinities;

    public override CityElementAffinity[] cityElementAffinities
    {
        get
        {
            return m_cityElementAffinities;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Debug.Assert(m_cityElementAffinities.Length > 0, "No CityElementAffinities set");
    }
}
