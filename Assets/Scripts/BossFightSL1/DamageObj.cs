using System;
using System.Collections;
using UnityEngine;

namespace SA
{
    public class DamageObj : MonoBehaviour
    {
        internal Transform spawnPos;

        public event Action DamageBoss;

        public ParticleSystem particle;
        internal ArenaController contr;

        Coroutine cor;
        private void OnEnable()
        {
            cor = StartCoroutine(Part());
            GetComponent<SphereCollider>().enabled = true;

            if (spawnPos != null)
                transform.position = spawnPos.position;
        }
        IEnumerator Part()
        {
            while (true)
            {
                particle.Play();
                yield return new WaitForSeconds(2f);
            }
        }
        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                GetComponent<SphereCollider>().enabled = false;

                StartCoroutine(DamagePut());
            }
        }
        IEnumerator DamagePut()
        {
            StopCoroutine(cor);

            contr.spawnDamageObjTransform.Add(spawnPos);
            spawnPos = null;

            yield return null;

            GetComponent<SphereCollider>().enabled = false;
            gameObject.SetActive(false);
            DamageBoss?.Invoke();
        }
    }
}