using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Advanced City Element", menuName = "City Generator/Advanced City Element", order = 1)]
public class AdvancedCityElement : CityElement
{
    [SerializeField] GameObject m_prefab;
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
