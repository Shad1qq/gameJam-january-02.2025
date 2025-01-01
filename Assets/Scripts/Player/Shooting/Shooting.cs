using DG.Tweening;
using UnityEngine;

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

    
    protected virtual void Shoot()
    {    
        GameObject bullet = Instantiate(prefab, firePoint.position, firePoint.rotation);        
    }

}
