using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Giganto : MonoBehaviour
{
    [SerializeField] private Vector3 gigantoSize;
    [SerializeField] private Vector3 normalSize;
    [SerializeField] private bool isGiganto;
    public bool IsGiganto => isGiganto;
    
    [Header("Timer")]
    [SerializeField] private float duration; 
    [SerializeField] private float cooldownTime;
    public bool isReady;

    private MovementController _movementController;
    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
        normalSize = transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        isGiganto = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && isReady)
        {
            _movementController.Jump();
            AudioController.instance.PlaySound("Giganto");
            StartCoroutine(ActivateGiganto());
        }

        if (isGiganto)
        {
            transform.gameObject.tag = "PlayerGiganto";
        }
        else
        {
            transform.gameObject.tag = "Player";
        }
    }
    

    IEnumerator ActivateGiganto()
    {
        
        isGiganto = true;
        isReady = false;  
        transform.localScale = gigantoSize;
        ParticleSystemController.Instance.PlayVFX("Giganto",transform.position,Quaternion.identity);
        ParticleSystemController.Instance.PlayVFX("AppearSmoke",transform.position,Quaternion.identity);
        CameraShake.instance.ShakeCamera(5,0.25f);

        
        yield return new WaitForSeconds(duration);

        
        transform.localScale = normalSize;
        isGiganto = false;
        ParticleSystemController.Instance.PlayVFX("Giganto",transform.position,Quaternion.identity);
        ParticleSystemController.Instance.PlayVFX("AppearSmoke",transform.position,Quaternion.identity);
        
        yield return StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        
        yield return new WaitForSeconds(cooldownTime);

        
        isReady = true;
        AudioController.instance.PlaySound("SkillIsReady");
    }
}
