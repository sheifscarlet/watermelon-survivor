using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI Components")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    [Header("Health System")]
    [SerializeField] PlayerHealthController playerHealth;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerHealth == null)
        {
            return;
        }
        // Initialize the slider's max and current values
        healthSlider.maxValue = playerHealth.MaxHealth;
        healthSlider.value = playerHealth.CurrentHealth;
        healthText.text = playerHealth.MaxHealth.ToString();
        // Subscribe to health change events
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }
    
    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthText.text = currentHealth.ToString();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    
    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
