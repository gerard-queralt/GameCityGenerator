using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunayVoronoi;

public class Road
{
    private Crossroad m_start;
    private Crossroad m_end;
    private int m_directionXAxis;
    private GameObject m_plane;

    private float m_deltaLeftX = 0f;
    private float m_deltaRightX = 0f;

    public struct PositionAndRotation
    {
        public Vector3 position;
        public Quaternion rotation;

        public PositionAndRotation(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    public enum LeftRight
    {
        Left,
        Right
    }

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

    public GameObject plane
    {
        get
        {
            return m_plane;
        }
    }

    public PositionAndRotation left
    {
        get
        {
            Vector3 position = new Vector3(m_start.x + m_deltaLeftX * m_directionXAxis, m_plane.transform.position.y, m_start.z);
            Quaternion rotation = m_plane.transform.rotation;
            return new PositionAndRotation(position, rotation);
        }
    }

    public PositionAndRotation right
    {
        get
        {
            Vector3 position = new Vector3(m_start.x + m_deltaRightX * m_directionXAxis, m_plane.transform.position.y, m_start.z);
            Quaternion rotation = m_plane.transform.rotation * Quaternion.AngleAxis(180f, Vector3.up);
            return new PositionAndRotation(position, rotation);
        }
    }

    public Road(Edge edge)
    {
        m_start = edge.Point1.crossroad;
        m_end = edge.Point2.crossroad;
        if (m_start.x < m_end.x)
        {
            m_directionXAxis = 1;
        }
        else
        {
            m_directionXAxis = -1;
        }
    }

    public void CreatePlane(Texture roadTexture, Bounds cityArea)
    {
        Vector2 begin = m_start.AsVector2;
        Vector2 end = m_end.AsVector2;
        m_plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        Vector2 center = new Vector2(begin.x + end.x, begin.y + end.y) / 2f;
        float planeY = PositionCalculator.FindGroundCoordinate(new Vector3(center.x, cityArea.max.y, center.y), cityArea.min.y);
        m_plane.transform.position = new Vector3(center.x, planeY + 0.01f, center.y);
        Bounds planeBounds = m_plane.GetComponent<MeshRenderer>().bounds;
        float scaleX = Vector2.Distance(begin, end) / planeBounds.size.x;
        m_plane.transform.localScale = new Vector3(scaleX, 1f, 0.5f);
        float angle = Vector2.Angle(begin - end, Vector2.right);
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

    public void IncreaseDelta(LeftRight leftRight, Bounds i_bounds)
    {
        float delta = i_bounds.size.x;
        if (leftRight == LeftRight.Left)
        {
            m_deltaLeftX += delta;
        }
        else
        {
            m_deltaRightX += delta;
        }
    }
}
