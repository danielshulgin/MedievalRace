using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatStats : MonoBehaviour
{
    [SerializeField]
    public int hp { get; private set; } = 100;

    public event Action Die = () => { };

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            Die();
            Debug.Log("Die!!!");
        }
    }

}
