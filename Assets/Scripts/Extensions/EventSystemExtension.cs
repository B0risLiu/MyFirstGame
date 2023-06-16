using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Extensions
{
    public static class EventSystemExtension
    {
        public static bool TryGetFirstComponentUnderPointer<T>(this EventSystem eventSystem, PointerEventData eventData, out T result) where T : class
        {
            result = null;
            var hits = new List<RaycastResult>();
            eventSystem.RaycastAll(eventData, hits);

            foreach (var hit in hits)
                if (hit.gameObject.TryGetComponent<T>(out result))
                    return true;

            return false;
        }
    }
}
