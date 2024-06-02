using UnityEngine;

namespace Assets.Scripts
{
    public class Utils
    {
        public static void CloseApplication()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
