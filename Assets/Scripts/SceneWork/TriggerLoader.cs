using System;
using System.Collections;
using UnityEngine;

public class TriggerLoader : MonoBehaviour
{
    public static Action LoadScene;
    public static Action DisablegO;
    [SerializeField] GameObject[] gameObjects;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ArenaControllerForTwoScene.coolDown = 5;
            Debug.Log("Activate");
            LoadScene?.Invoke();
            EnableAllGameObjects();
            
        }
    }

    private void EnableAllGameObjects()
    {
        DisablegO?.Invoke();
        foreach (GameObject go in gameObjects) 
        { 
            go.SetActive(true);                       
            Destroy(gameObject);
        }      
    }
}
