using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    [Header("Atributos")]
    public float speed;
    public float gravity;
    public float damage = 20;
    public int healthPlayer = 100;
    
    [Header("Componentes")]
    private CharacterController charCtrl;
    private Transform cam;
    private Vector3 moveDirection;
    private Animator anim;
    public AudioSource srcPlayer;
    public AudioClip clipP1;
    public AudioClip clipP2;
    
    [Header("Others")]
    public float smoothRotTime;
    private float turnSmoothVelocity;
    public float colliderRadius;
    public List<Transform> enemyList = new List<Transform>();
    public bool isWalking;
    public bool waitFor;
    public bool isHitting;
    public bool isDead;

    void Start()
    {
        GameController.instance.UpdateLives(healthPlayer);
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead){
         Move();
         GetMouseInput();
        }
     
    }


    void Move(){
        
        if (charCtrl.isGrounded)
        { 
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical);
            

        if (direction.magnitude > 0)
        {
            if (!anim.GetBool("attacking"))
            {
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
                moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward  * speed;
                anim.SetInteger("transition", 1);
                isWalking = true;
            }

            else
            {
                anim.SetBool("walking", false);
                moveDirection = Vector3.zero;
            }
             
        }
        
        else if (isWalking)
        {
            anim.SetBool("walking", false);
            moveDirection = Vector3.zero;
            anim.SetInteger("transition", 0);
            isWalking = false;
        }
        
        }
        moveDirection.y -= gravity * Time.deltaTime;
        charCtrl.Move(moveDirection * Time.deltaTime);
    }

    void GetMouseInput()
    {
        if (charCtrl.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (anim.GetBool("walking"))
                {
                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                }

                if (!anim.GetBool("walking"))
                {
                    StartCoroutine("Attack");
                    srcPlayer.PlayOneShot(clipP2);
                }
            }
        }
    }

    IEnumerator Attack()
    {
        if (!waitFor && !isHitting){
            waitFor = true;
            anim.SetBool("attacking", true);
            anim.SetInteger("transition", 2);
            yield return new WaitForSeconds(0.5f);
            
            GetEnemiesList();
        
            foreach (Transform enemys in enemyList){
                CombatEnemy enemy = GetComponent<CombatEnemy>();

                if (enemy != null)
                {
                    enemy.GetHit(damage);
                }
            } 
            yield return new WaitForSeconds(1.25f);
        
            anim.SetInteger("transition", 0);
            anim.SetBool("attacking", false);
            waitFor = false;

        }
    }

    void GetEnemiesList()
    {
        enemyList.Clear();
        foreach (Collider coll in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            if (coll.gameObject.CompareTag("Enemy"))
            {
                enemyList.Add(coll.transform);
                //GetHit();
            }
        }
    }

    public void GetHit(int damage){
        healthPlayer -= damage;

        if(healthPlayer > 0){
            StopCoroutine("Attack");
            anim.SetInteger("transition", 3);
            isHitting = true;
            StartCoroutine("RecoveryFromHit");
        }
        
        else{
            isDead = true;
            anim.SetTrigger("Die");
        }
    }

    IEnumerator RecoveryFromHit(){
        yield return new WaitForSeconds(1f);
        anim.SetInteger("transition", 0);
        isHitting = false;
        anim.SetBool("attacking", false); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }

}