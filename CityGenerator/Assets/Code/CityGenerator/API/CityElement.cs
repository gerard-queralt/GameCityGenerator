using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CityElement : ScriptableObject
{
    public abstract GameObject prefab
    {
        get;
    }

    public abstract uint inhabitants
    {
        get;
    }

    public abstract uint? instanceLimit
    {
        get;
    }
}
