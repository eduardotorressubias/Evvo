using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent agent;

    public PlayerController player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;


    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public float AlturaEnemigo; 

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    private Vector3 PosProjectile;
    public float yProject;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    Vector3 playerLook;


    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //check vision y rango de ataque
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if(playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }


    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);


            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //WalkPoint reached
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
            }
        }

    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);

    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        playerLook = new Vector3(player.transform.position.x, AlturaEnemigo, player.transform.position.z);
        transform.LookAt(playerLook);

        if (!alreadyAttacked)
        {
            //Attack code here
            PosProjectile = new Vector3(transform.position.x, transform.position.y + yProject, transform.position.z);
            Rigidbody rb = Instantiate(projectile, PosProjectile+(transform.forward*1.2f), Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            //
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

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), .5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Attack")
        {
            Debug.Log("Me ha dado");
            TakeDamage(1);
        }
    }
    private void OnDrawGiszmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }

    // Update is called once per frame
   
}
