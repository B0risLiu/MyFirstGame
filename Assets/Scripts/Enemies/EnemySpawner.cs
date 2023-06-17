using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _minDistanceBetweenPlayerAndSpownPoint;
    [SerializeField] private Player _player;
    [SerializeField] private Teleport _teleport;
    [SerializeField] private List<ObjectPool> _enemyPools;
    [Header("WavesSettings")]
    [SerializeField] private Transform _spawnPointsRootObject;
    [SerializeField] private List<EnemyAmountInWave> _enemiesInWave;

    private List<Enemy> _activeEnemies = new();
    private List<DamageableObject> _possibleTargets = new();
    private List<EnemyData> _enemiesData;

    private List<SpawnPoint> _spawnPoints = new();
    private WaitForSeconds _delayBetweenWaves;
    private Coroutine _workingCouroutine;

    private void Awake()
    {
        _possibleTargets.Add(_player);
        _spawnPointsRootObject.GetComponentsInChildren(_spawnPoints);
    }

    private void OnEnable()
    {
        if (_enemiesData != null)
            SpawnEnemies();

        _teleport.BuldingTeleported += OnBuldingTeleported;
    }

    private void Update()
    {
        if (_workingCouroutine == null)
            _workingCouroutine = StartCoroutine(SendWave());
    }

    private void OnDisable()
    {
        _teleport.BuldingTeleported -= OnBuldingTeleported;
        _possibleTargets.Remove(_player);

        foreach (Enemy enemy in _activeEnemies)
            enemy.ObjectDied -= OnEnemyDied;

        foreach (DamageableObject damageableObject in _possibleTargets)
            damageableObject.ObjectDied -= OnTargetDied;
    }

    public void LoadSpawnerData(List<EnemyData> enemiesData)
    {
        _enemiesData = enemiesData;
    }

    public void SetDelayBetweenWawes(float value)
    {
        _delayBetweenWaves = new WaitForSeconds((1 - value) * 100);
    }

    public void SaveSpawnerData(LoadingData loadingData)
    {
        List<EnemyData> enemiesDate = _activeEnemies
            .Select(obj =>
            {
                Enemy enemy = obj.GetComponent<Enemy>();
                return new EnemyData(enemy.Name, enemy.transform.position, enemy.transform.rotation, enemy.Health);
            })
            .ToList();
        loadingData.SaveSpawnerData(enemiesDate);
    }

    private IEnumerator SendWave()
    {
        if (TryGetNextSpawnPoint(out SpawnPoint spawnPoint))
            SpawnEnemies(spawnPoint);

        yield return _delayBetweenWaves;
        _workingCouroutine = null;
    }

    private bool TryGetNextSpawnPoint(out SpawnPoint result)
    {
        var spawnPointsWithDistance = new Dictionary<float, SpawnPoint>();

        foreach(SpawnPoint spawnPoint in _spawnPoints)
        {
            if (spawnPoint.IsSpawnPointFree())
            {
                float distanse = Vector3.Distance(_player.transform.position, spawnPoint.transform.position);
                spawnPointsWithDistance[distanse] = spawnPoint;
            }
        }

        result = spawnPointsWithDistance
            .Where(pair => pair.Key > _minDistanceBetweenPlayerAndSpownPoint)
            .OrderBy(pair => pair.Key)
            .FirstOrDefault().Value;
        return result != null;
    }

    private void SpawnEnemy(ObjectPool pool, Vector3 position, Quaternion rotation, int health = 0)
    {
        if (pool.TryGetObject(out GameObject poolObject) && poolObject.TryGetComponent(out Enemy enemyInPool))
        {
            AddTargets(enemyInPool);
            _activeEnemies.Add(enemyInPool);
            enemyInPool.ObjectDied += OnEnemyDied;
            enemyInPool.SetTarget();
            enemyInPool.transform.position = position;
            enemyInPool.transform.rotation = rotation;
            enemyInPool.gameObject.SetActive(true);
            
            if (health != 0)
                enemyInPool.ResetHealth(health);
        }
    }

    private void SpawnEnemies(SpawnPoint spawnPoint)
    {
        var pointsOfAppearingEnemy = new Queue<Transform>(spawnPoint.PointsOfAppearingEnemy);

        foreach (EnemyAmountInWave enemyInWave in _enemiesInWave)
        {
            foreach (ObjectPool objectPool in _enemyPools)
            {
                if (objectPool.TryGetPoolObjectComponent(out Enemy enemy) && enemy.Name == enemyInWave.Name)
                {
                    for (int i = 0; i < enemyInWave.Amount; i++)
                    {
                        SpawnEnemy(objectPool, pointsOfAppearingEnemy.Dequeue().position, new Quaternion());
                    }
                }
            }
        }
    }

    private void SpawnEnemies()
    {
        foreach (EnemyData enemyData in _enemiesData)
        {
            foreach (ObjectPool objectPool in _enemyPools)
            {
                if (objectPool.TryGetPoolObjectComponent(out Enemy enemy) && enemy.Name == enemyData.Name)
                    SpawnEnemy(objectPool, enemyData.Position, enemyData.Rotation, enemyData.Health);
            }
        }
    }

    private void OnEnemyDied(DamageableObject damageableObject)
    {
        Enemy dieEnemy = _activeEnemies.First(enemy => enemy.IsDead);
        _activeEnemies.Remove(dieEnemy);
        dieEnemy.ObjectDied -= OnEnemyDied;
    }

    private void OnTargetDied(DamageableObject damageableObject)
    {
        _possibleTargets.Remove(damageableObject);
        damageableObject.ObjectDied -= OnTargetDied;

        foreach (Enemy enemy in _activeEnemies)
            enemy.RemoveTarget(damageableObject);
    }

    private void OnBuldingTeleported(DamageableObject damageableObject)
    {
        _possibleTargets.Add(damageableObject);
        damageableObject.ObjectDied += OnTargetDied;
        
        foreach (Enemy enemy in _activeEnemies)
            enemy.AddTarget(damageableObject);
    }

    private void AddTargets(Enemy enemy)
    {
        foreach (DamageableObject damageableObject in _possibleTargets)
            enemy.AddTarget(damageableObject);
    }
}
