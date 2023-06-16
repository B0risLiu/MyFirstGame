using Assets.Scripts.Interfaces;
using UnityEngine;

public class ResourceDeposit : MonoBehaviour, IIDable
{
    [SerializeField] private int _resourceAmount;

    public int ID { get; private set; }
    public bool IsExtractionStarted { get; private set; }
    public int ResourceAmount => _resourceAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (IsExtractionStarted == false && other.TryGetComponent(out ResourceDetector resourceDetector))
            resourceDetector.StartSearch(GetComponent<ResourceDeposit>());

        if (other.TryGetComponent(out ResourceCollector resourceCollector))
        {
            resourceCollector.SetDevelopedDeposit(GetComponent<ResourceDeposit>());
            IsExtractionStarted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsExtractionStarted == false && other.TryGetComponent(out ResourceDetector resourceDetector))
            resourceDetector.StopSearch();

        if (other.TryGetComponent(out ResourceCollector resourceCollector))
            IsExtractionStarted = false;
    }

    public bool TryExtractResource(int amount)
    {
        if (_resourceAmount >= amount)
        {
            _resourceAmount -= amount;
            return true;
        }
        else
            return false;
    }

    public void AssignID(int id)
    {
        ID = id;
    }

    public void SetResourceAmount(int amount)
    {
        _resourceAmount = amount;
    }
}
