using UnityEngine;

public class ResourceCollector : Building
{
    [SerializeField] private int _resoursesPerMinute;

    private float _extractTime = 60;
    private float _runtime;
    private Player _player;
    private ResourceDeposit _developedDeposit;

    private void OnEnable()
    {
        ResetHealth();
    }

    private void Update()
    {
        if (_developedDeposit != null)
        {
            _runtime += Time.deltaTime;

            if (_runtime >= _extractTime)
            {
                _runtime = 0;

                if (_developedDeposit.TryExtractResource(_resoursesPerMinute))
                    _player.ChangeResourcesAmount(_resoursesPerMinute);
            }
        }
    }

    public void SetDevelopedDeposit(ResourceDeposit resourceDeposit)
    {
        _developedDeposit = resourceDeposit;
    }

    public void SetOwner(Player player)
    {
        _player = player;
    }

    protected override void Die()
    {
        Debug.Log("Коллектор БАБА-А-АХ!!!");
        _developedDeposit = null;
        gameObject.SetActive(false);
    }
}
