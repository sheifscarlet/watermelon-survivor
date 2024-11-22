using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private TextMeshPro healthText;
    public float maxHealth = 100;
    public float currentHealth;

    [Header("Death Settings")]
    public bool isDead = false;

    [Header("Enemy Type")]
    public bool isKamikaze = false; 

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (healthText != null)
            healthText.text = currentHealth.ToString("F0");
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        if (animator != null)
            animator.Play("root_Zombie_Damage");

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0; 
        GameManager.instance.AddKillCount();

        if (animator != null)
            animator.Play("root_Zombie_dead");

        if(!isKamikaze)
            GetComponent<EnemyAI>().enabled = false;

        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.DecrementActiveEnemies();
        }

        
        Invoke(nameof(ReturnToPool), 3f); 
    }

    private void ReturnToPool()
    {
       
        EnemyPool.instance.ReturnEnemy(gameObject, isKamikaze);
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        currentHealth = maxHealth;
        isDead = false;

        if(!isKamikaze)
            GetComponent<EnemyAI>().enabled = true;

        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGiganto"))
        {
            Die();
        }
    }
}
