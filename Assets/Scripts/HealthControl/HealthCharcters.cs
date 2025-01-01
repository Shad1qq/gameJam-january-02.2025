using System;
using UnityEngine;

public abstract class HealthCharcters : MonoBehaviour
{
    [Header("Character Health")]
    [SerializeField] private bool health = true;
    [SerializeField] protected int _maxHealth;

    [SerializeField] private float noDamageState = 0.5f;

    public event Action DamageReaction;
    public event Action DeadReaction;

    float timeDamage = 0;
    internal void DealDamage(HealthCharcters target, int damage)
    {
        if (Time.time - timeDamage >= noDamageState && health)
        {
            timeDamage = Time.time;

            Debug.Log($"{gameObject.name} наносит {damage} урона {target.gameObject.name}");

            target.TakeDamage(damage);
        }
    }

    public void Health(int i)
    {
        _maxHealth = i;
    }

    private void TakeDamage(int damage)
    {
        _maxHealth -= damage;
        Debug.Log($"{gameObject.name} получил {damage} урона. Осталось здоровья: {_maxHealth}");

        DamageReaction?.Invoke();

        if (!IsAlive())
            Die();
    }
    private bool IsAlive()
    {
        return _maxHealth > 0;
    }
    private void Die()
    {
        DeadReaction?.Invoke();
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} погиб");
    }
}
