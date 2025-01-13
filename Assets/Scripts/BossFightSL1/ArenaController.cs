using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SA
{
    public class ArenaController : MonoBehaviour
    {
        [Header("audio")]
        public AudioClip perehodClip;
        public AudioClip updateModClip;
        public AudioClip mainClip;

        Volume render;

        [Header("damage Obj")]
        public GameObject prefabDamageObj;
        public List<Transform> spawnDamageObjTransform = new();
        GameObject[] pullDamageObj = new GameObject[3];

        [Header("rukBoss")]
        public GameObject prefabRukBoss;
        GameObject[] rukBoss = new GameObject[4];
        public Transform[] positionRukStart = new Transform[6];
        public Transform[] statPos = new Transform[6];

        [Header("boss")]
        public GameObject prefabBoss;
        public Transform[] positionBoss1and2 = new Transform[3];
        GameObject[] boss = new GameObject[2];
        public Transform[] targetPointStartBoss =  new Transform[3];

        GameObject player;

        [Header("camera")]
        public GameObject cameraTarget;
        public GameObject cameraPosition;
        public float ang = -80;
        public float maxAng = -20;

        [Header("baground")]
        public GameObject bag;

        CameraManager han;
        ChController controlCh;

        private void Start()
        {
            StartCoroutine(Starts());
        }
        private void OnDisable()
        {
            controlCh.Events -= Pereh;
        }
        private IEnumerator Starts()
        {
            render = FindObjectOfType<Volume>();
            render.weight = 5f;

            controlCh = FindObjectOfType<ChController>();
            controlCh.PerehMod += PerehPref;

            player = FindObjectOfType<InputHandler>().gameObject;
            PerPulll p = FindObjectOfType<PerPulll>();
            p.arenaTry += UpdateArena;

            controlCh.Init();
            controlCh.Events += Pereh;

            han = player.GetComponent<InputHandler>().camManager;
            han.maxAngle = maxAng;

            player.GetComponent<InputHandler>().states.run = false;
            player.GetComponent<Pistol>().enabled = false;
            yield return null;

            for (int i = 0; i < 4; i++)
            {
                rukBoss[i] = Instantiate(prefabRukBoss);
                rukBoss[i].SetActive(false);
                yield return null;
            }

            Transform ruk = rukBoss[0].transform.Find("Armature");
            ruk.localScale = new Vector3(ruk.localScale.x, ruk.localScale.y, -ruk.localScale.z);
            ruk = rukBoss[2].transform.Find("Armature");
            ruk.localScale = new Vector3(ruk.localScale.x, ruk.localScale.y, -ruk.localScale.z);

            for (int i = 0; i < boss.Length; i++)
            {
                boss[i] = Instantiate(prefabBoss);
                boss[i].SetActive(false);
                yield return null;
            }

            for (int i = 0; i < pullDamageObj.Length; i++)
            {
                pullDamageObj[i] = Instantiate(prefabDamageObj);
                pullDamageObj[i].SetActive(false);
            }
            foreach (GameObject i in pullDamageObj)
            {
                DamageObj a = i.GetComponent<DamageObj>();
                a.contr = this;
            }
        }
        void DamageBoss()
        {

        }//sss

        IEnumerator Updates()
        {
            controlCh.dopStrel.SetActive(true);

            controlCh.Ch.SetActive(true);
            controlCh.strel.SetActive(true);

            han.target = cameraTarget.transform;
            han.lockOnTransform = cameraPosition.transform;
            han.lockOn = true;
            han.vert = true;

            han.follofSpeed = 2f;

            han.minAngle = ang;
            han.maxAngle = maxAng;
             
            yield return new WaitForSeconds(1f);

            boss[0].transform.position = positionBoss1and2[0].position;
            rukBoss[0].transform.position = positionRukStart[0].position;
            rukBoss[1].transform.position = positionRukStart[1].position;

            GetComponent<AudioSource>().PlayOneShot(perehodClip);

            StartCoroutine(Blur());

            bag.SetActive(true);

            boss[0].SetActive(true);
            boss[0].GetComponent<BossAnim>().targetPoint = targetPointStartBoss[0];
            StartCoroutine(boss[0].GetComponent<BossAnim>().Moved());

            rukBoss[0].SetActive(true);
            rukBoss[1].SetActive(true);
            rukBoss[0].GetComponent<BossAnim>().targetPoint = statPos[0];
            rukBoss[1].GetComponent<BossAnim>().targetPoint = statPos[1];
            StartCoroutine(rukBoss[0].GetComponent<BossAnim>().Moved());
            StartCoroutine(rukBoss[1].GetComponent<BossAnim>().Moved());

            yield return new WaitForSeconds(1f);

            StartCoroutine(controlCh.ChGoStrel());

            player.GetComponent<InputHandler>().states.run = true;
            han.lockOnTransform = player.transform;
            han.vert = false;

            MoveSet set = boss[0].GetComponent<MoveSet>();
            set.player = player;
            set.statPosition = positionBoss1and2[0];
            set.UpdateStates(Set.pocoiBoss);

            yield return new WaitForSeconds(1f);

            MoveSet sets = rukBoss[0].GetComponent<MoveSet>();
            sets.player = player;
            sets.statPosition = statPos[0];
            sets.statPosition.position += Vector3.up * 5f;
            sets.UpdateStates(Set.hlopRuk);

            sets = rukBoss[1].GetComponent<MoveSet>();
            sets.player = player;
            sets.statPosition = statPos[1];
            sets.statPosition.position += Vector3.up * 5f;
            sets.UpdateStates(Set.returnState);
        }
        void UpdateArena()
        {
            StartCoroutine(Updates());
        }
        IEnumerator Blur()
        {
            float progress = 0;

            if (render.profile.TryGet<DepthOfField>(out var filmicTonemapper))
                filmicTonemapper.active = true;
            while(progress < 1)
            {
                progress += Time.deltaTime;
                filmicTonemapper.focalLength.value = Mathf.Lerp(filmicTonemapper.focalLength.max, filmicTonemapper.focalLength.min, progress);
                yield return null;
            }
            filmicTonemapper.active = false;
        }

        void UpdateFaz()
        {
            StartCoroutine(controlCh.ChGoStrel());
        }
        void Pereh()
        {
            UpdateFaz();
        }

        DamageObj a;
        void PerehPref()
        {
            controlCh.PerehMod -= PerehPref;

            a = pullDamageObj[0].GetComponent<DamageObj>();
            int ran = Random.Range(0, spawnDamageObjTransform.Count - 1);
            Transform pos = spawnDamageObjTransform[ran];
            a.spawnPos = pos;
            pullDamageObj[0].SetActive(true);
            a.DamageBoss += PerehMod;

            spawnDamageObjTransform.Remove(pos);
        }
        void PerehMod()
        {
            a.DamageBoss -= PerehMod;
            a = null;

            MoveSet set = rukBoss[0].GetComponent<MoveSet>();
            set.StopAllCoroutines();
            set.gameObject.SetActive(false);

            set = rukBoss[1].GetComponent<MoveSet>();
            set.StopAllCoroutines();
            set.gameObject.SetActive(false);

            boss[0].SetActive(false);

            StartCoroutine(PerehodPrefMod());
        }
        IEnumerator PerehodPrefMod()
        {
            AudioSource au = GetComponent<AudioSource>();
            while (true)
            {
                yield return new WaitForSeconds(1f);

                han.lockOnTransform = cameraPosition.transform;
                han.lockOn = true;
                han.vert = true;

                yield return new WaitForSeconds(1f);

                au.PlayOneShot(updateModClip);

                //au.clip = mainClip;
                //au.Play();

                controlCh.Ch2.SetActive(true);
                controlCh.Ch.SetActive(false);
                //�������� ����� ���� � �������� �����+��� ���� � ����� �������� �������� �� ����� �� ��������
                StartCoroutine(SpawnDamageObj());

            }
        }

        IEnumerator SpawnDamageObj()
        {
            GameObject tar = null;
            while (true)
            {
                foreach(GameObject i in pullDamageObj)
                {
                    if (!i.activeInHierarchy)
                    {
                        tar = i;
                        break;
                    }
                }

                if(tar != null)
                {
                    DamageObj a = tar.GetComponent<DamageObj>();
                    int ran = Random.Range(0, spawnDamageObjTransform.Count - 1);
                    Transform pos = spawnDamageObjTransform[ran];
                    a.spawnPos = pos;
                    tar.SetActive(true);
                    spawnDamageObjTransform.Remove(pos);
                    tar = null;
                }

                yield return new WaitForSeconds(4f);
            }
        }
    }
}
