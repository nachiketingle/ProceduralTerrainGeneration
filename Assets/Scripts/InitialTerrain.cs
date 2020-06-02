using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialTerrain : MonoBehaviour
{
    public GameObject player;
    private HeightEditor editor;
    private Terrain terrain;
    private TerrainData terrainData;
    int res;

    // Start is called before the first frame update
    void Start()
    {
        editor = player.GetComponent<HeightEditor>();
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        res = terrainData.heightmapResolution;


        terrainData.terrainLayers = new TerrainLayer[] { editor.sand, editor.dirt, editor.snow };
        float[,] heights = new float[res, res];
        float[,,] maps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        float val;
        for (int i = 0; i < res; i++)
        {
            for (int j = 0; j < res; j++)
            {
                val = Mathf.Sqrt(Mathf.Pow(Mathf.Sin(i / editor.firstScale), 2) + Mathf.Pow(Mathf.Cos(0 / editor.firstScale), 2)) / 2; // Mathf.PerlinNoise(i / editor.firstScale, j / editor.firstScale); 
                val += Mathf.Pow(Mathf.PerlinNoise(i / editor.secondScale, j / editor.secondScale), editor.secondPower) * editor.secondAmp;
                heights[i, j] = val;

                if (i > terrainData.alphamapWidth - 1 || j > terrainData.alphamapHeight - 1)
                    continue;

                if (val < 0.333)
                {
                    //if (val < 0.2)
                    //    heights[i, j] = 0.2f;

                    maps[i, j, 0] = 1;
                    maps[i, j, 1] = 0;
                    maps[i, j, 2] = 0;
                }
                else if (val < 0.666)
                {
                    maps[i, j, 0] = 0;
                    maps[i, j, 1] = 1;
                    maps[i, j, 2] = 0;
                }
                else
                {
                    maps[i, j, 0] = 0;
                    maps[i, j, 1] = 0;
                    maps[i, j, 2] = 1;
                }
            }
        }


        terrainData.SetHeights(0, 0, heights);
        terrainData.SetAlphamaps(0, 0, maps);
    }

}
