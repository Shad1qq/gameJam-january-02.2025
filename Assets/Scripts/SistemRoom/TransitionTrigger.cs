using UnityEngine;

namespace RA
{
    public class TransitionTrigger : MonoBehaviour
    {
        public ManagerRoom manager;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                manager.OnTriggerExitTransition();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.OnTriggerEnterTransition();
            }
        }
    }
}
