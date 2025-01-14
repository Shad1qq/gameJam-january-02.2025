using System.Collections;
using UnityEngine;

public class MoveSet : MonoBehaviour
{
    [Header("хлопок")]
    public AudioClip audioHlopok;
    public ParticleSystem partice;

    [Header("скребет")]
    public AudioClip audioScr;
    public ParticleSystem particeScr;

    internal GameObject attacer;
    internal Transform statPosition;
    internal GameObject player;

    float hor = 1f;
    float speed = 1f;

    Set[] numStat = { Set.hlopRuk, Set.screbok}; // Массив чисел

    OpenAndCloseColliderDamage openCol;
    ControlPlits plitContr;

    AudioSource au;
    AudioSource audioSource;

    Animator anim => GetComponent<Animator>();
    private void OnEnable()
    {
        if(GetComponent<Collider>() != null)
        {
            plitContr = FindObjectOfType<ControlPlits>();
            openCol = GetComponent<OpenAndCloseColliderDamage>();
            if (openCol == null)
                openCol = gameObject.AddComponent<OpenAndCloseColliderDamage>();

            audioSource = GetComponent<AudioSource>();

            au = GetComponent<AudioSource>();
            if (au == null)
                au = gameObject.AddComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.volume = 0.2f;
            audioSource.loop = true;
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

        while (_time < 2f)
        {
            _time += Time.deltaTime;

            float sin = Mathf.Sin(f: (_time + Time.deltaTime) * speed) - Mathf.Sin(f: _time * speed);
            attacer.transform.localPosition += new Vector3(x: 0, y: sin * hor, z: 0);
            yield return null;
        }
        UpdateStates(GetRandomNumber());
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

        /*foreach (var i in openCol.plit)
        {
            plitContr.UpdateColorPlits(i);
            plitContr.UpdatePositionPlits(i);
        }*/

        yield return new WaitForSeconds(1f);
        openCol.CloseCollider();

        openCol.plit.Clear();

        UpdateStates(Set.returnState);
    }
    public IEnumerator ScrebokRuk()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = player.transform.position;

        audioSource.clip = audioScr;

        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, targetPos, time);
            yield return new WaitForSeconds(0.01f);
        }
        targetPos = new(startPos.x, targetPos.y, startPos.z);
        startPos = transform.position;
        time = 0;
        anim.SetBool("screb", true);
        openCol.OpenColloder();

        audioSource.Play();

        float _tim = 0f;
        while (time < 1f)
        {
            _tim += Time.deltaTime;
            if (_tim > 0.2f)
            {
                _tim = 0f;
                particeScr.Play();
            }

            time += Time.deltaTime / 2f;

            transform.position = Vector3.Lerp(startPos, targetPos, time);
            yield return null;
        }
        openCol.CloseCollider();
        GetComponent<Animator>().SetBool("screb", false);
        audioSource.Stop();

        UpdateStates(Set.returnState);
    }

    public void UpdateStates(Set s)
    {
        if(openCol != null)
            openCol.plit.Clear();

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
            case Set.screbok:
                StartCoroutine(ScrebokRuk());
                break;
        }
    }

    public Set GetRandomNumber()
    {
        int index = Random.Range(0, numStat.Length);
        return numStat[index];
    }
}

public enum Set
{
    pocoiBoss,
    returnState,
    pocoiRuc,
    hlopRuk,
    screbok
}
