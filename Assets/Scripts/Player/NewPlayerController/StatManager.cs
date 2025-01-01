using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class StatManager : HealthCharcters
    {
        [Header("Init")]
        public GameObject activModel;
        
        [Header("Inputs")]
        public float vertical;
        public float horizontal;
        public float moveAmount;
        public Vector3 moveDir;
        public bool rt, rb, lt, lb;

        [Header("Stats")]
        public float moveSpeed = 2;
        public float rotationSpeed = 5f;
        public float toGround = 0.5f;

        [Header("States")]
        public bool onGround;

        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rigid;

        [HideInInspector]
        public float delta;
        [HideInInspector]
        public LayerMask ignoreLayer;

        [Header("r������ �� ��������� �����")]
        [SerializeField] private Material damageMaterial;
        private Material defoltMaterial;
        private SkinnedMeshRenderer rend;

        [Header("UI Settings")]
        [SerializeField] private Slider sliderHealth;

        public void Init()
        {
            rend = GetComponentInChildren<SkinnedMeshRenderer>();//эта моя залупа все ломала ибо на activModel не было скин рендера
            defoltMaterial = rend.material;

            SetupAnimator();

            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.drag = 4;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;//настройка риджен боди по дефолту

            gameObject.layer = 8;//устанавливаем слой игроку в ручную на всякий пожарный
            ignoreLayer = ~(0 << 9);//ставим игнорирование слоев детектора земли

            DamageReaction += DamagePlayer;//моя залупа меняющая материал при всосе урона
            //anim.SetBool("onGround", true);я закомитил переход в состояние падения тк его у нас нету, это могло создать ошибку, раскомить если понадобится
        }
        void SetupAnimator()
        {
            if (activModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                if (anim == null)
                    Debug.Log("not model found");
                else
                    activModel = anim.gameObject;
            }

            if (anim == null)
                anim = activModel.GetComponent<Animator>();
        }
        
        public void FixedTick(float d)
        {
            delta = d;//уже известно что это

            rigid.drag = (moveAmount > 0 || !onGround) ? 0 : 4;//настройка сопротивления воздуха или чето такое оно влияет на поведение игрока вращает или типо того
            //если не двигаемся или в воздухе то нулевое сопротивление иначе равно 4
            float targetSpeed = moveSpeed;//по нейму ясно

            if (onGround)
                rigid.velocity = moveDir * (targetSpeed * moveAmount);//если мы на земле то мы можем ходить

            Vector3 targetDirection = moveDir;
            targetDirection.y = 0;
            if (targetDirection == Vector3.zero)
                targetDirection = transform.forward;
            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * moveAmount * rotationSpeed);
            transform.rotation = targetRotation;//поворот

            HandleMovementAnimations();//анимации тригерим
        }
        public void Tick(float d)
        {
            delta = d;//всасываем время фикс обновления
            onGround = OnGround();//проверка на земле ли мы

            //anim.SetBool("onGround", onGround); в методе инициализации уже сказал что это за залупа 
        }
        void HandleMovementAnimations()
        {
            bool mov = false;
            if (moveAmount != 0)
                mov = true;

            anim.SetBool("canMove", mov);//крч если движемся то передаем тру в аниматор
        }
        public bool OnGround()
        {
            bool r = false;

            Vector3 origin = transform.position + (Vector3.up * toGround);
            Vector3 dir = -Vector3.up;
            float dis = toGround + 0.3f;

            RaycastHit hit;
            Debug.DrawRay(origin, dir * dis);
            if (Physics.Raycast(origin, dir, out hit, dis, ignoreLayer))//рейкастим землю
            {
                r = true;
                Vector3 targetPosition = hit.point;
                transform.position = targetPosition;//присасываемся к земле?
            }

            return r;
        }

        private void DamagePlayer()//моя залупа с материалом работает с хил чарактером
        {
            if(sliderHealth != null)
                sliderHealth.value = _maxHealth;//������� �� ��������� ����
            if(damageMaterial != null)
            {
                rend.materials = new Material[] { damageMaterial }; // ������������� ������� ��������
                Invoke(nameof(RefreshMaterual), 0.2f);
            }
        }
        private void RefreshMaterual()//тоже моя залупа
        {
            rend.materials = new Material[] { defoltMaterial }; // ���������� �������� ��������
        }
    }
}
