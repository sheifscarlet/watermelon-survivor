using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner instance;
    public Terrain targetTerrain;    
    public GameObject foodPrefab; 
    public int numberOfPickUps = 5;  
    
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (targetTerrain != null )
        {
            SpawnFood();
        }
    }

    

    public void SpawnFood()
    {
        // Get the size of the terrain
        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        // Get the position of the terrain in the world
        Vector3 terrainPosition = targetTerrain.transform.position;
        
        
        for (int i = 0; i < numberOfPickUps; i++)
        {
            
            float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
            float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);

            
            float randomY = (targetTerrain.SampleHeight(new Vector3(randomX, 1, randomZ)) + terrainPosition.y)+1;

            
            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
            Instantiate(foodPrefab, randomPosition, Quaternion.identity);
                
        }
    }
}
