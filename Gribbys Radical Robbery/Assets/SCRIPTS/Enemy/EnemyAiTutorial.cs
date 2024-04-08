
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAiTutorial : MonoBehaviour
{
    //Physics of its body
    public NavMeshAgent agent;
    public Transform player;
    public MeshCollider MeshCollider;

    public float health;

    public Rigidbody rb;
    public float groundDrag;

    //How it moves
    public Transform ground;
    public GameObject NearestGround;
    public float speed;

    public LayerMask whatIsPlayer, enemyLandArea;

    public float minJumpHeight;
    public float maxJumpHeight;

    //Awareness
    public bool playerInJumpRange;
    public bool playerOnGround;
    public bool playerInPosRange;
    public bool enemyInPosRange;

    //Mechanics
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

    [Header("Attacks")]
    public bool dashCapable;
    public bool readyToDashAttack;
    public float cooldown;
    public float nextAttackTime;
    public bool hasDashAttacked;
    public bool isChasingPlayer;
    public bool isMelee;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public bool topBladePull;
    public bool bottomBladePull;
    public bool holdWeapons;

    //Animation
    public Animator MeleeEnemy;
    public Animator TopBladePull;
    public Animator BottomBladePull;

    //States
    public float sightRange, attackRange, jumpRange, posRange, normalAttackRange;
    public bool playerInSightRange, playerInAttackRange, playerInNormalAttackRange;

    public void Start()
    {
        readyToJump = true;
        holdWeapons = true;
    }

    public void Awake()
    {
        player = GameObject.Find("Player").transform;
        ground = GameObject.Find("Goal").transform;
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.enabled = true;
        MeshCollider.enabled = false;
    }

    public void Update()
    {
        //Check for ranges
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInJumpRange = Physics.CheckSphere(transform.position, jumpRange, whatIsPlayer);
        readyToLand = Physics.CheckSphere(transform.position, landingRange, whatIsGround);
        playerInPosRange = ground.GetComponent<Goal>().playerInPosRange;
        playerInNormalAttackRange = Physics.CheckSphere(transform.position, normalAttackRange, whatIsPlayer);

        //Health
        health = GetComponent<EnemyStats>().health;
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);

        //Physics
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
        MeshCollider = gameObject.GetComponent<MeshCollider>();

        playerOnGround = player.GetComponent<PlayerMovement>().grounded;
        grounded = Physics.Raycast(transform.position, Vector3.down, enemyHeight, whatIsGround);
        NearestGround = gameObject.GetComponent<SenseGround>().NearestGround;

        if (player.transform.position.y >= rb.transform.position.y + minJumpHeight && playerInJumpRange) preparedToJump = true;
        if (player.transform.position.y <= rb.transform.position.y + minJumpHeight && playerInJumpRange) preparedToJump = false;

        airborne = !grounded;

        landingZone = ground.GetComponent<Goal>();

        //Attacks and cooldowns
        if (holdWeapons)
        {
            topBladePull = false;
            bottomBladePull = false;
        }

        if (topBladePull)
        {
            bottomBladePull = false;
        }
        if (bottomBladePull)
        {
            topBladePull = false;
        }

        if (grounded)
        {
            MeshCollider.enabled = false;
            agent.enabled = true;
        }

        if (agent.enabled)
        {
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange && !isMelee) RangeAttackPlayer();
            if (playerInAttackRange && playerInSightRange && isMelee) TopMeleeAttackPlayer();
            if (playerInAttackRange && playerInSightRange && isMelee) BottomMeleeAttackPlayer();
        }
        else
        {
            if (playerInSightRange && !playerInAttackRange) RbChasePlayer();
            if (playerInAttackRange && playerInSightRange && !isMelee) RbRangeAttackPlayer();
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
        isChasingPlayer = true;

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

    public void RangeAttackPlayer()
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

    public void RbRangeAttackPlayer()
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

    public void TopMeleeAttackPlayer()
    {
        holdWeapons = false;
        TopBladePull.SetBool("TopBladePull", topBladePull);
        MeleeEnemy.SetBool("TopBladePull", topBladePull);
        topBladePull = true;
    }

    public void BottomMeleeAttackPlayer()
    {

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
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, normalAttackRange);
    }
}  
