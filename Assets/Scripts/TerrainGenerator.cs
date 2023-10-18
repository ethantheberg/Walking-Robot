using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
  public int width = 256;
  public int length = 256;
  public int height = 20;
  public float scale = 20.0f;
  void Start()
  {
    Terrain terrain = GetComponent<Terrain>();
    terrain.terrainData = GenerateTerrain(terrain.terrainData);
  }

  TerrainData GenerateTerrain(TerrainData terrainData)
  {
    terrainData.heightmapResolution = width + 1;

    terrainData.size = new Vector3(width, height, length);
    terrainData.SetHeights(0, 0, GenerateHeights());
    return terrainData;
  }

  float[,] GenerateHeights()
  {
    float[,] heights = new float[width, length];
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < length; y++)
      {
        heights[x, y] = CalculateHeight(x, y);
      }
    }
    return heights;
  }
  float CalculateHeight(int x, int y)
  {
    float xCoord = (float)x / width * scale;
    float yCoord = (float)y / length * scale;
    return Mathf.PerlinNoise(xCoord, yCoord);
  }
}
