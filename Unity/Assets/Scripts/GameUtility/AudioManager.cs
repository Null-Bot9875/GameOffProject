using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class AudioManager : SingletonWithMono<AudioManager>
    {
        [SerializeField] private GameObject _tmpGameObject;

        private Dictionary<string, AudioSource> _audioDic;

        public void Init()
        {
            _audioDic = new Dictionary<string, AudioSource>();
            _tmpGameObject = Resources.Load<GameObject>(GamePath.PrefabPath + "AudioSource");
        }

        //单次播放，多次播放该音效的时候后面调用的不生效
        public void PlayAudioSingle(string path)
        {
            Debug.Log($"Play{path}");
            var clip = Resources.Load<AudioClip>(path);
            if (clip == null)
                return;

            //正在播放
            if (_audioDic.ContainsKey(path))
            {
                return;
            }

            var audioSource = InitTempGo();
            _audioDic.Add(path, audioSource);
            audioSource.PlayOneShot(clip);

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(clip.length);
            sequence.AppendCallback(() =>
            {
                _audioDic.Remove(path);
                GameObject.Destroy(audioSource.gameObject);
            });
        }

        public void PlayAudioOnce(string path)
        {
            Debug.Log($"Play{path}");
            var clip = Resources.Load<AudioClip>(path);
            if (clip == null)
                return;
            var audioSource = InitTempGo();
            audioSource.PlayOneShot(clip);
            GameObject.Destroy(audioSource.gameObject, clip.length);
        }

        public void PlayAudioLoop(string path)
        {
            Debug.Log($"Play{path}");
            var clip = Resources.Load<AudioClip>(path);
            if (clip == null)
                return;

            AudioSource audioSource;
            if (!_audioDic.ContainsKey(path))
            {
                audioSource = InitTempGo();
                _audioDic.Add(path, audioSource);
            }
            else
            {
                audioSource = _audioDic[path];
            }

            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void StopAudioLoop(string path)
        {
            if (!_audioDic.ContainsKey(path))
                return;
            var audioSource = _audioDic[path];
            audioSource.Stop();
            GameObject.Destroy(audioSource.gameObject);
            _audioDic.Remove(path);
        }

        private AudioSource InitTempGo()
        {
            var go = GameObject.Instantiate(_tmpGameObject, transform);
            go.SetActive(true);
            var audio = go.GetComponent<AudioSource>();
            return audio;
        }
    }
}