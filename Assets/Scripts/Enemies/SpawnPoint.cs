using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float _radius;

    private List<Transform> _pointsOfAppearingEnemy = new List<Transform>();

    public List<Transform> PointsOfAppearingEnemy => _pointsOfAppearingEnemy;

    private void Awake()
    {
        gameObject.GetComponentsInChildren(_pointsOfAppearingEnemy);
    }

    public bool IsSpawnPointFree()
    {
        Collider[] collidersInSpawnPoint = Physics.OverlapSphere(transform.position, _radius);
        Collider enemyCollider = collidersInSpawnPoint.FirstOrDefault(collider => collider.gameObject.TryGetComponent(out Enemy enemy));
        return enemyCollider == null;
    }
}
