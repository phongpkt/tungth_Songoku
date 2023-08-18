using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ParticleSystem effect;

    [SerializeField] CapsuleCollider2D capsuleCollider;
    [SerializeField] BoxCollider2D boxCollider;

    [SerializeField] private float speed = 200;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float jumpForce = 200;

    [SerializeField] private Projectile skillProjectile;
    [SerializeField] private Projectile ultiProjectile;
    [SerializeField] private Transform firePosition;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isFlying;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isAttack;
    [SerializeField] private bool isCharging;

    //Cooldown time between attacks (in seconds)
    private float cooldown = 0.3f;
    //Max number of attacks in combo
    private int maxCombo = 3;
    //Current combo
    private int comboAttack = 0;
    //Time of last attack
    private float attackTimer;

    private void Awake()
    {
        effect.Stop();
    }
    public override void OnInit()
    {
        base.OnInit();
        capsuleCollider.enabled = true;
        boxCollider.enabled = false;
    }
    protected override void OnDeath()
    {
        capsuleCollider.enabled = false;
        boxCollider.enabled = true;
        base.OnDeath();
    }
    void Update()
    {
        // -1 < 0 < 1
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
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
            //attack
            if (Input.GetKeyDown(KeyCode.J))
            {
                rb.velocity = Vector2.zero;
                PunchCombo();
                return;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                rb.velocity = Vector2.zero;
                KickCombo();
                return;
            }
            //Skill 1
            if (Input.GetKeyDown(KeyCode.U))
            {
                rb.velocity = Vector2.zero;
                FireSkill();
                return;
            }
            //Skill 2
            if (Input.GetKeyDown(KeyCode.I))
            {
                rb.velocity = Vector2.zero;
                UltiSkill();
                return;
            }
            if (isAttack)
            {
                if ((Time.time - attackTimer) > cooldown)
                {
                    ResetAttack();
                }
                return;
            }
            //Charging
            if (isCharging)
            {
                Charging();
            }
            if(Input.GetKeyDown(KeyCode.L))
            {
                isCharging = true;
                effect.Play();
                return;
            }
            if (Input.GetKeyUp(KeyCode.L))
            {
                isCharging = false;
                effect.Stop();
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
            //Jump
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    Fly();
            //    return;
            //}
        }
    }
    private void FixedUpdate()
    {
        if (isAttack || isDead) return;
        //Move on ground
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if (isGrounded && !isJumping && !isAttack)
        {
            Idle();
        }
    }
    //=============Idle=================
    #region Idle
    private void Idle()
    {
        ChangeAnim("Idle");
        rb.velocity = Vector2.up * rb.velocity.y;
    }
    #endregion
    //=============Move=================
    #region Move
    private void Move()
    {
        ChangeAnim("Move");
    }
    #endregion
    //=============Jump=================
    #region Jump
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
    #endregion
    //=============Jump=================
    #region Fly
    private void Fly()
    {
        isFlying = true;
        isGrounded = false;
    }
    #endregion
    //=============Fall=================
    #region Fall
    private void Falling()
    {
        isJumping = false;
        ChangeAnim("Fall");
    }
    #endregion
    //=============Attack=================
    #region Attack
    private void FireSkill()
    {
        if (currentKi < 20)
        {
            return;
        }
        isAttack = true;
        if (attackTimer < Time.time)
        {
            attackTimer = Time.time + cooldown + 1f;
        }
        ChangeAnim("Skill1");
        OnSkill(20);

        Instantiate(skillProjectile, firePosition.position, firePosition.rotation);
    }
    private void UltiSkill()
    {
        if(currentKi < 100)
        {
            return;
        }
        isAttack = true;
        if (attackTimer < Time.time)
        {
            attackTimer = Time.time + cooldown + 2f;
        }
        ChangeAnim("Skill2");
        OnSkill(100);

        Instantiate(ultiProjectile, firePosition.position, firePosition.rotation);
    }
    private void PunchAttack(int combo)
    {
        ChangeAnim("Punch" + combo);
    }
    private void PunchCombo()
    {
        isAttack = true;
        if (attackTimer < Time.time && comboAttack < maxCombo)
        {
            comboAttack++;
            //Debug.Log("Punch" + comboAttack);
            attackTimer = Time.time + cooldown;
        }
        switch (comboAttack)
        {
            case 1:
                PunchAttack(comboAttack);
                break;
            case 2:
                PunchAttack(comboAttack);
                break;
            case 3:
                PunchAttack(comboAttack);
                break;
        }
    }
    private void KickAttack(int combo)
    {
        ChangeAnim("Kick" + combo);
    }
    private void KickCombo()
    {
        isAttack = true;
        if (attackTimer < Time.time && comboAttack < maxCombo)
        {
            comboAttack++;
            attackTimer = Time.time + cooldown;
        }
        switch (comboAttack)
        {
            case 1:
                KickAttack(comboAttack);
                break;
            case 2:
                KickAttack(comboAttack);
                break;
            case 3:
                KickAttack(comboAttack);
                break;
        }
    }
    private void ResetAttack()
    {
        isAttack = false;
        comboAttack = 0;
        ChangeAnim("Idle");
    }
    #endregion
    //=============Charging=================
    #region ChargeKi
    private void Charging()
    {
        ChangeAnim("ChargeKi");
        int time = 0;
        time += (int)Time.time;
        if(currentKi < 100)
        {
            OnChargeKi(time);
        }
        else
        {
            OnChargeKi(0);
        }
    }
    #endregion
    //=============CheckGround=================
    private bool CheckGround()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);
        return hit.collider != null;
    }
}
