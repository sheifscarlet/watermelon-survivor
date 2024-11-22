using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float duration = 5f;

    private PlayerHealthController playerHealthController;
    private bool isPickedUp = false;

    private void Awake()
    {
        var playerObject = GameObject.FindWithTag("Player");

        playerHealthController = playerObject.GetComponent<PlayerHealthController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPickedUp)
            return;

        if (other.CompareTag("Player") || other.CompareTag("PlayerGiganto"))
        {
            isPickedUp = true;
            

            // Activate shield on the player
            playerHealthController.ActivateShield(duration);
            AudioController.instance.PlaySound("PickUp");
            //Activate notification
            NotificationsController.instance.ActivateShieldNotification();
            // Deactivate the shield pickup 
            Destroy(gameObject);
        }
    }
}