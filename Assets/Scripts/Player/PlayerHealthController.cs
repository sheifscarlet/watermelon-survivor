using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    [Header("Health")] 
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;
    private int damageBlockers = 0;
    
    public bool CanTakeDamage
    {
        get { return damageBlockers <= 0; }
    }

    private bool isDead = false;

    public bool IsDead => isDead;

    [Header("Shield Variables")]
    private Coroutine shieldCoroutine;
    private float shieldDurationRemaining = 0f;
    private bool isShieldActive = false;

    [Header("Visual")] 
    [Header("Effects Settings")] 
    [SerializeField] private GameObject shieldVFX;
    [Header("Camera Shake")] 
    [SerializeField] private float intensity = 5f;
    [SerializeField] private float time = 0.25f;
    
    public event Action<int, int> OnHealthChanged;
    
    void Start()
    {
        shieldVFX.SetActive(false);
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Initial health update
    }

    

    public void TakeDamage(int damageAmount)
    {
        if (CanTakeDamage)
        {
            ParticleSystemController.Instance.PlayVFX("Damage", transform.position, Quaternion.identity);
            AudioController.instance.PlaySound("PlayerDamage");
            CameraShake.instance.ShakeCamera(intensity,time);
            currentHealth -= damageAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            // Invoke the health changed event
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                isDead = true;
                
            }
        }
        
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void AddDamageBlocker()
    {
        damageBlockers++;
    }

    public void RemoveDamageBlocker()
    {
        damageBlockers = Mathf.Max(0, damageBlockers - 1);
    }

    
    public void ActivateShield(float duration)
    {
        shieldDurationRemaining += duration;
        
        if (shieldCoroutine == null)
        {
            shieldCoroutine = StartCoroutine(ShieldCoroutine());
        }
    }

    private IEnumerator ShieldCoroutine()
    {
        if (!isShieldActive)
        {
            AddDamageBlocker();
            isShieldActive = true;
            shieldVFX.SetActive(true);
        }

        while (shieldDurationRemaining > 0)
        {
            shieldDurationRemaining -= Time.deltaTime;
            yield return null;
        }

        RemoveDamageBlocker();
        NotificationsController.instance.DeactivateShieldNotification();
        shieldVFX.SetActive(false);
        isShieldActive = false;
        shieldCoroutine = null;
    }
}
