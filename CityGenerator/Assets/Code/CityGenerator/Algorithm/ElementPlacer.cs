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

    private struct PositionAndRotation
    {
        public Vector3 position;
        public Quaternion rotation;
        //Necessary to increase the road's delta
        public Road road;
        public LeftRight side;
    }

    public ElementPlacer(PositionCalculator i_positionCalculator, Bounds i_area, uint targetInhabitants)
    {
        m_positionCalculator = i_positionCalculator;
        m_area = i_area;
        m_targetInhabitants = targetInhabitants;
    }

    public HashSet<GameObject> PlaceElements(HashSet<CityElement> i_elements, HashSet<Road> i_roads)
    {
        HashSet<GameObject> instances = new HashSet<GameObject>();
        while (m_currentInhabitants < m_targetInhabitants)
        {
            bool placedElementThisIteration = false;
            foreach (CityElement element in i_elements)
            {
                if (m_currentInhabitants + element.inhabitants <= m_targetInhabitants)
                {
                    List<PositionAndRotation> candidates = FindPositionCandidates(element, i_roads);

                    if (candidates.Count != 0)
                    {
                        PositionAndRotation bestCandidate = FindBestCandidate(element, candidates);
                        GameObject instance = PlaceElement(element, bestCandidate);
                        instances.Add(instance);
                        placedElementThisIteration = true;
                    }
                    else
                    {
                        Debug.Log("Element " + element.name + " could not be placed");
                    }
                }
            }
            if (!placedElementThisIteration)
            {
                Debug.Log("Target inhabitants could not be reached. Expected: " + m_targetInhabitants + ", Actual: " + m_currentInhabitants);
                break;
            }
        }
        return instances;
    }

    private List<PositionAndRotation> FindPositionCandidates(CityElement i_element, HashSet<Road> i_roads)
    {
        List<PositionAndRotation> candidates = new List<PositionAndRotation>();
        foreach (Road road in i_roads)
        {
            foreach (LeftRight side in System.Enum.GetValues(typeof(LeftRight)))
            {
                float delta = road.GetDelta(side);
                while (road.CanBePlaced(side, i_element.boundingBox, delta))
                {
                    Vector3 position = ComputePositionInRoad(i_element, road, side, delta);
                    Quaternion rotation = road.rotation;
                    if (side == LeftRight.Right)
                    {
                        rotation *= Quaternion.AngleAxis(180f, Vector3.up);
                    }

                    if (m_positionCalculator.CanElementBePlaced(i_element.boundingBox, position, rotation))
                    {
                        PositionAndRotation candidate = new PositionAndRotation
                        {
                            position = position,
                            rotation = rotation,
                            road = road,
                            side = side
                        };
                        candidates.Add(candidate);
                        break;
                    }
                    delta += 0.1f;
                }
            }
        }
        return candidates;
    }

    private PositionAndRotation FindBestCandidate(CityElement i_element, List<PositionAndRotation> i_candidates) //TMP
    {
        int index = Random.Range(0, i_candidates.Count);
        return i_candidates[index];
    }

    private GameObject PlaceElement(CityElement i_element, PositionAndRotation i_positionAndRotation)
    {
        GameObject prefab = i_element.prefab;
        GameObject instance = GameObject.Instantiate(prefab, m_area.center, prefab.transform.rotation);

        instance.transform.SetPositionAndRotation(i_positionAndRotation.position, i_positionAndRotation.rotation);

        m_currentInhabitants += i_element.inhabitants;
        IncreaseInstanceCount(i_element);

        CreateTemporaryCollider(instance, i_element.boundingBox);

        i_positionAndRotation.road.IncreaseDelta(i_positionAndRotation.side, i_element.boundingBox);

        return instance;
    }

    private Vector3 ComputePositionInRoad(CityElement i_element, Road i_road, LeftRight i_side, float i_delta)
    {
        Vector2 origin = i_road.start.AsVector2;
        Vector2 direction = i_road.direction;
        float deltaElement = i_element.boundingBox.extents.x;
        i_delta += deltaElement;
        Vector2 perpendicular = i_road.perpendicular;
        float width = i_road.width;
        if (i_side == LeftRight.Right)
        {
            width *= -1;
        }
        Vector2 positionInPlane = origin + direction * i_delta + perpendicular * width/2f;
        float height = m_positionCalculator.FindGroundCoordinate(new Vector3(positionInPlane.x, m_area.max.y, positionInPlane.y));
        Vector3 position = new Vector3(positionInPlane.x, height, positionInPlane.y);
        return position;
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

    private void CreateTemporaryCollider(GameObject i_instance, Bounds i_boundingBox)
    {
        GameObject tmpObject = GameObject.Instantiate(i_instance);
        tmpObject.layer = LayerMask.NameToLayer("CityGenerator_TMPObjects");
        BoxCollider collider = tmpObject.AddComponent<BoxCollider>();
        collider.center = i_boundingBox.center;
        collider.size = i_boundingBox.size;
    }
}
