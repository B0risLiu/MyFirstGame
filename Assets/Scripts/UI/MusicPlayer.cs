using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private float _pauseBetweenClips;
    [SerializeField] private List<AudioClip> _musicList;

    private Coroutine _workingCouroutine;

    private void OnEnable()
    {
        _workingCouroutine = StartCoroutine(PlayMusic());
    }

    private void OnDisable()
    {
        if (_workingCouroutine != null)
            StopCoroutine(_workingCouroutine);
    }

    private IEnumerator PlayMusic()
    {
        while (true)
        {
            int randomMusicIndex = Random.Range(0, _musicList.Count);
            _musicSource.PlayOneShot(_musicList[randomMusicIndex]);
            yield return new WaitForSeconds(_musicList[randomMusicIndex].length + _pauseBetweenClips);
        }
    }
}
