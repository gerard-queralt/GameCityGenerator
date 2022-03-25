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
                GameObject instance = Instantiate(prefab, area.center, prefab.transform.rotation /*tmp*/ * Quaternion.AngleAxis(180f, Vector3.up), cityParent.transform);
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
        //attempt to paint paths
        Texture pathTexture = m_params.pathTexture;
        DelaunayTriangulator triangulator = new DelaunayTriangulator();
        IEnumerable<Triangle> triangulation = triangulator.BowyerWatson(triangulator.GeneratePoints(6, area.min.x, area.min.z ,area.max.x, area.max.z));
        HashSet<Edge> edges = new HashSet<Edge>();
        foreach(Triangle triangle in triangulation)
        {
            Edge e01 = new Edge(triangle.Vertices[0], triangle.Vertices[1]);
            if(!edges.Contains(e01) && !edges.Contains(e01.Reverse()))
            {
                edges.Add(e01);
            }
            Edge e12 = new Edge(triangle.Vertices[1], triangle.Vertices[2]);
            if (!edges.Contains(e12) && !edges.Contains(e12.Reverse()))
            {
                edges.Add(e12);
            }
            Edge e20 = new Edge(triangle.Vertices[2], triangle.Vertices[0]);
            if (!edges.Contains(e20) && !edges.Contains(e20.Reverse()))
            {
                edges.Add(e20);
            }
        }
        foreach(Edge edge in edges)
        {
            Vector2 begin = new Vector2((float)edge.Point1.X, (float)edge.Point1.Y);
            Vector2 end = new Vector2((float)edge.Point2.X, (float)edge.Point2.Y);
            GameObject pathPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);

            Vector2 center = new Vector2(begin.x + end.x, begin.y + end.y) / 2f;
            float planeY = ElementPositioner.FindGroundCoordinate(new Vector3(center.x, area.max.y, center.y), area.min.y);
            pathPlane.transform.position = new Vector3(center.x, planeY+0.01f, center.y);
            Bounds planeBounds = pathPlane.GetComponent<MeshRenderer>().bounds;
            float scaleZ = Vector2.Distance(begin, end) / planeBounds.size.z;
            pathPlane.transform.localScale = new Vector3(0.5f, 1f, scaleZ);
            float angle = Vector2.Angle(begin - end, Vector2.up);
            pathPlane.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            
            Renderer planeRenderer = pathPlane.GetComponent<Renderer>();
            planeRenderer.material.mainTexture = pathTexture;
            Mesh planeMesh = pathPlane.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = planeMesh.vertices;
            Vector2[] uvs = new Vector2[vertices.Length];

            for (int i = 0; i < uvs.Length; i++)
            {
                Vector3 transformedVertex = pathPlane.transform.TransformPoint(vertices[i]) / 5f;
                uvs[i] = new Vector2(transformedVertex.x, transformedVertex.z);
            }
            planeMesh.uv = uvs;
        }
    }
}
