using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunayVoronoi;
using System.Linq;

public class RoadBuilder
{
    private PositionCalculator m_positionCalculator;
    private Bounds m_districtArea;
    private DelaunayTriangulator m_triangulator = new DelaunayTriangulator();

    public RoadBuilder(PositionCalculator i_positionCalculator, Bounds i_districtArea)
    {
        m_positionCalculator = i_positionCalculator;
        m_districtArea = i_districtArea;
    }

    public HashSet<Road> BuildRoads(float i_roadWidthMin, float i_roadWidthMax, uint i_nCrossroads)
    {
        IEnumerable<Point> points = GenerateCrossroads(i_nCrossroads);
        IEnumerable<Triangle> triangulation = GenerateTriangles(points);
        HashSet<Edge> edges = new HashSet<Edge>();
        foreach (Triangle triangle in triangulation)
        {
            Edge e01 = new Edge(triangle.Vertices[0], triangle.Vertices[1]);
            if (!edges.Contains(e01) && !edges.Contains(e01.Reverse()))
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
        HashSet<Road> roads = new HashSet<Road>();
        foreach (Edge edge in edges)
        {
            Road road = new Road(edge);
            road.width = Random.Range(i_roadWidthMin, i_roadWidthMax);
            roads.Add(road);
        }

        return roads;
    }

    public HashSet<GameObject> InstantiateRoads(HashSet<Road> i_roads, Texture i_roadTexture)
    {
        HashSet<GameObject> instances = new HashSet<GameObject>();
        foreach (Road road in i_roads)
        {
            GameObject instance = CreatePlane(road, i_roadTexture);
            instances.Add(instance);
        }
        return instances;
    }

    private GameObject CreatePlane(Road i_road, Texture i_roadTexture)
    {
        Vector2 start = i_road.start.AsVector2;
        Vector2 end = i_road.end.AsVector2;
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        Vector2 center = new Vector2(start.x + end.x, start.y + end.y) / 2f;
        float planeY = m_positionCalculator.FindGroundCoordinate(new Vector3(center.x, m_districtArea.max.y, center.y));
        plane.transform.position = new Vector3(center.x, planeY + 0.01f, center.y);
        
        Bounds planeBounds = plane.GetComponent<MeshRenderer>().bounds;
        float scaleX = Vector2.Distance(start, end) / planeBounds.size.x;
        float width = i_road.width;
        plane.transform.localScale = new Vector3(scaleX, 1f, width / planeBounds.size.z);
        
        plane.transform.rotation = i_road.rotation;

        Renderer planeRenderer = plane.GetComponent<Renderer>();

        Material tmpMaterial = new Material(planeRenderer.sharedMaterial);
        tmpMaterial.mainTexture = i_roadTexture;
        planeRenderer.sharedMaterial = tmpMaterial;

        MeshFilter meshFilter = plane.GetComponent<MeshFilter>();
        Mesh planeMesh = GameObject.Instantiate(meshFilter.sharedMesh);
        meshFilter.mesh = planeMesh;
        
        Vector3[] vertices = planeMesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            Vector3 transformedVertex = plane.transform.TransformPoint(vertices[i]) / 5f;
            uvs[i] = new Vector2(transformedVertex.x, transformedVertex.z);
        }
        planeMesh.uv = uvs;

        CreateTemporaryCollider(plane);

        return plane;
    }

    private void CreateTemporaryCollider(GameObject i_instance)
    {
        GameObject tmpObject = new GameObject();

        tmpObject.transform.SetPositionAndRotation(i_instance.transform.position, i_instance.transform.rotation);

        tmpObject.layer = LayerMask.NameToLayer("CityGenerator_TMPObjects");

        Bounds boundingBox = i_instance.GetComponent<MeshCollider>().bounds;
        Vector3 bbSize = boundingBox.size; //Default bounding box of the plane
        bbSize = Vector3.Scale(bbSize, i_instance.transform.localScale); //Scaled to the proportions of the plane
        bbSize.z *= 0.25f; //Reduced the Z axis (along which the elements will be placed) to allow some overlap
        BoxCollider collider = tmpObject.AddComponent<BoxCollider>();
        collider.center = boundingBox.center;
        collider.size = bbSize;
    }

    private IEnumerable<Point> GenerateCrossroads(uint i_nCrossroads)
    {
        List<Point> points = m_triangulator.GeneratePoints((int)i_nCrossroads * 100,
                                                           m_districtArea.min.x,
                                                           m_districtArea.min.z,
                                                           m_districtArea.max.x,
                                                           m_districtArea.max.z).ToList();
        float diagonalDistance = Vector2.Distance(new Vector2(m_districtArea.min.x, m_districtArea.min.z),
                                                  new Vector2(m_districtArea.max.x, m_districtArea.max.z));
        float radius = (diagonalDistance / 2f) / 100f;
        while (i_nCrossroads < points.Count)
        {
            points = FilterPoints(points, radius);
            radius += 0.1f;
        }
        return points;
    }

    private List<Point> FilterPoints(List<Point> i_points, float i_radius)
    {
        System.Random random = new System.Random();
        List<Point> randomOrder = i_points.OrderBy(item => random.Next()).ToList();
        List<Point> pointsToDelete = new List<Point>();
        foreach (Point pointA in randomOrder)
        {
            if (!pointsToDelete.Contains(pointA))
            {
                foreach (Point pointB in randomOrder)
                {
                    bool samePoint = pointA.crossroad.AsVector2.Equals(pointB.crossroad.AsVector2);
                    if (!samePoint && !pointsToDelete.Contains(pointB))
                    {
                        float distanceAB = Vector2.Distance(pointA.crossroad.AsVector2, pointB.crossroad.AsVector2);
                        if (distanceAB <= i_radius)
                        {
                            pointsToDelete.Add(pointB);
                        }
                    }
                }
            }
        }
        return i_points.Except(pointsToDelete).ToList();
    }

    private IEnumerable<Triangle> GenerateTriangles(IEnumerable<Point> i_points)
    {
        IEnumerable<Triangle> defaultTriangulation = m_triangulator.BowyerWatson(i_points);
        IEnumerable<Triangle> triangulation = defaultTriangulation.Where(triangle => 
                                                                         triangle.Vertices.All(point => 
                                                                                               i_points.Contains(point)));
        return triangulation;
    }
}
