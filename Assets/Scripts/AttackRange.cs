using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private Character owner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            Character target = collision.GetComponent<Character>();
            if(target != owner)
            {
                target.OnHit(5);
                if (owner.currentKi < 100)
                {
                    owner.OnChargeKi(10);
                }
                else
                {
                    owner.OnChargeKi(0);
                }
            }
        }
    }
}
