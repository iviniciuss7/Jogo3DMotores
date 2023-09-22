using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    private CharacterController charCtrl;
    private Animator anim;
    private Transform cam;
    private Vector3 moveDirection;
    public float gravity;
    public float smoothRotTime;
    private float turnSmoothVelocity;
    
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (charCtrl.isGrounded)
        { 
            float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude > 0)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            anim.SetInteger("transition", 1);
        }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        charCtrl.Move(moveDirection * speed * Time.deltaTime);
    }
}
