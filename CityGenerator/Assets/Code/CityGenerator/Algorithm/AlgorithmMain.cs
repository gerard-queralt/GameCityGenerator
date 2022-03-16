using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        HashSet<CityElement> elements = m_params.cityElements;
        Bounds area = m_params.area;
        Vector3 currentPosition = new Vector3(area.min.x, 0f, area.min.z);
        uint targetInhabitants = m_params.targetInhabitants;
        uint currentInhabitants = 0;
        while(currentInhabitants < targetInhabitants)
        {
            foreach (CityElement element in elements)
            {
                GameObject prefab = element.prefab;
                GameObject instance = Instantiate(prefab, area.center, prefab.transform.rotation);
                Bounds boundsOfElement = ComputeBoundsOfGameObject(instance);
                currentPosition.x += boundsOfElement.extents.x;
                if(currentPosition.x > area.max.x)
                {
                    currentPosition.x = area.min.x + boundsOfElement.extents.x;
                    currentPosition.z += boundsOfElement.extents.z;
                }
                instance.transform.position = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + boundsOfElement.extents.z);
                currentInhabitants += element.inhabitants;
            }
        }
    }

    private Bounds ComputeBoundsOfGameObject(GameObject gameObject)
    {
        Bounds boundsOfElement = new Bounds(gameObject.transform.position, Vector3.one);
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer renderer in renderers)
        {
            Bounds rendererBounds = renderer.bounds;
            boundsOfElement.Encapsulate(rendererBounds);
        }
        return boundsOfElement;
    }
}
