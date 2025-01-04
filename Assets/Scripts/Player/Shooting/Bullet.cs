using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    internal static Shooting shootClass;
    Collider currentCollider;
    private void Start()
    {
        shootClass = FindObjectOfType<Shooting>();
        
    }

    private void Update()
    {
        foreach (Collider collider in shootClass.hitColliders)
        {
            currentCollider = collider;         
        }
        if (currentCollider != null) 
        {
            GetComponent<Rigidbody>().transform.position = Vector3.Lerp(transform.position, currentCollider.transform.position, shootClass.bulletSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
             
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
