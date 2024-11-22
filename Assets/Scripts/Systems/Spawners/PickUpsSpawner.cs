using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Pool;


public class PickUpsSpawner : MonoBehaviour
{
    public static PickUpsSpawner instance;
    public Terrain targetTerrain;    
    public List<GameObject> pickUps; 
    public int numberOfPickUps = 5;  


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (targetTerrain != null && pickUps != null && pickUps.Count > 0)
        {
            SpawnPickUps();
        }
    }

    

    public void SpawnPickUps()
    {
        
        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainSize = terrainData.size;

       
        Vector3 terrainPosition = targetTerrain.transform.position;
        
        //Notification
        NotificationsController.instance.ActivateNewItemsNotification();
        StartCoroutine(DeactivateNotification());
        
        // Loop through each pickup in the pickUps list
        foreach (GameObject pickUpPrefab in pickUps)
        {
            // For each pickup type, spawn numberOfPickUps
            for (int i = 0; i < numberOfPickUps; i++)
            {
                // Generate random x, z 
                float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
                float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);

                // Get the height at the random (x, z) position
                float randomY = targetTerrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrainPosition.y;

                // Spawn the pickup at the random position
                Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
                Instantiate(pickUpPrefab, randomPosition, Quaternion.identity);
                
            }
        }
    }

    IEnumerator DeactivateNotification()
    {
        yield return new WaitForSeconds(3f);
        NotificationsController.instance.DeactivateNewItemsNotification();
    }
}
