using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerGiganto"))
        {
            AudioController.instance.PlaySound("Food");
            PlayerHealthController healthController = other.gameObject.GetComponent<PlayerHealthController>();
            healthController.ResetHealth();
            Destroy(gameObject);
        }
    }
}
