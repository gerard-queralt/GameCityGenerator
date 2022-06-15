using UnityEngine;

public abstract class CityElement : ScriptableObject
{
    public abstract GameObject prefab
    {
        get;
    }

    public abstract Bounds boundingBox
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

    public abstract float defaultAffinity
    {
        get;
    }
}
