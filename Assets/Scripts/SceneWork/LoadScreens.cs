
using DG.Tweening;
using UnityEngine;

namespace SceneWork
{
    public class LoadScreens : MonoBehaviour
    {
        
        public void Hide(GameObject gameObject) => gameObject.SetActive(false);
        public void Show(GameObject gameObject) 
        {
            gameObject.SetActive(true);
            gameObject.transform.DOMoveY(540f, 1);

        }

    }
}

