using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunayVoronoi;

public class Road
{
    private Crossroad m_start;
    private Crossroad m_end;
    private GameObject m_plane;

    public Crossroad start
    {
        get
        {
            return m_start;
        }
    }

    public Crossroad end
    {
        get
        {
            return m_end;
        }
    }

    public Road(Edge edge)
    {
        m_start = edge.Point1.crossroad;
        m_end = edge.Point2.crossroad;
    }

    public void CreatePlane(Texture roadTexture, Bounds cityArea)
    {
        Vector2 begin = new Vector2(m_start.x, m_start.z);
        Vector2 end = new Vector2(m_end.x, m_end.z);
        m_plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        Vector2 center = new Vector2(begin.x + end.x, begin.y + end.y) / 2f;
        float planeY = ElementPositioner.FindGroundCoordinate(new Vector3(center.x, cityArea.max.y, center.y), cityArea.min.y);
        m_plane.transform.position = new Vector3(center.x, planeY + 0.01f, center.y);
        Bounds planeBounds = m_plane.GetComponent<MeshRenderer>().bounds;
        float scaleZ = Vector2.Distance(begin, end) / planeBounds.size.z;
        m_plane.transform.localScale = new Vector3(0.5f, 1f, scaleZ);
        float angle = Vector2.Angle(begin - end, Vector2.up);
        m_plane.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        Renderer planeRenderer = m_plane.GetComponent<Renderer>();
        planeRenderer.material.mainTexture = roadTexture;
        Mesh planeMesh = m_plane.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = planeMesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            Vector3 transformedVertex = m_plane.transform.TransformPoint(vertices[i]) / 5f;
            uvs[i] = new Vector2(transformedVertex.x, transformedVertex.z);
        }
        planeMesh.uv = uvs;
    }
}
