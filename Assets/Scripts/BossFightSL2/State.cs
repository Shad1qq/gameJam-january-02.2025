using UnityEngine;

public abstract class State : MonoBehaviour
{
    public BossManager bossManager;
    private void Start()
    {
        bossManager = FindFirstObjectByType<BossManager>();
    }
    public abstract void IdleState();
    public abstract void FollowState();
    public abstract void AttackSlapState();
    public abstract void RotateArenaState();
    public abstract void SpawnFireState();
}
