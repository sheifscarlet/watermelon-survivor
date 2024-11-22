using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TwirlingSlices : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRangeWhileGiganto;
    [SerializeField] private float standartRange;
    [SerializeField] private bool isAbilityActivated;
    
    [Header("Timer")]
    [SerializeField] private float duration; 
    [SerializeField] private float cooldownTime;
    public bool isReady;
    
    [Header("Visual")]
    [SerializeField] private GameObject slices;
    [SerializeField] private GameObject slicesSFX;
    
    
    // Start is called before the first frame update
    void Start()
    {
        slices.SetActive(false);
        slicesSFX.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && isReady)
        {
            StartCoroutine(ActivateTwirlingSlices());
        }

        if (isAbilityActivated && gameObject.GetComponent<Giganto>().IsGiganto)
        {
            attackRange = attackRangeWhileGiganto;
        }
        else
        {
            attackRange = standartRange;
        }

        if (isAbilityActivated)
        {
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
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    
    
    IEnumerator ActivateTwirlingSlices()
    {
        
        isAbilityActivated = true;
        isReady = false;  
        
        //Activate visual
        slices.SetActive(true);
        slicesSFX.SetActive(true);
        
        yield return new WaitForSeconds(duration);

        //Deactivate visual
        slices.SetActive(false);
        slicesSFX.SetActive(false);
        isAbilityActivated = false;
        
        yield return StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        
        isReady = true;
        AudioController.instance.PlaySound("SkillIsReady");
    }
}
