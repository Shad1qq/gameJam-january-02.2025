using UnityEngine;

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
        public bool rollInput;
        public bool itemInput;

        [Header("Stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotationSpeed = 5f;
        public float toGround = 0.5f;
        public float rollSpeed = 1f;

        [Header("States")]
        public bool onGround;
        public bool run;
        public float endMoveh;
        public float endMovev;
        public bool lockOn;
        public bool inAction;
        public bool canMove;
        public bool isTwoHanded;
        public bool usingItem;

        [Header("Other")]
        public Transform lockOnTransform;
        public AnimationCurve roll_curve;

        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public AnimatorHook a_hook;

        [HideInInspector]
        public float delta;
        [HideInInspector]
        public LayerMask ignoreLayer;

        float actionDelay = 0;

        public void Init()
        {
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
            ignoreLayer = ~(1 << 9);

            anim.SetBool("onGround", true);
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
            delta = d;

            usingItem = anim.GetBool("interactive");

            DetectAction();
            HandleRolls();

            if (inAction)
            {
                anim.applyRootMotion = true;

                actionDelay += delta;
                if (actionDelay > 0.5f)
                {
                    inAction = false;
                    actionDelay = 0;
                }
                else
                    return;
            }

            canMove = anim.GetBool("canMoving");

            if (!canMove)
                return;

            a_hook.CloseRoll();

            anim.applyRootMotion = false;

            rigid.drag = (moveAmount > 0 || !onGround) ? 0 : 4;

            float targetSpeed = moveSpeed;

            if (usingItem)
                moveAmount = Mathf.Clamp(moveAmount, 0, 0.7f);

            Vector3 targetDir = (lockOn == false) ? moveDir
                :
                (lockOnTransform != null) ?
                lockOnTransform.position - transform.position : moveDir;

            if (run)
            {
                targetDir = moveDir;
                targetSpeed = runSpeed;
            }
            if (onGround)
                rigid.velocity = moveDir * (targetSpeed * moveAmount);

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

            if (canMove == false || usingItem)
                return;

            string targetAnim = null;

            targetAnim = "1";

            if (string.IsNullOrEmpty(targetAnim))
                return;

            canMove = false;
            inAction = true;

            anim.CrossFade(targetAnim, 0.2f);
        }
        public void Tick(float d)
        {
            delta = d;
            onGround = OnGround();

            anim.SetBool("onGround", onGround);
        }
        void HandleRolls()
        {
            if (!rollInput || usingItem)
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
                a_hook.rm_multi = -10f;

            anim.SetFloat("vertical", v);
            anim.SetFloat("horizontal", h);

            canMove = false;
            inAction = true;
            anim.Play("Rolls");
            a_hook.InitForRoll();
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
        public void HandledTwoHanded()
        {
            anim.SetBool("twoHanded", isTwoHanded);
        }
    }
}