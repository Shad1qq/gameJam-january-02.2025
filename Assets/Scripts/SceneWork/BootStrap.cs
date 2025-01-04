using System.Collections;
using SceneWork;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootStrap : MonoBehaviour
{
    private LoadScreens _loadScreens;
    [Header("UI Elements")]
    [SerializeField] private GameObject loadPanel;   
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider slider;
    [SerializeField] private Button button;

    AsyncOperation aOper;
    private void Awake()
    {
        _loadScreens = FindFirstObjectByType<LoadScreens>();       
    }
    public void StartGame()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {          
        aOper = SceneManager.LoadSceneAsync(1);
        aOper.allowSceneActivation = false;

        while (!aOper.isDone)
        {
            _loadScreens.Show(loadPanel);
            text.text = "Loading...";
            slider.value = aOper.progress;

            if (aOper.progress >= 0.9f && !aOper.allowSceneActivation)
            {
                text.text = "Load Is Completed!";
                button.enabled = true;
                break;
            }
            yield return null;
        }
    }
    public void LoadGameOnButton()
    {
        aOper.allowSceneActivation = true;
    }
}
