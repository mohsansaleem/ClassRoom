using UnityEngine;
using UnityEngine.EventSystems;

namespace PG.CastleBuilder
{
    public static class InputUtils
    {
        private static bool IsTouchSupported()
        {
            #if UNITY_EDITOR
            return false;
            #endif
            return Input.touchSupported;
        }
        
        public static bool IsPointerOverUI(int pointerIndex)
        {
            if (IsTouchSupported())
            {
                return Input.touchCount > pointerIndex && EventSystem.current.IsPointerOverGameObject(
                    pointerId: Input.GetTouch(pointerIndex).fingerId);
            }
            return EventSystem.current.IsPointerOverGameObject();
        }
        
        public static bool GetPointerDown(int pointerIndex)
        {
            if (IsTouchSupported())
            {
                return Input.touchCount > pointerIndex && Input.GetTouch(pointerIndex).phase == TouchPhase.Began;
            }
            return Input.GetMouseButtonDown(pointerIndex);
        }
        
        public static bool GetPointerMove(int pointerIndex)
        {
            if (IsTouchSupported())
            {
                return Input.touchCount > pointerIndex && Input.GetTouch(pointerIndex).phase == TouchPhase.Moved;
            }
            return Input.GetMouseButton(pointerIndex);
        }

        public static bool GetPointerUp(int pointerIndex)
        {
            if (IsTouchSupported())
            {
                return Input.touchCount > pointerIndex && Input.GetTouch(pointerIndex).phase == TouchPhase.Ended;
            }
            return Input.GetMouseButtonUp(pointerIndex);
        }
    }
}