using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    [SerializeField] private GameObject _helpPanel;

    private PlayerInput _input;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.Help.performed += ctx => OnHelpClick();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnHelpClick()
    {
        _helpPanel.SetActive(!_helpPanel.activeSelf);
    }
}
