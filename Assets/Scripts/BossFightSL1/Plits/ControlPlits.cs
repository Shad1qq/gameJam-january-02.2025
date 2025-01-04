using System.Collections;
using UnityEngine;

public class ControlPlits : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
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
        float[] t = new float[c.Length];
        Color[] color = new Color[c.Length];
        Color targetColor = (x) ? blueColor : redColor;

        for(int i = 0; i < c.Length; i++)
        {
            material[i] = plits[c[i]].GetComponent<Renderer>().material;
            color[i] = material[i].color;
        }
        yield return new WaitForSeconds(0.05f);

        while (true)
        {
            for (int i = 0; i < c.Length; i++)
            {
                t[i] += Time.deltaTime / transitionTime;
                Color lerpedColor = Color.Lerp(color[i], targetColor, t[i]);
                Color lerpedColorWithAlpha = new(lerpedColor.r, lerpedColor.g, lerpedColor.b);
                material[i].color = lerpedColorWithAlpha;
            }

            if (color[0] == targetColor)
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
        Vector3[] startPosition = new Vector3[c.Length];
        Vector3[] targetPosition = new Vector3[c.Length];
        float progress = 0;

        for (int i = 0; i < c.Length; i++)
        {
            position[i] = plits[c[i]].transform;
            targetPosition[i] = position[i].position + new Vector3(0f, (x)? -distance : distance, 0f);
            startPosition[i] = position[i].position;
        }
        yield return new WaitForSeconds(0.05f);

        while (progress < 1)
        {
            progress += Time.deltaTime * _speed;

            for (int i = 0; i < c.Length; i++)
            {
                position[i].position = Vector3.Lerp(startPosition[i], targetPosition[i], progress);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
