using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    public Slider kiBar;
    public Slider healthBar;
    public void SetMaxKi(int kiCharge)
    {
        kiBar.maxValue = kiCharge;
        kiBar.value = kiCharge;
    }
    public void SetKi(int kiCharge)
    {
        kiBar.value = kiCharge;
    }
    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }
    public void SetHealth(int health)
    {
        healthBar.value = health;
    }
}
