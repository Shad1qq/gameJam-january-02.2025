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
        bool space_input;

        bool rb_input;
        bool rt_input;
        bool lb_input;
        bool lt_input;

        float b_timer;

        bool rightAxis_down;

        StatManager states;
        CameraManager camManager;
        RagdolController ragControl;

        float delta;

        void Start()
        {
            Application.targetFrameRate = 60;

            states = GetComponent<StatManager>();
            
            camManager = CameraManager.singleton;
            camManager.Init(states);

            states.Init();

            ragControl = states.activModel.AddComponent<RagdolController>();
            ragControl.Init(states.anim);
        }

        void GetInputUp()
        {
            if(!y_input)
                y_input = Input.GetKeyUp(KeyCode.F);
            if (!rightAxis_down)
                rightAxis_down = Input.GetKeyUp(KeyCode.X);
        }
        void GetInput()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            b_input = Input.GetKey(KeyCode.LeftShift);
            a_input = Input.GetKey(KeyCode.Q);
            x_input = Input.GetKey(KeyCode.R);

            rt_input = Input.GetButton("RT");
            lt_input = Input.GetButton("LT");
            rb_input = Input.GetButton("RB");
            lb_input = Input.GetButton("LB");

            space_input = Input.GetKey(KeyCode.Space);

            if (b_input)
                b_timer += delta;
            else
                b_timer = 0;
        }

        private void FixedUpdate()
        {
            GetInput();
            UpdateStates();
            states.FixedTick(delta);
            camManager.Tick(delta);
            ResetInputNState();
        }
        private void Update()
        {
            GetInputUp();

            delta = Time.fixedDeltaTime;
            states.Tick(delta);
        }
        void UpdateStates()
        {
            if (states.isDead)
            {
                ragControl.RagdolTrue();
                return;
            }

            states.horizontal = horizontal;
            states.vertical = vertical;

            Vector3 v = vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;
            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);

            if (b_input && b_timer > 0.5f)//если держим кнопку долго то бежим если не долго то делаем рывок 
                states.run = (states.moveAmount > 0.2f);
            else
                states.run = false;//бег

            if (b_input && b_timer > 0 && b_timer < 0.5f)
                states.rollInput = true;//рывок

            states.rb = rb_input;
            states.rt = rt_input;
            states.lb = lb_input;
            states.lt = lt_input;

            states.jampingInput = space_input;

            if (rightAxis_down)
                TransformUpdateTarget(states.lockOnTransform);
        }
        private void TargetUpdate()
        {
            states.lockOn = !states.lockOn;

            camManager.lockOn = states.lockOn;
            camManager.lockOnTransform = states.lockOnTransform;
            states.lockOnTransform = camManager.lockOnTransform;
            rightAxis_down = false;
        }
        public void TransformUpdateTarget(Transform tr)
        {
            states.lockOnTransform = tr;
            TargetUpdate();
        }
        private void ResetInputNState()
        {
            if (states.rollInput)
                states.rollInput = false;
        }
    }
}
