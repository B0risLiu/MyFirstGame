using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))] 

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float _x;
    [SerializeField] private float _y;

    private RawImage _rawImage;

    private void Start()
    {
        _rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        _rawImage.uvRect = new Rect(_rawImage.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _rawImage.uvRect.size);
    }
}
