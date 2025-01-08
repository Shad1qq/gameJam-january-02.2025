using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ChController : MonoBehaviour
{
    public GameObject strel;

    public GameObject Ch;
    public GameObject Ch2;

    public GameObject[] osk;
    public float speed = 1f;
    public float heigth = 1f;
    float _time;

    AudioSource au;

    Vector2[] os;

    public float rotationTime = 20f;
    float gr;
    public event Action Events;
    private void FixedUpdate()
    {
        _time += Time.deltaTime;

        if(Ch2.activeInHierarchy)
            for (int i = 0; i < osk.Length; i++)
            {
                float sin = Mathf.Sin(f: (_time + Time.deltaTime) * os[i].y) - Mathf.Sin(f: _time * os[i].y);
                osk[i].transform.localPosition += new Vector3(x: 0, y: sin * os[i].x, z: 0);
            }
    }
    public void Init()
    {
        _time = UnityEngine.Random.Range(-10f, 10f);

        os = new Vector2[osk.Length];
        for (int i = 0; i < osk.Length; i++)
        {
            os[i].y = UnityEngine.Random.Range(-speed, speed);
            os[i].x = UnityEngine.Random.Range(-heigth, heigth);
        }

        au = GetComponent<AudioSource>();

        strel.SetActive(false);
        Ch.SetActive(false);
        Ch2.SetActive(false);

        gr = 360f / rotationTime;
    }
    public IEnumerator ChGoStrel()
    {
        int i = 0;
        while (i < rotationTime)
        {
            i++;
            UpStrel();

            yield return new WaitForSeconds(1f);
        }
        Events?.Invoke();
    }
    public void UpStrel()
    {
        strel.transform.Rotate(Vector3.forward, gr, Space.Self);

        au.Play();
    }
}
