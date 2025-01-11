using System;
using UnityEngine;

namespace SA
{
    public class StatManager : HealthCharcters
    {
        [Header("Init")]
        public GameObject activModel;

        [Header("Inputs")]
        internal float vertical;
        internal float horizontal;
        internal float moveAmount;
        internal Vector3 moveDir;
        internal bool rt, rb, lt, lb;
        internal bool rollInput;
        internal bool jampingInput;

        [Header("Stats")]
        public float moveSpeed = 2;
        public float runSpeed = 7f;
        public float rotationSpeed = 5f;
        public float toGround = 0.5f;
        public float rollSpeed = 10f;
        public float jumpForce = 5f;

        [Header("States")]
        internal bool onGround;
        internal bool run;
        internal float endMoveh;
        internal float endMovev;
        internal bool lockOn;
        internal bool inAction;
        internal bool canMove;
        internal bool jumping;
        float jumTimer = 0f;

        [Header("Other")]
        public Transform lockOnTransform;
        public AnimationCurve roll_curve;

        internal Animator anim;
        internal Rigidbody rigid;
        internal AnimatorHook a_hook;

        internal float delta;
        internal LayerMask ignoreLayer;

        float actionDelay = 0;

        internal bool dead;
        public event Action DeadEvent;
        void ReactDamage()
        {
        }
        void ReactionDead()
        {
            DeadEvent?.Invoke();
            dead = true;
        }
        public void Init()
        {
            DeadReaction += ReactionDead;
            DamageReaction += ReactDamage;
            SetupAnimator();

            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.drag = 4;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            a_hook = activModel.GetComponent<AnimatorHook>();
            if (a_hook == null)
                a_hook = activModel.AddComponent<AnimatorHook>();
            a_hook.Init(this);

            gameObject.layer = 8;
            ignoreLayer = ~((1 << 8) | (1 << 12) | (1 << 10));

            anim.SetBool("onGround", true);
        }
        private void OnDisable()
        {
            DeadReaction -= ReactionDead;
            DamageReaction -= ReactDamage;
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
            if (dead)
                return;

            delta = d;

            Jamping();
            DetectAction();
            HandleRolls();

            if (inAction)
            {
                anim.applyRootMotion = true;

                actionDelay += delta;
                if (actionDelay > 0.3f)
                {
                    inAction = false;
                    actionDelay = 0;
                }
                else
                    return;
            }

            if (jumping)
            {
                if (jumTimer > 0.3f)
                {
                    jumTimer = 0f;
                    jumping = false;
                }
                else
                    jumTimer += delta;
            }

            canMove = anim.GetBool("canMoving");

            if (!canMove)
                return;

            a_hook.CloseRoll();

            anim.applyRootMotion = false;

            rigid.drag = (moveAmount > 0 || !onGround) ? 0 : 4;

            float targetSpeed = moveSpeed;

            Vector3 targetDir = (lockOn == false) ? moveDir
                :
                (lockOnTransform != null) ?
                lockOnTransform.position - transform.position : moveDir;

            if (run)
            {
                targetDir = moveDir;
                targetSpeed = runSpeed;
            }

            Vector3 speeds;
            speeds = moveDir * (targetSpeed * moveAmount);
            if (!onGround)
                speeds.y = rigid.velocity.y;
            rigid.velocity = speeds;
            //Debug.Log(speeds);

            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * moveAmount * rotationSpeed);
            transform.rotation = targetRotation;

            anim.SetBool("lockOn", lockOn);

            if (lockOn == false)
                HandleMovementAnimations();
            else
                HandleLockOnAnimations(moveDir);
        }
        public void DetectAction()
        {
            if (rb == false && rt == false && lb == false && lt == false)
                return;

            if (!canMove || !onGround)
                return;

            string targetAnim = null;

            //подписываем анимации атаки если над
            targetAnim = "1";

            if (string.IsNullOrEmpty(targetAnim))
                return;

            canMove = false;
            inAction = true;

            anim.CrossFade(targetAnim, 0.2f);//тут переписать можно добавив поворот блокируовку если надо под нужный инпут сделать и если хочетка анимку накинуть по шаблону
        }
        public void Tick(float d)
        {
            delta = d;

            if(!jumping)
                onGround = OnGround();

            anim.SetBool("onGround", onGround);
        }
        void HandleRolls()
        {
            if (!rollInput || jumping || !onGround)
                return;

            if (!canMove)
                return;

            float v = vertical;
            float h = horizontal;

            v = (moveAmount > 0.3f) ? 1 : 0;
            h = 0;

            if (v != 0)
            {
                if (moveDir == Vector3.zero)
                    moveDir = transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = targetRot;
                a_hook.rm_multi = rollSpeed;
            }
            else
                a_hook.rm_multi = -5f;

            anim.SetFloat("vertical", v);
            anim.SetFloat("horizontal", h);

            canMove = false;
            inAction = true;

            anim.Play("Rolls");
            a_hook.InitForRoll();
        }
        void Jamping()
        {
            if (!jampingInput || jumping)
                return;

            if (!canMove)
                return;

            if (!onGround)
                return;

            rigid.velocity = new Vector3(rigid.velocity.x, jumpForce, rigid.velocity.z);

            jumping = true;
            onGround = false;

            anim.Play("Jump");
        }

        void HandleMovementAnimations()
        {
            anim.SetBool("run", run);
            anim.SetFloat("vertical", moveAmount, 0.2f, delta);
        }
        void HandleLockOnAnimations(Vector3 moveDir)
        {
            Vector3 relativDir = transform.InverseTransformDirection(moveDir);

            float h = relativDir.x;
            float v = relativDir.z;

            anim.SetFloat("vertical", v, 0.25f, delta);
            anim.SetFloat("horizontal", h, 0.25f, delta);

            anim.SetBool("run", run);
        }

        public bool OnGround()
        {
            bool r = false;

            Vector3 origin = transform.position + (Vector3.up * toGround);
            Vector3 dir = -Vector3.up;
            float dis = toGround + 0.3f;

            RaycastHit hit;
            Debug.DrawRay(origin, dir * dis);
            if (Physics.Raycast(origin, dir, out hit, dis, ignoreLayer))
            {
                r = true;
                Vector3 targetPosition = hit.point;
                transform.position = targetPosition;
            }

            return r;
        }
    }
}