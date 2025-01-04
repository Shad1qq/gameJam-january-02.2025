using System.Collections;
using UnityEngine;

public class ControlPlits : MonoBehaviour
{
    [SerializeField] private float timePos = 1f;
    [SerializeField] private float distance = 1f;

    internal Color blueColor = Color.blue;
    internal Color redColor = Color.red;
    public float startAlpha = 1f;
    public float endAlpha = 0f;
    public float transitionTime = 1.4f;

    internal GameObject[] plits;

    public void Init()
    {
        plits = new GameObject[transform.childCount];

        int i = 0;
        foreach (Transform t in transform)
        {
            plits[i++] = t.gameObject;
        }
    }
    public void UpdateColorPlits(int[] c, bool b = false)
    {
        StartCoroutine(nameof(UpdatePlitsColor), new object[] { c, b });
    }
    private IEnumerator UpdatePlitsColor(object value)
    {
        object[] parameters = value as object[];
        int[] c = parameters[0] as int[];
        bool x = (bool)parameters[1];

        Material[] material = new Material[c.Length];
        Color color;
        Color targetColor = (x) ? blueColor : redColor;
        float progress = 0;

        for(int i = 0; i < c.Length; i++)
            material[i] = plits[c[i]].GetComponent<Renderer>().material;
        color = material[0].color;
        yield return new WaitForSeconds(0.05f);

        while (true)
        {
            progress += Time.deltaTime / transitionTime;

            for (int i = 0; i < c.Length; i++)
            {
                Color lerpedColor = Color.Lerp(color, targetColor, progress);
                Color lerpedColorWithAlpha = new(lerpedColor.r, lerpedColor.g, lerpedColor.b, material[i].color.a);
                material[i].color = lerpedColorWithAlpha;
            }
            color = material[0].color;

            if (color == targetColor)
                break;

            yield return new WaitForSeconds(0.05f);
        }
    }
    public void UpdatePositionPlits(int[] c, bool b = false)
    {
        StartCoroutine(nameof(UpdatePlitsPosition), new object[] { c, b });
    }
    private IEnumerator UpdatePlitsPosition(object value)
    {
        object[] parameters = value as object[];
        int[] c = parameters[0] as int[];
        bool x = (bool)parameters[1];

        Transform[] position = new Transform[c.Length];
        Material[] material = new Material[c.Length];

        Vector3[] startPosition = new Vector3[c.Length];
        Vector3[] targetPosition = new Vector3[c.Length];
        float progress = 0;

        float targetDis;
        float lerpedAlpha;

        if (x)
            targetDis = distance;
        else
            targetDis = -distance;

        for (int i = 0; i < c.Length; i++)
        {
            position[i] = plits[c[i]].transform;
            targetPosition[i] = position[i].position + new Vector3(0f, targetDis, 0f);
            startPosition[i] = position[i].position;
            material[i] = plits[c[i]].GetComponent<Renderer>().material;
        }
        yield return new WaitForSeconds(0.05f);

        while (progress < 1)
        {
            progress += Time.deltaTime / timePos;

            for (int i = 0; i < c.Length; i++)
            {
                position[i].position = Vector3.Lerp(startPosition[i], targetPosition[i], progress);

                if(x)
                    lerpedAlpha = Mathf.Lerp(endAlpha, startAlpha, progress);
                else
                    lerpedAlpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                material[i].color = new Color(material[i].color.r, material[i].color.g, material[i].color.b, lerpedAlpha);
            }
            yield return new WaitForSeconds(0.05f);
        }
        foreach (var i in position)
            i.gameObject.SetActive(false);
    }
}
