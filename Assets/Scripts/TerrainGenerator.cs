using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator
{
    public float xPos;
    public float zPos;
    private TerrainData data;
    private TerrainData newData;
    private TerrainCreation creator;
    private int res;
    private HeightEditor editor;

    public TerrainGenerator(TerrainData data, float xPos, float zPos, TerrainCreation creator, HeightEditor editor)
    {
        this.data = data;
        this.xPos = xPos;
        this.zPos = zPos;
        this.creator = creator;
        this.editor = editor;
        newData = new TerrainData();
        // Set appropriate resolution
        newData.baseMapResolution = data.baseMapResolution;
        res = newData.heightmapResolution = data.heightmapResolution;
        newData.size = data.size;
        
    }

    public void GenerateTerrain()
    {   
        
        // Set height maps
        SetTerrainMap(newData);

        creator.SetData(newData);
    }

    void SetTerrainMap(TerrainData terrainData)
    {
        terrainData.terrainLayers = new TerrainLayer[] { editor.sand, editor.dirt, editor.snow };
        float[,] heights = new float[res, res];
        float[,,] maps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        float xOffset = res * xPos / 1000;
        float zOffset = res * zPos / 1000;

        creator.PrintString("xOffset: " + xOffset + " xPos: " + xPos + " Res: " + res);

        float val;
        for (int i = 0; i < res; i++)
        {
            for (int j = 0; j < res; j++)
            {
                val = Mathf.Sqrt(Mathf.Pow(Mathf.Sin((i + xOffset) / editor.firstScale), 2) + Mathf.Pow(Mathf.Cos(0 / editor.firstScale), 2)) / 2;  //Mathf.PerlinNoise((i + xOffset)/ editor.firstScale, (j + zOffset) / editor.firstScale);
                val += Mathf.Pow(Mathf.PerlinNoise((i + xOffset) / editor.secondScale, (j + zOffset) / editor.secondScale), editor.secondPower) * editor.secondAmp;
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
