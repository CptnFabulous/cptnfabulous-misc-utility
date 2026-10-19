using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class AsyncUtility
    {
        public delegate void LerpLoop(ref float t);

        public static IEnumerator WaitOnLerp(float secondsToWait, LerpLoop frameAction)
        {
            float t = 0;
            do
            {
                t += Time.deltaTime / secondsToWait;
                t = Mathf.Clamp01(t);
                frameAction.Invoke(ref t);
                if (t > 1) yield break;
                yield return t;
            }
            while (t < 1);
            t = 1;
            frameAction.Invoke(ref t);
        }
        public static IEnumerator LoopWhile(System.Action loop, System.Func<bool> criteria)
        {
            do
            {
                yield return null;
                loop.Invoke();
            }
            while (criteria.Invoke());
        }
    }
}
