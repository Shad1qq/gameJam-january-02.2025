using System.Collections.Generic;
using UnityEngine;

public class RagdolController : MonoBehaviour
{
    List<Collider> col = new();
    List<Rigidbody> rid = new();
    Animator anim;

    public void Init(Animator an)
    {
        anim = an;

        Collider[] c = GetComponentsInChildren<Collider>();
        foreach (var i in c)
            col.Add(i);

        Rigidbody[] r = GetComponentsInChildren<Rigidbody>();
        foreach (var i in r)
            rid.Add(i);

        foreach(Collider col in col)
        {
            col.isTrigger = true;
        }
        foreach(Rigidbody rb in rid)
        {
            rb.isKinematic = true;
        }
    }
    public void RagdolTrue()
    {
        anim.enabled = false;

        foreach (Collider col in col)
        {
            col.isTrigger = false;
        }
        foreach (Rigidbody rb in rid)
        {
            rb.isKinematic = false;
        }
    }
}
