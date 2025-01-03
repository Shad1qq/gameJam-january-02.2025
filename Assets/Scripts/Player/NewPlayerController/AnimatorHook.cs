using UnityEngine;

namespace SA
{
    public class AnimatorHook : MonoBehaviour
    {
        Animator anim;
        StatManager states;
        Rigidbody rigid;

        bool rolling;
        public float rm_multi;
        float roll_t;
        float delta;

        AnimationCurve roll_Curve;

        public void Init(StatManager st)
        {
            states = st;

            if (st != null)
            {
                anim = st.anim;
                rigid = st.rigid;
                roll_Curve = st.roll_curve;
                delta = st.delta;
            }
        }
        public void CloseRoll()
        {
            if (rolling == false)
                return;

            rm_multi = 1;
            roll_t = 0;
            rolling = false;
        }
        public void InitForRoll()
        {
            rolling = true;
            roll_t = 0;
        }
        private void OnAnimatorMove()
        {
            if (states == null)
                return;

            if (rigid == null)
                return;

            if (states.canMove)
                return;

            delta = states.delta;

            rigid.drag = 0;

            if (rm_multi == 0)
                rm_multi = 1; 

            if(rolling == false)
            {
                Vector3 delta2 = anim.deltaPosition;
                delta2.y = 0;
                Vector3 v = (delta2 * rm_multi) / delta;

                if(!rigid.isKinematic)
                    rigid.velocity = v;
            }
            else
            {
                roll_t += delta / 0.6f;
                if (roll_t > 1)
                    roll_t = 1;

                if (states == null)
                    return;

                float zValue = roll_Curve.Evaluate(roll_t);
                Vector3 v = Vector3.forward * zValue;
                Vector3 relativ = transform.TransformDirection(v);
                Vector3 v2 = (relativ * rm_multi);
                rigid.velocity = v2;
            }
        }
    }
}
