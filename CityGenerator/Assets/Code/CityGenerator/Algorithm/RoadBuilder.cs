using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunayVoronoi;
using System.Linq;

public class RoadBuilder
{
    private static DelaunayTriangulator m_triangulator;

    public static HashSet<Road> BuildRoads(Texture i_roadTexture, float i_roadWidthMin, float i_roadWidthMax, uint i_nCrossroads, Bounds i_cityArea)
    {
        m_triangulator = new DelaunayTriangulator();
        IEnumerable<Point> points = GenerateCrossroads(i_nCrossroads, i_cityArea);
        //IEnumerable<Point> points = m_triangulator.GeneratePoints((int)i_nCrossroads, i_cityArea.min.x, i_cityArea.min.z, i_cityArea.max.x, i_cityArea.max.z);
        IEnumerable<Triangle> triangulation = m_triangulator.BowyerWatson(points);
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
            float width = Random.Range(i_roadWidthMin, i_roadWidthMax);
            road.CreatePlane(i_roadTexture, width, i_cityArea);
            roads.Add(road);
        }

        return roads;
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
}
