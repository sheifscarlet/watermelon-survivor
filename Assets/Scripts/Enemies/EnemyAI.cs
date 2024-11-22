using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Chase, Attack, Escape, Freeze, Die, Knockback }

    [SerializeField] private EnemyState currentState = EnemyState.Chase;

    [Header("Attack Settings")]
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float nextAttackTime = 0f;
    [SerializeField] int attackDamage = 10;

    

    [Header("Freeze Settings")]
    [SerializeField] bool isFrozen = false;
    [SerializeField] float freezeDuration = 0f;
    private float freezeTimer = 0f;
    private float prevSpeed;

    // Components
    private GameObject player;
    private PlayerHealthController playerHealthController;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHealth enemyHealth;
    private Rigidbody rb;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player") ?? GameObject.FindGameObjectWithTag("PlayerGiganto");
        
        playerHealthController = player.GetComponent<PlayerHealthController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void Start()
    {
        if (agent != null) agent.isStopped = false;
        prevSpeed = animator != null ? animator.speed : 1f;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Freeze:
                Freeze();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Die:
                Die();
                break;
            case EnemyState.Knockback:
                break;
        }

        TransitionsHandler();
    }

    private void TransitionsHandler()
    {
        if (currentState == EnemyState.Knockback || currentState == EnemyState.Die)
            return;

        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player") 
                     ?? GameObject.FindGameObjectWithTag("PlayerGiganto");

            if (player == null)
            {
                return;
            }
        }

        if (enemyHealth.currentHealth <= 0 && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Die;
            return;
        }

        if (isFrozen && currentState != EnemyState.Freeze)
        {
            currentState = EnemyState.Freeze;
            freezeTimer = 0f;
            if (agent != null) agent.isStopped = true;
            return;
        }

        if (currentState == EnemyState.Freeze && !isFrozen)
        {
            currentState = EnemyState.Chase;
            if (agent != null) agent.isStopped = false;
            return;
        }

        if (currentState == EnemyState.Chase && player != null && 
            Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            currentState = EnemyState.Attack;
            if (agent != null) agent.isStopped = true;
            return;
        }

        if (currentState == EnemyState.Attack && player != null && 
            Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            currentState = EnemyState.Chase;
            if (agent != null) agent.isStopped = false;
            return;
        }
    }


    private void Chase()
    {
        if (agent != null && NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
    }

    private void Die()
    {
        if (agent != null) agent.isStopped = true;
        enemyHealth.Die();
    }

    private void Freeze()
    {
        if (agent != null) agent.isStopped = true;
        if (animator != null) animator.speed = 0;
        freezeTimer += Time.deltaTime;

        if (freezeTimer >= freezeDuration)
        {
            isFrozen = false;
            if (agent != null) agent.isStopped = false;
            currentState = EnemyState.Chase;
            if (animator != null) animator.speed = prevSpeed;
        }
    }
    
    public void FreezeEnemy(float duration)
    {
        if (currentState != EnemyState.Die && currentState != EnemyState.Knockback)
        {
            freezeDuration = duration;
            isFrozen = true;
            freezeTimer = 0f;
            if (agent != null) agent.isStopped = true;
            currentState = EnemyState.Freeze;
        }
    }


    

    private void Attack()
    {
        if (agent != null) agent.isStopped = true;

        if (Time.time >= nextAttackTime)
        {
            if (playerHealthController != null)
            {
                if (animator != null) animator.Play("root_Zombie_Attack");
                playerHealthController.TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void Knockback(Vector3 direction, float force, float duration)
    {
        if (currentState != EnemyState.Die && currentState != EnemyState.Knockback)
            StartCoroutine(KnockbackRoutine(direction, force, duration));
    }

    private IEnumerator KnockbackRoutine(Vector3 direction, float force, float duration)
    {
        
        if (enemyHealth.isDead) yield break;

        currentState = EnemyState.Knockback;

        if (agent != null)
        {
            agent.enabled = false;
        }

        rb.isKinematic = false;
        rb.useGravity = true;

        direction.y = 5f;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);

        yield return new WaitForSeconds(duration);

        if (agent != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            agent.enabled = true;
            currentState = EnemyState.Chase;
        }
    }

}
