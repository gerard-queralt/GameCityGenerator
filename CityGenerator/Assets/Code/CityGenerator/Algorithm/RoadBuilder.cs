using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunayVoronoi;

public class RoadBuilder
{
    public static HashSet<Road> BuildRoads(Texture i_roadTexture, float i_roadWidthMin, float i_roadWidthMax, uint i_nCrossroads, Bounds i_cityArea)
    {
        DelaunayTriangulator triangulator = new DelaunayTriangulator();
        IEnumerable<Point> points = triangulator.GeneratePoints((int)i_nCrossroads, i_cityArea.min.x, i_cityArea.min.z, i_cityArea.max.x, i_cityArea.max.z);
        IEnumerable<Triangle> triangulation = triangulator.BowyerWatson(points);
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
}
