using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : Character
{
    public bool isAttack;
    public bool isMoving = false;
    public bool isRight = true;

    [SerializeField] CapsuleCollider2D capsuleCollider;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] private SpriteRenderer sprites;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    private IState currentState;
    private Character target;
    public Character Target => target;
    public override void OnInit()
    {
        base.OnInit();
        capsuleCollider.enabled = true;
        boxCollider.enabled = false;
        ChangeState(new IdleState());
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        //Destroy(gameObject);
    }

    protected override void OnDeath()
    {
        ChangeState(null);
        capsuleCollider.enabled = false;
        boxCollider.enabled = true;
        base.OnDeath();
    }

    private void Update()
    {
        if (currentState != null && !isDead)
        {
            currentState.OnExecute(this);
        }
    }
    private void FixedUpdate()
    {
        if (isDead == true)
        {
            return;
        }
        if (isMoving)
        {
            ChangeAnim("patrol");
            rb.velocity = transform.right * moveSpeed;
        }
        else if (isAttack == false)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        sprites.color = Color.red;
        Invoke(nameof(ResetColor), 0.5f);
    }
    private void ResetColor()
    {
        sprites.color = Color.white;
    }
    internal void SetTarget(Character character)
    {
        this.target = character;
        if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }
    public void Attack()
    {
        rb.velocity = Vector2.zero;
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeactiveAttack), 0.5f);
    }
    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;

        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyWalls"))
        {
            ChangeDirection(!isRight);
        }
    }
    private void ActiveAttack()
    {
        isAttack = true;
    }
    private void DeactiveAttack()
    {
        isAttack = false;
    }
}
