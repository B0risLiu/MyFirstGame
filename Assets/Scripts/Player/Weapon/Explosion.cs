using Assets.Scripts.Enum;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LifeTimer))]

public class Explosion : BulletCollisionSubeffect
{
    [SerializeField] private int _damage;
    [SerializeField] private int _radius;
    [SerializeField] private float _shockwaveForce;
    [SerializeField] private float _shockwaveUpwardsModifier;
    [SerializeField] private float _100PerñentDamageZoneRadius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private WeaponLable _weaponWhichHasBeenShooted;

    private const float MinUpOffset = 0.01f;
    private const float MaxUpOffset = 0.09f;

    private TerrainChangeData _terrainChangeData;
    private WaitForSeconds _delay = new WaitForSeconds(0.2f);
    private float[,] _explosionDensityMapPrefab = new float[8, 8]
    {
        {1, 1, 1,0.5f,0.5f, 1, 1, 1},
        {1,0.5f,0.5f,0,0,0.5f,0.5f,1},
        {1,0.5f, 0, 0, 0, 0,0.5f,1},
        {0.5f, 0, 0, 0, 0, 0, 0,0.5f },
        {0.5f, 0, 0, 0, 0, 0, 0,0.5f },
        {1,0.5f, 0, 0, 0, 0,0.5f,1},
        {1,0.5f,0.5f,0,0,0.5f,0.5f,1},
        {1, 1, 1,0.5f,0.5f, 1, 1, 1}
    };

    public TerrainChangeData TerrainChangeData => _terrainChangeData;
    public WeaponLable WeaponWichHasBeenSooted => _weaponWhichHasBeenShooted;

    public override void SetPositionAndActivate(Vector3 position, Quaternion rotation)
    {
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 50, _layerMask))
        {
            transform.position = hit.point + Vector3.up * Random.Range(MinUpOffset, MaxUpOffset);
            transform.rotation = Quaternion.FromToRotation(transform.forward * -1, hit.normal) * transform.rotation;
        }

        gameObject.SetActive(true);
        Collider[] collidersInBlastRadius = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider collider in collidersInBlastRadius)
        {
            if (collider.gameObject.TryGetComponent(out DamageableObject actor))
                DamageActor(actor);
        }

        StartCoroutine(SendShockwave());
        SetTerrainImpact(Terrain.activeTerrain);
    }

    public void SaveTerrainChangeData(TerrainChangeData terrainChangeData)
    {
        _terrainChangeData = terrainChangeData;
    }

    private int CalculateDamage(float distance)
    {
        if (distance <= _100PerñentDamageZoneRadius)
            return _damage;
        else
            return Mathf.RoundToInt(_damage * (1 - distance / _radius));
    }

    private void DamageActor(DamageableObject actor)
    {
        float distance = Vector3.Distance(transform.position, actor.transform.position);
        int damage = CalculateDamage(distance);
        actor.TakeDamage(damage);
    }

    private IEnumerator SendShockwave()
    {
        yield return _delay;
        Collider[] collidersInBlastRadius = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider collider in collidersInBlastRadius)
        {
            if (collider.gameObject.TryGetComponent(out Rigidbody rigidbody) && rigidbody.isKinematic == false)
                rigidbody.AddExplosionForce(_shockwaveForce, transform.position, _radius, _shockwaveUpwardsModifier, ForceMode.Impulse);
        }
    }

    private void SetTerrainImpact(Terrain terrain)
    {
        Vector2 offset = new Vector2(_explosionDensityMapPrefab.GetLength(0) / 2, _explosionDensityMapPrefab.GetLength(1) / 2);
        Vector2 explosionTerrainPosition = GetExplosionPositionOnTerrain(terrain) - offset;
        int[,] newTerrainDensityMap = GetNewTerrainDensityMap(explosionTerrainPosition, terrain);
        terrain.terrainData.SetDetailLayer((int)explosionTerrainPosition.x, (int)explosionTerrainPosition.y, 0, newTerrainDensityMap);
    }

    private Vector2 GetExplosionPositionOnTerrain(Terrain terrain)
    {
        Vector3 explosionLocalPosition = transform.position - terrain.transform.position;
        Vector2 explosionNormalisedPosition = new Vector2(Mathf.InverseLerp(0, terrain.terrainData.size.x, explosionLocalPosition.x), 
            Mathf.InverseLerp(0, terrain.terrainData.size.z, explosionLocalPosition.z));
        return new Vector2(Mathf.FloorToInt(explosionNormalisedPosition.x * terrain.terrainData.detailWidth), 
            Mathf.FloorToInt(explosionNormalisedPosition.y * terrain.terrainData.detailHeight));
    }

    private int[,] GetNewTerrainDensityMap(Vector2 explosionTerrainPosition, Terrain terrain)
    {
        int[,] map = terrain.terrainData.GetDetailLayer((int)explosionTerrainPosition.x, (int)explosionTerrainPosition.y, _explosionDensityMapPrefab.GetLength(0), _explosionDensityMapPrefab.GetLength(1), 0);

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for(int y = 0; y < map.GetLength(1); y++)
                    map[x, y] = Mathf.FloorToInt(map[x, y] * _explosionDensityMapPrefab[x, y]);
        }

        _terrainChangeData = new TerrainChangeData(explosionTerrainPosition, map);
        return map;
    }
}
