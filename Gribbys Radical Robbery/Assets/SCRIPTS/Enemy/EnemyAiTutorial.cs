
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public GameObject enemy;

    public MeshCollider MeshCollider;

    public Transform ground;

    public GameObject NearestGround;

    public float speed;

    public LayerMask whatIsPlayer, enemyLandArea;

    public float health;

    public Rigidbody rb;

    public float groundDrag;

    public float minJumpHeight;

    public float maxJumpHeight;

    public bool playerInJumpRange;

    public bool playerOnGround;

    public bool playerInPosRange;

    public bool enemyInPosRange;

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
    public float landingRange;
    public bool readyToLand;
    public bool landingZone;

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
    public float sightRange, attackRange, jumpRange, posRange;
    public bool playerInSightRange, playerInAttackRange;

    public void Start()
    {
        readyToJump = true;
    }

    public void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemy = GameObject.Find("Enemy");
        ground = GameObject.Find("Goal").transform;
        agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.enabled = true;
        MeshCollider.enabled = false;
    }

    public void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInJumpRange = Physics.CheckSphere(transform.position, jumpRange, whatIsPlayer);
        readyToLand = Physics.CheckSphere(transform.position, landingRange, whatIsGround);
        playerInPosRange = ground.GetComponent<Goal>().playerInPosRange;

        if (player.transform.position.y >= rb.transform.position.y + minJumpHeight && playerInJumpRange) preparedToJump = true;
        if (player.transform.position.y <= rb.transform.position.y + minJumpHeight && playerInJumpRange) preparedToJump = false;

        NearestGround = enemy.GetComponent<SenseGround>().NearestGround;

        agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();

        rb = enemy.GetComponent<Rigidbody>();

        MeshCollider = enemy.GetComponent<MeshCollider>();

        playerOnGround = player.GetComponent<PlayerMovement>().grounded;

        grounded = Physics.Raycast(transform.position, Vector3.down, enemyHeight, whatIsGround);

        airborne = !grounded;

        landingZone = ground.GetComponent<Goal>();

        hurt = player.GetComponent<GrabAndStab>().dealDamage;

        if (grounded)
        {
            MeshCollider.enabled = false;
            agent.enabled = true;
        }

        if (agent.enabled)
        {
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
        else
        {
            if (playerInSightRange && !playerInAttackRange) RbChasePlayer();
            if (playerInAttackRange && playerInSightRange) RbAttackPlayer();
        }

        // when to jump
        if (readyToJump && grounded && preparedToJump && playerOnGround && playerInSightRange)
        {
            EnemyJump();

            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;

            Debug.Log(rb.velocity.magnitude);
        }
    }

    public IEnumerator EnemyLand()
    {
        MeshCollider.enabled = true;
        yield return new WaitForSeconds(0.01f);
        if (grounded)
        {
            agent.enabled = true;
            rb.isKinematic = true;
            rb.useGravity = false;
            MeshCollider.enabled = false;
        }
    }

    public void EnemyJump()
    {
        agent.enabled = false;
        // make the jump
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        // reset y velocity
        rb.velocity = new Vector3(0f, 0f, 0f);
        MeshCollider.enabled = true;

        StartCoroutine("EnemyLand");
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
    }

    public void RbChasePlayer()
    {
        if (airborne)
        {
            if (playerInPosRange)
            {
                rb.AddForce((NearestGround.transform.position - transform.position).normalized * speed);
            }
        }
    }

    public void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

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

    public void RbAttackPlayer()
    {
        transform.LookAt(player);

        if (airborne)
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

    public void ResetAttack()
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, jumpRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, landingRange);
    }
}  
