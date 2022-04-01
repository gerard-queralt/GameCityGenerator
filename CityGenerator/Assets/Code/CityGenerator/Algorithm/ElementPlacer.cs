using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Road;

public class ElementPlacer
{
    private static Dictionary<CityElement, uint> m_instanceCount = new Dictionary<CityElement, uint>();

    private static Bounds m_area;
    private static uint m_targetInhabitants;
    private static uint m_currentInhabitants;

    public static HashSet<GameObject> PlaceElements(HashSet<CityElement> i_elements, HashSet<Road> i_roads, Bounds i_area, uint i_targetInhabitants)
    {
        m_area = i_area;
        m_targetInhabitants = i_targetInhabitants;
        m_currentInhabitants = 0;

        HashSet<GameObject> instances = new HashSet<GameObject>();
        Road road = ChooseRoad(new List<Road>(i_roads));
        while (m_currentInhabitants < m_targetInhabitants)
        {
            foreach (CityElement element in i_elements)
            {
                GameObject instance = PlaceElement(element, road);

                instances.Add(instance);
            }
        }
        return instances;
    }

    private static GameObject PlaceElement(CityElement element, Road road)
    {
        GameObject prefab = element.prefab;
        GameObject instance = GameObject.Instantiate(prefab, m_area.center, prefab.transform.rotation);
        Bounds boundsOfElement = element.boundingBox;
        
        PositionAndRotation positionAndRotation;
        LeftRight side = ChooseSide(road);
        if(side == LeftRight.Left)
        {
            positionAndRotation = road.left;
        }
        else
        {
            positionAndRotation = road.right;
        }

        Vector3 parcelPosition = positionAndRotation.position;
        Quaternion rotation = positionAndRotation.rotation;

        Vector3 position = parcelPosition + Quaternion.AngleAxis(rotation.y, Vector3.up) * new Vector3(0f, 0f, 5f - (10f * (int)side) /*tmp*/);
        instance.transform.SetPositionAndRotation(position, rotation);
        road.IncreaseDelta(side, boundsOfElement);
        
        m_currentInhabitants += element.inhabitants;
        IncreaseInstanceCount(element);
        
        return instance;
    }

    private static Road ChooseRoad(List<Road> i_roads) //tmp
    {
        int index = Random.Range(0, i_roads.Count);
        return i_roads[index];
    }

    private static LeftRight ChooseSide(Road i_road) //tmp
    {
        LeftRight random = (LeftRight)Random.Range(0, 2); //max is exclusive
        return random;
    }

    private static void IncreaseInstanceCount(CityElement i_element)
    {
        if(m_instanceCount.ContainsKey(i_element))
        {
            m_instanceCount[i_element] += 1;
        }
        else
        {
            m_instanceCount.Add(i_element, 1);
        }
    }
}
