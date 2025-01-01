using SA;
using System.Collections;
using UnityEngine;

public class ContactAtack : HealthCharcters
{
    [Header("Enemy Attack Settings")]
    [SerializeField] private int _enemyDamage = 10;

    GameObject player = null;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            StartCoroutine(nameof(DamagePlayer));
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
    private IEnumerator DamagePlayer()
    {
        while (true)
        {
            if (player != null)
                Attack(player);
            else
                break;
            yield return new WaitForSeconds(1f);
        }
    }
    private void Attack(GameObject ob)
    {
         DealDamage(ob.GetComponent<StatManager>(), _enemyDamage);
    }
}
