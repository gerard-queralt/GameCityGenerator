using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "House Definition", menuName = "City Generator/House Definition", order = 1)]
public class House : CityElement
{
    [SerializeField] uint m_inhabitants;

    public override uint inhabitants
    {
        get
        {
            return inhabitants;
        }
    }

    public override RepeatType repeatType
    {
        get
        {
            return RepeatType.Unlimited;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
