using UnityEngine;

namespace RA
{
    public class ManagerRoom : MonoBehaviour
    {
        GenerateRoom gen => GetComponent<GenerateRoom>();

        private void Start()
        {
            gen.Init(this);
        }

        public void OnTriggerEnterTransition()
        {
            gen.MoveToNextRoom();
        }//вышел из комнаты
        public void OnTriggerExitTransition()
        {
            gen.MoveExitToRoom();
        }//вошел в комнату
    }
}