using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _template;
    [SerializeField] private Transform _container;
    [SerializeField] private int _capacity;

    protected List<GameObject> Pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < _capacity; i++)
        {
            GameObject spawnObject = Instantiate(_template, _container);
            spawnObject.SetActive(false);
            Pool.Add(spawnObject);
        }
    }

    public bool TryGetObject(out GameObject result)
    {
        result = Pool.FirstOrDefault(obj => obj.activeSelf == false);
        return result != null;
    }

    public bool TryGetPoolObjectComponent<T>(out T component) where T : Component
    {
        component = null;

        if (TryGetObject(out GameObject poolObject) && poolObject.TryGetComponent(out T poolObjectcomponent))
            component = poolObjectcomponent;

        return component != null;
    }

    public List<GameObject> GetAllActive()
    {
        return Pool.Where(obj => obj.activeSelf == true).ToList();
    }
}