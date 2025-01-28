using UnityEngine;
using UnityEngine.AI;
using Pocket_Zone_2.Utils;

public class Enemy : MonoBehaviour
{
    public State startingState;
    public float roamingDistanceMax = 7f;
    public float roamingDistanceMin = 3f;
    public float roamingTimerMax = 2f;
    public float detectionRange = 10f; 
    public Transform player;          

    public NavMeshAgent navMeshAgent;
    public State state;
    public float roamingTime;
    public Vector3 roamPosition;
    public Vector3 startingPosition;

    public float maxHealth = 50f;
    public float currentHealth;
    public float damage = 10f;        
    public float attackCooldown = 2f;   
    public float lastAttackTime = 0f;

    public enum State { Idle, Roaming, Chasing }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = startingState;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        startingPosition = transform.position;

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Roaming:
                RoamingBehavior();
                break;
            case State.Chasing:
                ChasingBehavior();
                break;
        }

        DetectPlayer();
        FlipTowardsMovement();
    }

    private void FlipTowardsMovement()
    {
        Vector3 velocity = navMeshAgent.velocity; 
        if (velocity.x > 0.1f)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (velocity.x < -0.1f)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void RoamingBehavior()
    {
        roamingTime -= Time.deltaTime;
        if (roamingTime < 0 || navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Roaming();
            roamingTime = roamingTimerMax;
        }
    }

    private void ChasingBehavior()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) > detectionRange)
            {
                state = State.Roaming;
                Roaming();
            }
        }
    }

    private void DetectPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                state = State.Chasing;
            }
            else if (state == State.Chasing && distanceToPlayer > detectionRange)
            {
                state = State.Roaming;
                Roaming();
            }
        }
    }

    private void Roaming()
    {
        roamPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * Random.Range(roamingDistanceMin, roamingDistanceMax);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            GetComponent<LootDrop>()?.DropLoot();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
