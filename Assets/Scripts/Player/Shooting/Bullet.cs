using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private Shooting shootClass;
    private Enemy enemy;
    private void Start()
    {
        shootClass = FindObjectOfType<Shooting>();
        enemy = FindObjectOfType<Enemy>();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * shootClass.bulletSpeed);
        if (Vector3.Distance(transform.position, shootClass.gameObject.transform.position) >= shootClass.maxDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemy.TakeDamage(25);
            Destroy(gameObject);
        }
    }
}
