using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SeedSurge : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;

    
    [Header("Timer")]
    [SerializeField] private float cooldownTime;
    public bool isReady;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && isReady)
        {
            UseSeedSurge();
        }
    }

    void CallVFX()
    {
        ParticleSystemController.Instance.PlayVFX("SeedSurge1",transform.position,quaternion.identity);
        ParticleSystemController.Instance.PlayVFX("SeedSurge2",transform.position,quaternion.identity);
    }

    void UseSeedSurge()
    {
        CallVFX();
        AudioController.instance.PlaySound("SeedSurge");
        CameraShake.instance.ShakeCamera(5,0.25f);
        
        // Get all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);

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
        
        isReady = false;
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        isReady = true;
        AudioController.instance.PlaySound("SkillIsReady");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
