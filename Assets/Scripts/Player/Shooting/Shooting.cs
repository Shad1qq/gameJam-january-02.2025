using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public abstract class Shooting : MonoBehaviour
{
    [Header("Shoot Settings")]
    [SerializeField]protected float coolDown;
    [SerializeField]protected float startcoolDown;
    [SerializeField]internal int damage;
    [SerializeField]internal float maxDistance;
    [SerializeField]internal float bulletSpeed;
    [SerializeField]protected GameObject prefab;
    [SerializeField]protected Transform firePoint;
    [SerializeField] protected float radius;
    internal float angle;
    internal Collider[] hitColliders;

    protected virtual void Shoot()
    {    
        GameObject bullet = Instantiate(prefab, firePoint.position, firePoint.rotation);        
    }
    protected virtual void FireRayCasts()
    {
        hitColliders = Physics.OverlapSphere(transform.position, radius, 1<<7);
        
        foreach (Collider collider in hitColliders) 
        {
            Vector3 targetPos = collider.transform.position;
            targetPos.y = transform.position.y;

            Vector3 targetDir = targetPos - transform.position;

            angle = Vector3.Angle(transform.position, targetDir);

            
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,0.3f);
        
        Gizmos.DrawSphere(transform.position, radius);
    }

}
