using UnityEngine;

namespace SA
{
    public class InputHandler : MonoBehaviour
    {
        float horizontal;
        float vertical;
        bool b_input;
        bool a_input;
        bool x_input;
        bool y_input;

        bool rb_input;//инпут клавиатуры
        float rt_axis;//инпут телефона\джостика
        bool rt_input;
        bool lb_input;
        float lt_axis;
        bool lt_input;

        StatManager states;
        CameraManager camManager;

        float delta;

        void Start()
        {
            camManager = CameraManager.singleton;
            camManager.Init(this.transform);

            states = GetComponent<StatManager>();
            states.Init();
        }
        public void GetInput()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            b_input = Input.GetButton("B");
            a_input = Input.GetButton("A");
            y_input = Input.GetButtonUp("Y");//эту залупу
            x_input = Input.GetButton("X");
            rt_input = Input.GetButton("RT");
            rt_axis = Input.GetAxis("RT");//настроить эту хуету в инпутах проекта
            if (rt_axis != 0)
                rt_input = true;

            lt_input = Input.GetButton("LT");
            lt_axis = Input.GetAxis("LT");//и гдето ще залупа которую надо вынести в апдейт тк с фиксир обновлением они плохо робят 
            if (lt_axis != 0)
                lt_input = true;
            rb_input = Input.GetButton("RB");
            lb_input = Input.GetButton("LB");//обрабатываем ввод если хочеш сделать новую систему или управление с мобилы тебе здесь копатся
        }

        private void FixedUpdate()
        {
            GetInput();
            UpdateStates();
            states.FixedTick(delta);
            camManager.Tick(delta);
        }
        private void Update()
        {
            delta = Time.fixedDeltaTime;//кешируем эту парашу для оптимизации я так понял 
            states.Tick(delta);
        }
        void UpdateStates()
        {
            states.horizontal = horizontal;
            states.vertical = vertical;

            Vector3 v = vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;
            states.moveDir = (v + h).normalized;//воо ограниченый вод а мову эмоунт это ввод относительно локальных кординат 
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);//вот мув эмоунт это ввод на передвижение ограниченый единицей или че не ясно

            states.rb = rb_input;
            states.rt = rt_input;
            states.lb = lb_input;
            states.lt = lt_input;//и тут их обнулять после всех операций
        }//в завис от ввода тут вызываем методы из стат менеджера например можно добавить состояние деша
    }
}
