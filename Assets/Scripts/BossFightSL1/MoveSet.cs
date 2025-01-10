using System.Collections;
using UnityEngine;

public class MoveSet : MonoBehaviour
{
    public GameObject attacer;
    public Transform statPosition;
    public GameObject player;

    float hor = 1f;
    float speed = 1f;

    private void Start()
    {
        attacer = gameObject;
    }
    public IEnumerator PocoiStatBoss()
    {
        float _time = 0;

        while (true)
        {
            _time += Time.deltaTime;

            float sin = Mathf.Sin(f: (_time + Time.deltaTime) * speed) - Mathf.Sin(f: _time * speed);
            attacer.transform.localPosition += new Vector3(x: 0, y: sin * hor, z: 0);

            Vector3 targetDir = player.transform.position - transform.position;
            targetDir.Normalize();

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;
            Quaternion targetRoot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRoot, Time.deltaTime * 7);

            yield return null;
        }
    }

    public IEnumerator ReturnStatPosition()
    {
        Vector3 stPoint;

        float progress = 0;
        stPoint = transform.position;
        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            transform.position = Vector3.LerpUnclamped(stPoint, statPosition.position, progress);
            yield return null;
        }
        UpdateStates(Set.pocoiRuc);
    }
    public IEnumerator PocoiStatRuk()
    {
        float _time = 0;

        while (true)
        {
            _time += Time.deltaTime;

            float sin = Mathf.Sin(f: (_time + Time.deltaTime) * speed) - Mathf.Sin(f: _time * speed);
            attacer.transform.localPosition += new Vector3(x: 0, y: sin * hor, z: 0);
            yield return null;
        }
    }
    public IEnumerator HlopocStatRuk()
    {
        float _time = 0;
        float progress = 0;
        Vector3 stPoint = transform.position;

        GetComponent<Animator>().SetBool("der", false);

        while (_time < 2f)
        {
            _time += Time.deltaTime;
            progress += Time.deltaTime;

            Vector3 targ = new(x: player.transform.position.x, y: player.transform.position.y + 5, z:player.transform.position.z);
            transform.position = Vector3.Lerp(stPoint, targ, progress);
            yield return null;
        }
        float progres = 0;
        stPoint = transform.position;
        while (progres < 1f)
        {
            progres += Time.deltaTime * 5;
            transform.position = Vector3.Lerp(stPoint, player.transform.position, progres);
            yield return null;
        }
    }

    public void UpdateStates(Set s)
    {
        StopAllCoroutines();

        switch (s)
        {
            case Set.pocoiBoss:
                StartCoroutine(PocoiStatBoss()); 
                break;
            case Set.pocoiRuc:
                StartCoroutine(PocoiStatRuk());
                break;
            case Set.returnState:
                StartCoroutine(ReturnStatPosition());
                break;
            case Set.hlopRuk:
                StartCoroutine(HlopocStatRuk());
                break;
        }
    }
}

public enum Set
{
    pocoiBoss,
    returnState,
    pocoiRuc,
    hlopRuk
}
