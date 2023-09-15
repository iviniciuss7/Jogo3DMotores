using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    private CharacterController charCtrl;
    private Animator anim;

    public float smoothRotTime;
    private float turnSmoothVelocity;
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude > 0)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            charCtrl.Move(direction * speed * Time.deltaTime);
            anim.SetInteger("transition", 1);
        }
    }
}
