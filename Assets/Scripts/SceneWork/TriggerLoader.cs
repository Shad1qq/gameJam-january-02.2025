using System;
using System.Collections;
using UnityEngine;

public class TriggerLoader : MonoBehaviour
{
    public static Action loadScene;
    [SerializeField] GameObject[] gameObjects;
    float coolDown;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Activate");
            loadScene?.Invoke();
            EnableAllGameObjects();
        }
    }

    private void EnableAllGameObjects()
    {
        
        foreach (GameObject go in gameObjects) 
        { 
            go.SetActive(true);                       
            Destroy(gameObject);
        }
        
    }
}
