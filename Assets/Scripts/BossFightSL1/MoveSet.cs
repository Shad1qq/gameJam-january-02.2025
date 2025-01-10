using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MoveSet : MonoBehaviour
{
    [Header("хлопок")]
    public AudioClip audioHlopok;
    public ParticleSystem partice;

    public GameObject attacer;
    public Transform statPosition;
    public GameObject player;

    float hor = 1f;
    float speed = 1f;

    OpenAndCloseColliderDamage openCol;
    ControlPlits plitContr;
    AudioSource au => GetComponent<AudioSource>();
    private void Start()
    {
        if(GetComponent<Collider>() != null)
        {
            plitContr = FindObjectOfType<ControlPlits>();
            openCol = GetComponent<OpenAndCloseColliderDamage>();
            if (openCol == null)
                openCol = gameObject.AddComponent<OpenAndCloseColliderDamage>();
        }

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

        GetComponent<Animator>().SetBool("der", true);
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

        openCol.OpenColloder();
        Vector3 pos = player.transform.position;
        while (progres < 1f)
        {
            progres += Time.deltaTime * 3;
            transform.position = Vector3.Lerp(stPoint, pos, progres);
            yield return null;
        }
        au.PlayOneShot(audioHlopok);
        partice.Play();

        yield return new WaitForSeconds(1f);
        openCol.CloseCollider();

        foreach(var i in openCol.plit)
        {
            plitContr.UpdateColorPlits(i);
            plitContr.UpdatePositionPlits(i);
        }
        openCol.plit.Clear();

        int c = Random.Range(0, 2);
        Set set;

        if (c == 1)
            set = Set.returnState;
        else
            set = Set.hlopRuk;

        UpdateStates(set);
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
