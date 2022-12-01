using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class AudioManager : SingletonWithMono<AudioManager>
    {
        [SerializeField] private GameObject _tmpGameObject;

        private Dictionary<string, AudioSource> _audioSFXDic;
        private AudioSource _audioMusic;

        public float SFXVolume { get; private set; } = 1;
        public float MusicVolume { get; private set; } = 1;

        public void Init()
        {
            _tmpGameObject = Resources.Load<GameObject>(GamePath.PrefabPath + "AudioSource");
            _audioSFXDic = new Dictionary<string, AudioSource>();
            _audioMusic = InitAudioGo(false);
        }

        //单次播放，多次播放该音效的时候后面调用的不生效
        public void PlayAudioSingle(string path)
        {
            var clip = Resources.Load<AudioClip>(path);
            if (clip == null)
                return;

            //正在播放
            if (_audioSFXDic.ContainsKey(path))
            {
                return;
            }

            var audioSource = InitAudioGo();
            _audioSFXDic.Add(path, audioSource);
            audioSource.PlayOneShot(clip);

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(clip.length);
            sequence.AppendCallback(() =>
            {
                _audioSFXDic.Remove(path);
                GameObject.Destroy(audioSource.gameObject);
            });
        }

        public void PlayAudioOnce(string path)
        {
            var clip = Resources.Load<AudioClip>(path);
            if (clip == null)
                return;
            var audioSource = InitAudioGo();
            audioSource.PlayOneShot(clip);
            GameObject.Destroy(audioSource.gameObject, clip.length);
        }

        public void PlayAudioLoop(string path)
        {
            var clip = Resources.Load<AudioClip>(path);
            if (clip == null)
                return;

            AudioSource audioSource;
            if (!_audioSFXDic.ContainsKey(path))
            {
                audioSource = InitAudioGo();
                _audioSFXDic.Add(path, audioSource);
            }
            else
            {
                audioSource = _audioSFXDic[path];
            }

            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }


        public void StopAudioLoop(string path)
        {
            if (!_audioSFXDic.ContainsKey(path))
                return;
            var audioSource = _audioSFXDic[path];
            audioSource.Stop();
            GameObject.Destroy(audioSource.gameObject);
            _audioSFXDic.Remove(path);
        }

        public void PlayMusicLoop(string path)
        {
            var clip = Resources.Load<AudioClip>(path);
            if (clip == null)
                return;
            _audioMusic.Stop();
            _audioMusic.clip = clip;
            _audioMusic.loop = true;
            _audioMusic.Play();
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
            _audioMusic.volume = MusicVolume;
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = volume;
            foreach (var item in _audioSFXDic)
            {
                item.Value.volume = SFXVolume;
            }
        }

        private AudioSource InitAudioGo(bool isSFX = true)
        {
            var go = GameObject.Instantiate(_tmpGameObject, transform);
            go.SetActive(true);
            var audio = go.GetComponent<AudioSource>();
            audio.volume = isSFX ? SFXVolume : MusicVolume;
            return audio;
        }
    }
}