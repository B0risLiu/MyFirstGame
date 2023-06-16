using TMPro;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _resourcesAmount;

    private void OnEnable()
    {
        _player.ResourcesAmountChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _player.ResourcesAmountChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(int money)
    {
        _resourcesAmount.text = money.ToString();
    }
}
