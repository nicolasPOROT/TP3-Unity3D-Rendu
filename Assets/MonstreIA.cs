using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MonsterAI : MonoBehaviour
{
    public CharacterStats stats; // Stats du monstre
    public float moveSpeed = 2f; // Vitesse de déplacement
    public float stopDistance = 1.5f; // Distance d’attaque
    public float attackCooldown = 1.5f;

    private Transform target; // Joueur détecté
    private CharacterStats targetStats;
    private Rigidbody rb;
    private float nextAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (stats == null)
            stats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > stopDistance)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                direction.y = 0;

                Vector3 move = direction * moveSpeed * Time.deltaTime;
                rb.MovePosition(transform.position + move);
            }
            else
            {
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }

            Vector3 lookDir = target.position - transform.position;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    void Attack()
    {
        if (targetStats != null)
        {
            Debug.Log("Le monstre attaque le joueur !");
            stats.Attack(targetStats);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            targetStats = other.GetComponent<CharacterStats>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
            targetStats = null;
        }
    }
}
