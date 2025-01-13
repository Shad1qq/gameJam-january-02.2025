using System;
using System.Collections;
using UnityEngine;

namespace SA
{
    public class DamageObj : MonoBehaviour
    {
        internal Transform spawnPos;

        public event Action DamageBoss;

        public AudioClip audi;
        AudioSource au;

        public ParticleSystem particle;
        public ParticleSystem particleDead;

        internal ArenaController contr;

        Coroutine cor;
        private void Start()
        {
            if(au == null)
            {
                au = gameObject.AddComponent<AudioSource>();
                au.clip = audi;
            }
        }
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
            if(particleDead != null)
                particleDead.Play();
            au.Play();

            StopCoroutine(cor);

            yield return new WaitForSeconds(0.2f);

            contr.spawnDamageObjTransform.Add(spawnPos);
            spawnPos = null;

            GetComponent<SphereCollider>().enabled = false;
            DamageBoss?.Invoke();
            gameObject.SetActive(false);
        }
    }
}