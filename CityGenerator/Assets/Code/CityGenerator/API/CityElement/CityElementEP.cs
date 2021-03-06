using UnityEngine;

public class CityElementEP : CityElement
{
    [SerializeField] GameObject m_prefab;

    public override GameObject prefab
    {
        get
        {
            return m_prefab;
        }
    }

    public override Bounds boundingBox
    {
        get
        {
            return PositionCalculator.ComputeBoundsOfGameObject(m_prefab);
        }
    }

    public override uint inhabitants
    {
        get
        {
            return 0;
        }
    }

    public override uint? instanceLimit
    {
        get
        {
            return null;
        }
    }

    public override float defaultAffinity
    {
        get
        {
            return 0f;
        }
    }

    protected virtual void Awake()
    {

        Debug.Assert(m_prefab != null, "Prefab not set in CityElement[" + name + "]");
    }
}
