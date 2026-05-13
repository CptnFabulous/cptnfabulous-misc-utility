using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class AnimationUtility
    {
        public static bool TrySetAnimatorTrigger(Animator animator, string name)
        {
            if (!AnimatorParameterExists(animator, name)) return false;
            animator.SetTrigger(name);
            return true;
        }
        public static bool TrySetAnimatorBool(Animator animator, string name, bool value)
        {
            if (!AnimatorParameterExists(animator, name)) return false;
            animator.SetBool(name, value);
            return true;
        }
        public static bool TrySetAnimatorInteger(Animator animator, string name, int value)
        {
            if (!AnimatorParameterExists(animator, name)) return false;
            animator.SetInteger(name, value);
            return true;
        }
        public static bool TrySetAnimatorFloat(Animator animator, string name, float value)
        {
            if (!AnimatorParameterExists(animator, name)) return false;
            animator.SetFloat(name, value);
            return true;
        }
        public static bool AnimatorParameterExists(Animator animator, string name)
        {
            if (animator == null) return false;
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name == name) return true;
            }
            return false;
        }
    }
}
