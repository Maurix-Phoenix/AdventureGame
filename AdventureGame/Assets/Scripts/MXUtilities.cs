
using System;
using System.Collections;
using System.Threading;
using UnityEngine;


public static class MXUtilities
{
    public static class MXDebug
    {
        public enum LogType
        {
            Normal,
            Warning,
            Error,
        }
        public static void Log(string logMessage, LogType type = LogType.Normal)
        {
            #if UNITY_EDITOR
            switch (type)
            {
                case LogType.Normal: { Debug.Log($"<color=orange>MXDebug</color> <color=white>[LOG]:</color> {logMessage}"); break; }
                case LogType.Warning: { Debug.LogWarning($"<color=orange>MXDebug</color> <color=yellow>[WARNING]:</color> {logMessage}"); break; }
                case LogType.Error: { Debug.LogError($"<color=orange>MXDebug</color> <color=red>[ERROR]:</color> {logMessage}"); break; }
                default: { break; }
            }
            #endif
        }
    }

    public static class MXProgramFlow
    {
        public static IEnumerator EWait(float waitTime)
        {
            float counter = 0;
            while(counter < waitTime)
            {
                counter += Time.deltaTime;
                yield return null;
            }
        }
    }
}
