using System.Collections;
using UnityEngine.SceneManagement;

public class AdditiveLoader
{
   public IEnumerator LoadScene(int index)
    {
        yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
    }
}
