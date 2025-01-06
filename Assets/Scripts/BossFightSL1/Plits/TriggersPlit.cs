using UnityEngine;

namespace SA
{
    public class TriggersPlit : MonoBehaviour
    {
        internal PerPulll pull;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Plit"))
                pull.OnEnter(other);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Plit"))
                pull.OnExit(other);
        }
    }
}