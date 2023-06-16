using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _spead;
    [SerializeField] private float _distance;
    [SerializeField] private float _height;
    [SerializeField] private Transform _mainBaseCameraLocation;

    private Vector3 _targetPosition;
    private bool _isCameraFollowingModeOn;

    private void Update()
    {
        if(_isCameraFollowingModeOn)
            CameraMove(_player.transform);
    }

    public void SaveCameraData(LoadingData loadingData)
    {
        loadingData.SaveCameraData(transform.position, transform.rotation, _isCameraFollowingModeOn);
    }

    public void FollowingModeOn(bool value)
    {
        _isCameraFollowingModeOn = value;
    }

    public void SetCameraToLocation(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void SetCameraToMainBase()
    {
        SetCameraToLocation(_mainBaseCameraLocation.position, _mainBaseCameraLocation.rotation);
    }

    public void SetCameraToLandingPoint(Transform landingPoint)
    {
        transform.position = GetCameraTargetLocation(landingPoint);
        transform.rotation = Quaternion.LookRotation(landingPoint.position - transform.position);
    }

    private void CameraMove(Transform target)
    {
        _targetPosition = GetCameraTargetLocation(target);
        transform.position = Vector3.Lerp(transform.position, _targetPosition, _spead * Time.deltaTime);
    }

    private Vector3 GetCameraTargetLocation(Transform target)
    {
        return new Vector3(target.position.x, target.position.y + _height, target.position.z - _distance);
    }
}
