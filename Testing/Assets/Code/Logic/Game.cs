using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private static Game s_instance = null;
    private Character m_character;
    private SaveData m_data;

    public struct SaveData
    {
        public int numTimesXPGiven;
    }

    private Game()
    {
        m_character = new Character();

        SaveDelegate saveDelegate = new SaveDelegate();
        SaveDelegate.LoadResult result = saveDelegate.Load(out m_data, out m_character.m_data);
        if (result == SaveDelegate.LoadResult.Failed)
        {
            m_character = new Character();
            m_data = new SaveData();
            m_data.numTimesXPGiven = 0;
        }
    }

    public static Game instance
    {
        get {
            if (s_instance == null)
            {
                s_instance = new Game();
            }
            return s_instance;
        }
    }

    public ICharacter character
    {
        get
        {
            return m_character;
        }
    }

    public void AddXP(int i_xp_value)
    {
        m_character.AddXP(i_xp_value);
        m_data.numTimesXPGiven++;
        Save();
    }

    private void Save()
    {
        SaveDelegate saveDelegate = new SaveDelegate();
        saveDelegate.Save(m_data, m_character.m_data);
    }
}
