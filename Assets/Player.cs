using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    protected string currentAnim;
    
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private float speed = 200;
    [SerializeField] private float horizontal;
    [SerializeField] private float jumpForce = 200;

    private bool isGrounded;
    private bool isJumping;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        // -1 < 0 < 1
        horizontal = Input.GetAxisRaw("Horizontal");

        isGrounded = CheckGround();
        if (isGrounded)
        {
            //Check jump
            if (isJumping)
            {
                return;
            }
            //Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                return;
            }
            //Move
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                Move();
                return;
            }
        }
        else
        {
            //Falling
            if(rb.velocity.y < 0)
            {
                Falling();
            }
        }
    }
    private void FixedUpdate()
    {
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        //Idle
        else if (isGrounded && !isJumping)
        {
            Idle();
        }
    }
    //=============Idle=================
    private void Idle()
    {
        ChangeAnim("Idle");
        rb.velocity = Vector2.up * rb.velocity.y;
    }
    //=============Move=================
    private void Move()
    {
        ChangeAnim("Move");
    }
    //=============Jump=================
    private void Jump()
    {
        if (!isGrounded)
        {
            return;
        }
        isGrounded = false;
        isJumping = true;
        ChangeAnim("Jump");
        rb.AddForce(jumpForce * Vector2.up);
    }
    private void Falling()
    {
        isJumping = false;
        ChangeAnim("Fall");
    }
    private bool CheckGround()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);
        return hit.collider != null;
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }
}
