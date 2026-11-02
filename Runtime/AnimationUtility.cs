using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class AnimationUtility
    {
        public static bool TrySetTrigger(this Animator animator, string name)
        {
            if (!ParameterExists(animator, name)) return false;
            animator.SetTrigger(name);
            return true;
        }
        public static bool TrySetBool(this Animator animator, string name, bool value)
        {
            if (!ParameterExists(animator, name)) return false;
            animator.SetBool(name, value);
            return true;
        }
        public static bool TrySetInteger(this Animator animator, string name, int value)
        {
            if (!ParameterExists(animator, name)) return false;
            animator.SetInteger(name, value);
            return true;
        }
        public static bool TrySetFloat(this Animator animator, string name, float value)
        {
            if (!ParameterExists(animator, name)) return false;
            animator.SetFloat(name, value);
            return true;
        }
        public static bool ParameterExists(this Animator animator, string name)
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
