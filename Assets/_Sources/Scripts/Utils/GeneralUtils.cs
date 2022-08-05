using System.Collections;
using UnityEngine;

namespace Utils
{
    public static class GeneralUtils
    {
        public static void AfterXFrames(this MonoBehaviour monoBehaviour, int frames, System.Action action)
        {
            monoBehaviour.StartCoroutine(AfterXFramesCoroutine(frames, action));
        }
        
        private static IEnumerator AfterXFramesCoroutine(int frames, System.Action action)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            action();
        }
        
        public static void AfterXSeconds(this MonoBehaviour monoBehaviour, float seconds, System.Action action)
        {
            monoBehaviour.StartCoroutine(AfterXSecondsCoroutine(seconds, action));
        }
        
        private static IEnumerator AfterXSecondsCoroutine(float seconds, System.Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }
    }
}