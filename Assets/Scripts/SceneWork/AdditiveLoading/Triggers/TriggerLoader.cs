using System;
using System.Collections;
using UnityEngine;

public class TriggerLoader : MonoBehaviour
{
    public static Action loadScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Activate");
            loadScene?.Invoke();
        }
    }
}
