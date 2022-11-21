using UnityEngine;

namespace Game
{
    public class AudioManager : SingletonWithMono<AudioManager>
    {
        [SerializeField] private AudioSource _bgm;
        [SerializeField] private AudioSource _effect;

        public void PlayBGM(string path)
        {
            _bgm.clip = Resources.Load<AudioClip>(path);
            _bgm.Play();
        }
        
        public void PlayEffect(string path)
        {
            _effect.clip = Resources.Load<AudioClip>(path);
            _effect.Play();
        }
    }
}