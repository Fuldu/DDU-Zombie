using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum ZombieStates { Idle, Patrolling, Chasing }
public class Enemy : MonoBehaviour
{



    [Header("Status")]
    public int Health;
    public ZombieStates ActiveState;
    public Vector2 PatrolPos;
    public bool SeesPlayer;
    public int UnfocusTimer;
    public int StartPatrolTimer;


    [Header("Config")]
    public float ChaseSpeed;
    public int defUnfocusTime;
    public float DefaultVisionRange;
    public float ChaseVisionRange;



    [Header("Patrol State")]
    public float PatrolSpeed;
    public float MinDistToNewPatrolPoint;
    public float MaxDistToNewPatrolPoint;
    public float DistForNewPatrolPoint;
    public float DistToChangePoint;
    public int defStartPatrolTime;
    public LayerMask patrolPointTestingMask;


    [Header("Drops")]
    public GameObject GoldDropPre;
    public int ChanceToDrop;
    public float DropVelocity;



    [Header("Misc")]
    public LayerMask visionMask;


    private GameObject Player;


    Rigidbody2D rb;

    private void Start()
    {
        Player = FindObjectOfType<Player>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }



    private void Update()
    {

        float activeVisionRange = DefaultVisionRange;

        Vector2 playerDir = new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y - transform.position.y).normalized;


        if (ActiveState == ZombieStates.Chasing)
        {
            transform.up = playerDir;
            rb.velocity = transform.up * ChaseSpeed;

            activeVisionRange = ChaseVisionRange;
        }
        else if (ActiveState == ZombieStates.Patrolling)
        {

            Vector2 patrolPointDir = new Vector2(PatrolPos.x - transform.position.x, PatrolPos.y - transform.position.y).normalized;

            transform.up = patrolPointDir;
            rb.velocity = transform.up * PatrolSpeed;


            float distToPatrolPoint = Vector2.Distance(transform.position, PatrolPos);

            if (distToPatrolPoint <= DistForNewPatrolPoint)
            {

                StartIdle();
            }
            else if (distToPatrolPoint > DistToChangePoint)
            {
                NewPatrolPoint();
            }
        }
        else if (ActiveState == ZombieStates.Idle)
        {
            rb.velocity = Vector2.zero;
        }



        Debug.DrawRay(transform.position, playerDir * activeVisionRange, Color.green);
        RaycastHit2D visionCast = Physics2D.Raycast(transform.position, playerDir, activeVisionRange, visionMask);


        // If visionCast has hit something
        if (visionCast)
        {
            //Debug.Log(test.collider.gameObject.name);
            if (visionCast.collider.GetComponent<Player>())
            {
                SeesPlayer = true;
                Alert();
            }
            else
            {
                SeesPlayer = false;
            }
            
        }
        else
        {
            SeesPlayer = false;
        }


        



    }



    private void FixedUpdate()
    {
        if (!SeesPlayer && UnfocusTimer > 0)
        {
            UnfocusTimer--;
        }

        if (ActiveState == ZombieStates.Chasing)
        {
            if (UnfocusTimer <= 0)
            {
                StartIdle();
            }
        }



        if (ActiveState == ZombieStates.Idle)
        {
            StartPatrolTimer--;
            if (StartPatrolTimer <= 0)
            {
                StartPatrol();
            }
        }



    }






    private void NewPatrolPoint()
    {


        while (true)
        {
            Vector2 testingPoint = new Vector2(UnityEngine.Random.Range(-MaxDistToNewPatrolPoint, MaxDistToNewPatrolPoint),
                UnityEngine.Random.Range(-MaxDistToNewPatrolPoint, MaxDistToNewPatrolPoint));

            //Debug.Log(testingPoint);



            testingPoint = new Vector2(testingPoint.x + transform.position.x, testingPoint.y + transform.position.y);


            Vector2 dirToPoint = new Vector2(testingPoint.x - transform.position.x, testingPoint.y - transform.position.y).normalized;
            RaycastHit2D testingCast = Physics2D.Raycast(transform.position, dirToPoint, Vector2.Distance(transform.position, testingPoint) + 0.5f, patrolPointTestingMask);


            if (!testingCast)
            {
                PatrolPos = testingPoint;
                break;
            }

        }

    }


    private void StartPatrol()
    {
        ActiveState = ZombieStates.Patrolling;
        NewPatrolPoint();
    }


    private void StartIdle()
    {
        ActiveState = ZombieStates.Idle;
        StartPatrolTimer = defStartPatrolTime;
    }



    public void Alert()
    {
        //Debug.Log(gameObject.name + " is alerted");

        UnfocusTimer = defUnfocusTime;
        ActiveState = ZombieStates.Chasing;

    }


    public void TakeDamage(int dmg)
    {
        if (Health - dmg <= 0)
        {
            Die();
        }
        else
        {
            Health -= dmg;
        }
    }


    private void Die()
    {
        if (UnityEngine.Random.Range(1, 101) <= ChanceToDrop)
        {
            GameObject newDrop = Instantiate(GoldDropPre, transform.position, Quaternion.identity);

        }

        Destroy(gameObject);
    }


}
