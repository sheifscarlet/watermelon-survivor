using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class MelonRush : MonoBehaviour
{
    [Header("Rush Variables")]
    [SerializeField] private float rushForce;
    [SerializeField] private float rushTime;
    public bool isReady;
    [SerializeField] private bool isInRush;
    private Vector3 _rushDirection;
    
    [Header("Enemy Knockback Settings")]

    [SerializeField] float knockbackForce = 5f; 
    [SerializeField] float knockbackDuration = 0.1f;
    
    [Header("Cooldown Settings")] [SerializeField]
    private float cooldownTime; 

    private float _cooldownTimer;

    [Header("Timer")] private float _rushTimer;

    // Components
    private Rigidbody _rb;
    private PlayerHealthController _playerHealthController;
    [SerializeField] private Camera playerCamera; 

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerHealthController = GetComponent<PlayerHealthController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProcessRush();
        CooldownUpdate();

        if (Input.GetKeyDown(KeyCode.F) && isReady)
        {
            StartRush();
        }
    }

    public void StartRush()
    {
        ParticleSystemController.Instance.PlayVFX("MelonRush",transform.position,quaternion.identity);
        AudioController.instance.PlaySound("WatermelonDash");
        isInRush = true;
        _playerHealthController.AddDamageBlocker();

        
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0; 
        _rushDirection = cameraForward.normalized; 

        isReady = false;
        _rushTimer = rushTime;
        _cooldownTimer = cooldownTime;
    }

    private void ProcessRush()
    {
        if (_rushTimer > 0)
        {
            
            _rb.AddForce(_rushDirection * rushForce, ForceMode.VelocityChange);
            _rushTimer -= Time.deltaTime;
        }
        else if (isInRush)
        {
            isInRush = false;
            _playerHealthController.RemoveDamageBlocker();
        }
    }

    private void CooldownUpdate()
    {
        if (!isReady)
        {
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;
            }
            else
            {
                
                isReady = true;
                AudioController.instance.PlaySound("SkillIsReady");

            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (isInRush)
            {
                EnemyAI enemyAI = other.GetComponent<EnemyAI>();

                if (enemyAI != null)
                {
                    
                    Vector3 bounceDirection = (other.transform.position - transform.position).normalized;
                    enemyAI.Knockback(bounceDirection, knockbackForce, knockbackDuration);
                }
            }
        }
    }
}            
  

