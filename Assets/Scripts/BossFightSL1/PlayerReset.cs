using UnityEngine;

public class PlayerReset : HealthCharcters
{
    public GameObject positionSpawn;
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = positionSpawn.transform.position;
            DealDamage(other.GetComponent<HealthCharcters>(), damage);
            positionSpawn.SetActive(true);
            positionSpawn.GetComponent<ParticleSystem>().Play();
            Invoke(nameof(DeletEffect), 2f);
        }
    }
    void DeletEffect()
    {
        positionSpawn.SetActive(false);
    }
}
