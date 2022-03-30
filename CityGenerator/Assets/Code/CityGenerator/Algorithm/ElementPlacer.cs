using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementPlacer
{
    private static Dictionary<CityElement, uint> m_instanceCount = new Dictionary<CityElement, uint>();

    private static Bounds m_area;
    private static uint m_targetInhabitants;
    private static uint m_currentInhabitants;
    private static Vector3 m_currentPosition; //TMP

    public static HashSet<GameObject> PlaceElements(HashSet<CityElement> i_elements, HashSet<Road> i_roads, Bounds i_area, uint i_targetInhabitants)
    {
        m_area = i_area;
        m_targetInhabitants = i_targetInhabitants;
        m_currentInhabitants = 0;

        HashSet<GameObject> instances = new HashSet<GameObject>();
        
        m_currentPosition = new Vector3(m_area.min.x, 0f, m_area.min.z);
        while (m_currentInhabitants < m_targetInhabitants)
        {
            foreach (CityElement element in i_elements)
            {
                GameObject instance = PlaceElement(element);

                instances.Add(instance);
            }
        }
        return instances;
    }

    private static GameObject PlaceElement(CityElement element)
    {
        GameObject prefab = element.prefab;
        GameObject instance = GameObject.Instantiate(prefab, m_area.center, prefab.transform.rotation);
        Bounds boundsOfElement = element.boundingBox;
        m_currentPosition.x += boundsOfElement.extents.x;
        if (m_currentPosition.x > m_area.max.x)
        {
            m_currentPosition.x = m_area.min.x + boundsOfElement.extents.x;
            m_currentPosition.z += boundsOfElement.extents.z;
        }
        Vector3 raycastStartPosition = new Vector3(m_currentPosition.x, m_area.max.y, m_currentPosition.z + boundsOfElement.extents.z);
        float groundCoord = PositionCalculator.FindGroundCoordinate(raycastStartPosition, m_area.min.y);
        Vector3 newPosition = new Vector3(m_currentPosition.x, groundCoord, m_currentPosition.z + boundsOfElement.extents.z);
        Quaternion newRotation = prefab.transform.rotation * Quaternion.AngleAxis(180f, Vector3.up);
        instance.transform.SetPositionAndRotation(newPosition, newRotation);
        m_currentInhabitants += element.inhabitants;
        return instance;
    }
}
