using UnityEngine;

public class Crossroad
{
    private float m_x;
    private float m_z;

    public float x
    {
        get
        {
            return m_x;
        }
    }

    public float z
    {
        get
        {
            return m_z;
        }
    }

    public Vector2 AsVector2
    {
        get
        {
            return new Vector2(m_x, m_z);
        }
    }

    public Crossroad(float x, float z)
    {
        m_x = x;
        m_z = z;
    }
}
