using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveUnloader
{
    public IEnumerator UnloadScene(int lastScene)
    {
        yield return SceneManager.UnloadSceneAsync(lastScene);
    }
}
