using System.Collections.Generic;
using UnityEngine;

public class CameraColliderSten : MonoBehaviour
{
    [HideInInspector]
    public List<Renderer> ob = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Renderer>() || other.gameObject.layer != 3)
            return;
        var i = other.GetComponent<Renderer>();
        i.material.shader = Shader.Find("Custom/TransparentShader");
        ob.Add(i);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Renderer>() || other.gameObject.layer != 3)
            return;
        var i = other.GetComponent<Renderer>();
        i.material.shader = Shader.Find("Universal Render Pipeline/Lit");
        ob.Remove(i);
    }
}
