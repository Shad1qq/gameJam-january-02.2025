using System.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace SA
{
    public class ArenaController : MonoBehaviour
    {
        GameObject player;

        public GameObject cameraTarget;
        public GameObject cameraPosition;

        public float ang = -60;
        CameraManager han;
        private void Start()
        {
            player = FindObjectOfType<InputHandler>().gameObject;
            PerPulll p = FindObjectOfType<PerPulll>();
            p.arenaTry += UpdateArena;
        }
        void UpdateArena()
        {
            han = player.GetComponent<InputHandler>().camManager;
            han.target = cameraTarget.transform;
            han.lockOnTransform = cameraPosition.transform;
            han.lockOn = true;
            han.follofSpeed = 2f;
            han.minAngle = ang;
            han.vert = true;

            Invoke(nameof(UpArena), 2f);
        }

        void UpArena()
        {
            han.lockOnTransform = player.transform;
            han.vert = false;

            StartCoroutine(CameraUpdate());
        }
        IEnumerator CameraUpdate()
        {
            yield return null;
        }
    }
}
