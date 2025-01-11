using SA;
using UnityEngine;

public class ArenaControllerForTwoScene : MonoBehaviour
{
    [SerializeField]GameObject blackScreen;
    [SerializeField]GameObject lockOn;
    [SerializeField]StatManager statManager;
    CameraManager camManager;
    internal static float coolDown = 5;

    private void Start()
    {
        camManager = FindAnyObjectByType<CameraManager>();
        statManager = FindFirstObjectByType<StatManager>();
    }

    private void Update()
    {
        if(blackScreen.activeInHierarchy && coolDown < 0)
        {
            blackScreen.SetActive(false);
            SwitchCameraPos();
        }
        else coolDown -= Time.fixedDeltaTime; 
    }

    void SwitchCameraPos()
    {
        camManager.lockOn = true;
        camManager.vert = true;
        camManager.lockOnTransform = statManager.gameObject.transform;
        camManager.target = lockOn.transform;
    }
}
