using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunayVoronoi;
using System.Linq;

public class RoadBuilder
{
    private static DelaunayTriangulator m_triangulator;

    public static HashSet<Road> BuildRoads(float i_roadWidthMin, float i_roadWidthMax, uint i_nCrossroads, Bounds i_cityArea)
    {
        m_triangulator = new DelaunayTriangulator();
        IEnumerable<Point> points = GenerateCrossroads(i_nCrossroads, i_cityArea);
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

    public static HashSet<GameObject> InstantiateRoads(HashSet<Road> i_roads, Texture i_roadTexture, Bounds i_cityArea)
    {
        HashSet<GameObject> instances = new HashSet<GameObject>();
        foreach (Road road in i_roads)
        {
            GameObject instance = CreatePlane(road, i_roadTexture, i_cityArea);
            instances.Add(instance);
        }
        return instances;
    }

    private static GameObject CreatePlane(Road i_road, Texture i_roadTexture, Bounds i_cityArea)
    {
        Vector2 start = i_road.start.AsVector2;
        Vector2 end = i_road.end.AsVector2;
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        Vector2 center = new Vector2(start.x + end.x, start.y + end.y) / 2f;
        float planeY = PositionCalculator.FindGroundCoordinate(new Vector3(center.x, i_cityArea.max.y, center.y), i_cityArea.min.y);
        plane.transform.position = new Vector3(center.x, planeY + 0.01f, center.y);
        
        Bounds planeBounds = plane.GetComponent<MeshRenderer>().bounds;
        float scaleX = Vector2.Distance(start, end) / planeBounds.size.x;
        float width = i_road.width;
        plane.transform.localScale = new Vector3(scaleX, 1f, width / planeBounds.size.z);
        
        plane.transform.rotation = i_road.rotation;

        Renderer planeRenderer = plane.GetComponent<Renderer>();
        planeRenderer.material.mainTexture = i_roadTexture;
        Mesh planeMesh = plane.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = planeMesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            Vector3 transformedVertex = plane.transform.TransformPoint(vertices[i]) / 5f;
            uvs[i] = new Vector2(transformedVertex.x, transformedVertex.z);
        }
        planeMesh.uv = uvs;

        return plane;
    }

    private static IEnumerable<Point> GenerateCrossroads(uint i_nCrossroads, Bounds i_cityArea)
    {
        List<Point> points = m_triangulator.GeneratePoints((int)i_nCrossroads * 100,
                                                           i_cityArea.min.x,
                                                           i_cityArea.min.z,
                                                           i_cityArea.max.x,
                                                           i_cityArea.max.z).ToList();
        float diagonalDistance = Vector2.Distance(new Vector2(i_cityArea.min.x, i_cityArea.min.z),
                                                  new Vector2(i_cityArea.max.x, i_cityArea.max.z));
        float radius = (diagonalDistance / 2f) / 100f;
        while (i_nCrossroads < points.Count)
        {
            points = FilterPoints(points, radius);
            radius += 0.1f;
        }
        return points;
    }

    private static List<Point> FilterPoints(List<Point> i_points, float i_radius)
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

    private static IEnumerable<Triangle> GenerateTriangles(IEnumerable<Point> i_points)
    {
        IEnumerable<Triangle> defaultTriangulation = m_triangulator.BowyerWatson(i_points);
        IEnumerable<Triangle> triangulation = defaultTriangulation.Where(triangle => 
                                                                         triangle.Vertices.All(point => 
                                                                                               i_points.Contains(point)));
        return triangulation;
    }
}
