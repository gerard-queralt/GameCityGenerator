using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Road;
using System.Linq;

public class ElementPlacer
{
    private PositionCalculator m_positionCalculator;
    private Bounds m_area;
    private uint m_targetInhabitants;
    private Dictionary<CityElement, uint> m_instanceCount = new Dictionary<CityElement, uint>();
    private uint m_currentInhabitants = 0;

    public ElementPlacer(PositionCalculator i_positionCalculator, Bounds i_area, uint targetInhabitants)
    {
        m_positionCalculator = i_positionCalculator;
        m_area = i_area;
        m_targetInhabitants = targetInhabitants;
    }

    public HashSet<GameObject> PlaceElements(HashSet<CityElement> i_elements, HashSet<Road> i_roads)
    {
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

    private GameObject PlaceElement(CityElement i_element, Road i_road)
    {
        GameObject prefab = i_element.prefab;

        LeftRight side = ChooseSide(i_road);
        if (!i_road.CanBePlaced(side, i_element.boundingBox))
        {
            side = (LeftRight)(((int)side + 1) % 2); //Left becomes Right and viceversa
            if (!i_road.CanBePlaced(side, i_element.boundingBox))
            {
                return null;
            }
        }

        Vector3 position = ComputePositionInRoad(i_element, i_road, side,);
        Quaternion rotation = i_road.rotation;
        if (side == LeftRight.Right)
        {
            rotation *= Quaternion.AngleAxis(180f, Vector3.up);
        }

        if (m_positionCalculator.CanElementBePlaced(i_element.boundingBox, position, rotation))
        {
            GameObject instance = GameObject.Instantiate(prefab, m_area.center, prefab.transform.rotation);

            i_road.IncreaseDelta(side, i_element.boundingBox);
            instance.transform.SetPositionAndRotation(position, rotation);

            m_currentInhabitants += i_element.inhabitants;
            IncreaseInstanceCount(i_element);

            CreateTemporalCopyOfElementInstance(instance, i_element.boundingBox);

            return instance;
        }
        return null;
    }

    private Vector3 ComputePositionInRoad(CityElement i_element, Road i_road, LeftRight i_side, float delta)
    {
        Vector2 origin = i_road.start.AsVector2;
        Vector2 direction = i_road.direction;
        float deltaElement = i_element.boundingBox.extents.x;
        delta += deltaElement;
        Vector2 perpendicular = i_road.perpendicular;
        float width = i_road.width;
        if (i_side == LeftRight.Right)
        {
            width *= -1;
        }
        Vector2 positionInPlane = origin + direction * delta + perpendicular * width/2f;
        float height = m_positionCalculator.FindGroundCoordinate(new Vector3(positionInPlane.x, m_area.max.y, positionInPlane.y), m_area.min.y);
        Vector3 position = new Vector3(positionInPlane.x, height, positionInPlane.y);
        return position;
    }

    private Road ChooseRoad(List<Road> i_roads) //tmp
    {
        int index = Random.Range(0, i_roads.Count);
        return i_roads[index];
    }

    private LeftRight ChooseSide(Road i_road) //tmp
    {
        LeftRight random = (LeftRight)Random.Range(0, 2); //max is exclusive
        return random;
    }

    private void IncreaseInstanceCount(CityElement i_element)
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

    private void CreateTemporalCopyOfElementInstance(GameObject i_instance, Bounds i_boundingBox)
    {
        GameObject tmpCopy = GameObject.Instantiate(i_instance);
        tmpCopy.layer = LayerMask.NameToLayer("CityGenerator_TMPObjects");
        BoxCollider collider = tmpCopy.AddComponent<BoxCollider>();
        collider.center = i_boundingBox.center;
        collider.size = i_boundingBox.size;
    }
}
