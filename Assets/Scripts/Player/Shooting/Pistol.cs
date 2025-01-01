using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Shooting
{
    private void Start()
    {
        coolDown = startcoolDown;
    }
    void FixedUpdate()
    {      
        if(Input.GetKeyDown(KeyCode.Mouse0) && coolDown <= 0f)
        {
            Shoot();
            coolDown = startcoolDown;
        }
        
            
    }
    private void Update() => coolDown -= Time.deltaTime;

}
