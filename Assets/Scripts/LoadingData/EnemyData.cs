using Assets.Scripts.Enum;
using UnityEngine;

public class EnemyData
{
    public EnemyName Name { get; private set; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
    public int Health { get; private set; }

    public EnemyData(EnemyName name, Vector3 position, Quaternion rotation, int health)
    {
        Name = name;
        Position = position;
        Rotation = rotation;
        Health = health;
    }
}
