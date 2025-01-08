using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenesAdditive : MonoBehaviour
{
    float coolDown = 5;
    [SerializeField]Transform point;
    [SerializeField] private int index;
    [SerializeField]string firstBossScene;
    [SerializeField] GameObject[] enemies;
    bool isCreatedAllow = true;

    private void Start()
    {
        StartCoroutine(LoadFirstScenes());
        TriggerLoader.loadScene += () => StartCoroutine(UnloadScene());
    }

    private void Update()
    {
        coolDown -= Time.deltaTime;
        
        if (enemies.Length <= 0 && coolDown <= 0 && isCreatedAllow == true)
        {
            StartCoroutine(LoadScenes(index));    
            coolDown = 5;
            isCreatedAllow = false;


        }
        else
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            point = GameObject.FindGameObjectWithTag("Point").transform;
        }  
        
        
    }
    IEnumerator LoadScenes(int index)
    {
        yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        Scene lastScene = SceneManager.GetSceneByBuildIndex(index);
        GameObject[] gO = lastScene.GetRootGameObjects();
        foreach (GameObject go in gO) 
        {
            if(go.tag == "Transform")
            {
                go.transform.position = point.position;
                go.transform.rotation = point.rotation;
            }
        }
        
    }
    IEnumerator LoadFirstScenes()
    {
        yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }
    IEnumerator UnloadScene()
    {
        int sceneCount = SceneManager.loadedSceneCount;
        if(sceneCount > 0)
        {
            Scene lastLoadedScene = SceneManager.GetSceneAt(sceneCount -2);
            yield return SceneManager.UnloadSceneAsync(lastLoadedScene);
        }
    }

}
