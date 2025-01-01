using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _hp;
    private int _maxhp;

    private void Start()
    {
        _hp = _maxhp;
    }

    internal void TakeDamage(int damage)
    {
        _hp -= damage;
    }

    private void Update()
    {
        if(_hp <= 0)
        {
            isDead();
        }
    }

    void isDead()
    {
        Destroy(gameObject);
    }
}
