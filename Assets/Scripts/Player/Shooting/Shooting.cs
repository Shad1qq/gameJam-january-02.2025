using UnityEngine;

public abstract class Shooting : MonoBehaviour
{
    [Header("Shoot Settings")]
    public float coolDown;
    [SerializeField] protected float startcoolDown;
    [SerializeField] internal int damage;
    [SerializeField] internal float maxDistance;
    [SerializeField] internal float bulletSpeed;
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float radius;
    internal Vector3 targetPos;
    internal Collider[] hitColliders;

    public bool Gizmo = false;

    protected virtual void Shoot()
    {    
        Instantiate(prefab, firePoint.position, firePoint.rotation);        
    }
    protected virtual void FireRayCasts()
    {
        hitColliders = Physics.OverlapSphere(transform.position, radius, 1<<7); 
    }

    private void OnDrawGizmos()
    {
        if (!Gizmo)
            return;

        Gizmos.color = new Color(1,0,0,0.3f);
        
        Gizmos.DrawSphere(transform.position, radius);
    }

}
