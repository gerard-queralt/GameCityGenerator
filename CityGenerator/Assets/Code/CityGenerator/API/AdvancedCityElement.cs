using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Advanced City Element", menuName = "City Generator/Advanced City Element", order = 1)]
public class AdvancedCityElement : CityElement
{
    [SerializeField] GameObject m_prefab;
    [SerializeField] Vector3 m_sizeOfBox;
    [SerializeField] uint m_inhabitants = 0;
    [SerializeField] bool m_setInstanceLimit = false;
    [SerializeField] uint m_instanceLimit = 0;
    [Range(0f, 1f)][SerializeField] float m_defaultAffinity = 0.5f;

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
            Bounds computedBounds = PositionCalculator.ComputeBoundsOfGameObject(m_prefab);
            if (m_sizeOfBox == Vector3.zero)
            {
                return computedBounds;
            }
            return new Bounds(new Vector3(0f, computedBounds.center.y, 0f), m_sizeOfBox);
        }
    }

    public override uint inhabitants
    {
        get
        {
            return m_inhabitants;
        }
    }

    public override uint? instanceLimit
    {
        get
        {
            if (!m_setInstanceLimit)
                return null;
            return m_instanceLimit;
        }
    }

    public override float defaultAffinity
    {
        get
        {
            return m_defaultAffinity;
        }
    }

    private void Awake()
    {
        Debug.Assert(m_prefab != null, "Prefab not set in AdvancedCityElement[" + name + "]");
    }
}
