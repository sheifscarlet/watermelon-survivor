using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public static ParticleSystemController Instance;  

    [System.Serializable]
    public class ParticleEffect
    {
        public string effectName;  
        public GameObject particlePrefab;  
        public int poolSize = 10;  
    }

    public List<ParticleEffect> particleEffects;  

    private Dictionary<string, Queue<GameObject>> particlePools;  

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        particlePools = new Dictionary<string, Queue<GameObject>>();

        // Initialize pools for each effect
        foreach (ParticleEffect effect in particleEffects)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < effect.poolSize; i++)
            {
                GameObject obj = Instantiate(effect.particlePrefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            particlePools.Add(effect.effectName, pool);
        }
    }

   
    public void PlayVFX(string effectName, Vector3 position, Quaternion rotation)
    {
        if (particlePools.ContainsKey(effectName))
        {
            GameObject particle = GetParticleFromPool(effectName, position, rotation);
            if (particle != null)
            {
                
                particle.SetActive(true);
                particle.GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            Debug.LogWarning("Effect " + effectName + " not found!");
        }
    }

    
    
    private GameObject GetParticleFromPool(string effectName, Vector3 position, Quaternion rotation)
    {
        Queue<GameObject> pool = particlePools[effectName];

        if (pool.Count > 0)
        {
            GameObject particle = pool.Dequeue();
            particle.transform.position = position;
            particle.transform.rotation = rotation;

            // If the particle doesn't have a ParticleReturner, add it
            ParticleReturner returner = particle.GetComponent<ParticleReturner>();
            if (returner == null)
            {
                returner = particle.AddComponent<ParticleReturner>();
            }

            
            returner.Initialize(effectName);

            return particle;
        }
        else
        {
            
            ParticleEffect effect = particleEffects.Find(e => e.effectName == effectName);
            if (effect != null)
            {
                GameObject newParticle = Instantiate(effect.particlePrefab, position, rotation);

               
                ParticleReturner returner = newParticle.AddComponent<ParticleReturner>();
                returner.Initialize(effectName);

                return newParticle;
            }
        }
        return null;
    }

    // Return particle to pool when it's disabled automatically
    public void ReturnToPool(string effectName, GameObject particle)
    {
        if (particlePools.ContainsKey(effectName))
        {
            particle.SetActive(false);  
            particlePools[effectName].Enqueue(particle);  
        }
    }
}
