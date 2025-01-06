using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PerPulll : MonoBehaviour
    {
        public GameObject particlePref;

        public Transform perPlitPoint;

        public Transform arenaPlitPoint;

        public GameObject player;
        ControlPlits plit;

        List<GameObject> plitDis = new();
        List<GameObject> plitDel = new();

        SphereCollider playCol;

        public event Action arenaTry;

        public float yAr = 4f;

        public float radius = 8f;

        List<GameObject> partPull = new();

        void Start()
        {
            for(int i = 0; i < 5; i++)
            {
                var r = Instantiate(particlePref);
                r.SetActive(false);
                partPull.Add(r);
            }

            plit = FindObjectOfType<ControlPlits>();

            playCol = player.AddComponent<SphereCollider>();
            playCol.isTrigger = true;
            playCol.radius = radius;
            playCol.layerOverridePriority = 5;
            playCol.includeLayers = 11;
            playCol.excludeLayers = ~(1 << 11);

            var q = player.AddComponent<TriggersPlit>();
            q.pull = this;

            var tr = arenaPlitPoint.transform;

            StartCoroutine(UpdatePlit());
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 8)
            {
                arenaTry?.Invoke();

                StopAllCoroutines();
                plit.StopAllCoroutines();
                StartCoroutine(Del());
            }
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
                plit.UpdatePositionPlits(i.gameObject, true, z: yAr);
            }
            yield return new WaitForSeconds(3f);

            Destroy(perPlitPoint.gameObject, 0);
            foreach(var i in partPull)
                Destroy(i, 0);
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
                    if(distance < (radius / 5.5))
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
            GameObject par = null;

            yield return new WaitForSeconds(0.3f);

            foreach(var i in partPull)
            {
                if (!i.activeInHierarchy)
                {
                    i.transform.position = ob.transform.position;
                    i.SetActive(true);
                    i.GetComponent<ParticleSystem>().Play();
                    par = i;
                    break;
                }
            }

            plit.UpdatePositionPlits(ob, false);

            yield return new WaitForSeconds(0.5f);
            if(par != null)
                par.SetActive(false);
        }
    }
}
