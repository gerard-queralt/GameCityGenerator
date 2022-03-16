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
        Vector3 currentPosition = new Vector3(area.max.x, 0f, area.min.z);
        uint targetInhabitants = m_params.targetInhabitants;
        uint currentInhabitants = 0;
        while(currentInhabitants < targetInhabitants)
        {
            foreach (CityElement element in elements)
            {
                GameObject prefab = element.prefab;
                GameObject instance = Instantiate(prefab, area.center, prefab.transform.rotation);
                Bounds sizeOfElement = instance.GetComponent<MeshRenderer>().bounds; //does not work
                currentPosition += sizeOfElement.extents;
                instance.transform.position = currentPosition;
                currentInhabitants += element.inhabitants;
            }
        }
    }
}
