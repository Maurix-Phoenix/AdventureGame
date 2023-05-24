using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public static void SafeSpawn(GameObject prefab, Vector3 position,
    Transform parent = null, bool findNearValidPosition = false, float nearPositionRange = 1f, bool spawnAnyways = false)
    {

        if (prefab != null)
        {
            if (spawnAnyways)
            {
                GameObject.Instantiate(prefab, position, Quaternion.identity, parent);
            }
            else
            {

            }
        }
        else
        {
            MXDebug.Log("");
        }
    }
}
