using UnityEngine;
using UnityEngine.SceneManagement;

public class TransformObject
{
    private GameObject[] sceneObjects;
    private Transform point;
    public void TransferObject(int lastScene)
    {
        Debug.Log("IsWorking?");
        point = GameObject.FindGameObjectWithTag("Point").transform;  
        Debug.Log(point.name);
        Scene lastSceneObj = SceneManager.GetSceneByBuildIndex(lastScene);
        Debug.Log(lastSceneObj.name + lastSceneObj.buildIndex);
        sceneObjects = lastSceneObj.GetRootGameObjects();
        foreach (GameObject gO in sceneObjects)
        {
            Debug.Log(gO.name);
            if (gO.name == "Transform") 
            {      
                Debug.Log("Catch");
                gO.transform.position = point.position;
                gO.transform.rotation = point.rotation;
                    
            }
                
        }          
    }
    
}

