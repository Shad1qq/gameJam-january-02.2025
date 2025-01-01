using UnityEngine;

namespace SA
{
    public class CameraManager : MonoBehaviour
    {
        public bool lockOn;//блокировка камеры чето типо захвата цели 

        public float follofSpeed = 9f;
        public float mouseSpeed = 2f;
        public float controllerSpeed = 7f;

        public Transform target;//то за чем мы следим, чтобы следить не за игроком а за всей сценой прийдется добавить сюда еще переменную и отредактировать состояние блокировка

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
            target = t;//лутаем игрока

            camTrans = Camera.main.transform;//лутаем камеру
            pivot = camTrans.parent;//лутаем пивот крч юзаем этот обжект для смещения камеры
        }

        public void Tick(float d)
        {
            float h = Input.GetAxis("Mouse X");
            float v = 0; //Input.GetAxis("Mouse Y")

            float c_h = 0; //Input.GetAxis("RigthAxis X") хуета не настрона 
            float c_v = 0; //Input.GetAxis("RigthAxis Y") избавился от вертикального ввода закомитил старую обработку ввода оставил только по иксу с мыши но можно добавить и по игрику 

            float targetSpeed = mouseSpeed;

            if (c_h != 0 || c_h != 0)
            {
                h = c_h;
                v = c_v;
                targetSpeed = controllerSpeed;
            }// убеждаемся что ввод с разных устройств будет работать одинакого тк на джостиках ты можеш нажать не на условную единиццу а в 0.5 силы

            FollovTarget(d);
            HandleRotation(d, v, h, targetSpeed);
        }
        void FollovTarget(float d)
        {
            float speed = d * follofSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
            transform.position = targetPosition;//двигаемся с игроком в унисон поход
        }
        void HandleRotation(float d, float v, float h, float targetSpeed)
        {
            if(turnSmoothing > 0)
            {
                smothX = Mathf.SmoothDamp(smothX, h, ref smothXvelocity, turnSmoothing);
                smothY = Mathf.SmoothDamp(smothY, v, ref smothYvelocity, turnSmoothing);
            }//углы хуелера какието для вычисления поворота
            else
            {
                smothX = h;
                smothY = v;
            }

            if (lockOn)
            {

            }//менять поведение в зависимости от параметра(это состояние блокировки оставил на будущее но там придумаем как его редакнуть)

            lookAngle += smothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);//поворот 

            tillAngle -= smothY * targetSpeed;
            tillAngle = Mathf.Clamp(tillAngle, minAngle, maxAngle);
            pivot.localRotation = Quaternion.Euler(tillAngle, 0, 0);//ага пивот и этот поворачиваем збс аа понял по пивоту кордината у повороот а по контролеру х
        }

        public static CameraManager singleton;
        void Awake()
        {
            singleton = this;
        }//сбрасываем камеру если в сцене неск камер то будут баги поэт сбрасываем
    }
}
