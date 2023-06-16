 using UnityEngine;
using UnityEngine.InputSystem;

public class TorsoRotatior : MonoBehaviour
{
    [SerializeField] private LayerMask _hitLayerMask;
    [SerializeField] private Transform _target;

    private void Update()
    {
        Vector3 mousePosition = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), Camera.main.nearClipPlane);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, _hitLayerMask);

        if (hits.Length > 0)
        {
            Vector3 direction = new Vector3(hits[0].point.x, _target.position.y, hits[0].point.z);
            _target.position = direction;
        }
    }
}
