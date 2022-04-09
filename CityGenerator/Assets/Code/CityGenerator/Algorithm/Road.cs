using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunayVoronoi;

public class Road
{
    private GameObject m_plane;
    private Crossroad m_start;
    private Crossroad m_end;
    private float m_width;
    private float m_deltaLeftX = 0f;
    private float m_deltaRightX = 0f;

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

    public float width
    {
        get
        {
            return m_width;
        }
    }

    public Quaternion rotation
    {
        get
        {
            return m_plane.transform.rotation;
        }
    }

    public Vector2 direction
    {
        get
        {
            return (m_end.AsVector2 - m_start.AsVector2).normalized;
        }
    }

    public Vector2 perpendicular
    {
        get
        {
            return Vector2.Perpendicular(direction).normalized;
        }
    }

    public float height
    {
        get
        {
            return m_plane.transform.position.y;
        }
    }

    public Road(Crossroad start, Crossroad end)
    {
        m_start = start;
        m_end = end;
    }

    public Road(Edge i_edge)
    {
        m_start = i_edge.Point1.crossroad;
        m_end = i_edge.Point2.crossroad;
    }

    public void CreatePlane(Texture i_roadTexture, float i_width, Bounds i_cityArea)
    {
        Vector2 begin = m_start.AsVector2;
        Vector2 end = m_end.AsVector2;
        m_plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        m_width = i_width;

        Vector2 center = new Vector2(begin.x + end.x, begin.y + end.y) / 2f;
        float planeY = PositionCalculator.FindGroundCoordinate(new Vector3(center.x, i_cityArea.max.y, center.y), i_cityArea.min.y);
        m_plane.transform.position = new Vector3(center.x, planeY + 0.01f, center.y);
        Bounds planeBounds = m_plane.GetComponent<MeshRenderer>().bounds;
        float scaleX = Vector2.Distance(begin, end) / planeBounds.size.x;
        m_plane.transform.localScale = new Vector3(scaleX, 1f, m_width / planeBounds.size.z);
        float angle = Vector2.Angle(begin - end, Vector2.right);
        m_plane.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        Renderer planeRenderer = m_plane.GetComponent<Renderer>();
        planeRenderer.material.mainTexture = i_roadTexture;
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

    public float GetDelta(LeftRight i_leftRight)
    {
        if(i_leftRight == LeftRight.Left)
        {
            return m_deltaLeftX;
        }
        return m_deltaRightX;
    }

    public void IncreaseDelta(LeftRight i_leftRight, Bounds i_bounds)
    {
        float delta = i_bounds.size.x;
        if (i_leftRight == LeftRight.Left)
        {
            m_deltaLeftX += delta;
        }
        else
        {
            m_deltaRightX += delta;
        }
    }

    public bool CanBePlaced(LeftRight i_leftRight, Bounds i_bounds)
    {
        float sizeOfElement = i_bounds.size.x;
        float delta = GetDelta(i_leftRight) + sizeOfElement;
        float width = this.width;
        if (i_leftRight == LeftRight.Right)
        {
            width *= -1;
        }
        Vector2 maxPositionOfElement = start.AsVector2 + direction * delta + perpendicular * width;
        return Vector2.Dot((end.AsVector2 - start.AsVector2).normalized, (maxPositionOfElement - end.AsVector2).normalized) < 0f &&
               Vector2.Dot((start.AsVector2 - end.AsVector2).normalized, (maxPositionOfElement - start.AsVector2).normalized) < 0f;
    }
}
