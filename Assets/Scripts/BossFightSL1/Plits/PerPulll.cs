using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerPulll : MonoBehaviour
{
    public Transform perPlitPoint;

    public Transform arenaPlitPoint;

    public GameObject player;
    ControlPlits plit;

    List<GameObject> plitDis = new();
    List<GameObject> plitDel = new();

    SphereCollider playCol;

    public float radius = 8f;
    void Start()
    {
        plit = FindObjectOfType<ControlPlits>();

        playCol = player.AddComponent<SphereCollider>();
        playCol.isTrigger = true;
        player.GetComponent<SphereCollider>().radius = radius;

        var q = player.AddComponent<TriggersPlit>();
        q.pull = this;
       
        StartCoroutine(UpdatePlit());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
            StartCoroutine(Del());
    }
    IEnumerator Del()
    {
        plitDel.Clear();
        plitDis.Clear();

        Destroy(playCol, 0);

        foreach(Transform i in perPlitPoint.transform)
        {
            plit.UpdateColorPlits(i.gameObject);
            plit.UpdatePositionPlits(i.gameObject, false);
        }
        foreach (Transform i in arenaPlitPoint.transform)
        {
            plit.UpdateColorPlits(i.gameObject, ver: true);
            plit.UpdatePositionPlits(i.gameObject, true);
        }
        yield return new WaitForSeconds(2f);

        Destroy(perPlitPoint.gameObject, 0);

        Destroy(this.gameObject, 0);
    }

    public IEnumerator UpdatePlit()
    {
        while (true)
        {
            List<GameObject> del = new();
            foreach(var i in plitDis)
            {
                float distance = Vector3.Distance(player.transform.position, i.transform.position);
                if(distance < (radius / 4))
                {
                    plitDel.Add(i);
                    del.Add(i);
                }
            }
            foreach(var i in del)
                plitDis.Remove(i);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void OnEnter(Collider collision)
    {
        if(!plitDel.Contains(collision.gameObject))
            if (!plitDis.Contains(collision.gameObject))
            {
                plitDis.Add(collision.gameObject);
                plit.UpdateColorPlits(collision.gameObject, ver: true);
                plit.UpdatePositionPlits(collision.gameObject, true);
            }
    }
    public void OnExit(Collider collision)
    {
        if (plitDel.Contains(collision.gameObject))
        {
            plit.UpdateColorPlits(collision.gameObject);
            plitDel.Remove(collision.gameObject);
            StartCoroutine(PositionUp(collision.gameObject));
        }
    }
    IEnumerator PositionUp(GameObject ob)
    {
        yield return new WaitForSeconds(0.3f);

        plit.UpdatePositionPlits(ob, false);
    }
}
