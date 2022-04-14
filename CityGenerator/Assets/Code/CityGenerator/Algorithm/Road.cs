using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunayVoronoi;

public class Road
{
    private Crossroad m_start;
    private Crossroad m_end;
    private Quaternion m_rotation;
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

    public float width
    {
        get
        {
            return m_width;
        }
        set
        {
            m_width = value;
        }
    }

    public Quaternion rotation
    {
        get
        {
            return m_rotation;
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

    public Road(Crossroad start, Crossroad end)
    {
        m_start = start;
        m_end = end;
        ComputeRotation();
    }

    public Road(Edge i_edge)
    {
        m_start = i_edge.Point1.crossroad;
        m_end = i_edge.Point2.crossroad;
        ComputeRotation();
    }

    private void ComputeRotation()
    {
        float angle = Vector2.Angle(m_start.AsVector2 - m_end.AsVector2, Vector2.right);
        m_rotation = Quaternion.AngleAxis(angle, Vector3.up);
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

    public bool CanBePlaced(LeftRight i_leftRight, Bounds i_bounds, float i_delta)
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
