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
        GameObject cityParent = new GameObject("City");

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
                GameObject instance = Instantiate(prefab, area.center, prefab.transform.rotation, cityParent.transform);
                Bounds boundsOfElement = ElementPositioner.ComputeBoundsOfGameObject(instance);
                currentPosition.x += boundsOfElement.extents.x;
                if(currentPosition.x > area.max.x)
                {
                    currentPosition.x = area.min.x + boundsOfElement.extents.x;
                    currentPosition.z += boundsOfElement.extents.z;
                }
                Vector3 raycastStartPosition = new Vector3(currentPosition.x, area.max.y, currentPosition.z + boundsOfElement.extents.z);
                float groundCoord = ElementPositioner.FindGroundCoordinate(raycastStartPosition, area.min.y);
                instance.transform.position = new Vector3(currentPosition.x, groundCoord, currentPosition.z + boundsOfElement.extents.z);
                currentInhabitants += element.inhabitants;
            }
        }
    }
}
