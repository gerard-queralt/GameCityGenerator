using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CityElement : ScriptableObject
{
    public enum RepeatType
    {
        Unique,
        GivenNumber,
        Unlimited
    }

    [SerializeField] GameObject m_prefab;

    public GameObject prefab
    {
        get
        {
            return m_prefab;
        }
    }

    public virtual uint inhabitants
    {
        get
        {
            return 0;
        }
    }

    public virtual RepeatType repeatType
    {
        get
        {
            return RepeatType.Unique;
        }
    }

    public virtual uint instances
    {
        get
        {
            return 1;
        }
    }

    protected virtual void Awake()
    {
        Debug.Assert(m_prefab != null, "Prefab not set in CityElement[" + name + "]");
    }
}
