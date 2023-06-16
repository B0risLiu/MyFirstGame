using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [SerializeField] protected Animator SceneTransitionAnimator;
    [SerializeField] protected GameObject FadePanel;
    [SerializeField] protected Slider LoadingProgressBar;
    [SerializeField] protected float LoadingProgressBarSpeed;

    protected const string Close = "Close";
    protected const string Open = "Open";

    protected AsyncOperation SceneLoading;
    protected bool IsAnimationComplete;

    public void OnClosingAnimationComplete()
    {
        IsAnimationComplete = true;
    }

    public void OnOpeningAnimationComplete()
    {
        FadePanel.SetActive(false);
    }

    protected IEnumerator UpdateLoadingProgressBar()
    {
        while (SceneLoading.progress < 1)
        {
            LoadingProgressBar.value = Mathf.Lerp(LoadingProgressBar.value, SceneLoading.progress, Time.deltaTime * LoadingProgressBarSpeed);
            yield return null;
        }
    }

    protected void StartClosingAnimation()
    {
        FadePanel.SetActive(true);
        SceneTransitionAnimator.SetTrigger(Close);
        LoadingProgressBar.value = 0;
    }

    protected void StartOpeningAnimation()
    {
        FadePanel.SetActive(true);
        SceneTransitionAnimator.SetTrigger(Open);
        LoadingProgressBar.value = 1;
    }
}
