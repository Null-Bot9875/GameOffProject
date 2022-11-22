using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameUIBattlePanel : MonoBehaviour
    {
        [SerializeField] private Button _settingsBtn;
        [SerializeField] private Button _reloadBtn;
        [SerializeField] private GameObject _settingsPanel;

        private void Awake()
        {
            _settingsBtn.onClick.AddListener(OnClickSettingsBtn);
            _reloadBtn.onClick.AddListener(OnClickReloadBtn);
        }

        private void OnClickSettingsBtn()
        {
            GameObject.Instantiate(_settingsPanel, GameDataCache.Instance.Canvas.transform);
        }

        private void OnClickReloadBtn()
        {
            GameSceneManager.Instance.ReloadScene();
        }
    }
}