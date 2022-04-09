using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunayVoronoi;

[RequireComponent(typeof(CityGeneratorParameters))]
public class AlgorithmMain : MonoBehaviour
{
    CityGeneratorParameters m_params;

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
        Bounds area = m_params.area;

        //HashSet<Road> roads = RoadBuilder.BuildRoads(m_params.roadTexture, m_params.roadWidthMin, m_params.roadWidthMax, m_params.nCrossroads, area);
        Road road = new Road(new Crossroad(0, 0), new Crossroad(100, 100));
        Road road1 = new Road(new Crossroad(0, 0), new Crossroad(-100, 100));
        
        //debug
        HashSet<Road> roads = new HashSet<Road>();
        road.CreatePlane(m_params.roadTexture, 10f, area);
        road1.CreatePlane(m_params.roadTexture, 10f, area);
        roads.Add(road);
        roads.Add(road1);
        
        HashSet<GameObject> elementInstances = ElementPlacer.PlaceElements(m_params.cityElements, roads, area, m_params.targetInhabitants);

        CreateHierarchy(elementInstances, roads);
    }

    private void CreateHierarchy(HashSet<GameObject> i_elements, HashSet<Road> i_roads)
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

        foreach(Road road in i_roads)
        {
            road.plane.transform.SetParent(roadParent);
        }
    }
}
