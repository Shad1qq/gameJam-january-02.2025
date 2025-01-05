using UnityEngine;

public class Bullet : MonoBehaviour
{
    internal static Shooting shootClass;
    Collider currentCollider;
    float closestDistance = Mathf.Infinity;
    internal float angle;
    private Transform playerTr;
    
    private void Start()
    {
        shootClass = FindObjectOfType<Shooting>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    private void Update()
    {
        foreach (Collider collider in shootClass.hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance) 
            {
                closestDistance = distance;
                currentCollider = collider;           
            }
            Vector3 currentRotation = new Vector3(currentCollider.transform.position.x, playerTr.position.y, currentCollider.transform.position.z);
            playerTr.LookAt(currentRotation);
            

        }
        if (currentCollider != null) 
            GetComponent<Rigidbody>().transform.position = Vector3.Lerp(transform.position, currentCollider.transform.position, shootClass.bulletSpeed * Time.deltaTime);
        else
            Destroy(gameObject);

        if (Vector3.Distance(transform.position, shootClass.gameObject.transform.position) >= shootClass.maxDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 8)
            Destroy(gameObject);
    }
}
