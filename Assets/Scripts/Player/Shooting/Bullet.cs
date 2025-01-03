using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    internal static Shooting shootClass;
    
    private void Start()
    {
        shootClass = FindObjectOfType<Shooting>();
        
    }

    private void Update()
    {
        
        if (shootClass.hitColliders.Length > 0) 
        {
            GetComponent<Rigidbody>().transform.Translate(new Vector3(0, shootClass.angle) * shootClass.bulletSpeed);
        }
        else GetComponent<Rigidbody>().transform.Translate(Vector3.forward * shootClass.bulletSpeed);
        if (Vector3.Distance(transform.position, shootClass.gameObject.transform.position) >= shootClass.maxDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
