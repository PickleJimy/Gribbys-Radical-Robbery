
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool agentEnabled;

    public Transform player;

    public GameObject enemy;

    public LayerMask whatIsPlayer;

    public float health;

    public Rigidbody rb;

    public float groundDrag;

    public float minJumpHeight;

    public float maxJumpHeight;

    public bool playerInJumpRange;

    public bool playerOnGround;

    [Header("Ground Check")]
    public float enemyHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public bool readyToJump;
    public bool preparedToJump;
    public bool airborne;

    public bool hurt;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange, jumpRange;
    public bool playerInSightRange, playerInAttackRange;

    public void Start()
    {
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemy = GameObject.Find("Enemy");
    }

    public void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInJumpRange = Physics.CheckSphere(transform.position, jumpRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        if (player.transform.position.y >= rb.transform.position.y + minJumpHeight && playerInJumpRange) preparedToJump = true;

        rb = GetComponent<Rigidbody>();

        playerOnGround = player.GetComponent<PlayerMovement>().grounded;

        grounded = Physics.Raycast(transform.position, Vector3.down, enemyHeight, whatIsGround);

        airborne = !grounded;

        if (grounded)
        {
            Debug.Log("On the ground");
        }

        agentEnabled = agent.enabled;

        hurt = player.GetComponent<GrabAndStab>().dealDamage;

        // when to jump
        if (readyToJump && grounded && preparedToJump && playerOnGround && playerInSightRange)
        {
            readyToJump = true;

            agentEnabled = false;

            airborne = true;

            Invoke(nameof(ResetJump), jumpCooldown);

            agent.SetDestination(transform.position);
            // disable the agent
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
                
            // make the jump
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddRelativeForce(transform.up * jumpForce, ForceMode.Impulse);
            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;

            Debug.Log(rb.velocity.magnitude);
        }
    }
    public void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    public void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position);

        if (!agentEnabled)
        {
            rb.AddForce(player.position);
        }
    }

    public void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!agentEnabled)
        {
            rb.AddForce(transform.position);
        }

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Check for collision back to the ground, and re-enable the NavMeshAgent
    /// </summary>
    public void ResetJump()
    {
        agentEnabled = true;
        readyToJump = true;
        if (!grounded)
        {
            if (agentEnabled)
            {
                agent.updatePosition = true;
                agent.updateRotation = true;
                agent.isStopped = false;
            }
            rb.isKinematic = true;
            rb.useGravity = true;
            grounded = true;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, jumpRange);
    }
}  
