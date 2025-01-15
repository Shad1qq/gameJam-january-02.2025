using System.Collections;
using UnityEngine;

public class MoveSet : MonoBehaviour
{
    [Header("атака гастер бластером")]
    public float distance = 15f;
    public GameObject particle;
    public AudioClip audioGaster;

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

    Set[] numStat = { Set.hlopRuk, Set.screbok, Set.gasterBluster }; // Массив чисел

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

            particle.SetActive(false);
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

        while (_time < Random.Range(1f, 6f))
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
        openCol.CloseCollider();

        /*foreach (var i in openCol.plit)
        {
            plitContr.UpdateColorPlits(i);
            plitContr.UpdatePositionPlits(i);
        }*/

        yield return new WaitForSeconds(1f);

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
    public IEnumerator GasterBlusterRuk()
    {
        Quaternion root = transform.rotation;
        Vector3 targetPos = CalculatePosition(player.transform.position, player.transform.forward, distance, Random.Range(0f, 360f));
        Vector3 startPos = transform.position;

        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, targetPos, time);


            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0; // Игнорируем вертикальную компоненту
            if (direction != Vector3.zero)
            {
                // Вычисляем угол поворота
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                angle += 90;
                if (transform.Find("Armature").localScale.z < 0)
                    angle -= 180;

                // Создаем новый поворот с фиксированными значениями для X и Z
                transform.rotation = Quaternion.Euler(0, angle, 90);
            }

            yield return new WaitForSeconds(0.01f);
        }

        time = 0;
        anim.SetBool("Atack", true);
        anim.Play("gota");

        startPos = particle.transform.localScale;
        targetPos = startPos;
        targetPos.y = 0.1f;
        targetPos.z = 0.1f;

        yield return new WaitForSeconds(0.5f);

        au.PlayOneShot(audioGaster);

        particle.SetActive(true);
        
        while(time < 1f)
        {
            time += Time.deltaTime * 2f;

            particle.transform.localScale = Vector3.Lerp(startPos, targetPos, time);
            yield return null;
        }
        time = 0;

        yield return new WaitForSeconds(0.2f);

        startPos = particle.transform.localScale;
        targetPos = new(startPos.x, 0f, 0f);

        while (time < 1f)
        {
            time += Time.deltaTime * 3f;

            particle.transform.localScale = Vector3.Lerp(startPos, targetPos, time);
            yield return null;
        }

        anim.SetBool("Atack", false);
        transform.rotation = root;
        particle.SetActive(false);

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
            case Set.gasterBluster:
                StartCoroutine(GasterBlusterRuk());
                break;
        }
    }

    public Set GetRandomNumber()
    {
        int index = Random.Range(0, numStat.Length);
        return numStat[index];
    }
    Vector3 CalculatePosition(Vector3 referencePosition, Vector3 referenceDirection, float distance, float angle)
    {
        // Преобразуем угол в радианы
        float angleInRadians = angle * Mathf.Deg2Rad;

        // Вычисляем смещение по оси X и Z
        float offsetX = distance * Mathf.Cos(angleInRadians);
        float offsetZ = distance * Mathf.Sin(angleInRadians);

        // Создаем вектор смещения
        Vector3 offset = new Vector3(offsetX, 0, offsetZ);

        // Поворачиваем вектор смещения в направлении referenceDirection
        Quaternion rotation = Quaternion.LookRotation(referenceDirection);
        Vector3 rotatedOffset = rotation * offset;

        // Вычисляем конечную позицию
        Vector3 finalPosition = referencePosition + rotatedOffset;

        return finalPosition;
    }
}

public enum Set
{
    pocoiBoss,
    returnState,
    pocoiRuc,
    hlopRuk,
    screbok,
    gasterBluster
}
