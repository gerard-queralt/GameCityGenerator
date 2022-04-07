using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Road;
using System.Linq;

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
        List<Road> roads = new List<Road>(i_roads);
        while (m_currentInhabitants < m_targetInhabitants)
        {
            foreach (CityElement element in i_elements)
            {
                Road road = ChooseRoad(roads);
                GameObject instance = PlaceElement(element, road);

                List<Road> triedRoads = new List<Road>();
                triedRoads.Add(road);
                while (instance == null && triedRoads.Count < roads.Count)
                {
                    road = ChooseRoad(roads.Except(triedRoads).ToList());
                    instance = PlaceElement(element, road);
                    triedRoads.Add(road);
                }

                if (instance != null)
                {
                    instances.Add(instance);
                    if (m_currentInhabitants >= m_targetInhabitants)
                    {
                        break;
                    }
                }
                else
                {
                    Debug.Log("Element " + element.name + " could not be placed");
                    m_currentInhabitants += element.inhabitants; //TMP, for debugging
                }
            }
        }
        return instances;
    }

    private static GameObject PlaceElement(CityElement i_element, Road i_road)
    {
        GameObject prefab = i_element.prefab;
        GameObject instance = GameObject.Instantiate(prefab, m_area.center, prefab.transform.rotation);

        LeftRight side = ChooseSide(i_road);
        if (!i_road.CanBePlaced(side, i_element.boundingBox))
        {
            side = (LeftRight)(((int)side + 1) % 2); //Left becomes Right and viceversa
            if (!i_road.CanBePlaced(side, i_element.boundingBox))
            {
                return null;
            }
        }

        Vector3 position = ComputePositionInRoad(i_element, i_road, side);
        Quaternion rotation = i_road.rotation;
        if (side == LeftRight.Right)
        {
            rotation *= Quaternion.AngleAxis(180f, Vector3.up);
        }
        instance.transform.SetPositionAndRotation(position, rotation);

        m_currentInhabitants += i_element.inhabitants;
        IncreaseInstanceCount(i_element);
        
        return instance;
    }

    private static Vector3 ComputePositionInRoad(CityElement i_element, Road i_road, LeftRight i_side)
    {
        Bounds boundsOfElement = i_element.boundingBox;
        float deltaElement = boundsOfElement.extents.x;
        Vector2 origin = i_road.start.AsVector2;
        Vector2 direction = i_road.direction;
        float delta = i_road.GetDelta(i_side) + deltaElement;
        Vector2 perpendicular = i_road.perpendicular;
        float width = i_road.width;
        if (i_side == LeftRight.Right)
        {
            width *= -1;
        }
        Vector2 positionInPlane = origin + direction * delta + perpendicular * width;
        Vector3 position = new Vector3(positionInPlane.x, i_road.height, positionInPlane.y);
        i_road.IncreaseDelta(i_side, boundsOfElement);
        return position;
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
