using System.Collections;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
    public AnimationCurve vert;
    public AnimationCurve hor;

    internal Transform targetPoint;
    public IEnumerator Moved()
    {
        Vector3 stPoint;

        float progress = 0;
        stPoint = transform.position;
        while(progress < 1f)
        {
            progress += Time.deltaTime;
            transform.position = Vector3.LerpUnclamped(stPoint, targetPoint.position, hor.Evaluate(progress));
            transform.position = transform.position + Vector3.up * vert.Evaluate(progress);
            yield return null;
        }
        progress = 0;
    }
}
