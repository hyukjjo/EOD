using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundBGM : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _audioClipWait;
    [SerializeField] private AudioClip _audioClipStart;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        //_audioSource.clip = _audioClipWait;
        //_audioSource.Play();

        GameManager.Instance.MaintenanceStart += () =>
        {
            _audioSource.Stop();
            _audioSource.clip = _audioClipWait;
            _audioSource.Play();
        };

        GameManager.Instance.RoundStart += () =>
        {
            _audioSource.Stop();
            _audioSource.clip = _audioClipStart;
            _audioSource.Play();
        };

        GameManager.Instance.InfiniteModeStart += () =>
        {
            _audioSource.Stop();
            _audioSource.clip = _audioClipStart;
            _audioSource.Play();
        };
    }
}
