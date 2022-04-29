using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class AlgorithmMain
{
    CityGeneratorParameters m_params;

    private string m_tmpObjectsLayerName = "CityGenerator_TMPObjects";

    public AlgorithmMain(CityGeneratorParameters i_params)
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

        Bounds area = m_params.area;

        PositionCalculator positionCalculator = new PositionCalculator(area);
        ElementPlacer elementPlacer = new ElementPlacer(positionCalculator, area, m_params.targetInhabitants);
        RoadBuilder roadBuilder = new RoadBuilder(positionCalculator);

        HashSet<Road> roads = roadBuilder.BuildRoads(m_params.roadWidthMin, m_params.roadWidthMax, m_params.nCrossroads, area);

        HashSet<GameObject> roadInstances = roadBuilder.InstantiateRoads(roads, m_params.roadTexture, area);
        HashSet<GameObject> elementInstances = elementPlacer.PlaceElements(m_params.cityElements, roads, m_params.affinities);

        CreateHierarchy(elementInstances, roadInstances);

        if (layerCreated)
        {
            DestroyAllTmpObjects();
            LayerCreator.DeleteLayer(m_tmpObjectsLayerName);
        }
    }

    private void CreateHierarchy(HashSet<GameObject> i_elements, HashSet<GameObject> i_roads)
    {
        Transform cityParent = new GameObject("City").transform;
        Transform elementParent = new GameObject("Elements").transform;
        Transform roadParent = new GameObject("Roads").transform;

        elementParent.SetParent(cityParent);
        roadParent.SetParent(cityParent);

        foreach(GameObject element in i_elements)
        {
            element.transform.SetParent(elementParent);
        }

        foreach(GameObject road in i_roads)
        {
            road.transform.SetParent(roadParent);
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
