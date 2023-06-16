using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class AttackingBodyPart : MonoBehaviour
{
    public event UnityAction PlayerIsHitted;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            PlayerIsHitted.Invoke();
    }
}
