using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightEditor : MonoBehaviour
{
    // TerrainLayers
    public TerrainLayer sand;
    public TerrainLayer dirt;
    public TerrainLayer snow;

    public float firstScale;
    [Range(0.0001f, 500f)]
    public float secondScale;
    [Range(1f, 25f)]
    public int secondPower;
    [Range(0f, 0.5f)]
    public float secondAmp;

}
