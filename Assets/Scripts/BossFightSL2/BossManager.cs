using UnityEngine;

public class BossManager : MonoBehaviour
{
    RightHand rightHand = new RightHand();
    [SerializeField] float handSpeed;
    private void Start()
    {
        InitRH();  
    }
    private void Update()
    {
        ChangeValue(handSpeed);
        if (ArenaControllerForTwoScene.coolDown < -1f)
        {
            FollowPlayer();
        }
        
    }
    private void InitRH() => rightHand.InitRH();
    private void FollowPlayer() => rightHand.FollowPlayer();
    private void ChangeValue(float handSpeed)
    {
        rightHand.ChangeSpeed(handSpeed);
    }


}
