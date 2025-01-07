using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class AdditiveManager : MonoBehaviour
{
    [SerializeField]private int _index;
    [SerializeField] private int index2scene;
    [SerializeField]private int _lastScene;

    AdditiveLoader _loader = new AdditiveLoader();
    AdditiveUnloader _unloader = new AdditiveUnloader();
    AdditiveSettings settings = new AdditiveSettings();
    TransformObject transformObject = new TransformObject();

    private void Awake()
    {
        LoadScene(_index);
    }
    private void Start()
    {         
        AddEnemies();
        TriggerLoader.loadScene += () => UnloadScene(_lastScene);
        settings.action += () => AfterEnemyDead(index2scene);
    }

    private void Update()
    {         
        CheckEnemies();       
    }

    private void LoadScene(int index)
    {
        StartCoroutine(_loader.LoadScene(index));
    }
    private void UnloadScene(int lastScene)
    {
        StartCoroutine(_unloader.UnloadScene(lastScene));
    }
    private void AddEnemies()
    {
        settings.AddEnemiesInArray();
    }
    private void CheckEnemies()
    {
        settings.ifRemoveEnemiesInArray();
    }
    private void TransferScene(int lastScene)
    {
        transformObject.TransferObject(lastScene);
    }
    private void CheckLastSceneIndex()
    {
        _lastScene = SceneManager.sceneCount - 1;
    }
    private void AfterEnemyDead(int index2scene)
    {
        LoadScene(index2scene);
        CheckLastSceneIndex();
        TransferScene(_lastScene);
    }



}
