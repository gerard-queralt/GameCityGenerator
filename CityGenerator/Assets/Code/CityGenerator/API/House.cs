using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "House Definition", menuName = "City Generator/House Definition", order = 1)]
public class House : CityElementEP
{
    [SerializeField] uint m_inhabitants;

    public override uint inhabitants
    {
        get
        {
            return inhabitants;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Debug.Assert(m_inhabitants > 0, "Zero inhabitants in House[" + name + "]");
    }
}
