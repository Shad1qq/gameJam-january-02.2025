using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlits : MonoBehaviour
{
    [SerializeField] private float timePos = 1f;
    [SerializeField] private Vector3 distance = new(0, 1f, 0);

    internal Color blueColor = new(0, 0, 1, 1);
    internal Color redColor = new(1, 0, 0, 1);
    internal Color standartColor = new(1, 1, 1, 1);

    public float startAlpha = 1f;
    public float endAlpha = 0f;
    public float transitionTime = 0.6f;

    internal List<GameObject> buferPlit = new();

    public void UpdateColorPlits(GameObject c, bool ver = false)
    {
        var startCol = (ver) ? redColor : standartColor;
        var endColor = (ver) ? blueColor : redColor;
        StartCoroutine(FadeBlock(c, startCol, endColor, transitionTime));
    }
    public void UpdatePositionPlits(GameObject c, bool Ver = false)
    {
        Vector3 target = c.transform.position + (Ver ? distance : -distance);
        StartCoroutine(UpdatePlitsPosition(c, c.transform.position, target, transitionTime, (Ver) ? endAlpha : startAlpha, (Ver) ? startAlpha : endAlpha, (Ver) ? false : true));
    }
    IEnumerator FadeBlock(GameObject block, Color startColor, Color endColor, float duration)
    {
        float time = 0f;
        Renderer renderer = block.GetComponent<Renderer>();

        while (time < duration)
        {
            renderer.material.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
            yield return new WaitForSeconds(0.05f);
        }

        renderer.material.color = endColor;

        if (endColor == redColor)
        {
            buferPlit.Add(block);
            block.GetComponent<Collider>().isTrigger = true;
        }
        else if(endColor == blueColor)
        {
            block.GetComponent<Collider>().isTrigger = false;
            StartCoroutine(FadeBlock(block, endColor, standartColor, transitionTime));
        }
    }
    public void ResetBuffer()
    {
        foreach( var i in buferPlit)
        {
            i.SetActive(true);
            StartCoroutine(UpdatePlitsPosition(i, i.transform.position, i.transform.position + distance, transitionTime, endAlpha, startAlpha, false));
        }
    }
    private IEnumerator UpdatePlitsPosition(GameObject block, Vector3 startPos, Vector3 endPosition, float time, float startAl, float endAl, bool buferize)
    {
        float progress = 0;
        float lerpedAlpha;
        Renderer renderer = block.GetComponent<Renderer>();

        while (progress < 1)
        {
            progress += Time.deltaTime / time;

                block.transform.position = Vector3.Lerp(startPos, endPosition, progress);

            lerpedAlpha = Mathf.Lerp(startAl, endAl, progress);

            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, lerpedAlpha);
            yield return new WaitForSeconds(0.05f);
        }
        if (buferize)
            block.SetActive(false);
    }
}
