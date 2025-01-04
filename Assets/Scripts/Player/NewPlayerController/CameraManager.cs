using UnityEngine;

namespace SA
{
    public class CameraManager : MonoBehaviour
    {
        public bool lockOn;
        public float follofSpeed = 9f;
        public float mouseSpeed = 2f;

        public Transform target;
        public Transform lockOnTransform;

        public Transform pivot;
        public Transform camTrans;
        StatManager states;

        readonly float turnSmoothing = .1f;
        public float minAngle = -35f;
        public float maxAngle = 35f;

        float smothX;
        float smothY;
        float smothXvelocity;
        float smothYvelocity;

        public float lookAngle;
        public float tillAngle;

        bool usedRigthAxis;

        public void Init(StatManager st)
        {
            states = st;
            target = st.transform;

            camTrans = Camera.main.transform;
            pivot = camTrans.parent;
        }

        public void Tick(float d)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float targetSpeed = mouseSpeed;

            FollovTarget(d);
            HandleRotation(d, v, h, targetSpeed);
        }
        void FollovTarget(float d)
        {
            float speed = d * follofSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
            transform.position = targetPosition;
        }
        void HandleRotation(float d, float v, float h, float targetSpeed)
        {
            if(turnSmoothing > 0)
            {
                smothX = Mathf.SmoothDamp(smothX, h, ref smothXvelocity, turnSmoothing);
                smothY = Mathf.SmoothDamp(smothY, v, ref smothYvelocity, turnSmoothing);
            }
            else
            {
                smothX = h;
                smothY = v;
            }

            tillAngle -= smothY * targetSpeed;
            tillAngle = Mathf.Clamp(tillAngle, minAngle, maxAngle);
            pivot.localRotation = Quaternion.Euler(tillAngle, 0, 0);

            if (lockOn && lockOnTransform != null)
            {
                Vector3 targetDir = lockOnTransform.position - transform.position;
                targetDir.Normalize();

                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;
                Quaternion targetRoot = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRoot, d * 9);
                lookAngle = transform.eulerAngles.y;

                return;
            }

            lookAngle += smothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);
        }
        public static CameraManager singleton;

        void Awake()
        {
            singleton = this;
        }
    }
}
