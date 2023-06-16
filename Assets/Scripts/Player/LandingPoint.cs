using UnityEngine;

public class LandingPoint : MonoBehaviour
{
    private Vector3 _offset = new Vector3(0, 0.1f, 0);

    public void SetNewPosition(Vector3 newPosition)
    {
        transform.position = newPosition + _offset;
    }
}
