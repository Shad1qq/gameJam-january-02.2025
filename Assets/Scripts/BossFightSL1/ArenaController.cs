using System.Collections;
using UnityEngine;

namespace SA
{
    public class ArenaController : MonoBehaviour
    {
        GameObject player;

        public GameObject cameraTarget;
        public GameObject cameraPosition;
        public GameObject bag;

        public float ang = -80;
        public float maxAng = -20;

        CameraManager han;
        ChController controlCh;
        private void Start()
        {
            controlCh = FindObjectOfType<ChController>();
            player = FindObjectOfType<InputHandler>().gameObject;
            PerPulll p = FindObjectOfType<PerPulll>();
            p.arenaTry += UpdateArena;

            han = player.GetComponent<InputHandler>().camManager;

            han.maxAngle = maxAng;

            player.GetComponent<InputHandler>().states.run = false;

            controlCh.Init();
            controlCh.Events += Pereh;
        }
        void UpdateArena()
        {
            controlCh.Ch.SetActive(true);
            controlCh.strel.SetActive(true);

            player.GetComponent<InputHandler>().states.run = false;

            han.target = cameraTarget.transform;
            han.lockOnTransform = cameraPosition.transform;
            han.lockOn = true;
            han.follofSpeed = 2f;
            han.minAngle = ang;
            han.maxAngle = maxAng;
            han.vert = true;

            Invoke(nameof(UpsArena), 1f);
        }
        void UpsArena()
        {
            StartCoroutine(controlCh.ChGoStrel());
            Invoke(nameof(UpArena), 1f);
        }
        void UpArena()
        {
            player.GetComponent<InputHandler>().states.run = true;
            han.lockOnTransform = player.transform;
            han.vert = false;
            bag.SetActive(true);
        }

        void UpdateFaz()
        {
            StartCoroutine(controlCh.ChGoStrel());
        }
        void PerehMod()
        {
            controlCh.Ch2.SetActive(true);
            controlCh.Ch.SetActive(false);
            StartCoroutine(controlCh.ChGoStrel());
        }
        void Pereh()
        {
            if (!controlCh.Ch2.activeInHierarchy)
            {
                PerehMod();
                return;
            }

            UpdateFaz();
        }
    }
}
