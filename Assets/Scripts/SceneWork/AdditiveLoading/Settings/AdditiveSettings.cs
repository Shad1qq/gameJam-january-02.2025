using System;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveSettings
{
    [SerializeField]private List<GameObject> enemies = new List<GameObject>();
    public Action action;
    bool isWork = true;
    public void AddEnemiesInArray()
    {
        foreach(GameObject gO in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(gO);
        }
        
    }
    public void ifRemoveEnemiesInArray()
    {
        if (enemies.Count <= 0 & isWork == true) 
        {       
            Debug.Log("enemies in null");
            action?.Invoke();
            isWork = false;
        }
        foreach (GameObject go in enemies.ToArray()) 
        {
            if(go == null)
            {
                enemies.Remove(go);
            }
        }
    }
}


