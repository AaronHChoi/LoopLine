using UnityEngine;

namespace Core.Utilities
{
    public class MouseController : MonoBehaviour
    {
        private void Start()
        {
            LockCursor();
        }
        public static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        public static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    } 
}