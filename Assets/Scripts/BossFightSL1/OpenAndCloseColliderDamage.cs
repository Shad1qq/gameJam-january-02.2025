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
        if(col.gameObject.layer == 8)
        {
            DealDamage(col.GetComponent<HealthCharcters>(), damage);
        }
        if(col.tag == "Plit")
        {
            plit.Add(col.gameObject);
        }
    }
}
