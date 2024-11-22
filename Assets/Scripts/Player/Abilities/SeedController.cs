using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float explosionRange;
    private const float TIME_BEFORE_EXPLOSION = 2f;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystemController.Instance.PlayVFX("AppearSmoke",transform.position,Quaternion.identity);
        StartCoroutine(Explosion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        
        Destroy(gameObject);
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(TIME_BEFORE_EXPLOSION);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            else if (collider.CompareTag("Kamikaze"))
            {
                collider.GetComponent<KamikazeController>().Explode();
            }
        }
        ParticleSystemController.Instance.PlayVFX("SeedExplode",transform.position,quaternion.identity);
        AudioController.instance.PlaySound("SBBarrageExplosion");
        CameraShake.instance.ShakeCamera(2.5f,0.25f);
        
        Die();

    }
    
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
