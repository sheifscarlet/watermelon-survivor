using System.Collections;
using UnityEngine;

public class SeedBombBarrage : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float seedSpawnRange;
    [SerializeField] private int amountOfSeeds;
    [SerializeField] private GameObject seedPrefab;
    

    [Header("Timer")]
    [SerializeField] private float cooldownTime;
    public bool isReady = true; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && isReady)
        {
            SpawnSeeds();
            StartCoroutine(Cooldown()); 
        }
    }
    
    public void SpawnSeeds()
    {
        
        AudioController.instance.PlaySound("SBBarrageSpawn");
        for (int i = 0; i < amountOfSeeds; i++) 
        {
            // Generate a random position within the radius
            Vector3 randomPosition = Random.insideUnitSphere * seedSpawnRange;
            
            randomPosition.y = 0; 
            randomPosition += transform.position;
            
            Instantiate(seedPrefab, randomPosition, Quaternion.identity);
        }
    }

    private IEnumerator Cooldown()
    {
        isReady = false; 
        yield return new WaitForSeconds(cooldownTime); 
        isReady = true; 
        AudioController.instance.PlaySound("SkillIsReady");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, seedSpawnRange);
    }
}