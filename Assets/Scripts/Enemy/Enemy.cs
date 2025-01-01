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
    //��������� �����
    private void TakeDamage(int damage, bool enablelog /*<= �������� � ��������� ����*/)
    {
        _hp -= damage;
        if(enablelog)
            Debug.Log($"�������� �������� ���� � ������� {Bullet.shootClass.damage} \n ������� ��������: {_hp}");
    }

    private void Update()
    {
        CheckStatus();
    }

    //�������� ������� ������
    private void CheckStatus()
    {
        if (_hp <= 0) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    //������ � ����������
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
