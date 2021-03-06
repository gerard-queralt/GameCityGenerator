using System.Collections.Generic;
using UnityEngine;

public class AlgorithmMain
{
    private City m_params;

    private string m_tmpObjectsLayerName = "CityGenerator_TMPObjects";

    public AlgorithmMain(City i_params)
    {
        m_params = i_params;
    }

    public void Run()
    {
        bool layerCreated = LayerCreator.CreateLayer(m_tmpObjectsLayerName);
        if (!layerCreated)
        {
            Debug.Log("Temporary layer could not be created");
        }

        HashSet<District> districts = m_params.districts;
        HashSet<GameObject> districtObjects = new HashSet<GameObject>();
        foreach (District district in districts)
        {
            Bounds area = district.area;

            PositionCalculator positionCalculator = new PositionCalculator(area);
            ElementPlacer elementPlacer = new ElementPlacer(positionCalculator, area, district.targetInhabitants);
            RoadBuilder roadBuilder = new RoadBuilder(positionCalculator, area);

            HashSet<Road> roads = roadBuilder.BuildRoads(district.roadWidthMin, district.roadWidthMax, district.nCrossroads);

            HashSet<GameObject> roadInstances = roadBuilder.InstantiateRoads(roads, district.roadTexture);
            HashSet<GameObject> elementInstances = elementPlacer.PlaceElements(district.cityElements, roads, m_params.affinities, m_params.heuristic);

            GameObject districtObject = CreateDistrictHierarchy(elementInstances, roadInstances);
            districtObjects.Add(districtObject);
        }

        CreateCityHierarchy(districtObjects);

        if (layerCreated)
        {
            DestroyAllTmpObjects();
            LayerCreator.DeleteLayer(m_tmpObjectsLayerName);
        }
    }

    private GameObject CreateDistrictHierarchy(HashSet<GameObject> i_elements, HashSet<GameObject> i_roads)
    {
        Transform districtParent = new GameObject("District").transform;
        Transform elementParent = new GameObject("Elements").transform;
        Transform roadParent = new GameObject("Roads").transform;

        elementParent.SetParent(districtParent);
        roadParent.SetParent(districtParent);

        foreach (GameObject element in i_elements)
        {
            element.transform.SetParent(elementParent);
        }

        foreach (GameObject road in i_roads)
        {
            road.transform.SetParent(roadParent);
        }

        return districtParent.gameObject;
    }

    private void CreateCityHierarchy(HashSet<GameObject> i_districts)
    {
        Transform cityParent = new GameObject("City").transform;
        cityParent.SetParent(m_params.gameObject.transform);
        foreach (GameObject district in i_districts)
        {
            district.transform.SetParent(cityParent);
        }
    }

    private void DestroyAllTmpObjects()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int layer = LayerMask.NameToLayer(m_tmpObjectsLayerName);
        foreach (GameObject gameObject in allObjects)
        {
            if(gameObject.layer == layer)
            {
                GameObject.DestroyImmediate(gameObject);
            }
        }
    }
}
