using UnityEngine;

namespace SA
{
    public class InputHandler : MonoBehaviour
    {
        float horizontal;
        float vertical;
        bool b_input;
        bool a_input;
        bool r_input;
        bool y_input;
        bool space_input;

        bool rb_input;
        bool rt_input;
        bool lb_input;
        bool lt_input;

        bool shotMouse;

        float b_timer;

        bool rightAxis_down;

        StatManager states;
        CameraManager camManager;
        RagdolController ragControl;
        Pistol pis;

        float delta;

        bool r_inputs;

        void Start()
        {
            Application.targetFrameRate = 60;

            states = GetComponent<StatManager>();
            
            camManager = CameraManager.singleton;
            camManager.Init(states);

            states.Init();

            ragControl = states.activModel.AddComponent<RagdolController>();
            ragControl.Init(states.anim);

            pis = GetComponent<Pistol>();
            if(pis == null)
                pis = gameObject.AddComponent<Pistol>();

            states.run = !states.run;
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
            r_input = Input.GetKey(KeyCode.R);

            rt_input = Input.GetButton("RT");
            lt_input = Input.GetButton("LT");
            rb_input = Input.GetButton("RB");
            lb_input = Input.GetButton("LB");

            shotMouse = Input.GetKey(KeyCode.Mouse0);

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
            pis.FixUpdate();
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

            if (b_input && b_timer > 0 && b_timer < 0.5f)
                states.rollInput = true;//рывок

            states.rb = rb_input;
            states.rt = rt_input;
            states.lb = lb_input;
            states.lt = lt_input;

            states.jampingInput = space_input;

            pis.shot = shotMouse;
            pis.coolDown -= delta;

            if (r_input)
            {
                if(r_inputs)
                    states.run = !states.run;
                r_inputs = false;
            }
            else
                r_inputs = true;

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
