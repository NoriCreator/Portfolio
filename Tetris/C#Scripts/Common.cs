using UnityEngine;
using System.Collections;

namespace Commons
{
    public struct IntVector2
    {
        public int x;
        public int y;

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class UnityCommonsFunc
    {
        public static void EndGame()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
    #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
    #else
                Application.Quit();
    #endif
            }
        }
    }
}