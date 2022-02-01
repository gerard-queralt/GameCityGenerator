using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeDirector : MonoBehaviour
{
    [SerializeField] GameObject m_pieces;
    [SerializeField] GameObject[] m_bigDecos;
    MapData m_map;

    class MapData
    {
        public MapData()
        {
            m_cells = new int[][]
                {
                    new int[] { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1 },
                    new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                    new int[] { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1 },
                    new int[] { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1 },
                    new int[] { 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1 },
                    new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                    new int[] { 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1 },
                    new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                    new int[] { 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1 },
                    new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
                    new int[] { 1, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1 }
                };
        }

        public int[][] m_cells;
    }

    private void Awake()
    {
        Debug.Assert(m_pieces != null, "Pieces prefab not set in BiomeDirector");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_map = GenerateMap();
        InstantiateMap(m_map);

        Shader.SetGlobalFloat("_TileOffset_LV0_X", 0.0f);
        Shader.SetGlobalFloat("_TileOffset_LV0_Y", 2.0f);
        Shader.SetGlobalFloat("_TileOffset_LV1_X", 0.0f);
        Shader.SetGlobalFloat("_TileOffset_LV1_Y", 1.0f);
        Shader.SetGlobalFloat("_TileOffset_LV2_X", 0.0f);
        Shader.SetGlobalFloat("_TileOffset_LV2_Y", 0.0f);
    }

    MapData GenerateMap()
    {
        return new MapData();
    }

    void InstantiateMap(MapData i_mapData)
    {
        float deltaX = 0.0f;
        float deltaZ = ((i_mapData.m_cells.Length - 2) / 2) * 6.0f;
        for (int row = 0; row < i_mapData.m_cells.Length - 2; row += 2)
        {
            for (int col = 0; col < i_mapData.m_cells[row].Length - 2; col += 2)
            {
                long tileId =     i_mapData.m_cells[row + 2][col + 2] * 100000000
                                + i_mapData.m_cells[row + 2][col + 1] * 10000000
                                + i_mapData.m_cells[row + 2][col + 0] * 1000000
                                + i_mapData.m_cells[row + 1][col + 2] * 100000
                                + i_mapData.m_cells[row + 1][col + 1] * 10000
                                + i_mapData.m_cells[row + 1][col + 0] * 1000
                                + i_mapData.m_cells[row + 0][col + 2] * 100
                                + i_mapData.m_cells[row + 0][col + 1] * 10
                                + i_mapData.m_cells[row + 0][col + 0] * 1;
                string pieceName = "piece_" + tileId.ToString("D9");
                Transform pieceTransform = m_pieces.transform.Find(pieceName);
                Debug.Assert(pieceTransform != null);
                if (pieceTransform != null)
                {
                    Instantiate(pieceTransform.gameObject, new Vector3(deltaX, 0.0f, deltaZ), Quaternion.AngleAxis(-90.0f, Vector3.right), transform);
                }
                deltaX += 6.0f;
            }
            deltaX = 0.0f;
            deltaZ -= 6.0f;
        }
    }

    public GameObject FindBigDecoToSpawn()
    {
        return m_bigDecos[0];
    }
}
