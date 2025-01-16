using SA;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public Transform[] startPoints;
    public GameObject rightHand;
    public GameObject leftHand;
    public Transform player;
    public int randHand;
    [SerializeField]bool handIsWork;

    private States state;
    float coolDownBeforeAttack = 1;
    private void Start()
    {
        Init();
        SetupDefaultState();
    }
    private void Update()
    {
        if(ArenaControllerForTwoScene.coolDown<0)
        {           
            if(handIsWork == false)
            {
                state.FollowState();          
            }
            else
            {            
                if (coolDownBeforeAttack < 0)
                {
                    state.AttackSlapState();      
                    coolDownBeforeAttack = 1;
                    handIsWork = false;
                }
                else coolDownBeforeAttack -= Time.deltaTime;
            }
        }
    }
    void Init()
    {
        rightHand = GameObject.FindGameObjectWithTag("RightHand");
        leftHand = GameObject.FindGameObjectWithTag("LeftHand");
        player = FindFirstObjectByType<StatManager>().transform;
        state = gameObject.AddComponent<States>();   
        randHand = Random.Range(0, 1);
    }
    void SetupDefaultState()
    {
        state.IdleState();
    }

}
