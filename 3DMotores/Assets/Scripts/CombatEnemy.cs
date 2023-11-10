using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class CombatEnemy : MonoBehaviour
{
    [Header("Atributos")]
    public float totalHealth = 40;
    public int attackDamage;
    public float speedMove;
    public float lookRadius;
    public float colliderRadius = 2f;
    
    [Header("Componentes")] 
    private Animator animEnemy;
    private CapsuleCollider  collider;
    private NavMeshAgent navEnemy;
    private Transform target;

    [Header("Booleanos")] 
    private bool walking;
    private bool attacking;
    private bool waitFor;
    private bool hiting;
    private bool playerIsDead;

    [Header("WayPoints")]
    public List<Transform> wayPoint = new List <Transform>();
    public int currentPathIndex;
    public float pathDistance;
    
    
    void Start()
    {
        animEnemy = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
        navEnemy = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        if (totalHealth > 0)
        {
            float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            navEnemy.isStopped = false;
            if (!attacking)
            {
                navEnemy.SetDestination(target.position);
                animEnemy.SetBool("Walk Forward", true);
                walking = true;
            }
            
            
            
            if (distance <= navEnemy.stoppingDistance)
            {
                //attack
                StartCoroutine("AttackEnemy");

            }

            else
            {
                attacking = false;
            }
        }

        else
        {
            //navEnemy.isStopped = true;
            animEnemy.SetBool("Walk Forward", false);
            walking = false;
            attacking = false;
            MoveToWayPoint();
        }
        } 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void MoveToWayPoint(){
        if (wayPoint.Count > 0){ 
            float distance = Vector3.Distance(wayPoint[currentPathIndex].position, transform.position);
            navEnemy.destination = wayPoint[currentPathIndex].position;
            
            if (distance <= pathDistance){
                currentPathIndex = UnityEngine.Random.Range(0, wayPoint.Count); 
            } 
            animEnemy.SetBool("Walk Forward", true);
            walking = true;
        }

    }

    private IEnumerator AttackEnemy()
    {
        if (!waitFor && !hiting  && !playerIsDead)
        {
            waitFor = true;
            attacking = true;
            walking = false;
            animEnemy.SetBool("Walk Forward", false);
            animEnemy.SetBool("Bite Attack", true);
        
            yield return new WaitForSeconds(1.2f);
            GetPlayer();
                
           // yield return new WaitForSeconds(1f);
            waitFor = false;
        }
        if (playerIsDead){
            animEnemy.SetBool("Walk Forward", false);
            animEnemy.SetBool("Bite Attack", false);
            walking = false;
            attacking = false;
            navEnemy.isStopped = true;
        }
        
    }

    void GetPlayer()
    {
        foreach (Collider coll in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                coll.gameObject.GetComponent<PlayerControl>().GetHit(attackDamage);
               playerIsDead = coll.gameObject.GetComponent<PlayerControl>().isDead;
            }
        }
    }

    public void GetHit(float damage)
    {
        totalHealth -= damage;
                
        if (totalHealth > 0)
        {
            StopCoroutine("AttackEnemy");
            animEnemy.SetTrigger("Take Damage");
            hiting = true;
            StartCoroutine("RecoveryHit");
        }

        else
        {
            animEnemy.SetTrigger("Die"); 
        }
    }

    IEnumerator RecoveryHit()
    {
        yield return new WaitForSeconds(1f);
        animEnemy.SetBool("Walk Forward", false);
        animEnemy.SetBool("Bite Attack", false);
        hiting = false;
        waitFor = false;

    }

}
