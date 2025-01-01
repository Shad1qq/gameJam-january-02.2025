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
        GetComponent<Rigidbody>().transform.Translate(Vector3.forward * shootClass.bulletSpeed);
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
