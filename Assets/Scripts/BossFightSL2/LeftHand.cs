using UnityEngine;

public class LeftHand
{
    private Transform playerTransform;
    private GameObject lefthand;
    private float lefthandSpeed = 3;
    Vector3 offset = new Vector3(0, 5f, 0);

    public void InitLH()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lefthand = GameObject.Find("lefthand");
    }
    public void FollowPlayer()
    {
        lefthand.transform.position = Vector3.MoveTowards(lefthand.transform.position, playerTransform.position + offset, lefthandSpeed * Time.deltaTime);
        BossManager.handIsWork = true;
    }
    public void ChangeSpeed(float hS)
    {
        if (hS > 0)
        {
            lefthandSpeed = hS;
        }
        else
        {
            lefthandSpeed = 0;
            Debug.LogError("HandSpeed(left) cannot be less 0. Setup default Value ");
        }
    }
}
