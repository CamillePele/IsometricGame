using System.Collections;
using UnityEngine;

namespace Utils
{
    public static class GeneralUtils
    {
        public static void AfterXFrames(MonoBehaviour monoBehaviour, int frames, System.Action action)
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
    }
}