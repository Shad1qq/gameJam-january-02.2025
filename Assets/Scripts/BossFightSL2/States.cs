using UnityEngine;

public class States : State
{
    [SerializeField] GameObject[] spawnPoints;
    public override void AttackSlapState()
    {
        bossManager.rightHand.transform.position = new Vector3(bossManager.rightHand.transform.position.x, bossManager.player.position.y, bossManager.rightHand.transform.position.z);

    }

    public override void FollowState()
    {
        
        if(bossManager.randHand == 0)
        {
            bossManager.rightHand.transform.position = Vector3.MoveTowards(bossManager.rightHand.transform.position, bossManager.player.position + new Vector3(0, 5, 0), 4 * Time.deltaTime);
        }
        else
            bossManager.leftHand.transform.position = Vector3.MoveTowards(bossManager.leftHand.transform.position, bossManager.player.position + new Vector3(0, 5, 0), 4 * Time.deltaTime);

    }

    public override void IdleState()
    {
        bossManager.leftHand.transform.position = Vector3.MoveTowards(bossManager.leftHand.transform.position, bossManager.startPoints[0].transform.position, 4 * Time.deltaTime);
        bossManager.rightHand.transform.position = Vector3.MoveTowards(bossManager.rightHand.transform.position, bossManager.startPoints[1].transform.position, 4 * Time.deltaTime);
    }

    public override void RotateArenaState()
    {
        throw new System.NotImplementedException();
    }

    public override void SpawnFireState()
    {
        throw new System.NotImplementedException();
    }
}

