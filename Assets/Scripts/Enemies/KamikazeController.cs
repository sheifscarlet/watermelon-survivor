using UnityEngine;
using UnityEngine.AI;

public class KamikazeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float minSpeed = 2;
    [SerializeField] private float maxSpeed = 4;
    private float moveSpeed;

    [Header("Damage Settings")]
    [SerializeField] private int damage;

    private GameObject player;
    private NavMeshAgent _agent;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed);
        _agent.speed = moveSpeed;
    }

    private void Update()
    {
        if (_agent.isOnNavMesh && player != null)
        {
            _agent.SetDestination(player.transform.position);
        }
        else
        {
            //Debug.LogWarning("Kamikaze is not on NavMesh or player is missing.");
            gameObject.SetActive(false);
        }
    }

    public void Explode()
    {
        ParticleSystemController.Instance.PlayVFX("KamikazeExplode", transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthController>().TakeDamage(damage);
            Explode();
        }
    }
}