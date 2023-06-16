using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]

public class TextField : MonoBehaviour
{
    [SerializeField] private string _defaultText;
    [SerializeField] private float _showTime;

    private TextMeshProUGUI _textMeshPro;
    private Coroutine _workingCouroutine;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _textMeshPro.text = _defaultText;
    }

    public void ShowText(string text)
    {
        if (_workingCouroutine != null)
        {
            StopCoroutine(_workingCouroutine);
            _workingCouroutine = null;
        }
        
        _workingCouroutine = StartCoroutine(ShowTextWithDelay(text));
    }

    private IEnumerator ShowTextWithDelay(string text)
    {
        var delay = new WaitForSeconds(_showTime);
        _textMeshPro.text = text;
        yield return delay;
        _textMeshPro.text = _defaultText;
        _workingCouroutine = null;
    }
}
