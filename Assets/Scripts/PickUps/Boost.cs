using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] private float duration;
    [Header("Boost Settings")] 
    [SerializeField] private float newMaxSpeed;
    [SerializeField] private float newMoveSpeed;

    // Components
    private MovementController playerMovement;
    private bool isPickedUp = false;

    private void Awake()
    {
        var playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
        {
            return;
        }

        playerMovement = playerObject.GetComponent<MovementController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp)
            return;

        if (other.CompareTag("Player") || other.CompareTag("PlayerGiganto"))
        {
            isPickedUp = true;

            // Activate boost 
            playerMovement.ActivateBoost(duration, newMoveSpeed, newMaxSpeed);
            AudioController.instance.PlaySound("PickUp");
            //Activate Notification
            NotificationsController.instance.ActivateBoostNotification();
            // Deactivate the boost pickup 
            Destroy(gameObject);
        }
    }
}