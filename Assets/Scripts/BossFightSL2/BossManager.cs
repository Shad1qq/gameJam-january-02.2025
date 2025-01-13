using UnityEngine;

public class BossManager : MonoBehaviour
{
    RightHand rightHand = new RightHand();
    LeftHand leftHand = new LeftHand();

    [SerializeField] float handSpeed;
    [SerializeField] float lefthandSpeed;
    public static bool handIsWork = false;
    private void Start()
    {
        InitRH();  
    }
    private void Update()
    {
        ChangeValue(handSpeed, lefthandSpeed);
        if (ArenaControllerForTwoScene.coolDown < -1f && handIsWork == false)
        {
            FollowPlayer();
        }
        
    }
    private void InitRH() => rightHand.InitRH();
    private void FollowPlayer() => rightHand.MoveSet();
    private void ChangeValue(float handSpeed, float lefthandSpeed)
    {
        rightHand.ChangeSpeed(handSpeed);
        leftHand.ChangeSpeed(lefthandSpeed);
    }


}
