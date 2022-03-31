using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Road road = new List<Road>(i_roads)[0]; //TMP
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
        Road.PositionAndRotation positionAndRotation = road.left;
        Vector3 centerOfRoad = positionAndRotation.position;
        Quaternion rotation = positionAndRotation.rotation;
        Vector3 position = centerOfRoad + PositionAfterRotationInXZPlane(new Vector3(0f, 0f, 5f), rotation.y * Mathf.Deg2Rad);
        instance.transform.SetPositionAndRotation(position, rotation);
        m_currentInhabitants += element.inhabitants;
        return instance;
    }

    private static Vector3 PositionAfterRotationInXZPlane(Vector3 position, float angle)
    {
        float xPrime = position.x * Mathf.Cos(angle) - position.z * Mathf.Sin(angle);
        float zPrime = position.z * Mathf.Cos(angle) + position.x * Mathf.Sin(angle);
        return new Vector3(xPrime, position.y, zPrime);
    }
}
