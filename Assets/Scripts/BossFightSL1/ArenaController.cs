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
        private void Start()
        {
            player = FindObjectOfType<InputHandler>().gameObject;
            PerPulll p = FindObjectOfType<PerPulll>();
            p.arenaTry += UpdateArena;

            han = player.GetComponent<InputHandler>().camManager;

            han.maxAngle = maxAng;
        }
        void UpdateArena()
        {
            player.GetComponent<InputHandler>().states.run = false;

            han.target = cameraTarget.transform;
            han.lockOnTransform = cameraPosition.transform;
            han.lockOn = true;
            han.follofSpeed = 2f;
            han.minAngle = ang;
            han.maxAngle = maxAng;
            han.vert = true;

            Invoke(nameof(UpArena), 2f);
        }

        void UpArena()
        {
            player.GetComponent<InputHandler>().states.run = true;
            han.lockOnTransform = player.transform;
            han.vert = false;
            bag.SetActive(true);

            StartCoroutine(CameraUpdate());
        }
        IEnumerator CameraUpdate()
        {
            yield return null;
        }
    }
}
