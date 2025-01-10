using System.Collections.Generic;
using UnityEngine;

public class OpenAndCloseColliderDamage : HealthCharcters
{
    BoxCollider colider => GetComponent<BoxCollider>();

    public List<GameObject> plit = new();
    public int damage = 10;

    private void Start()
    {
        colider.enabled = false;
    }
    public void CloseCollider()
    {
        colider.enabled = false;
    }
    public void OpenColloder()
    {
        colider.enabled = true;
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == 8 && col.GetComponent<HealthCharcters>())
        {
            DealDamage(col.GetComponent<HealthCharcters>(), damage);
        }
        if(col.CompareTag("Plit"))
        {
            plit.Add(col.gameObject);
        }
    }
}
