using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class CanvasManager : MonoBehaviour
    {
        [Header("DeadPanel")]
        public Image panelDead;
        public Button returnButton;
        public event Action DeadEventButton;

        StatManager stat;
        private void Start()
        {
            panelDead.gameObject.SetActive(false);
            returnButton.gameObject.SetActive(false);
            returnButton.onClick.AddListener(DeadButtons);

            panelDead.color = new(panelDead.color.r, panelDead.color.g, panelDead.color.b, 0f);
            stat = FindObjectOfType<StatManager>();
            stat.DeadEvent += Dead;
        }
        void Dead()
        {
            panelDead.gameObject.SetActive(true);
            StartCoroutine(DeadAlpha());
        }
        IEnumerator DeadAlpha()
        {
            float time = 0;
            Color startColor = panelDead.color;
            Color endColor = new(startColor.r, startColor.g, startColor.b, 1f);

            while (time < 1)
            {
                time += Time.deltaTime;

                float t = Mathf.Clamp01(time / 1);
                panelDead.color = Color.Lerp(startColor, endColor, t);

                yield return null;
            }
            returnButton.gameObject.SetActive(true);
        }
        public void DeadButtons()
        {
            returnButton.gameObject.SetActive(false);
            panelDead.gameObject.SetActive(false);
            panelDead.color = new(panelDead.color.r, panelDead.color.g, panelDead.color.b, 0f);

            DeadEventButton?.Invoke();
        }
    }
}
