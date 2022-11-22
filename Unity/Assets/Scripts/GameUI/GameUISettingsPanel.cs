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

            _resumeBtn.onClick.AddListener(OnClickResumeBtn);
        }

        private void OnClickResumeBtn()
        {
            GameObject.Destroy(gameObject);
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }
    }
}