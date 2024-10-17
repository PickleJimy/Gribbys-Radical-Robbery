
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEditor;

public class EnemyAiTutorial : MonoBehaviour
{
    //Physics of its body
    public NavMeshAgent agent;
    public Transform player;
    public GameObject nearestPoint;
    public MeshCollider MeshCollider;

    public float health;

    public Rigidbody rb;
    public float groundDrag;

    //How it moves
    public Transform ground;
    public GameObject NearestGround;
    public GameObject NextNearestGround;
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
    public float cooldown;
    public float nextAttackTime;
    public bool hasDashAttacked;
    public bool isChasingPlayer;
    public bool isMelee;
    public bool isRanged;
    public float projectileSpeed;
    public float upwardForce;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    public bool alreadyAttacked;
    public GameObject projectile;

    //Animation
    Animator _animator;
    string _currentState;
    const string MELEE_ENEMY_ATTACK = "Blade_Attack";
    public Animator MeleeEnemy;
    public Animator Blade;

    //States
    public float angle, visionAngle, sightRange, attackRange, jumpRange, posRange, discomfortRange;
    public bool canSeePlayer, playerInVisionZone, playerInAttackRange, playerInDiscomfortRange, isAttackingPlayer, attackingEnemy;

    public void Start()
    {
        readyToJump = true;

        //Animation
        _animator = gameObject.GetComponent<Animator>();
        Blade.enabled = false;
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
        nearestPoint = gameObject.GetComponent<FindPlayerDirectionPoints>().NearestPoint;

        //Check for ranges
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInJumpRange = Physics.CheckSphere(transform.position, jumpRange, whatIsPlayer);
        readyToLand = Physics.CheckSphere(transform.position, landingRange, whatIsGround);
        playerInPosRange = ground.GetComponent<Goal>().playerInPosRange;
        playerInDiscomfortRange = Physics.CheckSphere(transform.position, discomfortRange, whatIsPlayer);

        //Sight
        canSeePlayer = gameObject.GetComponent<FieldOfView>().canSeePlayer;

        angle = gameObject.GetComponent<FieldOfView>().angle;

        if (canSeePlayer)
        {
            angle = 360;
            sightRange = 10;
        }

        if (!canSeePlayer)
        {
            angle = 140;
            sightRange = 0;
        }

        if (canSeePlayer && !playerInVisionZone)
        {
            transform.Rotate(player.position);
        }

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
        NextNearestGround = gameObject.GetComponent<SenseGround>().NearestGround;

        if (player.transform.position.y >= rb.transform.position.y + minJumpHeight && playerInJumpRange) preparedToJump = true;
        if (player.transform.position.y <= rb.transform.position.y + minJumpHeight && playerInJumpRange) preparedToJump = false;

        airborne = !grounded;

        landingZone = ground.GetComponent<Goal>();

        if (grounded)
        {
            agent.enabled = true;
            MeshCollider.enabled = false;
        }

        //Attacks and cooldowns
        
        if (agent.enabled)
        {
            if (!canSeePlayer && !playerInAttackRange) Patroling();
            if (canSeePlayer && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && canSeePlayer && !isMelee && isRanged) RangeAttackPlayer();
            if (playerInAttackRange && canSeePlayer && isMelee) MeleeAttackPlayer();
        }

        if (isAttackingPlayer)
        {
            attackingEnemy = true;
        }

        // when to jump
        if (readyToJump && grounded && preparedToJump && playerOnGround && canSeePlayer && enemyInPosRange)
        {
            EnemyJump();

            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;

            Debug.Log(rb.velocity.magnitude);
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (newState == _currentState)
        {
            return;
        }

        _animator.Play(newState);
        _currentState = newState;
    }

    bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
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

    public void RangeAttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        isAttackingPlayer = true;

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        //Backing up from player
        if (playerInDiscomfortRange && canSeePlayer)
        {

        }
    }

    public void MeleeAttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        Blade.enabled = true;
        Blade.SetBool(MELEE_ENEMY_ATTACK, true);

        isAttackingPlayer = true;

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;

        Blade.enabled = false;
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, jumpRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, landingRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, discomfortRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}  
