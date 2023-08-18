using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{
    [SerializeField] private SpriteRenderer sprites;
    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        sprites.color = Color.red;
        Invoke(nameof(ResetColor), 0.5f);
    }
    public void ResetColor()
    {
        sprites.color = Color.white;
    }
}
