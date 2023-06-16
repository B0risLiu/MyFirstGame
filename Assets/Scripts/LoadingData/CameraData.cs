using UnityEngine;

public class CameraData
{
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    public bool IsCameraFollowingModeOn { get; private set; }

    public CameraData(Vector3 position, Quaternion rotation, bool isCameraFollowingModeOn)
    {
        Position = position;
        Rotation = rotation;
        IsCameraFollowingModeOn = isCameraFollowingModeOn;
    }
}
