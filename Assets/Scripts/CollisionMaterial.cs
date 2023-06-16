using Assets.Scripts.Enum;
using UnityEngine;

public class CollisionMaterial : MonoBehaviour
{
    [SerializeField] private MaterialName _material;

    public MaterialName Name => _material;
}
