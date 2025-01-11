using UnityEngine;
public class RightHand
{
    private Transform playerTransform;
    private GameObject rightHand;
    private float handSpeed = 3;
    Vector3 offset = new Vector3(0, 5f, 0);
    
    public void InitRH()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rightHand = GameObject.Find("righthand");
    }
    public void FollowPlayer()
    {
        rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, playerTransform.position + offset, handSpeed * Time.deltaTime);
        Debug.Log("This Work");
    }
    public void ChangeSpeed(float hS)
    {
        if(hS > 0)
        {
            handSpeed = hS;
        }
        else
        {
            handSpeed = 0;
            Debug.LogError("HandSpeed(right) cannot be less 0. Setup default Value ");      
        }
    }
}

