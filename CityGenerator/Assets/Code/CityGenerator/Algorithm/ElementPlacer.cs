using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using static Road;
using static CityGeneratorParameters;
using System;

public class ElementPlacer
{
    private PositionCalculator m_positionCalculator;
    private Bounds m_area;
    private uint m_targetInhabitants;
    private Dictionary<CityElement, uint> m_instanceCount = new Dictionary<CityElement, uint>();
    private uint m_currentInhabitants = 0;
    private HashSet<CityElementInstance> m_placedElements = new HashSet<CityElementInstance>();

    private struct PositionAndRotation
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    public ElementPlacer(PositionCalculator i_positionCalculator, Bounds i_area, uint i_targetInhabitants)
    {
        m_positionCalculator = i_positionCalculator;
        m_area = i_area;
        m_targetInhabitants = i_targetInhabitants;
    }

    public HashSet<GameObject> PlaceElements(HashSet<CityElement> i_elements,
                                             HashSet<Road> i_roads,
                                             Dictionary<CityElementPair, float> i_affinities,
                                             HeuristicCalculator i_heuristic)
    {
        ElementPositionSelector selector = new ElementPositionSelector(i_affinities, i_heuristic);
        HashSet<GameObject> instances = new HashSet<GameObject>();
        while (i_elements.Select(e => ElementCanBeAdded(e)).Any(b => b == true)) //while exists an element that can be added
        {
            bool placedElementThisIteration = false;
            foreach (CityElement element in i_elements)
            {
                if (ElementCanBeAdded(element))
                {
                    List<PositionAndRotation> candidates = FindPositionCandidates(element, i_roads);

                    if (candidates.Count != 0)
                    {
                        PositionAndRotation bestCandidate = FindBestCandidate(element, candidates, selector);
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

    private bool ElementCanBeAdded(CityElement i_element)
    {
        bool instLimNotReached = InstanceLimitNotReached(i_element);
        bool addedInhNotOverLimit = m_currentInhabitants + i_element.inhabitants <= m_targetInhabitants;
        return instLimNotReached && addedInhNotOverLimit;
    }

    private bool InstanceLimitNotReached(CityElement i_element)
    {
        if (i_element.instanceLimit == null)
        {
            return true;
        }
        if (m_instanceCount.TryGetValue(i_element, out uint currentInstances))
        {
            return currentInstances < i_element.instanceLimit;
        }
        return true;
    }

    private List<PositionAndRotation> FindPositionCandidates(CityElement i_element, HashSet<Road> i_roads)
    {
        List<PositionAndRotation> candidates = new List<PositionAndRotation>();
        foreach (Road road in i_roads)
        {
            foreach (LeftRight side in System.Enum.GetValues(typeof(LeftRight)))
            {
                float delta = 0f;
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
                            rotation = rotation
                        };
                        candidates.Add(candidate);
                    }
                    delta += 0.1f;
                }
            }
        }
        return candidates;
    }

    private PositionAndRotation FindBestCandidate(CityElement i_element,
                                                  List<PositionAndRotation> i_candidates,
                                                  ElementPositionSelector i_selector)
    {
        int indexOfBest = -1;
        int index = 0;
        float heuristicOfBest = 0; //necessary initialization, but ignored the first iteration
        foreach (PositionAndRotation candidate in i_candidates)
        {
            Vector3 position = candidate.position;
            float heuristicOfCandidate = i_selector.ComputeHeuristic(i_element, position, m_placedElements);
            if (indexOfBest == -1 || heuristicOfBest < heuristicOfCandidate)
            {
                heuristicOfBest = heuristicOfCandidate;
                indexOfBest = index;
            }
            ++index;
        }
        return i_candidates[indexOfBest];
    }

    private GameObject PlaceElement(CityElement i_element, PositionAndRotation i_positionAndRotation)
    {
        GameObject prefab = i_element.prefab;
        GameObject instance = GameObject.Instantiate(prefab, m_area.center, prefab.transform.rotation);

        instance.transform.SetPositionAndRotation(i_positionAndRotation.position, i_positionAndRotation.rotation);

        m_currentInhabitants += i_element.inhabitants;
        IncreaseInstanceCount(i_element);

        CreateTemporaryCollider(instance, i_element);

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

    private void CreateTemporaryCollider(GameObject i_instance, CityElement i_elementCreatedFrom)
    {
        GameObject tmpObject = new GameObject();
        
        tmpObject.transform.SetPositionAndRotation(i_instance.transform.position, i_instance.transform.rotation);
        
        tmpObject.layer = LayerMask.NameToLayer("CityGenerator_TMPObjects");
        
        CityElementInstance elementInstance = tmpObject.AddComponent<CityElementInstance>();
        elementInstance.m_element = i_elementCreatedFrom;
        m_placedElements.Add(elementInstance);

        Bounds boundingBox = i_elementCreatedFrom.boundingBox;
        BoxCollider collider = tmpObject.AddComponent<BoxCollider>();
        collider.center = boundingBox.center;
        collider.size = boundingBox.size;
    }

    public class CityElementInstance : MonoBehaviour
    {
        public CityElement m_element;
    }
}
