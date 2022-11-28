using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AudioManager : SingletonWithMono<AudioManager>
    {
        [SerializeField] private GameObject _tmpGameObject;

        private Dictionary<string, GameObject> _audioDic;

        public void Init()
        {
            _audioDic = new Dictionary<string, GameObject>();
        }

        public void PlayAudioOnce(string path)
        {
            return;
            var audioSource = InitTempGo(path);
            var clip = Resources.Load<AudioClip>(path);
            audioSource.PlayOneShot(clip);
            GameObject.Destroy(audioSource.gameObject, clip.length);
        }

        public void PlayAudioLoop(string path)
        {
            return;
            var audioSource = InitTempGo(path);
            _audioDic.Add(path, audioSource.gameObject);
            var clip = Resources.Load<AudioClip>(path);
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void StopAudioLoop(string path)
        {
            return;
            var audioSource = _audioDic[path].GetComponent<AudioSource>();
            audioSource.Stop();
            GameObject.Destroy(audioSource.gameObject);
        }

        private AudioSource InitTempGo(string path)
        {
            var go = GameObject.Instantiate(_tmpGameObject, transform);
            go.SetActive(true);
            var audio = go.GetComponent<AudioSource>();
            return audio;
        }
    }
}