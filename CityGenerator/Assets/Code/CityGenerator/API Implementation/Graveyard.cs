using UnityEngine;

[CreateAssetMenu(fileName = "Graveyard Definition", menuName = "City Generator/Graveyard Definition", order = 1)]
public class Graveyard : CityElementEP
{
    [SerializeField] uint m_instanceLimit = 0;

    public override uint? instanceLimit
    {
        get
        {
            return m_instanceLimit;
        }
    }
}
