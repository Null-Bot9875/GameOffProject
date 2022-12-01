using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameUISettingsPanel : MonoBehaviour
    {
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Button _resumeBtn;

        private void Awake()
        {
            Time.timeScale = 0;
            GameDataCache.Instance.Player.IsMove = false;
            _resumeBtn.onClick.AddListener(OnClickResumeBtn);
            _sfxSlider.SetValueWithoutNotify(AudioManager.Instance.SFXVolume);
            _musicSlider.SetValueWithoutNotify(AudioManager.Instance.MusicVolume);
            _sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
            _musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        }

        private void OnClickResumeBtn()
        {
            GameObject.Destroy(gameObject);
            AudioManager.Instance.PlayAudioOnce(GamePath.SFXUIClick);
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
            GameDataCache.Instance.Player.IsMove = true;
        }
    }
}