using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[RequireComponent(typeof(CityGeneratorParameters))]
public class AlgorithmMain : MonoBehaviour
{
    CityGeneratorParameters m_params;

    private string m_tmpObjectsLayerName = "CityGenerator_TMPObjects";

    private void Awake()
    {
        m_params = GetComponent<CityGeneratorParameters>();
        Debug.Assert(m_params != null, "CityGeneratorParameters not found");
    }

    private void Start()
    {
        Run(); //tmp
    }

    public void Run()
    {
        bool layerCreated = LayerCreator.CreateLayer(m_tmpObjectsLayerName);
        if (!layerCreated)
        {
            Debug.Log("Temporary layer could not be created");
        }

        Bounds area = m_params.area;

        //HashSet<Road> roads = RoadBuilder.BuildRoads(m_params.roadTexture, m_params.roadWidthMin, m_params.roadWidthMax, m_params.nCrossroads, area);

        //debug
        Road road = new Road(new Crossroad(0, 0), new Crossroad(100, 100));
        road.width = 10f;
        Road road1 = new Road(new Crossroad(0, 0), new Crossroad(-100, 100));
        road1.width = 10f;
        HashSet<Road> roads = new HashSet<Road>();
        roads.Add(road);
        roads.Add(road1);

        PositionCalculator positionCalculator = new PositionCalculator();
        ElementPlacer elementPlacer = new ElementPlacer(positionCalculator, area, m_params.targetInhabitants);
        RoadBuilder roadBuilder = new RoadBuilder(positionCalculator);

        HashSet<GameObject> roadInstances = roadBuilder.InstantiateRoads(roads, m_params.roadTexture, area);
        HashSet<GameObject> elementInstances = elementPlacer.PlaceElements(m_params.cityElements, roads);

        CreateHierarchy(elementInstances, roadInstances);

        if (layerCreated)
        {
            //DestroyAllTmpObjects();
            //LayerCreator.DeleteLayer(m_tmpObjectsLayerName);
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
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int layer = LayerMask.NameToLayer(m_tmpObjectsLayerName);
        foreach (GameObject gameObject in allObjects)
        {
            if(gameObject.layer == layer)
            {
                //Debug.Log(LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer)) + " original: " + LayerMask.GetMask(m_tmpObjectsLayerName));
                Destroy(gameObject);
            }
        }
    }
}
