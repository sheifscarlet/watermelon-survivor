using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationsController : MonoBehaviour
{
    public static NotificationsController instance;
    
    
    [SerializeField] private GameObject shieldNotification;
    [SerializeField] private GameObject boostNotification;
    [SerializeField] private GameObject newItemNotification;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        shieldNotification.SetActive(false);
        boostNotification.SetActive(false);
        newItemNotification.SetActive(false);
    }


    public void ActivateShieldNotification()
    {
        shieldNotification.SetActive(true);
    }
    public void DeactivateShieldNotification()
    {
        shieldNotification.SetActive(false);
    }
    
    public void ActivateBoostNotification()
    {
        boostNotification.SetActive(true);
    }
    public void DeactivateBoostNotification()
    {
        boostNotification.SetActive(false);
    }
    public void ActivateNewItemsNotification()
    {
        newItemNotification.SetActive(true);
    }
    public void DeactivateNewItemsNotification()
    {
        newItemNotification.SetActive(false);
    }
    
}
