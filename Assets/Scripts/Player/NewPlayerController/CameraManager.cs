using UnityEngine;

namespace SA
{
    public class CameraManager : MonoBehaviour
    {
        public bool lockOn;//���������� ������ ���� ���� ������� ���� 

        public float follofSpeed = 9f;
        public float mouseSpeed = 2f;
        public float controllerSpeed = 7f;

        public Transform target;//�� �� ��� �� ������, ����� ������� �� �� ������� � �� ���� ������ ��������� �������� ���� ��� ���������� � ��������������� ��������� ����������

        [HideInInspector]
        public Transform pivot;
        [HideInInspector]
        public Transform camTrans;

        float turnSmoothing = .1f;
        public float minAngle = -35f;
        public float maxAngle = 35f;

        float smothX;
        float smothY;
        float smothXvelocity;
        float smothYvelocity;

        public float lookAngle;
        public float tillAngle;

        public void Init(Transform t)
        {
            target = t;//������ ������

            camTrans = Camera.main.transform;//������ ������
            pivot = camTrans.parent;//������ ����� ��� ����� ���� ������ ��� �������� ������
        }

        public void Tick(float d)
        {
            float h = Input.GetAxis("Mouse X");
            float v = 0; //Input.GetAxis("Mouse Y")

            float c_h = 0; //Input.GetAxis("RigthAxis X") ����� �� �������� 
            float c_v = 0; //Input.GetAxis("RigthAxis Y") ��������� �� ������������� ����� ��������� ������ ��������� ����� ������� ������ �� ���� � ���� �� ����� �������� � �� ������ 

            float targetSpeed = mouseSpeed;

            if (c_h != 0 || c_h != 0)
            {
                h = c_h;
                v = c_v;
                targetSpeed = controllerSpeed;
            }// ���������� ��� ���� � ������ ��������� ����� �������� ��������� �� �� ��������� �� ����� ������ �� �� �������� �������� � � 0.5 ����

            FollovTarget(d);
            HandleRotation(d, v, h, targetSpeed);
        }
        void FollovTarget(float d)
        {
            float speed = d * follofSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
            transform.position = targetPosition;//��������� � ������� � ������ �����
        }
        void HandleRotation(float d, float v, float h, float targetSpeed)
        {
            if(turnSmoothing > 0)
            {
                smothX = Mathf.SmoothDamp(smothX, h, ref smothXvelocity, turnSmoothing);
                smothY = Mathf.SmoothDamp(smothY, v, ref smothYvelocity, turnSmoothing);
            }//���� ������� ������� ��� ���������� ��������
            else
            {
                smothX = h;
                smothY = v;
            }

            if (lockOn)
            {

            }//������ ��������� � ����������� �� ���������(��� ��������� ���������� ������� �� ������� �� ��� ��������� ��� ��� ���������)

            lookAngle += smothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);//������� 

            tillAngle -= smothY * targetSpeed;
            tillAngle = Mathf.Clamp(tillAngle, minAngle, maxAngle);
            pivot.localRotation = Quaternion.Euler(tillAngle, 0, 0);//��� ����� � ���� ������������ ��� �� ����� �� ������ ��������� � �������� � �� ���������� �
        }

        public static CameraManager singleton;
        void Awake()
        {
            singleton = this;
        }//���������� ������ ���� � ����� ���� ����� �� ����� ���� ���� ����������
    }
}
