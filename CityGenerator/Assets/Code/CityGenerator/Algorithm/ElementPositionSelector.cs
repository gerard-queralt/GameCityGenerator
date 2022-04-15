using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CityGeneratorParameters;

public class ElementPositionSelector
{
    private float m_maxHeight = 0f;
    private float m_neighbourRadius = 10f; //needs adjusting
    private HashSet<CityElement> m_elements;
    private Dictionary<CityElementPair, float> m_affinities;

    public ElementPositionSelector(HashSet<CityElement> i_elements, Dictionary<CityElementPair, float> i_affinities)
    {
        m_elements = i_elements;
        m_affinities = i_affinities;
    }

    public float ComputeHeuristic(CityElement i_element, Vector3 i_position)
    {
        List<CityElement> neighbours = FindNeighbours(i_position);
        float heuristic = 0;
        foreach (CityElement neighbour in neighbours)
        {
            heuristic += AffinityBetween(i_element, neighbour);
        }
        return heuristic;
    }

    //Possible extension: check all the area of the city and use the distance to give weight to the heuristic
    private List<CityElement> FindNeighbours(Vector3 i_position)
    {
        Vector3 origin = i_position;
        float radius = m_neighbourRadius;
        origin.y += radius /*tmp*/ + 10f;
        float maxDistance = origin.y /*tmp*/ + 100f;
        LayerMask mask = LayerMask.GetMask("CityGenerator_TMPObjects");
        QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Ignore;
        RaycastHit[] hits = Physics.SphereCastAll(origin, radius, Vector3.down, maxDistance, mask, queryTrigger);

        List<CityElement> neighbours = new List<CityElement>();
        foreach (RaycastHit hit in hits)
        {
            String cityElementName = hit.collider.gameObject.name;
            CityElement neighbour = FindElementFromName(cityElementName);
            if (neighbour != null)
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours;
    }

    private CityElement FindElementFromName(string i_name)
    {
        foreach (CityElement element in m_elements)
        {
            if (element.name == i_name)
            {
                return element;
            }
        }
        return null;
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
