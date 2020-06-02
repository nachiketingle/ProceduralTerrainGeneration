using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class TerrainCreation : MonoBehaviour
{
    // The starting terrain
    public Terrain initialTerrain;

    // Terrain that we are currently on
    private Terrain currTerrain;
    private TerrainData data;

    TerrainData newData;
    TerrainGenerator generator;

    private HeightEditor editor;

    Vector3 center;

    // Start is called before the first frame update
    void Start()
    {
        currTerrain = initialTerrain;
        data = currTerrain.terrainData;
        editor = GetComponent<HeightEditor>();
        center = new Vector3(500, 0, 500);
        CreateNeighbors(currTerrain);

    }

    // Update is called once per frame
    void Update()
    {
        CheckBounds();
    }

    void CheckBounds()
    {
        Vector3 pos = transform.position;
        float extents = data.bounds.extents.x;

        // Check all 4 directions
        // Top neighbor
        if(pos.z > center.z + extents)
        {
            // Set Current Terrain
            currTerrain = currTerrain.topNeighbor;
            center.z += 2*extents;
        }   
        else if(pos.z < center.z - extents)
        {
            // Set Current Terrain
            currTerrain = currTerrain.bottomNeighbor;
            center.z -= 2*extents;
        }
        else if(pos.x > center.x + extents)
        {
            // Set Current Terrain
            currTerrain = currTerrain.rightNeighbor;
            center.x += 2*extents;
        }
        else if(pos.x < center.x - extents)
        {
            // Set Current Terrain
            currTerrain = currTerrain.leftNeighbor;
            center.x -= 2*extents;
        }
        else
        {
            return;
        }

        // Set new data
        data = currTerrain.terrainData;

        // Create Neighbors
        CreateNeighbors(currTerrain);

    }

    void CreateNeighbors(Terrain curr)
    {
        float extents = data.bounds.extents.x;

        // Check which neighbors are null
        if (curr.topNeighbor == null)
        {
            generator = new TerrainGenerator(data, center.x - extents, center.z + 1 * extents, this, editor);
            generator.GenerateTerrain();
            curr.SetNeighbors(curr.leftNeighbor, CreateTerrain().GetComponent<Terrain>(), curr.rightNeighbor, curr.bottomNeighbor);
        }

        if(curr.bottomNeighbor == null)
        {
            generator = new TerrainGenerator(data, center.x - extents, center.z - 3 * extents, this, editor);
            generator.GenerateTerrain();
            curr.SetNeighbors(curr.leftNeighbor, curr.topNeighbor, curr.rightNeighbor, CreateTerrain().GetComponent<Terrain>());
        }

        if(curr.rightNeighbor == null)
        {
            generator = new TerrainGenerator(data, center.x + 1 * extents, center.z - extents, this, editor);
            generator.GenerateTerrain();
            curr.SetNeighbors(curr.leftNeighbor, curr.topNeighbor, CreateTerrain().GetComponent<Terrain>(), curr.bottomNeighbor);
        }

        if(curr.leftNeighbor == null)
        {
            generator = new TerrainGenerator(data, center.x - 3 * extents, center.z - extents, this, editor);
            generator.GenerateTerrain();
            curr.SetNeighbors(CreateTerrain().GetComponent<Terrain>(), curr.topNeighbor, curr.rightNeighbor, curr.bottomNeighbor);
        }
    }

    public void SetData(TerrainData data)
    {
        newData = data;
    }

    GameObject CreateTerrain()
    {
        if (newData == null)
            return null;

        GameObject terrain = Terrain.CreateTerrainGameObject(newData);
        terrain.transform.position = new Vector3(generator.xPos, 0, generator.zPos);
        terrain.tag = "Ground";
        newData = null;

        return terrain;
    }

    public void PrintString(string message)
    {
        print(message);
    }
}
