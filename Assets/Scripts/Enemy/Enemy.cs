using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]private int _hp;
    [SerializeField]private int _maxhp;
    private HealthCharcters hlthcharacters;
    [SerializeField] private int damage;

    private void Start() 
    {
        _hp = _maxhp;
        hlthcharacters = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthCharcters>();
    }
    //Получиние урона
    private void TakeDamage(int damage, bool enablelog /*<= включает и выключает логи*/)
    {
        _hp -= damage;
        if(enablelog)
            Debug.Log($"Сущность получает урон в размере {Bullet.shootClass.damage} \n Текущее здоровье: {_hp}");
    }

    private void Update()
    {
        CheckStatus();
    }

    //проверка статуса игрока
    private void CheckStatus()
    {
        if (_hp <= 0) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    //работа с коллизиями
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))    
            TakeDamage(Bullet.shootClass.damage, true);   
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            hlthcharacters.DealDamage(hlthcharacters, damage);
    }
}
