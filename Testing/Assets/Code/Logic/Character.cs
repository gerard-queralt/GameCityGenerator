using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public int level
    {
        get;
    }
}

public class Character : ICharacter
{
    public SaveData m_data;

    public struct SaveData
    {
        public int level;
        public int xp;
    }

    public Character()
    {
        m_data = new SaveData();
        m_data.level = 0;
        m_data.xp = 0;
    }

    public int level
    {
        get
        {
            return m_data.level;
        }
    }

    public void AddXP(int i_xp_value)
    {
        Debug.Assert(i_xp_value >= 0, "Experience points can't be negative");
        if (i_xp_value > 0)
        {
            m_data.xp += i_xp_value;
            int lastLevel = m_data.level;
            m_data.level = m_data.xp / 10;
            if (lastLevel != m_data.level)
            {
                //Level up!
            }
        }
    }
}
