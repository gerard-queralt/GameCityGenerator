using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDelegate
{
    private static int k_version = 1;

    public void Save(Game.SaveData i_gameSaveData, Character.SaveData i_characterSaveData)
    {
        FileStream file = new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + "data.bin", FileMode.OpenOrCreate);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(k_version);
        writer.Write(i_gameSaveData.numTimesXPGiven);
        writer.Write(i_characterSaveData.level);
        writer.Write(i_characterSaveData.xp);
        writer.Close();
        file.Close();
    }

    public enum LoadResult
    {
        Ok,
        Failed
    }
    public LoadResult Load(out Game.SaveData o_gameSaveData, out Character.SaveData o_characterSaveData)
    {
        o_gameSaveData = new Game.SaveData();
        o_characterSaveData = new Character.SaveData();
        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + "data.bin", FileMode.Open);
            BinaryReader reader = new BinaryReader(file);
            int version = reader.ReadInt32();
            if (version == k_version)
            {
                o_gameSaveData.numTimesXPGiven = reader.ReadInt32();
                o_characterSaveData.level = reader.ReadInt32();
                o_characterSaveData.xp = reader.ReadInt32();
            }
            reader.Close();
            file.Close();
            return LoadResult.Ok;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return LoadResult.Failed;
        }
    }
}
